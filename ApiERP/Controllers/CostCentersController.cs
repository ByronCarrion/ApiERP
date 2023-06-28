using ApiERP.Models.ModelExtensions;
using ApiERP.Services;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace ApiERP.Controllers
{
    /// <summary>
    /// Consulta de Centros de Costos
    /// </summary>
    [Authorize]
    [RoutePrefix("api/CentrosCostos")]
    public class CostCentersController : ApiController
    {
        private ConexionMySQL _db = new ConexionMySQL();

        /// <summary>
        /// Obtiene el listado de todos los centros de costos
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Ok. Devuelve el listado completo de todos los centros de costos</response>
        /// <response code="400">Error. Devuelve la descripción del Problema</response>
        /// <response code="401">El usuario no está autorizado para realizar esta petición</response>
        [Route("todos")]
        [HttpGet]
        [ResponseType(typeof(List<CentrosCostos>))]
        [MethodGroup("Centros de Costos")]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(CCExample))]
        // GET: CostCenters
        public IHttpActionResult todos()
        {
            try
            {
                DataTable resultado = _db.ExecuteTable(
                                                        "Select concat(ccentid1,                                                                        " +
                                                        "                case when ccentid2 > 0 then concat('.',ccentid2) else '' end,                  " +
                                                        "                case when ccentid3 > 0 then concat('.',ccentid3) else '' end,                  " +
                                                        "                case when ccentid4 > 0 then concat('.',ccentid4) else '' end) as Codigo,       " +
                                                        "       concat(ccentname1,                                                                      " +
                                                        "                case when ccentid2 > 0 then concat('-', ccentname2) else '' end,               " +
                                                        "                case when ccentid3 > 0 then concat('-',ccentname3) else '' end,                " +
                                                        "                case when ccentid4 > 0 then concat('-',ccentname4) else '' end) as Descripcion, '1' as Activo " +
                                                        "from ibwdcstcenter where cccompid = 3 order by ccentid1, ccentid2, ccentid3, ccentid4;         ");


                List<CentrosCostos> centros = resultado.AsEnumerable().
                                                Select(row => new CentrosCostos
                                                {
                                                    Codigo = row.Field<string>("Codigo"),
                                                    Descripcion = row.Field<string>("Descripcion"),
                                                    Activo = row.Field<string>("Activo")

                                                }).ToList();


                Console.WriteLine(centros.ToList());

                return Ok(centros);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}