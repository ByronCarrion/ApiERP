using ApiERP.Models;
using ApiERP.Models.ModelExtensions;
using ApiERP.Services;
using ApiERP.Services.Utility;
using Newtonsoft.Json;
using Swashbuckle.Examples;
using System;
using System.Data;
using System.Net;
using System.Web.Http;

namespace ApiERP.Controllers
{
    /// <summary>
    /// Creación de Tickets de Biomasa
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Biomasa")]

    public class BiomasaController : ApiController
    {
        private ConexionMySQL _db = new ConexionMySQL();
        private dbInventEntities _sql = new dbInventEntities();

        /// <summary>
        /// Crea un Ticket en el ERP
        /// </summary>
        /// <param name="datos">Datos del ticket (JSON)</param>
        /// <returns></returns>
        /// /// <response code="200">Ok. Ticket creado correctamente</response>
        /// <response code="400">Error. Devuelve la descripción del Problema</response>
        /// <response code="401">El usuario no está autorizado para realizar esta petición</response>
        [Route("nuevoTicket")]
        [HttpPost]
        [MethodGroup("Biomasa")]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(BiomasaExample))]
        public IHttpActionResult nuevoTicket([FromBody] BiomasaSiagri datos)
        {
            try
            {
                DataTable dt;

                //INSERTAMOS EN EL ERP EL JSON RECIBIDO
                dt = _db.ExecuteTable($"INSERT INTO IBLOGAPIS (LOGAPINAME,LOGAPICALL,LOGDOCUMENTID,LOGDATAJSON,LOGAPIADDWHO,LOGAPIADDDATE,LOGAPIADDIP) VALUES ('BIOMASA','CREATE','{datos.NUMDOC}','{JsonConvert.SerializeObject(datos)}','SIAGRI',NOW(),'127.0.0.1');");
                dt.Clear();

                //if (!ModelState.IsValid)
                //return BadRequest("Error, Documento con datos no válidos");

                if (!(datos.NUMDOC > 0))
                    return BadRequest("Error, Debe indicar un número de Ticket.!");

                if (String.IsNullOrEmpty(datos.CONDUC.Trim()))
                    return BadRequest("Error, Debe indicar nombre del conductor.! ");

                if (String.IsNullOrEmpty(datos.PLACAB.Trim()))
                    return BadRequest("Error, Debe indicar el numero de Placa.! ");

                if (datos.TARA == 0 && datos.BRUTO == 0)
                    return BadRequest("Error, Peso Tara o Bruto debe ser mayor a Cero.!");

                if (!(datos.CODPROD > 0))
                    return BadRequest("Error, Debe indicar ID Producto.!");

                //MyLogger.GetInstance().Info("Llamado al Biomasa Controller. Nuevo Ticket method, Datos Recibidos: " + JsonConvert.SerializeObject(datos));

                int idCompany = 2;
                if (datos.NUMDOC >= 60000000 && datos.NUMDOC <= 89999999)
                    idCompany = 1;
                if (datos.NUMDOC > 0 && datos.NUMDOC < 60000000)
                    idCompany = 3;
                if (datos.NUMDOC > 89999999)
                    idCompany = 3;

                dt = _db.ExecuteTable($"SELECT COALESCE(TCKSEQUENCE,0) as ID FROM IBAPCNTTICK WHERE TCKCOMPANY = '{idCompany}' AND TCKSEQUENCE='{datos.NUMDOC}';");
                int numeroTicket = 0;
                if (dt.Rows.Count > 0)
                    numeroTicket = int.Parse(dt.Rows[0]["ID"].ToString());

                if (numeroTicket > 0)
                    return BadRequest($"Error, al insertar Numero de Ticket: {numeroTicket}, ya existe en el ERP.!");

                Biomasa biomasa = new Biomasa();
                biomasa.TCKCOMPANY = idCompany;
                biomasa.TCKSEQUENCE = datos.NUMDOC;
                biomasa.TCKIDEXTERNO = 0;
                biomasa.TCKDATETIME = datos.DATETIMEIN;
                biomasa.TCKCONDUCT = datos.CONDUC.ToString().Trim();
                biomasa.TCKMATRIC = datos.PLACAB.ToString().Trim();
                biomasa.TCKNUMBASCULA = "";
                biomasa.TCKWGHTBT = datos.BRUTO;
                biomasa.TCKWGHTTR = datos.TARA;
                biomasa.TCKWGHTNT = datos.NETO;
                biomasa.WHO = "SIAGRI";
                biomasa.IP = "127.0.0.1";
                biomasa.ACTION = "ADDTICKET";

                if (biomasa.Insert())
                {
                    try
                    {
                        biomasa.TCKCOMPANY = idCompany;
                        biomasa.TCKSEQUENCE = datos.NUMDOC;
                        biomasa.TCKIDEXTERNO = 0;
                        biomasa.TCKDATETIME = datos.DATETIMEIN;
                        biomasa.TCKCONDUCT = datos.CONDUC.ToString().Trim();
                        biomasa.TCKMATRIC = datos.PLACAB.ToString().Trim();
                        biomasa.TCKNUMBASCULA = "";
                        biomasa.TCKWGHTBT = datos.BRUTO;
                        biomasa.TCKWGHTTR = datos.TARA;
                        biomasa.TCKWGHTNT = datos.NETO;
                        biomasa.WHO = "SIAGRI";
                        biomasa.IP = "127.0.0.1";
                        biomasa.ACTION = "UPDATEBASCULAOPEN";
                        biomasa.Insert();
                    }
                    catch (Exception e)
                    {
                        return BadRequest("Error al insertar Ticket.!" + e.Message.ToString());
                    }

                    try
                    {
                        if (datos.BRUTO > 0 && datos.TARA > 0 && datos.NETO > 0)
                        {
                            biomasa.TCKCOMPANY = idCompany;
                            biomasa.TCKSEQUENCE = datos.NUMDOC;
                            biomasa.TCKIDEXTERNO = 0;
                            biomasa.TCKDATETIME = datos.DATETIMEIN;
                            biomasa.TCKCONDUCT = datos.CONDUC.ToString().Trim();
                            biomasa.TCKMATRIC = datos.PLACAB.ToString().Trim();
                            biomasa.TCKNUMBASCULA = "";
                            biomasa.TCKWGHTBT = datos.BRUTO;
                            biomasa.TCKWGHTTR = datos.TARA;
                            biomasa.TCKWGHTNT = datos.NETO;
                            biomasa.WHO = "SIAGRI";
                            biomasa.IP = "127.0.0.1";
                            biomasa.ACTION = "UPDATEBASCULACLOSE";
                            biomasa.Insert();

                            biomasa.TCKCOMPANY = idCompany;
                            biomasa.TCKSEQUENCE = datos.NUMDOC;
                            biomasa.TCKIDEXTERNO = 0;
                            biomasa.TCKDATETIME = datos.DATETIMEIN;
                            biomasa.TCKCONDUCT = datos.CONDUC.ToString().Trim();
                            biomasa.TCKMATRIC = datos.PLACAB.ToString().Trim();
                            biomasa.TCKNUMBASCULA = "";
                            biomasa.TCKWGHTBT = datos.BRUTO;
                            biomasa.TCKWGHTTR = datos.TARA;
                            biomasa.TCKWGHTNT = datos.NETO;
                            biomasa.WHO = "SIAGRI";
                            biomasa.IP = "127.0.0.1";
                            biomasa.ACTION = "CLOSEBASCULA";
                            biomasa.Insert();
                        }
                    }
                    catch (Exception e)
                    {
                        return BadRequest("Error al insertar Ticket.!" + e.Message.ToString());
                    }
                    return Ok($"Ticket Creado Exitosamente.! {datos.NUMDOC}");
                }
                else
                    return BadRequest("Error al insertar Ticket.! ");
            }
            catch (Exception e)
            {
                return BadRequest("Error al insertar Ticket.!" + e.Message.ToString());
            }
        }

        /// <summary>
        /// Actualiza los datos del Ticket en el ERP
        /// </summary>
        /// <param name="datos">Datos del ticket (JSON)</param>
        /// <returns></returns>
        /// /// <response code="200">Ok. Requisa creada correctamente</response>
        /// <response code="400">Error. Devuelve la descripción del Problema</response>
        /// <response code="401">El usuario no está autorizado para realizar esta petición</response>
        [Route("updateTicket")]
        [HttpPost]
        [MethodGroup("Biomasa")]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(BiomasaExample))]
        public IHttpActionResult updateTicket([FromBody] BiomasaSiagri datos)
        {
            try
            {
                DataTable dt;

                //INSERTAMOS EN EL ERP EL JSON RECIBIDO
                dt = _db.ExecuteTable($"INSERT INTO IBLOGAPIS (LOGAPINAME,LOGAPICALL,LOGDOCUMENTID,LOGDATAJSON,LOGAPIADDWHO,LOGAPIADDDATE,LOGAPIADDIP) VALUES ('BIOMASA','UPDATE','{datos.NUMDOC}','{JsonConvert.SerializeObject(datos)}','SIAGRI',NOW(),'127.0.0.1');");
                dt.Clear();

                //if (!ModelState.IsValid)
                //return BadRequest("Error, Documento con datos no válidos");

                if (!(datos.NUMDOC > 0))
                    return BadRequest("Error, Debe indicar un número de Ticket.!");

                if (String.IsNullOrEmpty(datos.CONDUC.Trim()))
                    return BadRequest("Error, Debe indicar nombre del conductor.! ");

                if (String.IsNullOrEmpty(datos.PLACAB.Trim()))
                    return BadRequest("Error, Debe indicar el numero de Placa.! ");

                if (datos.TARA == 0 && datos.BRUTO == 0)
                    return BadRequest("Error, Peso Tara o Bruto debe ser mayor a Cero.!");

                if (!(datos.CODPROD > 0))
                    return BadRequest("Error, Debe indicar ID Producto.!");

                MyLogger.GetInstance().Info("Llamado al Biomasa Controller. Actualiza Ticket method, Datos Recibidos: " + JsonConvert.SerializeObject(datos));

                int idCompany = 2;
                if (datos.NUMDOC >= 60000000 && datos.NUMDOC <= 89999999)
                    idCompany = 1;
                if (datos.NUMDOC > 0 && datos.NUMDOC < 60000000)
                    idCompany = 3;
                if (datos.NUMDOC > 89999999)
                    idCompany = 3;

                dt = _db.ExecuteTable($"SELECT COALESCE(TCKSEQUENCE,0) as ID FROM IBAPCNTTICK WHERE TCKCOMPANY = '{idCompany}' AND TCKSEQUENCE='{datos.NUMDOC}';");
                int numeroTicket = 0;
                if (dt.Rows.Count > 0)
                    numeroTicket = int.Parse(dt.Rows[0]["ID"].ToString());

                if (!(numeroTicket > 0))
                    return BadRequest($"Error, al actualizar Ticket Numero: {numeroTicket}, No existe en el ERP.!");

                Biomasa biomasa = new Biomasa();
                biomasa.TCKCOMPANY = idCompany;
                biomasa.TCKSEQUENCE = datos.NUMDOC;
                biomasa.TCKIDEXTERNO = 0;
                biomasa.TCKDATETIME = datos.DATETIMEIN;
                biomasa.TCKCONDUCT = datos.CONDUC.ToString().Trim();
                biomasa.TCKMATRIC = datos.PLACAB.ToString().Trim();
                biomasa.TCKNUMBASCULA = "";
                biomasa.TCKWGHTBT = datos.BRUTO;
                biomasa.TCKWGHTTR = datos.TARA;
                biomasa.TCKWGHTNT = datos.NETO;
                biomasa.WHO = "SIAGRI";
                biomasa.IP = "127.0.0.1";
                biomasa.ACTION = "UPDATEBASCULAOPEN";

                if (datos.BRUTO > 0 && datos.TARA > 0 && datos.NETO > 0)
                {
                    biomasa.TCKCOMPANY = idCompany;
                    biomasa.TCKSEQUENCE = datos.NUMDOC;
                    biomasa.TCKIDEXTERNO = 0;
                    biomasa.TCKDATETIME = datos.DATETIMEIN;
                    biomasa.TCKCONDUCT = datos.CONDUC.ToString().Trim();
                    biomasa.TCKMATRIC = datos.PLACAB.ToString().Trim();
                    biomasa.TCKNUMBASCULA = "";
                    biomasa.TCKWGHTBT = datos.BRUTO;
                    biomasa.TCKWGHTTR = datos.TARA;
                    biomasa.TCKWGHTNT = datos.NETO;
                    biomasa.WHO = "SIAGRI";
                    biomasa.IP = "127.0.0.1";
                    biomasa.ACTION = "UPDATEBASCULACLOSE";
                    biomasa.Insert();

                    biomasa.TCKCOMPANY = idCompany;
                    biomasa.TCKSEQUENCE = datos.NUMDOC;
                    biomasa.TCKIDEXTERNO = 0;
                    biomasa.TCKDATETIME = datos.DATETIMEIN;
                    biomasa.TCKCONDUCT = datos.CONDUC.ToString().Trim();
                    biomasa.TCKMATRIC = datos.PLACAB.ToString().Trim();
                    biomasa.TCKNUMBASCULA = "";
                    biomasa.TCKWGHTBT = datos.BRUTO;
                    biomasa.TCKWGHTTR = datos.TARA;
                    biomasa.TCKWGHTNT = datos.NETO;
                    biomasa.WHO = "SIAGRI";
                    biomasa.IP = "127.0.0.1";
                    biomasa.ACTION = "CLOSEBASCULA";
                    biomasa.Insert();
                }

                if (biomasa.Insert())
                    return Ok($"Ticket Actualizado Correctamente.! {datos.NUMDOC}");
                else
                    return BadRequest("Error al Actualizar Datos del Ticket.!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        /// <summary>
        /// Rechaza el Ticket en el ERP
        /// </summary>
        /// <param name="datos">Datos del ticket (JSON)</param>
        /// <returns></returns>
        /// /// <response code="200">Ok. Ticket Rechazado correctamente</response>
        /// <response code="400">Error. Devuelve la descripción del Problema</response>
        /// <response code="401">El usuario no está autorizado para realizar esta petición</response>
        [Route("recchazaTicket")]
        [HttpPost]
        [MethodGroup("Biomasa")]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(BiomasaExample))]
        public IHttpActionResult recchazaTicket([FromBody] BiomasaSiagri datos)
        {
            try
            {
                DataTable dt;

                //INSERTAMOS EN EL ERP EL JSON RECIBIDO
                dt = _db.ExecuteTable($"INSERT INTO IBLOGAPIS (LOGAPINAME,LOGAPICALL,LOGDOCUMENTID,LOGDATAJSON,LOGAPIADDWHO,LOGAPIADDDATE,LOGAPIADDIP) VALUES ('BIOMASA','REJECT','{datos.NUMDOC}','{JsonConvert.SerializeObject(datos)}','SIAGRI',NOW(),'127.0.0.1');");
                dt.Clear();

                //if (!ModelState.IsValid)
                //  return BadRequest("Error, Documento con datos no válidos");

                if (!(datos.NUMDOC > 0))
                    return BadRequest("Error, Debe indicar un número de Ticket.!");

                if (String.IsNullOrEmpty(datos.CONDUC.Trim()))
                    return BadRequest("Error, Debe indicar nombre del conductor.! ");

                if (String.IsNullOrEmpty(datos.PLACAB.Trim()))
                    return BadRequest("Error, Debe indicar el numero de Placa.! ");

                if (datos.TARA == 0 && datos.BRUTO == 0)
                    return BadRequest("Error, Peso Tara o Bruto debe ser mayor a Cero.!");

                if (!(datos.CODPROD > 0))
                    return BadRequest("Error, Debe indicar ID Producto.!");

                MyLogger.GetInstance().Info("Llamado al Biomasa Controller. Rechaza Ticket method, Datos Recibidos: " + JsonConvert.SerializeObject(datos));

                int idCompany = 2;
                if (datos.NUMDOC >= 60000000 && datos.NUMDOC <= 89999999)
                    idCompany = 1;
                if (datos.NUMDOC > 0 && datos.NUMDOC < 60000000)
                    idCompany = 3;
                if (datos.NUMDOC > 89999999)
                    idCompany = 3;

                dt = _db.ExecuteTable($"SELECT COALESCE(TCKSEQUENCE,0) as ID FROM IBAPCNTTICK WHERE TCKCOMPANY = '{idCompany}' AND TCKSEQUENCE='{datos.NUMDOC}';");
                int numeroTicket = 0;
                if (dt.Rows.Count > 0)
                    numeroTicket = int.Parse(dt.Rows[0]["ID"].ToString());
                if (!(numeroTicket > 0))
                    return BadRequest($"Error, al actualizar Ticket Numero: {numeroTicket}, No existe en el ERP.!");
                dt.Clear();

                dt = _db.ExecuteTable($"SELECT COALESCE(TCKSEQUENCE,0) as ID FROM IBAPCNTTICK WHERE TCKCOMPANY = '{idCompany}' AND TCKSEQUENCE='{datos.NUMDOC}' AND TCKSTATUS IN('B','WIP');");
                int ticketCerrado = 0;
                if (dt.Rows.Count > 0)
                    ticketCerrado = int.Parse(dt.Rows[0]["ID"].ToString());
                if (!(ticketCerrado > 0))
                    return BadRequest($"Error al Rechazar/Anular Ticket Numero: {ticketCerrado}, Ya esta Cerrado ó Rechazado.!");
                dt.Clear();

                if (ticketCerrado > 0)
                    dt = _db.ExecuteTable($"UPDATE IBAPCNTTICK SET TCKSTATUS='R',TCKAREA='-', TCKUPDWHO='SIAGRI',TCKUPDDATE=NOW() WHERE TCKCOMPANY='{idCompany}' AND TCKSEQUENCE='{datos.NUMDOC}' ;");
                return Ok($"Ticket Rechazado Correctamente.! {datos.NUMDOC}");
            }
            catch (Exception e)
            {
                return BadRequest($"Error al Rechazar/Anular Ticket Numero: {datos.NUMDOC}, {e.Message.ToString()}");
            }
        }

    }

}
