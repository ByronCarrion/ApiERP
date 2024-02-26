using ApiERP.Models;
using ApiERP.Models.ModelExtensions;
using ApiERP.Services;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
//using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;


namespace ApiERP.Controllers
{
    /// <summary>
    /// Consulta de Productos
    /// </summary>
    [Authorize]
    [RoutePrefix("api/productos")]
    public class ArticlesController : ApiController
    {
        private dbInventEntities _db = new dbInventEntities();
        private ConexionMySQL _dbMysql = new ConexionMySQL();

        /// <summary>
        /// Obtiene el listado de todos los productos
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Ok. Devuelve el listado completo de todos los productos</response>
        /// <response code="401">No Autorizado</response>
        /// <response code="500">Error. Descripción del problema</response>
        [Route("todos")]
        [HttpGet]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(ArticleExample))]
        [MethodGroup("Productos")]
        public IHttpActionResult todos()
        {
            try
            {
                DataTable Saldos = _dbMysql.ExecuteTable("SELECT PRODCOD as codigo, PRODNAME as descripcion,UMNAME as UnidadMedida,SUBSTRING(PRODCOD,1,5) as tipo,cast(SAINVCOSTAVG as decimal (16,2)) as CostoUSD ,CAST( SAINVADDDATE AS CHAR) as  FechaEstandar FROM IBSALDOINV INNER JOIN IBPRODUCTO ON SAINVIDPROD = PRODID and SAINVIDCOMPANY = PRODIDCOMP INNER JOIN IBUNITMESAURE ON UMID = PRODIDUNIT and  UMCOMPID =  SAINVIDCOMPANY WHERE SAINVIDCOMPANY = 3");

                return Ok(Saldos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        /// <summary>
        /// Obtiene el listado de los tipos de productos
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Ok. Devuelve el listado completo de todos los tipos de productos</response>
        /// <response code="500">Error. Descripción del Problema</response>
        /// <response code="401">No Autorizado</response>       
        [Route("tipos")]
        [HttpGet]
        [ResponseType(typeof(List<tClasProducto>))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(ArticleTypeExample))]
        [MethodGroup("Productos")]
        public IHttpActionResult ConsultaTiposProductos()
        {
            try
            {
                var tipos = _db.Database.SqlQuery<tClasProducto>("Select DISTINCT RTRIM(LTRIM(CLASE)) + ' ' + RTRIM(LTRIM(GRUPO)) Codigo " +
                                                            ", SUBSTRING(RTRIM(LTRIM(GRUPODESCR)), 1, 40) Descripcion " +
                                                            " from tClasProducto where RTRIM(LTRIM(CLASE)) + ' ' + RTRIM(LTRIM(GRUPO)) is not null")
                            .ToList();


                return Ok(tipos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        /// <summary>
        /// Consulta el stock actual y disponible del producto
        /// </summary>
        /// <param name="producto">Código del Producto</param>
        /// <param name="bodega">Código de la Bodega</param>
        /// <returns></returns>
        /// <response code="200">Ok. Devuelve el stock actual y disponible del producto</response>
        /// <response code="401">No Autorizado</response>
        /// <response code="400">Error. Descripción del problema</response>
        [Route("stock")]
        [HttpGet]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(ArticleStockExample))]
        [MethodGroup("Productos")]
        public IHttpActionResult ConsultaStock(string producto, string bodega)
        {
            try
            {

                DataTable Stock = _dbMysql.ExecuteTable("SELECT\r\nPRODCOD as Codigo,\r\nBODEGAID as CodBodega,\r\nUMNAME as UnidadMedida,\r\nSAINVSTCKACT as StockActual,\r\nSAINVSTCKACT as StockDisponible,\r\ncast(SAINVCOSTAVG as decimal (16,2)) as CostoUSD,\r\nCAST( SAINVADDDATE AS CHAR) as  FechaEstandar\r\nFROM IBSALDOINV \r\nINNER JOIN IBPRODUCTO ON SAINVIDPROD = PRODID and SAINVIDCOMPANY = PRODIDCOMP\r\nINNER JOIN IBUNITMESAURE ON UMID = PRODIDUNIT and  UMCOMPID =  SAINVIDCOMPANY\r\nINNER JOIN IBBODEGA ON BODEGAID = SAINVIDWARH and  SAINVIDCOMPANY = BODCOMPANYID\r\nWHERE  PRODCOD = '" + producto + "' and BODEGAID = '"+bodega+"' and SAINVIDCOMPANY = 3");

                return Ok(Stock);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        /// <summary>
        /// Consulta el Stock disponible y Actual de todos los productos
        /// </summary>
        /// <returns></returns>
        /// <response code="401">No Autorizado</response>
        /// <response code="200">Ok. Devuelve el listado de todos los productos con su stock</response>
        /// <response code="400">Error. Descripción del problema</response>
        [Route("stockCompleto")]
        [HttpGet]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(ArticleStockCExample))]
        [MethodGroup("Productos")]
        public IHttpActionResult ConsultaStockCompleto()
        {
            try
            {
                DataTable Stock = _dbMysql.ExecuteTable("SELECT\r\nPRODCOD as Codigo,\r\nBODEGAID as CodBodega , UMNAME as UnidadMedida, SAINVSTCKACT as StockActual,\r\nSAINVSTCKACT as StockDisponible , cast(SAINVCOSTAVG as decimal (16,2)) as CostoUSD ,\r\n CAST( SAINVADDDATE AS CHAR) as  FechaEstandar FROM IBSALDOINV \r\nINNER JOIN IBPRODUCTO ON SAINVIDPROD = PRODID and SAINVIDCOMPANY = PRODIDCOMP\r\nINNER JOIN IBUNITMESAURE ON UMID = PRODIDUNIT and  UMCOMPID =  SAINVIDCOMPANY\r\nINNER JOIN IBBODEGA ON BODEGAID = SAINVIDWARH and  SAINVIDCOMPANY = BODCOMPANYID\r\nWHERE  SAINVIDCOMPANY = 3");

                return Ok(Stock);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }


    }
}
