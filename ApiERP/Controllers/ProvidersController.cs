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
    /// Consulta de Proveedores
    /// </summary>
    [Authorize]
    [RoutePrefix("api/proveedores")]
    public class ProvidersController : ApiController
    {
        private ConexionMySQL _db = new ConexionMySQL();
        private ConexionSQLServer _db2 = new ConexionSQLServer();

        /// <summary>
        /// Obtiene el listado de todos los proveedores
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Ok. Devuelve el listado completo de todos los proveedores</response>
        /// <response code="401">El Usuario no está autorizado para realizar esta petición</response>
        /// <response code="400">Error. Descripción del Problema</response>
        [Route("todos")]
        [HttpGet]
        [ResponseType(typeof(List<Proveedores>))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(ProviderExample))]
        [MethodGroup("Proveedores")]
        public IHttpActionResult todos()
        {
            try
            {
                DataTable resultado = _db2.ExecuteTable("SELECT Compania, Codigo, Nombre, CPF, Tipo, Estado, TipoPropietario from dbApiERP.dbo.PROVEEDORESERP where codigo not in(select codigo from openquery([BIOSALC] , 'select codigo from montelimar.forn where codigo <> ''GN'' ' ))");

                return Ok(resultado);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        /// <summary>
        /// Obtiene el listado de todos los tipos de proveedores
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Ok. Devuelve el listado completo de todos los tipos de proveedores</response>
        /// <response code="401">El Usuario no está autorizado para realizar esta petición</response>
        /// <response code="400">Error. Descripción del Problema</response>
        [Route("tipos")]
        [HttpGet]
        [ResponseType(typeof(List<TipoProveedor>))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(ProviderTypeExample))]
        [MethodGroup("Proveedores")]
        public IHttpActionResult tipos()
        {
            try
            {
                DataTable tipos = _db.ExecuteTable("select COMPANYID as Compania, CODE as Codigo, DESCR as Descripcion from IBUNVENDTYPE WHERE COMPANYID = 3;");

                List<TipoProveedor> proveedor = tipos.AsEnumerable().
                                                Select(row => new TipoProveedor
                                                {
                                                    Compania = row.Field<int>("Compania"),
                                                    Codigo = row.Field<string>("Codigo"),
                                                    Descripcion = row.Field<string>("Descripcion")
                                                }).ToList();

                return Ok(proveedor);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}
