using ApiERP.Models;
using ApiERP.Models.ModelExtensions;
using ApiERP.Services;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
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
                var tipos = _db.Database.SqlQuery<tClasProducto>("Select DISTINCT RTRIM(LTRIM(CLASE)) + ' ' + RTRIM(LTRIM(GRUPO)) Codigo " +
                                                " from tClasProducto where RTRIM(LTRIM(CLASE)) + ' ' + RTRIM(LTRIM(GRUPO)) is not null")
                    .Select(x => x.Codigo)
                    .ToList();

                var prods = _db.tProductos
                    .Join(_db.tProductosCostos,
                           pe => pe.CodProducto,
                           p => p.CodProducto,
                           (pe, p) => new { PE = pe, P = p })
                           .Where(x => x.PE.Descripcion.Length > 0 && !x.PE.Descripcion.Contains("NO USAR")
                           && !x.PE.Descripcion.Contains("INHABILITADO")
                           && tipos.Contains(x.PE.CodProducto.Substring(0, 5)))
                                .Select(x => new
                                {
                                    Codigo = x.P.CodProducto,
                                    Descripcion = x.PE.Descripcion.Substring(0, 39),
                                    UnidadMedida = x.PE.UnidadMedida,
                                    Tipo = x.P.CodProducto.Substring(0, 5),
                                    CostoUSD = x.P.CostoPromedioUS,
                                    FechaEstandar = x.P.UltimaFechaCompra
                                }).Take(1);

                return Ok(prods);
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
                var existe = _db.tProductos.Where(x => x.CodProducto.Equals(producto)).Count();
                var existeB = _db.tBodegas.Where(x => x.CodBodega.Equals(bodega)).Count();
                var costo = _db.tProductosCostos.Where(x => x.CodProducto.Equals(producto)).Count();
                var tieneRegistroInv = _db.tProductosInventario
                                        .Where(x => x.CodProducto.Equals(producto) && x.CodBodega.Equals(bodega))
                                        .Count();

                if (existe == 0)
                    return BadRequest("Código de Producto no existe");

                if (existeB == 0)
                    return BadRequest("Código de Bodega no existe");

                if (tieneRegistroInv == 0)
                    return BadRequest("Este producto no ha sido agregado al inventario");


                if (costo == 0)
                    return BadRequest("Este producto no tiene costo");

                var stock = _db.tProductosInventario
                            .Join(_db.tProductos,
                            pe => pe.CodProducto,
                            p => p.CodProducto,
                            (pe, p) => new { PE = pe, P = p }
                            )
                            .Join(_db.tProductosCostos,
                            tProductosInventario => tProductosInventario.PE.CodProducto,
                            pc => pc.CodProducto,
                            (pe, pc) => new { PC = pc, PE = pe, }
                            )
                            .Where(x => x.PE.P.CodProducto.Equals(producto) && x.PE.PE.CodBodega.Equals(x.PE.PE.CodBodega))

                            .Select(x => new
                            {
                                CodProducto = x.PE.PE.CodProducto,
                                CodBodega = x.PE.PE.CodBodega,
                                UndMedida = x.PE.P.UnidadMedida,
                                StockActual = x.PE.PE.ExistenciaActual,
                                StockDisponible = x.PE.PE.ExistenciaDisponible,
                                CostoUSD = x.PC.CostoPromedioUS,
                                FechaEstandar = x.PC.UltimaFechaCompra
                            });

                return Ok(stock);
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
                /*
                var stock = _db.tProductosInventario
                            .Join(_db.tProductos,
                            pe => pe.CodProducto,
                            p => p.CodProducto,
                            (pe, p) => new { PE = pe, P = p }
                            )
                            .Select(x => new
                            {
                                CodProducto = x.PE.CodProducto,
                                CodBodega = x.PE.CodBodega,
                                Descripcion = x.P.Descripcion,
                                UndMedida = x.P.UnidadMedida,
                                StockActual = x.PE.ExistenciaActual,
                                StockDisponible = x.PE.ExistenciaDisponible
                            });*/

                var stock = _db.tProductosInventario
                            .Join(_db.tProductos,
                            pe => pe.CodProducto,
                            p => p.CodProducto,
                            (pe, p) => new { PE = pe, P = p }
                            )
                            .Join(_db.tProductosCostos,
                            tProductosInventario => tProductosInventario.PE.CodProducto,
                            pc => pc.CodProducto,
                            (pe, pc) => new { PC = pc, PE = pe, }
                            )
                            .Select(x => new
                            {
                                CodProducto = x.PE.PE.CodProducto,
                                CodBodega = x.PE.PE.CodBodega,
                                UndMedida = x.PE.P.UnidadMedida,
                                StockActual = x.PE.PE.ExistenciaActual,
                                StockDisponible = x.PE.PE.ExistenciaDisponible,
                                CostoUSD = x.PC.CostoPromedioUS,
                                FechaEstandar = x.PC.UltimaFechaCompra
                            });


                return Ok(stock);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }


    }
}
