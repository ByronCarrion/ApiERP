using ApiERP.Models.ModelExtensions;
using ApiERP.Services;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace ApiERP.Controllers
{
    /// <summary>
    /// Consulta de Clientes
    /// </summary>
    [Authorize]
    [RoutePrefix("api/clientes")]
    public class CustomersController : ApiController
    {

        private ConexionSQLServer _db = new ConexionSQLServer();

        /// <summary>
        /// Obtiene el listado de todos los clientes
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Ok. Devuelve el listado completo de todos los clientes</response>
        /// <response code="400">Error. Devuelve la descripción del error</response>
        /// <response code="401">No Autorizado</response>
        [Route("todos")]
        [HttpGet]
        [ResponseType(typeof(List<Clientes>))]
        [MethodGroup("Clientes")]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(CustomerExample))]
        public IHttpActionResult todos()
        {
            try
            {
                DataTable resultado = _db.ExecuteTable("SELECT Codigo, Nombre, Ruc, Activo FROM dbApiERP.dbo.CLIENTEERP where codigo not in (select codigo from openquery([BIOSALC], 'select codigo from montelimar.cliente'))");

                return Ok(resultado);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

    }
}
