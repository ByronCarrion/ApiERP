using ApiERP.Models.ModelExtensions;
using ApiERP.Services;
using ApiERP.Services.Utility;
using Newtonsoft.Json;
using Swashbuckle.Examples;
using System;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiERP.Controllers
{
    /// <summary>
    /// Creación de Requisas
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Requisas")]
    public class RequestController : ApiController
    {
        private ConexionMySQL _db = new ConexionMySQL();
        private ConexionSQLServer _dbsql = new ConexionSQLServer();
        //private dbInventEntities _sql = new dbInventEntities();

        /// <summary>
        /// Crea una nueva Requisa con el detalle en el ERP
        /// </summary>
        /// <param name="datos">Datos de la Requisa (JSON)</param>
        /// <returns></returns>
        /// /// <response code="200">Ok. Requisa creada correctamente</response>
        /// <response code="400">Error. Devuelve la descripción del Problema</response>
        /// <response code="401">El usuario no está autorizado para realizar esta petición</response>
        [Route("nueva")]
        [HttpPost]
        [MethodGroup("Requisas")]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(RequisaExample))]
#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task<IHttpActionResult> nueva([FromBody] RequisaSiagri datos)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest("Documento con datos no válidos");
                if (String.IsNullOrEmpty(datos.REQOT))
                    return BadRequest("Debe indicar un número de Orden de Servicio");
                //VALIDA QUE EL CONCEPTO NO VENGA VACÍO
                if (String.IsNullOrEmpty(datos.REQMEMO))
                    return BadRequest("Debe escribir un concepto para la requisa");

                //VALIDAMOS CADA ITEM QUE TRAE LA REQUISA.
                try
                {
                    int i = 1;
                    foreach (var item in datos.Detalle)
                    {
                        if (String.IsNullOrEmpty(item.REQDETIDPRODMCS))
                            return BadRequest("Linea Detalle " + i + " Codigo de producto no puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETPRODDESC))
                            return BadRequest("Linea Detalle " + i + " Descripcion de producto no puede ser vacio o nulo.");
                        if (item.REQDETSTOCKORD <= 0)
                            return BadRequest("Linea Detalle " + i + " Cantidad a Requisas no puede ser 0.");
                        if (item.REQDETCOSTTRAN <= 0)
                            return BadRequest("Linea Detalle " + i + " Costo de Producto no puede ser 0.");
                        if (String.IsNullOrEmpty(item.REQDETADDWHO))
                            return BadRequest("Linea Detalle " + i + " REQDETADDWHO puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETDDDATE.ToString()))
                            return BadRequest("Linea Detalle " + i + " REQDETDDDATE puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETADDIP))
                            return BadRequest("Linea Detalle " + i + " REQDETADDIP puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETCLOSEBY))
                            return BadRequest("Linea Detalle " + i + " REQDETADDWHO puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETCLOSEDATE.ToString()))
                            return BadRequest("Linea Detalle " + i + " REQDETDDDATE puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETCLOSEIP))
                            return BadRequest("Linea Detalle " + i + " REQDETADDIP puede ser vacio o nulo.");
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(" metodo 1 - Error al Validar Detalle de Items de la Requisa: " + ex.Message);
                }

                DataTable dt;
                //INSERTAMOS EN EL ERP EL JSON RECIBIDO

                dt = _db.ExecuteTable($"INSERT INTO IBLOGAPIS (LOGAPINAME,LOGAPICALL,LOGDOCUMENTID,LOGDATAJSON,LOGAPIADDWHO,LOGAPIADDDATE,LOGAPIADDIP) VALUES ('REQUISA','CREATE','{datos.REQUISAID}','{JsonConvert.SerializeObject(datos)}','{datos.REQADDWHO}',NOW(),'{datos.REQADDIP}');");
                dt.Clear();


                //CONSULTA SI LA FINCA SELECCIONADA EXISTE EN ERP
                String claseID = "0";
                var consulta = $"SELECT ENTCLASSID FROM IBENTCLASS WHERE ENTCOMPID = '{datos.REQCOMPANYID}' AND SUBSTRING(ENTCODE,3,2) = '{datos.REQCLASSID}' AND IF(ENTCOMPID=3, ENTCLASSID NOT IN(67,68,69,70,71,72,61,62),ENTCLASSID>0) LIMIT 1;";
                dt = _db.ExecuteTable(consulta);
                if (dt != null && dt.Rows.Count > 0)
                    claseID = dt.Rows[0]["ENTCLASSID"].ToString();
                dt.Clear();

                int QEMPID = 0;
                String USERNAME = "";
                if (datos.REQEMPID !=0 )
                {
                    dt = _db.ExecuteTable($"SELECT EYEMPID, EYUSERNAME FROM CM_CGP_ERP.IBPYEMPDT WHERE EYCOMPID = '{datos.REQCOMPANYID}' and EYIDSIAGRIEMP = '{datos.REQEMPID}';");
                    if (dt.Rows.Count > 0)
                        QEMPID = Int32.Parse(dt.Rows[0]["EYEMPID"].ToString());                      
                    dt.Clear();

                    dt = _db.ExecuteTable($"SELECT EYUSERNAME FROM CM_CGP_ERP.IBPYEMPDT WHERE EYCOMPID = '{datos.REQCOMPANYID}' and EYIDSIAGRIEMP = '{datos.REQEMPID}';");
                    if (dt.Rows.Count > 0)
                        USERNAME = dt.Rows[0]["EYUSERNAME"].ToString().ToUpper();
                    dt.Clear();

                }

         
                //CUENTAS CONTABLES
                int IdCatalogo = 0;
                String Level1 = "000";
                String Level2 = "000";
                String Level3 = "000";
                String Level4 = "000";
                String Level5 = "000";
                String Level6 = "000";

                if (datos.REQCOMPANYID == 2)
                {
                    //CUENTA CONTABLE DE COMPANIA DE PRUEBA: 92.02.001.007	Fertilizantes Y Agroquímicos Produccion en Proceso
                    //24675	007	000	000	000	000	000
                    IdCatalogo = 24675;
                    Level1 = "007";
                    Level2 = "000";
                    Level3 = "000";
                    Level4 = "000";
                    Level5 = "000";
                    Level6 = "000";
                }
                if (datos.REQCOMPANYID == 3)
                {
                    //CUENTA CONTABLE DE MONTELIMAR: 92.02.001.007	Fertilizantes Y Agroquímicos Produccion en Proceso
                    //1946	026	002	001	007	000	000
                    IdCatalogo = 1946;
                    Level1 = "026";
                    Level2 = "002";
                    Level3 = "001";
                    Level4 = "007";
                    Level5 = "000";
                    Level6 = "000";
                }

                //SI EXISTE RECUPERAMOS EL CENTRO DE COSTO ASIGNADO.
                String CentroCosto = "0.0.0.0.0";
                Decimal levelCC1 = 0;
                Decimal levelCC2 = 0;
                Decimal levelCC3 = 0;
                Decimal levelCC4 = 0;
                Decimal levelCC5 = 0;
                if (datos.REQACTIVITY.Trim().Length > 0)
                {
                    dt = _db.ExecuteTable($"SELECT COALESCE(ACLOTIDCC,'0.0.0.0.0') AS CentroCosto FROM IBACTIVITYLOTE WHERE ACLOTIDCOMPANY = '{datos.REQCOMPANYID}' AND ACLOTCODEACTIVIDAD='{datos.REQACTIVITY}';");
                    if (dt.Rows.Count > 0)
                        CentroCosto = dt.Rows[0]["CentroCosto"].ToString();
                    dt.Clear();
                }
                var centroCostoLevels = CentroCosto.Split('.');
                levelCC1 = decimal.Parse(centroCostoLevels[0]);
                levelCC2 = decimal.Parse(centroCostoLevels[1]);
                levelCC3 = decimal.Parse(centroCostoLevels[2]);
                levelCC4 = decimal.Parse(centroCostoLevels[3]);
                levelCC5 = decimal.Parse(centroCostoLevels[4]);

                //OBTENEMOS EL ID CON EL QUE SE VA A GENERAR LA REQUISA.
                dt = _db.ExecuteTable($"SELECT COALESCE(MAX(REQUISAID),0) + 1 as ID FROM IBBODREQUISA WHERE REQCOMPANYID = '{datos.REQCOMPANYID}';");
                int proximoId = 0;
                if (dt.Rows.Count > 0)
                    proximoId = int.Parse(dt.Rows[0]["ID"].ToString());
                dt.Clear();


                Requisas req = new Requisas();

                req.REQCOMPANYID = datos.REQCOMPANYID; //PROVISIONAL PARA PRUEBA
                req.REQUISAID = 0;
                req.REQREFNUM = datos.REQUISAID.ToString(); //Guardamos el idRequisa (Siagri) en NUMREF
                req.REQBODEGAID = datos.REQBODEGAID;
                req.REQEMPID = QEMPID;
                req.REQUISASTATUS = datos.REQUISASTATUS.ToString();
                req.REQGLID = IdCatalogo;
                req.REQGL1 = Level1;
                req.REQGL2 = Level2;
                req.REQGL3 = Level3;
                req.REQGL4 = Level4;
                req.REQGL5 = Level5;
                req.REQGL6 = Level6;
                req.REQCCID1 = levelCC1;
                req.REQCCID2 = levelCC2;
                req.REQCCID3 = levelCC3;
                req.REQCCID4 = levelCC4;
                req.REQCCID5 = levelCC5;
                req.REQTOPMNGRID = datos.REQCOMPANYID == 3 ? 3 : 4;//ID GERENCIA
                req.REQCOMPSCTID = datos.REQCOMPANYID == 3 ? 9 : 4;//ID DEPARTAMENTO
                req.REQSECTIONID = 0; //DEPARTAMENTO DE MCS
                req.REQCLASSID = Int16.Parse(claseID); //FINCA
                req.REQLOT = datos.REQLOT;
                req.REQACTIVITY = datos.REQACTIVITY;
                req.REQOT = datos.REQUISAID.ToString();
                req.REQUISADATE = datos.REQUISADATE;
                req.REQUISAFORDATE = datos.REQFORDATE;
                req.REQMEMO = datos.REQMEMO.Substring(0, datos.REQMEMO.IndexOf("-")) + "-" + USERNAME;
                req.REQTRIDCURR = 1;
                req.REQCOIDCURR = 0;
                req.REQBSIDCURR = 0;
                req.REQEQUIPO = "";
                req.REQADDWHO = USERNAME;
                req.REQADDDATE = datos.REQADDDATE;
                req.REQADDIP = datos.REQADDIP;
                req.REQUPDWHO = USERNAME;
                req.REQUPDDATE = datos.REQUPDDATE;
                req.REQUPDIP = datos.REQUPDIP;
                req.REQAPROBY = USERNAME;
                req.REQAPRODATE = datos.REQAPRODATE;
                req.REQAPROIP = datos.REQAPROIP;
                req.REQCLOSEBY = USERNAME;
                req.REQCLOSEDATE = datos.REQCLOSEDATE;
                req.REQCLOSEIP = datos.REQCLOSEIP;
                req.ACTION = "ADDREQUISA";
                req.REQPROC = "SIA";

                if (req.Insert())
                {
                    DetalleReq det;
                    int i = 1;
                  //  double costo = 0;
                    foreach (var item in datos.Detalle)
                    {
                      //  costo = 0;
                      
                        det = new DetalleReq();
                        det.REQDETCOMPID = datos.REQCOMPANYID; //compañía de pruebas momentáneo
                        det.REQDETID = proximoId;
                        det.REQDETIDLIN = i;
                        det.REQDETIDPROD = 0;
                        det.REQDETIDPRODMCS = item.REQDETIDPRODMCS;
                        det.REQDETPRODDESC = item.REQDETPRODDESC;
                        det.REQDETUMMCS = item.REQDETUMMCS;
                        det.REQDETMEMO = item.REQDETMEMO;
                        det.REQDETSTOCKORD = item.REQDETSTOCKORD;
                        det.REQDETSTOCKGET = item.REQDETSTOCKORD;
                        det.REQDETCOSTTRAN = item.REQDETCOSTTRAN;                        
                        det.REQDETTRIDCURR = 1;
                        det.REQDETCOIDCURR = 0;
                        det.REQDETBSIDCURR = 0;
                        det.REQDETADDWHO = USERNAME;
                        det.REQDETDDDATE = item.REQDETDDDATE;
                        det.REQDETADDIP = item.REQDETADDIP;
                        det.REQDETCLOSEBY = USERNAME;
                        det.REQDETCLOSEDATE = item.REQDETCLOSEDATE;
                        det.REQDETCLOSEIP = item.REQDETCLOSEIP;
                        det.ACTION = "ADDDETAIL";
                        det.Insert();
                        i++;
                    }
                }
                else
                    BadRequest("Error al insertar la cabecera de la requisa");
                var response = new
                {
                    IdRequisaERP = proximoId,
                    Mensaje = "Requisa creada correctamente"
                };
                return Ok(response);


            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Actualiza los datos del Encabezado y Detalle de la Requisa en el ERP.
        /// </summary>
        /// <param name="datos">Datos de la Requisa (JSON)</param>
        /// <returns></returns>
        /// /// <response code="200">Ok. Requisa creada correctamente</response>
        /// <response code="400">Error. Devuelve la descripción del Problema</response>
        /// <response code="401">El usuario no está autorizado para realizar esta petición</response>
        [Route("update")]
        [HttpPost]
        [MethodGroup("Requisas")]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(RequisaExample))]
        public async Task<IHttpActionResult> update([FromBody] RequisaSiagri datos)
        {
            try
            {
                MyLogger.GetInstance().Info("Llamado al Requisa Controller. Update Requisa method, Datos Recibidos: " + JsonConvert.SerializeObject(datos));

                if (!ModelState.IsValid)
                    return BadRequest("Documento con datos no válidos");

                //VALIDA QUE CONTENGA UN NÚMERO DE ORDEN DE SERVICIO
                if (String.IsNullOrEmpty(datos.REQOT))
                    return BadRequest("Debe indicar un número de Orden de Servicio");

                //VALIDA QUE EL CONCEPTO NO VENGA VACÍO
                if (String.IsNullOrEmpty(datos.REQMEMO))
                    return BadRequest("Debe escribir un concepto para la requisa");

                //VALIDAMOS CADA ITEM QUE TRAE LA REQUISA.
                try
                {
                    int i = 1;
                    foreach (var item in datos.Detalle)
                    {
                        if (String.IsNullOrEmpty(item.REQDETIDPRODMCS))
                            return BadRequest("Linea Detalle " + i + " Codigo de producto no puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETPRODDESC))
                            return BadRequest("Linea Detalle " + i + " Descripcion de producto no puede ser vacio o nulo.");
                        if (item.REQDETSTOCKORD <= 0)
                            return BadRequest("Linea Detalle " + i + " Cantidad a Requisas no puede ser 0.");
                        if (item.REQDETCOSTTRAN <= 0)
                            return BadRequest("Linea Detalle " + i + " Costo de Producto no puede ser 0.");
                        if (String.IsNullOrEmpty(item.REQDETADDWHO))
                            return BadRequest("Linea Detalle " + i + " REQDETADDWHO puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETDDDATE.ToString()))
                            return BadRequest("Linea Detalle " + i + " REQDETDDDATE puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETADDIP))
                            return BadRequest("Linea Detalle " + i + " REQDETADDIP puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETCLOSEBY))
                            return BadRequest("Linea Detalle " + i + " REQDETADDWHO puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETCLOSEDATE.ToString()))
                            return BadRequest("Linea Detalle " + i + " REQDETDDDATE puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETCLOSEIP))
                            return BadRequest("Linea Detalle " + i + " REQDETADDIP puede ser vacio o nulo.");
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    MyLogger.GetInstance().Error("Error al Validar Detalle de Items de la Requisa: " + ex.Message);
                    return BadRequest("Error al Validar Detalle de Items de la Requisa: " + ex.StackTrace);
                }

                DataTable dt;

                int QEMPID = 0;
                String USERNAME = "";
                if (datos.REQEMPID != 0)
                {
                    dt = _db.ExecuteTable($"SELECT EYEMPID, EYUSERNAME FROM CM_CGP_ERP.IBPYEMPDT WHERE EYCOMPID = '{datos.REQCOMPANYID}' and EYIDSIAGRIEMP = '{datos.REQEMPID}';");
                    if (dt.Rows.Count > 0)
                        QEMPID = Int32.Parse(dt.Rows[0]["EYEMPID"].ToString());
                    dt.Clear();

                    dt = _db.ExecuteTable($"SELECT EYUSERNAME FROM CM_CGP_ERP.IBPYEMPDT WHERE EYCOMPID = '{datos.REQCOMPANYID}' and EYIDSIAGRIEMP = '{datos.REQEMPID}';");
                    if (dt.Rows.Count > 0)
                        USERNAME = dt.Rows[0]["EYUSERNAME"].ToString().ToUpper();
                    dt.Clear();

                }

                //INSERTAMOS EN EL ERP, EL JSON RECIBIDO.
                dt = _db.ExecuteTable($"INSERT INTO IBLOGAPIS (LOGAPINAME,LOGAPICALL,LOGDOCUMENTID,LOGDATAJSON,LOGAPIADDWHO,LOGAPIADDDATE,LOGAPIADDIP) VALUES ('REQUISA','UPDATE','{datos.REQUISAID}','{JsonConvert.SerializeObject(datos)}','{datos.REQADDWHO}',NOW(),'{datos.REQADDIP}');");
                dt.Clear();
                //REVISAMOS SI LA REQUISA YA ESTA EN ESTATUS CERRADA O RECHAZADA
                dt = _db.ExecuteTable($"SELECT REQUISAID FROM IBBODREQUISA WHERE REQCOMPANYID='{datos.REQCOMPANYID}' AND REQUISAID = '{datos.REQUISAID}' AND REQUISASTATUS IN ('C','R');");
                if (dt.Rows.Count > 0)
                    return BadRequest($"Error, la Requisa ID ERP # {datos.REQUISAID} ya esta en estatus Cerrada o Rechazado, no se puede actualizar;");
                dt.Clear();

                //VALIDAMOS POR CADA ITEM, QUE EXISTA EL ITEM Y TENGA EXISTENCIA
                // string CodBodega = datos.REQBODEGAID < 10 ? "00" + datos.REQBODEGAID.ToString() : "0" + datos.REQBODEGAID.ToString();
                string CodBodega = datos.REQBODEGAID.ToString();
                try
                {
                    foreach (var item in datos.Detalle)
                    {
                        dt = _db.ExecuteTable($"SELECT PRODCOD as CodProducto, PRODNAME as Descripcion FROM IBPRODUCTO WHERE PRODCOD = '{item.REQDETIDPRODMCS}'");
                        //dt = _dbsql.ExecuteTable($"SELECT CodProducto,Descripcion FROM dbInvent.dbo.tProductos WHERE CodProducto = '{item.REQDETIDPRODMCS}';");
                        if (dt.Rows.Count == 0)
                            return BadRequest($"El producto {item.REQDETIDPRODMCS} no existe");
                        dt.Clear();

                        dt = _db.ExecuteTable($"SELECT PRODCOD as CodProducto, PRODNAME as Descripcion, SAINVSTCKACT as EXISTENCIA, SAINVCOSTAVG as COSTO, UMNAME as UM, BODEGAID Bodega FROM IBSALDOINV INNER JOIN IBPRODUCTO ON SAINVIDPROD = PRODID and SAINVIDCOMPANY = PRODIDCOMP INNER JOIN IBUNITMESAURE ON UMID = PRODIDUNIT and  UMCOMPID = SAINVIDCOMPANY  INNER JOIN IBBODEGA ON BODEGAID = SAINVIDWARH and  SAINVIDCOMPANY = BODCOMPANYID WHERE SAINVIDCOMPANY = 3  and  BODEGAID =  '{CodBodega}' and SAINVCOSTAVG > 0 and PRODCOD = '{item.REQDETIDPRODMCS}';");
                        //dt = _dbsql.ExecuteTable($"SELECT PR.CodProducto AS CodProducto,PR.Descripcion AS Descripcion,PI.ExistenciaActual AS EXISTENCIA,PC.CostoPromedioMN AS COSTO,PR.UnidadMedida AS UM,PI.CodBodega AS Bodega FROM dbInvent.dbo.tProductos PR INNER JOIN dbInvent.dbo.tProductosCostos PC ON (PR.CodProducto = PC.CodProducto)  INNER JOIN dbInvent.dbo.tProductosInventario PI ON (PR.CodProducto = PI.CodProducto)  WHERE PI.CodBodega = '{CodBodega}' AND PI.ExistenciaActual >= 0 AND PC.ExistenciaActual>0 AND PC.CostoPromedioMN>0 AND PR.CodProducto = '{item.REQDETIDPRODMCS}';");
                        if (dt.Rows.Count == 0)
                            return BadRequest($"El producto {item.REQDETIDPRODMCS} no tiene inventario disponible en la bodega {datos.REQBODEGAID}");
                        dt.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MyLogger.GetInstance().Error("Error al Validar Detalle de Items de la Requisa: " + ex.Message);
                    return BadRequest("Error al Validar Detalle de Items de la Requisa: " + ex.Message);
                }

                //CONSULTA SI LA FINCA SELECCIONADA EXISTE EN ERP
                String claseID = "0";
                var consulta = $"SELECT ENTCLASSID FROM IBENTCLASS WHERE ENTCOMPID = '{datos.REQCOMPANYID}' AND SUBSTRING(ENTCODE,3,2) = '{datos.REQCLASSID}' AND IF(ENTCOMPID=3, ENTCLASSID NOT IN(67,68,69,70,71,72,61,62),ENTCLASSID>0) LIMIT 1;";
                dt = _db.ExecuteTable(consulta);
                if (dt != null && dt.Rows.Count > 0)
                    claseID = dt.Rows[0]["ENTCLASSID"].ToString();
                dt.Clear();

                //SI EXISTE RECUPERAMOS EL CENTRO DE COSTO ASIGNADO.
                String CentroCosto = "0.0.0.0.0";
                Decimal levelCC1 = 0;
                Decimal levelCC2 = 0;
                Decimal levelCC3 = 0;
                Decimal levelCC4 = 0;
                Decimal levelCC5 = 0;
                if (datos.REQACTIVITY.Trim().Length > 0)
                {
                    dt = _db.ExecuteTable($"SELECT COALESCE(ACLOTIDCC,'0.0.0.0.0') AS CentroCosto FROM IBACTIVITYLOTE WHERE ACLOTIDCOMPANY = '{datos.REQCOMPANYID}' AND ACLOTCODEACTIVIDAD='{datos.REQACTIVITY}';");
                    if (dt.Rows.Count > 0)
                        CentroCosto = dt.Rows[0]["CentroCosto"].ToString();
                    dt.Clear();
                }
                var centroCostoLevels = CentroCosto.Split('.');
                levelCC1 = decimal.Parse(centroCostoLevels[0]);
                levelCC2 = decimal.Parse(centroCostoLevels[1]);
                levelCC3 = decimal.Parse(centroCostoLevels[2]);
                levelCC4 = decimal.Parse(centroCostoLevels[3]);
                levelCC5 = decimal.Parse(centroCostoLevels[4]);

                //RECUPERAMOS EL CENTRO DE COSTO DEL MCS
                String centroCostoMCS = "0";
                if (datos.REQACTIVITY.Trim().Length > 0)
                {
                    dt = _db.ExecuteTable($"SELECT CASE WHEN (ccentid1>0 AND ccentid2 >0 AND ccentid3 >0 AND ccentid4 >0 AND ccentid5>0) THEN COALESCE(ccequivlv5,'') WHEN (ccentid1>0 AND ccentid2 >0 AND ccentid3 >0 AND ccentid4 >0 AND ccentid5=0) THEN COALESCE(ccequivlv4,'') WHEN (ccentid1>0 AND ccentid2 >0 AND ccentid3 >0 AND ccentid4 =0 AND ccentid5=0) THEN COALESCE(ccequivlv3,'') WHEN (ccentid1>0 AND ccentid2 >0 AND ccentid3 =0 AND ccentid4 =0 AND ccentid5=0) THEN COALESCE(ccequivlv2,'') WHEN (ccentid1>0 AND ccentid2 =0 AND ccentid3 =0 AND ccentid4 =0 AND ccentid5=0) THEN COALESCE(ccequivlv1,'') ELSE '' END AS CCEQUIVALENTE FROM ibwdcstcenter WHERE cccompid='{datos.REQCOMPANYID}' AND CONCAT(ccentid1,'.',ccentid2,'.',ccentid3,'.',ccentid4,'.',ccentid5)='{CentroCosto}';");
                    if (dt.Rows.Count > 0)
                        centroCostoMCS = dt.Rows[0]["CCEQUIVALENTE"].ToString();
                    dt.Clear();
                }

                //RECUPERAMOS NOMBRE DEL EMPLEADO ENTREGAR A:
                String entregarA = "";
                if (datos.REQEMPID > 0)
                {
                    dt = _db.ExecuteTable($"SELECT GETENTNAME('{datos.REQCOMPANYID}','E','{datos.REQEMPID}') AS EMPLEADO;");
                    if (dt.Rows.Count > 0)
                        entregarA = dt.Rows[0]["EMPLEADO"].ToString();
                    dt.Clear();
                }
                //VERIFICAMOS SI YA HAY DETALLE INSERTADO, PARA BORRARLO Y VOLVERLO A INSERTAR
                dt = _db.ExecuteTable($"SELECT COUNT(*) as totalLineas FROM IBREQUISADETAIL WHERE REQDETCOMPID='{datos.REQCOMPANYID}' AND REQDETID='{datos.REQUISAID}' ;");
                int totalLineas = 0;
                if (dt.Rows.Count > 0)
                    totalLineas = int.Parse(dt.Rows[0]["totalLineas"].ToString());
                dt.Clear();
                if (totalLineas > 0)
                    dt = _db.ExecuteTable($"DELETE FROM IBREQUISADETAIL WHERE REQDETCOMPID='{datos.REQCOMPANYID}' AND REQDETID='{datos.REQUISAID}' ;");
                dt.Clear();

                //CUENTAS CONTABLES
                int IdCatalogo = 0;
                String Level1 = "000";
                String Level2 = "000";
                String Level3 = "000";
                String Level4 = "000";
                String Level5 = "000";
                String Level6 = "000";

                if (datos.REQCOMPANYID == 2)
                {
                    //CUENTA CONTABLE DE COMPANIA DE PRUEBA: 92.02.001.007	Fertilizantes Y Agroquímicos Produccion en Proceso
                    //24675	007	000	000	000	000	000
                    IdCatalogo = 24675;
                    Level1 = "007";
                    Level2 = "000";
                    Level3 = "000";
                    Level4 = "000";
                    Level5 = "000";
                    Level6 = "000";
                }
                if (datos.REQCOMPANYID == 3)
                {
                    //CUENTA CONTABLE DE MONTELIMAR: 92.02.001.007	Fertilizantes Y Agroquímicos Produccion en Proceso
                    //1946	026	002	001	007	000	000
                    IdCatalogo = 1946;
                    Level1 = "026";
                    Level2 = "002";
                    Level3 = "001";
                    Level4 = "007";
                    Level5 = "000";
                    Level6 = "000";
                }
                
                //VERIFICAMOS QUE LA REQUISA AUN ESTE EN ESTATUS BORRADOR, PARA QUE SE PUEDA PROCEDER A ACTUALIZAR.
                dt = _db.ExecuteTable($"SELECT REQUISAID FROM IBBODREQUISA WHERE REQCOMPANYID='{datos.REQCOMPANYID}' AND REQUISAID='{datos.REQUISAID}' AND REQUISASTATUS='B' ;");
                int existeRequisa = 0;
                //VERIFICAMOS QUE LA REQUISA AUN ESTE EN ESTATUS BORRADO O CERRRAD0/DESPACHADO, PARA QUE SE PUEDA PROCEDER A ACTUALIZAR.
                dt = _db.ExecuteTable($"SELECT REQUISAID FROM IBBODREQUISA WHERE REQCOMPANYID='{datos.REQCOMPANYID}' AND REQUISAID='{datos.REQUISAID}' AND REQUISASTATUS IN('B','C') ;");
                if (dt.Rows.Count > 0)
                {
                    existeRequisa = Int32.Parse(dt.Rows[0]["REQUISAID"].ToString());
                    dt.Clear();
                }
                if (existeRequisa <= 0)
                {
                    MyLogger.GetInstance().Error($"RequestController. Update method, Error al actualizar los datos de la Requisa ID: {datos.REQUISAID}, el Estatus Actual de la Requisa no lo permite.");
                    return BadRequest($"Error al actualizar los datos de la Requisa ID: {datos.REQUISAID}, el Estatus Actual de la Requisa no lo permite.");
                }
                Requisas req = new Requisas();

                req.REQCOMPANYID = datos.REQCOMPANYID;
                req.REQUISAID = datos.REQUISAID;
                req.REQREFNUM = datos.REQOT.ToString();
                req.REQBODEGAID = datos.REQBODEGAID;
                req.REQEMPID = QEMPID;
                req.REQUISASTATUS = datos.REQUISASTATUS.ToString();
                req.REQGLID = IdCatalogo;
                req.REQGL1 = Level1;
                req.REQGL2 = Level2;
                req.REQGL3 = Level3;
                req.REQGL4 = Level4;
                req.REQGL5 = Level5;
                req.REQGL6 = Level6;
                req.REQCCID1 = levelCC1;
                req.REQCCID2 = levelCC2;
                req.REQCCID3 = levelCC3;
                req.REQCCID4 = levelCC4;
                req.REQCCID5 = levelCC5;              
                req.REQTOPMNGRID = datos.REQCOMPANYID == 3 ? 3 : 4;//ID GERENCIA
                req.REQCOMPSCTID = datos.REQCOMPANYID == 3 ? 9 : 4;//ID DEPARTAMENTO
                req.REQSECTIONID = 0; //DEPARTAMENTO DE MCS
                req.REQCLASSID = Int16.Parse(claseID); //FINCA
                req.REQLOT = datos.REQLOT;
                req.REQACTIVITY = datos.REQACTIVITY;
                req.REQOT = datos.REQOT.ToString();
                req.REQUISADATE = datos.REQUISADATE;
                req.REQUISAFORDATE = datos.REQFORDATE;
                req.REQMEMO = datos.REQMEMO.Substring(0, datos.REQMEMO.IndexOf("-")) + "-" + USERNAME;
                req.REQTRIDCURR = 1;
                req.REQCOIDCURR = 0;
                req.REQBSIDCURR = 0;
                req.REQEQUIPO = "";
                req.REQADDWHO = USERNAME;
                req.REQADDDATE = datos.REQADDDATE;
                req.REQADDIP = datos.REQADDIP;
                req.REQUPDWHO = USERNAME;
                req.REQUPDDATE = datos.REQUPDDATE;
                req.REQUPDIP = datos.REQUPDIP;
                req.REQAPROBY = USERNAME;
                req.REQAPRODATE = datos.REQAPRODATE;
                req.REQAPROIP = datos.REQAPROIP;
                req.REQCLOSEBY = USERNAME;
                req.REQCLOSEDATE = datos.REQCLOSEDATE;
                req.REQCLOSEIP = datos.REQCLOSEIP;
                req.ACTION = "UPDREQUISA";
                req.REQPROC = "SIA";

                if (req.Insert())
                {
                    DetalleReq det;
                    int i = 1;
                    //  double costo = 0;
                    foreach (var item in datos.Detalle)
                    {
                        //  costo = 0;

                        det = new DetalleReq();
                        det.REQDETCOMPID = datos.REQCOMPANYID; //compañía de pruebas momentáneo
                        det.REQDETID = datos.REQUISAID;
                        det.REQDETIDLIN = i;
                        det.REQDETIDPROD = 0;
                        det.REQDETIDPRODMCS = item.REQDETIDPRODMCS;
                        det.REQDETPRODDESC = item.REQDETPRODDESC;
                        det.REQDETUMMCS = item.REQDETUMMCS;
                        det.REQDETMEMO = item.REQDETMEMO;
                        det.REQDETSTOCKORD = item.REQDETSTOCKORD;
                        det.REQDETSTOCKGET = item.REQDETSTOCKORD;
                        det.REQDETCOSTTRAN = item.REQDETCOSTTRAN;
                        det.REQDETTRIDCURR = 1;
                        det.REQDETCOIDCURR = 0;
                        det.REQDETBSIDCURR = 0;
                        det.REQDETADDWHO = USERNAME;
                        det.REQDETDDDATE = item.REQDETDDDATE;
                        det.REQDETADDIP = item.REQDETADDIP;
                        det.REQDETCLOSEBY = USERNAME;
                        det.REQDETCLOSEDATE = item.REQDETCLOSEDATE;
                        det.REQDETCLOSEIP = item.REQDETCLOSEIP;
                        det.ACTION = "ADDDETAIL";
                        det.Insert();
                        i++;
                    }
                }
                else
                    BadRequest("Error al Actualizar la cabecera de la requisa");
                var response = new
                {                  
                    Mensaje = "Requisa Actualizada correctamente"
                };
                return Ok(response);

               
            }
            catch (Exception e)
            {
                MyLogger.GetInstance().Info("RequestController. Update method, Error: " + e.Message);
                return BadRequest(e.Message.ToString());
            }
        }

        /// <summary>
        /// Actualiza los datos del Encabezado y Detalle de la Requisa en el ERP.
        /// </summary>
        /// <param name="datos">Datos de la Requisa (JSON)</param>
        /// <returns></returns>
        /// /// <response code="200">Ok. Requisa creada correctamente</response>
        /// <response code="400">Error. Devuelve la descripción del Problema</response>
        /// <response code="401">El usuario no está autorizado para realizar esta petición</response>
        [Route("anular")]
        [HttpPost]
        [MethodGroup("Requisas")]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(RequisaExample))]
        public IHttpActionResult anular([FromBody] RequisaSiagri datos)
        {
            try
            {
                MyLogger.GetInstance().Info("Llamado al Requisa Controller. Anular Requisa method, Datos Recibidos: " + JsonConvert.SerializeObject(datos));
                if (!ModelState.IsValid)
                    return BadRequest("Documento con datos no válidos");

                //VALIDA QUE CONTENGA UN NÚMERO DE ORDEN DE SERVICIO
                if (String.IsNullOrEmpty(datos.REQOT))
                    return BadRequest("Debe indicar un número de Orden de Servicio");

                //VALIDA QUE EL CONCEPTO NO VENGA VACÍO
                if (String.IsNullOrEmpty(datos.REQMEMO))
                    return BadRequest("Debe escribir un concepto para la requisa");

                //VALIDAMOS CADA ITEM QUE TRAE LA REQUISA.
                try
                {
                    int i = 1;
                    foreach (var item in datos.Detalle)
                    {
                        if (String.IsNullOrEmpty(item.REQDETIDPRODMCS))
                            return BadRequest("Linea Detalle " + i + " Codigo de producto no puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETPRODDESC))
                            return BadRequest("Linea Detalle " + i + " Descripcion de producto no puede ser vacio o nulo.");
                        if (item.REQDETSTOCKORD <= 0)
                            return BadRequest("Linea Detalle " + i + " Cantidad a Requisas no puede ser 0.");
                        if (item.REQDETCOSTTRAN <= 0)
                            return BadRequest("Linea Detalle " + i + " Costo de Producto no puede ser 0.");
                        if (String.IsNullOrEmpty(item.REQDETADDWHO))
                            return BadRequest("Linea Detalle " + i + " REQDETADDWHO puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETDDDATE.ToString()))
                            return BadRequest("Linea Detalle " + i + " REQDETDDDATE puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETADDIP))
                            return BadRequest("Linea Detalle " + i + " REQDETADDIP puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETCLOSEBY))
                            return BadRequest("Linea Detalle " + i + " REQDETADDWHO puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETCLOSEDATE.ToString()))
                            return BadRequest("Linea Detalle " + i + " REQDETDDDATE puede ser vacio o nulo.");
                        if (String.IsNullOrEmpty(item.REQDETCLOSEIP))
                            return BadRequest("Linea Detalle " + i + " REQDETADDIP puede ser vacio o nulo.");
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    MyLogger.GetInstance().Error("Error al Validar Detalle de Items de la Requisa: " + ex.Message);
                    return BadRequest("Error al Validar Detalle de Items de la Requisa: " + ex.Message);
                }

                DataTable dt;



                //INSERTAMOS EN EL ERP, EL JSON RECIBIDO.
                dt = _db.ExecuteTable($"INSERT INTO IBLOGAPIS (LOGAPINAME,LOGAPICALL,LOGDOCUMENTID,LOGDATAJSON,LOGAPIADDWHO,LOGAPIADDDATE,LOGAPIADDIP) VALUES ('REQUISA','ANULAR','{datos.REQUISAID}','{JsonConvert.SerializeObject(datos)}','{datos.REQADDWHO}',NOW(),'{datos.REQADDIP}');");
                dt.Clear();

                //REVISAMOS SI LA REQUISA YA ESTA EN ESTATUS CERRADA O RECHAZADA
                dt = _db.ExecuteTable($"SELECT REQUISAID FROM IBBODREQUISA WHERE REQCOMPANYID='{datos.REQCOMPANYID}' AND REQUISAID = '{datos.REQUISAID}' AND REQUISASTATUS IN ('R');");
                if (dt.Rows.Count > 0)
                    return BadRequest($"Error, la Requisa ID ERP # {datos.REQUISAID} ya esta en estatus Rechazado, no se puede Rechazar;");
                dt.Clear();
                //VALIDAMOS POR CADA ITEM, QUE EXISTA EL ITEM Y TENGA EXISTENCIA
                string CodBodega = datos.REQBODEGAID < 10 ? "00" + datos.REQBODEGAID.ToString() : "0" + datos.REQBODEGAID.ToString();
            

                //CONSULTA SI LA FINCA SELECCIONADA EXISTE EN ERP
                String claseID = "0";
                var consulta = $"SELECT ENTCLASSID FROM IBENTCLASS WHERE ENTCOMPID = '{datos.REQCOMPANYID}' AND SUBSTRING(ENTCODE,3,2) = '{datos.REQCLASSID}' AND IF(ENTCOMPID=3, ENTCLASSID NOT IN(67,68,69,70,71,72,61,62),ENTCLASSID>0) LIMIT 1;";
                dt = _db.ExecuteTable(consulta);
                if (dt != null && dt.Rows.Count > 0)
                    claseID = dt.Rows[0]["ENTCLASSID"].ToString();
                dt.Clear();
                //return BadRequest($"El código de Finca: {datos.Finca}, no existe en ERP");

                //CUENTAS CONTABLES
                int IdCatalogo = 0;
                String Level1 = "000";
                String Level2 = "000";
                String Level3 = "000";
                String Level4 = "000";
                String Level5 = "000";
                String Level6 = "000";

                if (datos.REQCOMPANYID == 2)
                {
                    //CUENTA CONTABLE DE COMPANIA DE PRUEBA: 92.02.001.007	Fertilizantes Y Agroquímicos Produccion en Proceso
                    //24675	007	000	000	000	000	000
                    IdCatalogo = 24675;
                    Level1 = "007";
                    Level2 = "000";
                    Level3 = "000";
                    Level4 = "000";
                    Level5 = "000";
                    Level6 = "000";
                }
                if (datos.REQCOMPANYID == 3)
                {
                    //CUENTA CONTABLE DE MONTELIMAR: 92.02.001.007	Fertilizantes Y Agroquímicos Produccion en Proceso
                    //1946	026	002	001	007	000	000
                    IdCatalogo = 1946;
                    Level1 = "026";
                    Level2 = "002";
                    Level3 = "001";
                    Level4 = "007";
                    Level5 = "000";
                    Level6 = "000";
                }

                //SI EXISTE RECUPERAMOS EL CENTRO DE COSTO ASIGNADO.
                String CentroCosto = "0.0.0.0.0";
                Decimal levelCC1 = 0;
                Decimal levelCC2 = 0;
                Decimal levelCC3 = 0;
                Decimal levelCC4 = 0;
                Decimal levelCC5 = 0;


                if (datos.REQACTIVITY.Trim().Length > 0)
                {
                    dt = _db.ExecuteTable($"SELECT COALESCE(ACLOTIDCC,'0.0.0.0.0') AS CentroCosto FROM IBACTIVITYLOTE WHERE ACLOTIDCOMPANY = '{datos.REQCOMPANYID}' AND ACLOTCODEACTIVIDAD='{datos.REQACTIVITY}';");
                    if (dt.Rows.Count > 0)
                        CentroCosto = dt.Rows[0]["CentroCosto"].ToString();
                    dt.Clear();
                }
                var centroCostoLevels = CentroCosto.Split('.');
                levelCC1 = decimal.Parse(centroCostoLevels[0]);
                levelCC2 = decimal.Parse(centroCostoLevels[1]);
                levelCC3 = decimal.Parse(centroCostoLevels[2]);
                levelCC4 = decimal.Parse(centroCostoLevels[3]);
                levelCC5 = decimal.Parse(centroCostoLevels[4]);

                dt = _db.ExecuteTable($"SELECT COUNT(*) as totalLineas FROM IBREQUISADETAIL WHERE REQDETCOMPID='{datos.REQCOMPANYID}' AND REQDETID='{datos.REQUISAID}' ;");
                int totalLineas = 0;
                if (dt.Rows.Count > 0)
                    totalLineas = int.Parse(dt.Rows[0]["totalLineas"].ToString());
                if (totalLineas > 0)
                    dt = _db.ExecuteTable($"DELETE FROM IBREQUISADETAIL WHERE REQDETCOMPID='{datos.REQCOMPANYID}' AND REQDETID='{datos.REQUISAID}' ;");

                //RECUPERAMOS EL ID DE LA REQUISA DEL MCS, PARA CAMBIARLE EL ESTATUS A LA REQUISA CREADA EN EL MCS.
                int idRequisaMCS = 0;
                dt = _db.ExecuteTable($"SELECT REQIDMCS FROM IBBODREQUISA WHERE REQCOMPANYID='{datos.REQCOMPANYID}' AND REQUISAID='{datos.REQUISAID}' AND REQUISASTATUS IN('B','C') ;");
                if (dt.Rows.Count > 0)
                    idRequisaMCS = int.Parse(dt.Rows[0]["REQIDMCS"].ToString());
                dt.Clear();

                int existeRequisa = 0;
                //VERIFICAMOS QUE LA REQUISA AUN ESTE EN ESTATUS BORRADO O CERRRAD0/DESPACHADO, PARA QUE SE PUEDA PROCEDER A ACTUALIZAR.
                dt = _db.ExecuteTable($"SELECT REQUISAID FROM IBBODREQUISA WHERE REQCOMPANYID='{datos.REQCOMPANYID}' AND REQUISAID='{datos.REQUISAID}' AND REQUISASTATUS IN('B','C') ;");
                if (dt.Rows.Count > 0)
                {
                    existeRequisa = Int32.Parse(dt.Rows[0]["REQUISAID"].ToString());
                    dt.Clear();
                }
                if (existeRequisa <= 0)
                {
                    MyLogger.GetInstance().Error($"RequestController. Update method, Error al actualizar los datos de la Requisa ID: {datos.REQUISAID}, el Estatus Actual de la Requisa no lo permite.");
                    return BadRequest($"Error al actualizar los datos de la Requisa ID: {datos.REQUISAID}, el Estatus Actual de la Requisa no lo permite.");
                }
                //RECUPERAMOS EL ESTATUS ACTUAL DE LA REQUISA, PARA SABER SI FUE DESPACHADA, Y MANDAR A APLICAR UN AJUSTE AL MCS.
                dt = _db.ExecuteTable($"SELECT REQUISASTATUS FROM IBBODREQUISA WHERE REQCOMPANYID='{datos.REQCOMPANYID}' AND REQUISAID='{datos.REQUISAID}';");
                String estatusRequisa = "";
                if (dt.Rows.Count > 0)
                    estatusRequisa = dt.Rows[0]["REQUISASTATUS"].ToString();
                dt.Clear();

                int QEMPID = 0;
                String USERNAME = "";
                if (datos.REQEMPID != 0)
                {
                    dt = _db.ExecuteTable($"SELECT EYEMPID, EYUSERNAME FROM CM_CGP_ERP.IBPYEMPDT WHERE EYCOMPID = '{datos.REQCOMPANYID}' and EYIDSIAGRIEMP = '{datos.REQEMPID}';");
                    if (dt.Rows.Count > 0)
                        QEMPID = Int32.Parse(dt.Rows[0]["EYEMPID"].ToString());
                    dt.Clear();

                    dt = _db.ExecuteTable($"SELECT EYUSERNAME FROM CM_CGP_ERP.IBPYEMPDT WHERE EYCOMPID = '{datos.REQCOMPANYID}' and EYIDSIAGRIEMP = '{datos.REQEMPID}';");
                    if (dt.Rows.Count > 0)
                        USERNAME = dt.Rows[0]["EYUSERNAME"].ToString().ToUpper();
                    dt.Clear();

                }
                Requisas req = new Requisas();

                req.REQCOMPANYID = datos.REQCOMPANYID; //PROVISIONAL PARA PRUEBA
                req.REQUISAID = datos.REQUISAID;
                req.REQREFNUM = datos.REQOT.ToString(); //Guardamos el idRequisa (Siagri) en NUMREF
                req.REQBODEGAID = datos.REQBODEGAID;
                req.REQEMPID = QEMPID;
                req.REQUISASTATUS = "R";
                req.REQGLID = IdCatalogo;
                req.REQGL1 = Level1;
                req.REQGL2 = Level2;
                req.REQGL3 = Level3;
                req.REQGL4 = Level4;
                req.REQGL5 = Level5;
                req.REQGL6 = Level6;
                req.REQCCID1 = levelCC1;
                req.REQCCID2 = levelCC2;
                req.REQCCID3 = levelCC3;
                req.REQCCID4 = levelCC4;
                req.REQCCID5 = levelCC5;              
                req.REQTOPMNGRID = datos.REQCOMPANYID == 3 ? 3 : 4;//ID GERENCIA
                req.REQCOMPSCTID = datos.REQCOMPANYID == 3 ? 9 : 4;//ID DEPARTAMENTO
                req.REQSECTIONID = 0; //DEPARTAMENTO DE MCS
                req.REQCLASSID = Int16.Parse(claseID); //FINCA
                req.REQLOT = datos.REQLOT;
                req.REQACTIVITY = datos.REQACTIVITY;
                req.REQOT = datos.REQOT.ToString();
                req.REQUISADATE = datos.REQUISADATE;
                req.REQUISAFORDATE = datos.REQFORDATE;
                req.REQMEMO = datos.REQMEMO.Substring(0, datos.REQMEMO.IndexOf("-")) + "-" + USERNAME;
                req.REQTRIDCURR = 1;
                req.REQCOIDCURR = 0;
                req.REQBSIDCURR = 0;
                req.REQADDWHO = USERNAME;
                req.REQADDDATE = datos.REQADDDATE;
                req.REQADDIP = datos.REQADDIP;
                req.REQUPDWHO = USERNAME;
                req.REQUPDDATE = datos.REQUPDDATE;
                req.REQUPDIP = datos.REQUPDIP;
                req.REQAPROBY = USERNAME;
                req.REQAPRODATE = datos.REQAPRODATE;
                req.REQAPROIP = datos.REQAPROIP;
                req.REQCLOSEBY = USERNAME;
                req.REQCLOSEDATE = datos.REQCLOSEDATE;
                req.REQCLOSEIP = datos.REQCLOSEIP;
                req.ACTION = "UPDREQUISA";
                req.REQPROC = "SIA";
                req.Insert();

                return Ok($"Requisa Anulada Correctamente.");             
            }
            catch (Exception e)
            {
                return BadRequest("Error al Anular/Rechazar Requisa: " + e.Message.ToString());
            }
        }

    }
}
