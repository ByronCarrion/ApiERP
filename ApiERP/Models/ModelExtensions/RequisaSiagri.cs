using System;
using System.Collections.Generic;

namespace ApiERP.Models.ModelExtensions
{
    public class RequisaSiagri
    {
        //PARAMETROS A RECIBIR DE SIAGRI, PARA EL ENCABEZADO.
        //1.ID DE REQUISA GENERADO POR SIAGRI.
        //2.ID BODEGA
        //3.ID EMPLEADO QUE REQUISA.
        //4.ID CLASE Ó FINCA
        //5.ID Ó CODIGO DE LOTE
        //6.GRUPO Ó ACTIVIDAD
        //7.ESTATUS C:Despachada.
        //8.FECHA DE REQUISA.
        //9.FECHA NECESITADO PARA.
        //10. DESCRIPCION Ó COMENTARIOS.
        //11.REQUISADO POR USUARIO:
        //12.REQUISADO POR IP:
        //13.APROBADO POR USUARIO:
        //14.APROBADO POR IP:
        //13.APROBADO POR USUARIO:
        //14.APROBADO POR IP:

        public int REQCOMPANYID { get; set; }
        public int REQUISAID { get; set; }
        public int REQBODEGAID { get; set; }
        public int REQEMPID { get; set; }
        public int REQTOPMNGRID { get; set; }
        //Obtener Id Gerencia del Empleado que requisa
        public int REQCOMPSCTID { get; set; }
        //Obtener Id Departamento del Empleado que requisa
        public int REQCLASSID { get; set; }
        // ID FINCA
        public string REQLOT { get; set; }
        public string REQACTIVITY { get; set; }
        public string REQOT { get; set; }
        public string REQUISASTATUS { get; set; }
        public DateTime REQUISADATE { get; set; }
        public DateTime REQFORDATE { get; set; }
        public string REQMEMO { get; set; }
        public string REQADDWHO { get; set; }
        public DateTime REQADDDATE { get; set; }
        public string REQADDIP { get; set; }
        public string REQUPDWHO { get; set; }
        public DateTime REQUPDDATE { get; set; }
        public string REQUPDIP { get; set; }
        public string REQAPROBY { get; set; }
        public DateTime REQAPRODATE { get; set; }
        public string REQAPROIP { get; set; }
        public string REQCLOSEBY { get; set; }
        public DateTime REQCLOSEDATE { get; set; }
        public string REQCLOSEIP { get; set; }
        public List<RequisaSiagriDetalle> Detalle { get; set; }

    }

    //PARAMETROS A RECIBIR DE SIAGRI, PARA EL DETALLE.
    //1.ID DE REQUISA GENERADO POR SIAGRI.
    //2.ID-CODIGO DE PRODUCTO
    //3.NOMBRE DEL PRODUCTO.
    //4.UNIDAD DE MEDIDA.
    //5.CANTIDAD REQUISADO
    //6.CANTIDAD DESPACHADA
    //7.COSTO POR UNIDAD.
    public class RequisaSiagriDetalle
    {
        public int REQDETID { get; set; }
        public string REQDETIDPRODMCS { get; set; }
        public string REQDETPRODDESC { get; set; }
        public string REQDETMEMO { get; set; }
        public string REQDETUMMCS { get; set; }
        public double REQDETSTOCKORD { get; set; }
        public double REQDETSTOCKGET { get; set; }
        public double REQDETSTOCKPEND { get; set; }
        public double REQDETCOSTTRAN { get; set; }
        public string REQDETADDWHO { get; set; }
        public DateTime REQDETDDDATE { get; set; }
        public string REQDETADDIP { get; set; }
        public string REQDETCLOSEBY { get; set; }
        public DateTime REQDETCLOSEDATE { get; set; }
        public string REQDETCLOSEIP { get; set; }
    }
}