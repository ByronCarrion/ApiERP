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
    /// Consulta de Trabajadores
    /// </summary>
    [Authorize]
    [RoutePrefix("api/trabajadores")]
    public class EmployeesController : ApiController
    {

        private ConexionSQLServer _db = new ConexionSQLServer();

        /// <summary>
        /// Obtiene el listado de todos los trabajadores
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Ok. Devuelve el listado completo de todos los trabajadores</response>
        /// <response code="400">Error. Devuelve la descripción del error</response>
        /// <response code="401">El Usuario no está autorizado para realizar esta petición</response>
        [Route("todos")]
        [HttpGet]
        [ResponseType(typeof(List<Trabajadores>))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(EmployeeExample))]
        [MethodGroup("Trabajadores")]
        public IHttpActionResult todos()
        {
            try
            {
                DataTable resultado = _db.ExecuteTable(" Select Compania,RTRIM(Codigo) as Codigo,Nombre,Dni,Activo,CAST(FechaNacimiento as varchar) as FechaNacimiento, Sexo,Profesion,Funcion,Planta,TURMA from dbApiERP.dbo.EMPLEADOSERP where Codigo not in(select codigo from openquery([BIOSALC] , 'select codigo from montelimar.func' )) ");




                return Ok(resultado);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        /// <summary>
        /// Obtiene el listado de todos los cargos
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Ok. Devuelve el listado completo de todos los cargos</response>
        /// <response code="400">Error. Devuelve la descripción del Problema</response>
        /// <response code="401">El Usuario no está autorizado para realizar esta petición</response>
        [Route("profesiones")]
        [HttpGet]
        [ResponseType(typeof(List<Profesiones>))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(EmployeeProofExample))]
        [MethodGroup("Trabajadores")]
        public IHttpActionResult profesiones()
        {
            try
            {
                DataTable resultado = _db.ExecuteTable("select JPCOMPID as Compania, JPPROFID as Codigo, JPPROFNM as Descripcion from IBPYJOBPROFL WHERE JPCOMPID = 3;");

                List<Profesiones> profesion = resultado.AsEnumerable().
                                                Select(row => new Profesiones
                                                {
                                                    Compania = row.Field<int>("Compania"),
                                                    Codigo = row.Field<int>("Codigo"),
                                                    Descripcion = row.Field<string>("Descripcion")
                                                }).ToList();

                return Ok(profesion);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}
