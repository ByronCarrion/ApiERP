using ApiERP.Models;
using ApiERP.Models.ModelExtensions;
using ApiERP.Services;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace ApiERP.Controllers
{
    /// <summary>
    /// Login del Usuario
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {

        private dbApiERPEntities _db = new dbApiERPEntities();
        /// <summary>
        /// Método para generar Token
        /// </summary>
        /// <param name="login">Credenciales del Usuario (JSON)</param>
        /// <returns></returns>
        /// <response code="200">Ok. Devuelve el token generado para ser enviado como cabecera de las peticiones</response>
        /// <response code="400">Envía un error si el usuario o contraseña son incorrectos</response>
        /// <response code="401">El usuario no está autorizado para realizar la consulta</response>
        // GET: Login
        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(UserModel login)
        {
            try
            {

                if (login == null)
                    return BadRequest("Datos incorrectos");

                if (String.IsNullOrEmpty(login.UserName))
                    return BadRequest("Usuario incorrecto");

                if (String.IsNullOrEmpty(login.Password))
                    return BadRequest("Contraseña incorrecta");

                //string directory = HttpContext.Current.Request.PhysicalApplicationPath;
                //StreamReader read = new StreamReader(directory + "Datos/usuarios.json");
                //string jsonString = read.ReadToEnd();
                //var usuarios = JsonConvert.DeserializeObject<List<UserModel>>(jsonString);

                //var log = usuarios.Where(x => x.UserName.Equals(login.UserName) && x.Password.Equals(login.Password)).Count();

                var log = _db.Users.Where(x => x.UserName.Equals(login.UserName) && x.Password.Equals(login.Password)).Count();

                if (log <= 0)
                    return BadRequest("Usuario o Contraseña Incorrectos");

                //TODO: Validate credentials Correctly, this code is only for demo !!
                //bool isCredentialValid = (login.Password == "123456");
                //if (isCredentialValid)
                //{
                var token = TokenGenerator.GenerateTokenJwt(login.UserName);

                var usuario = _db.Users.Where(x => x.UserName.Equals(login.UserName)).FirstOrDefault();

                usuario.Token = token;
                usuario.UltimoIngreso = DateTime.Now;

                _db.Entry(usuario).State = EntityState.Modified;
                _db.SaveChanges();


                return Ok(token);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message.ToString());
            }
        }
    }
}