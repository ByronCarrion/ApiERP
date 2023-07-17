using ApiERP.Services;
using ApiERP.Services.Utility;
using MySql.Data.MySqlClient;
//using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;

namespace ApiERP.Models.ModelExtensions
{
    public class Requisas
    {
        public int REQCOMPANYID { get; set; }
        public int REQUISAID { get; set; }
        public int REQBODEGAID { get; set; }
        public int REQEMPID { get; set; }
        public int REQGLID { get; set; }
        public string REQGL1 { get; set; }
        public string REQGL2 { get; set; }
        public string REQGL3 { get; set; }
        public string REQGL4 { get; set; }
        public string REQGL5 { get; set; }
        public string REQGL6 { get; set; }
        public string REQCCTYPEENT { get; set; }
        public int REQCCTYPEID { get; set; }
        public Decimal REQCCID1 { get; set; }
        public Decimal REQCCID2 { get; set; }
        public Decimal REQCCID3 { get; set; }
        public Decimal REQCCID4 { get; set; }
        public Decimal REQCCID5 { get; set; }
        public int REQTOPMNGRID { get; set; }
        public int REQCOMPSCTID { get; set; }
        public int REQSECTIONID { get; set; }
        public int REQCLASSID { get; set; }
        public string REQLOT { get; set; }
        public string REQEQUIPO { get; set; }
        public string REQACTIVITY { get; set; }
        public string REQOT { get; set; }
        public string REQUISASTATUS { get; set; }
        public DateTime REQUISADATE { get; set; }
        public DateTime REQUISAFORDATE { get; set; }
        public string REQREFNUM { get; set; }
        public string REQMEMO { get; set; }
        public int REQTRIDCURR { get; set; }
        public int REQCOIDCURR { get; set; }
        public int REQBSIDCURR { get; set; }
        public decimal REQTRAMOUNT { get; set; }
        public decimal REQCOAMOUNT { get; set; }
        public decimal REQBSAMOUNT { get; set; }
        public int? REQIDMCS { get; set; }
        public decimal REQEXRATECO { get; set; }
        public decimal REQEXRATEBS { get; set; }
        public string REQCLAVEEQUIPO { get; set; }
        public string REQPROC { get; set; }
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
        public string ACTION { get; set; }

        public List<DetalleReq> REQDETALLE { get; set; }

        public bool Insert()
        {
            ConexionMySQL con = new ConexionMySQL();
            string SpName;
            MySqlParameter[] SpParameters = new MySqlParameter[53];
            bool Answer;
            int i = 0;
            try
            {
                SpName = "BODREQUISA_SIAGRI";
                SpParameters[i] = new MySqlParameter("@p_REQCOMPANYID", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQCOMPANYID;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQUISAID", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQUISAID;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQBODEGAID", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQBODEGAID;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQRECPEPID", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQEMPID", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQEMPID;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQGLID", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQGLID;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQGL1", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQGL1;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQGL2", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQGL2;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQGL3", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQGL3;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQGL4", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQGL4;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQGL5", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQGL5;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQGL6", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQGL6;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQCCTYPEENT", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = "L";
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQCCTYPEID", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 1;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQCCENTID", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQCCTYPEID;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQCCID1", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQCCID1;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQCCID2", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQCCID2;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQCCID3", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQCCID3;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQCCID4", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQCCID4;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQCCID5", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQCCID5;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQTOPMNGRID", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQTOPMNGRID;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQCOMPSCTID", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQCOMPSCTID;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQSECTIONID", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQSECTIONID;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQCLASSID", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQCLASSID;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQLOT", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQLOT;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQEQUIPO", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQEQUIPO;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQACTIVITY", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQACTIVITY;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQOT", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQOT;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQUISASTATUS", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQUISASTATUS;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQUISADATE", MySqlDbType.Date);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQUISADATE;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQFORDATE", MySqlDbType.Date);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQUISAFORDATE;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQREFNUM", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQREFNUM;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQMEMO", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQMEMO;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQTRIDCURR", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQCOIDCURR", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQBSIDCURR", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQTRAMOUNT", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQCOAMOUNT", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQBSAMOUNT", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQIDMCS", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQEXRATECO", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQEXRATEBS", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQCLAVEEQUIPO", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQEQUIPO;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQPROC", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = "SIA";
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQADDWHO", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQADDWHO;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQADDIP", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQADDIP;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQUPDWHO", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQUPDWHO;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQUPDIP", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQUPDIP;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQAPROBY", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQAPROBY;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQAPROIP", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQAPROIP;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQCLOSEBY", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQCLOSEBY;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQCLOSEIP", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQCLOSEIP;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_action", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = ACTION;

                Answer = con.ExecuteSP(SpName, ref SpParameters);
                return Answer;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
               
                return false;

            }
        }
    }

    public class DetalleReq
    {
        public int REQDETCOMPID { get; set; }
        public int REQDETID { get; set; }
        public int REQDETIDLIN { get; set; }
        public int REQDETIDPROD { get; set; }
        public string REQDETIDPRODMCS { get; set; }
        public string REQDETUMMCS { get; set; }
        public string REQDETMEMO { get; set; }
        public double REQDETSTOCKORD { get; set; }
        public double REQDETSTOCKGET { get; set; }
        public int REQDETTRIDCURR { get; set; }
        public int REQDETCOIDCURR { get; set; }
        public int REQDETBSIDCURR { get; set; }
        public double REQDETCOSTTRAN { get; set; }
        public double REQDETCOSTCOMP { get; set; }
        public double REQDETCOSTBASE { get; set; }
        public double REQDETTRAMOUNT { get; set; }
        public double REQDETCOAMOUNT { get; set; }
        public double REQDETBSAMOUNT { get; set; }
        public double REQDETEXRATECO { get; set; }
        public double REQDETEXRATEBS { get; set; }
        public string REQDETPRODDESC { get; set; }
        public string REQDETUBICACION { get; set; }
        public string REQIDDETMCS { get; set; }
        public string REQDETADDWHO { get; set; }
        public DateTime REQDETDDDATE { get; set; }
        public string REQDETADDIP { get; set; }
        public string REQDETCLOSEBY { get; set; }
        public DateTime REQDETCLOSEDATE { get; set; }
        public string REQDETCLOSEIP { get; set; }
        public string ACTION { get; set; }

        public bool Insert()
        {
            ConexionMySQL con = new ConexionMySQL();
            string SpName;
            MySqlParameter[] SpParameters = new MySqlParameter[30];
            bool Answer;
            int i = 0;
            try
            {
                SpName = "BODREQDET_SIAGRI";
                SpParameters[i] = new MySqlParameter("@p_REQDETCOMPID", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETCOMPID;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETID", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETID;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETIDLIN", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETIDLIN;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETIDPROD", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETIDPROD;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@P_REQDETIDPRODMCS", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETIDPRODMCS;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@P_REQDETUMMCS", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETUMMCS;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETMEMO", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETMEMO;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETSTOCKORD", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETSTOCKORD;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETSTOCKGET", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETSTOCKGET;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETTRIDCURR", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETTRIDCURR;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETCOIDCURR", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETCOIDCURR;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETBSIDCURR", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETBSIDCURR;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETCOSTTRAN", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETCOSTTRAN;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETCOSTCOMP", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETCOSTBASE", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETTRAMOUNT", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETCOAMOUNT", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETBSAMOUNT", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETEXRATECO", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETEXRATEBS", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETPRODDESC", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETPRODDESC;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETUBICACION", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = "";
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQIDDETMCS", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETADDWHO", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETADDWHO;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETDDDATE", MySqlDbType.Timestamp);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETDDDATE;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETADDIP", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETADDIP;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETCLOSEBY", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETCLOSEBY;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETCLOSEDATE", MySqlDbType.Timestamp);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETCLOSEDATE;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_REQDETCLOSEIP", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = REQDETCLOSEIP;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_action", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = ACTION;

                MyLogger.GetInstance().Info("Requisa Clase. SP a Ejecutar: " + SpParameters.ToString());
                Answer = con.ExecuteSP(SpName, ref SpParameters);
                MyLogger.GetInstance().Info("Requisa Clase. Repuesta Recibida: " + Answer);
                return Answer;
            }
            catch (Exception ex)
            {
                MyLogger.GetInstance().Error("Requisa Clase. Error al Actualizar Detalle de Requisa: " + ex.Message);
                return false;
            }
        }


    }

    /// text  
    public class RequisaSiagriAnulacionToMCS
    {
        //INSERT INTO [dbo].[tAjustes]
        public string CodProducto { get; set; }
        public string CodBodega { get; set; }
        public double Cantidad { get; set; }
        public string Notas { get; set; }
        public string Estado { get; set; }
        public string AgregadoPor { get; set; }
        public DateTime AgregadoEl { get; set; }
        public string AutorizadoPor { get; set; }
        public DateTime AutorizadoEl { get; set; }
        public string AutorizadoNotas { get; set; }
        public double ProCostoUnitario { get; set; }

        public bool InsertAjuste()
        {
            ConexionSQLServer con = new ConexionSQLServer();
            DataTable dt;
            int lastIdAjuste = 0;
            int proximoIdAjuste = 0;
            try
            {
                //VERIFICAMOS EL ULTIMO ID DE AJUSTE INSERTADO
                dt = con.ExecuteTable($"SELECT TOP (1) tAjusteId FROM dbo.tAjustes ORDER BY tAjusteId DESC;");
                if (dt.Rows.Count > 0)
                    lastIdAjuste = int.Parse(dt.Rows[0]["tAjusteId"].ToString());
                dt.Clear();
                //INSERTAMOS EL REGISTRO
                dt = con.ExecuteTable($"INSERT INTO dbo.tAjustes (CodProducto,CodBodega,Cantidad,Notas,Estado,AgregadoPor,AutorizadoPor,AutorizadoEl,AutorizadoNotas,ProCostoUnitario) " +
                    $"VALUES('{CodProducto}','{CodBodega}','{Cantidad}','{Notas}','{Estado}','{AgregadoPor}','{AutorizadoPor}',getdate(),'{AutorizadoNotas}','{ProCostoUnitario}');");
                dt.Clear();
                //VERIFICAMOS EL ULTIMO ID DE AJUSTE INSERTADO
                dt = con.ExecuteTable($"SELECT TOP (1) tAjusteId FROM dbo.tAjustes ORDER BY tAjusteId DESC;");
                if (dt.Rows.Count > 0)
                    proximoIdAjuste = int.Parse(dt.Rows[0]["tAjusteId"].ToString());
                dt.Clear();
                //VERIFICAMOS SI REALMENTE SE INSERTO EL AJUSTE.
                if (proximoIdAjuste > lastIdAjuste)
                {
                    dt = con.ExecuteTable($"UPDATE dbo.tAjustes SET Estado='1' WHERE tAjusteId='{proximoIdAjuste}' AND Estado='0';");
                    dt.Clear();
                }
                if (proximoIdAjuste > lastIdAjuste)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MyLogger.GetInstance().Info("Requisa class. RequisaSiagriAnulacionToMCS method, InsertAjuste: " + ex.Message);
                return false;
            }

        }
    }

    ///
    public class RequisaInsertHeadToMCS
    {
        public DateTime Fecha { get; set; }
        public string CodBodega { get; set; }
        public string Uso { get; set; }
        public string Departamento { get; set; }
        public string EntregarA { get; set; }
        public string CodSeccion { get; set; }
        public string CtaContable { get; set; }
        public string CentroCosto { get; set; }
        public string iActivityOT { get; set; }
        public string Finca { get; set; }
        public string Lote { get; set; }
        public string Equipo { get; set; }
        public string Actividad { get; set; }
        public string InternalCode { get; set; }
        public string Estado { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEl { get; set; }

        ///  
        public bool InsertaRequisaMCS()
        {
            try
            {
                ConexionSQLServer con = new ConexionSQLServer();
                DataTable dt;
                int lastIdRequisa = 0;
                int proximoIdRequisa = 0;
                //VERIFICAMOS EL ULTIMO ID DE REQUISA INSERTADO
                dt = con.ExecuteTable($"SELECT TOP (1) IdRequisa FROM dbo.tRequisas ORDER BY IdRequisa DESC;");
                if (dt.Rows.Count > 0)
                    lastIdRequisa = int.Parse(dt.Rows[0]["IdRequisa"].ToString());
                dt.Clear();
                //INSERTAMOS LA REQUISA
                //"INSERT INTO dbo.tRequisas(Fecha,CodBodega,Uso,Departamento,EntregarA,CodSeccion,CtaContable,CentroCosto,iActivityOT,Finca,Lote,Equipo,Actividad,InternalCode,Estado,CreadoPor,CreadoEl) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,CURRENT_TIMESTAMP)";
                dt = con.ExecuteTable($"INSERT INTO dbo.tRequisas (Fecha,CodBodega,Uso,Departamento,EntregarA,CodSeccion,CtaContable,CentroCosto,iActivityOT,Finca,Lote,Equipo,Actividad,InternalCode,Estado,CreadoPor,CreadoEl) VALUES(CONVERT(DATE ,SUBSTRING('{Fecha.Date}', 1, 10), 105),'{CodBodega}','{Uso}','{Departamento}','{EntregarA}','{CodSeccion}','{CtaContable}','{CentroCosto}','{iActivityOT}','{Finca}','{Lote}','{Equipo}','{Actividad}','{InternalCode}','{Estado}','{CreadoPor}',getdate());");
                dt.Clear();
                //VERIFICAMOS EL ULTIMO ID DE REQUISA INSERTADO
                dt = con.ExecuteTable($"SELECT TOP (1) IdRequisa FROM dbo.tRequisas ORDER BY IdRequisa DESC;");
                if (dt.Rows.Count > 0)
                    proximoIdRequisa = int.Parse(dt.Rows[0]["IdRequisa"].ToString());
                dt.Clear();
                //VERIFICAMOS SI REALMENTE SE INSERTO LA REQUISA.
                if (proximoIdRequisa > lastIdRequisa)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MyLogger.GetInstance().Info("Requisa class. RequisaInsertHeadToMCS method, InsertaRequisaMCS: " + ex.Message);
                return false;
            }
        }
    }

    /// text  
    public class RequisaInsertDetailToMCS
    {
        public int IdRequisa { get; set; }
        public string Producto { get; set; }
        public double CantSolicitada { get; set; }
        public double CantDespachada { get; set; }
        public string Ubicacion { get; set; }
        public string Estado { get; set; }
        public string ProDescripcion { get; set; }
        public string ProUM { get; set; }
        public double ProCostoUnitario { get; set; }

        public bool InsertaRequisaDetailMCS()
        {
            try
            {
                ConexionSQLServer con = new ConexionSQLServer();
                DataTable dt;
                int lastIdRequisaDetail = 0;
                int proximoIdRequisaDetail = 0;
                //VERIFICAMOS EL ULTIMO ID DE REQUISA DETALLE INSERTADO
                dt = con.ExecuteTable($"SELECT TOP (1) IdRequisaDetail FROM dbo.tRequisaDetail WHERE IdRequisa='{IdRequisa}' ORDER BY IdRequisaDetail DESC;");
                if (dt.Rows.Count > 0)
                    lastIdRequisaDetail = int.Parse(dt.Rows[0]["IdRequisaDetail"].ToString());
                dt.Clear();
                //INSERTAMOS LA REQUISA DETALLE
                //INSERT INTO dbo.tRequisaDetail(IdRequisa,Producto,CantSolicitada,CantDespachada,Ubicacion,Estado,ProDescripcion,ProUM,ProCostoUnitario) VALUES(?,?,?,?,?,?,?,?,?)
                dt = con.ExecuteTable($"INSERT INTO dbo.tRequisaDetail (IdRequisa,Producto,CantSolicitada,CantDespachada,Ubicacion,Estado,ProDescripcion,ProUM,ProCostoUnitario) " +
                    $"VALUES ('{IdRequisa}','{Producto}','{CantSolicitada}','{CantDespachada}','{Ubicacion}','{Estado}','{ProDescripcion}','{ProUM}','{ProCostoUnitario}');");
                dt.Clear();
                //VERIFICAMOS EL ULTIMO ID DE REQUISA DETALLE INSERTADO
                dt = con.ExecuteTable($"SELECT TOP (1) IdRequisaDetail FROM dbo.tRequisaDetail WHERE IdRequisa='{IdRequisa}' ORDER BY IdRequisaDetail DESC;");
                if (dt.Rows.Count > 0)
                    proximoIdRequisaDetail = int.Parse(dt.Rows[0]["IdRequisaDetail"].ToString());
                dt.Clear();
                //VERIFICAMOS SI REALMENTE SE INSERTO EL DETALLE LA REQUISA.
                if (proximoIdRequisaDetail > lastIdRequisaDetail)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MyLogger.GetInstance().Info("Requisa class. RequisaInsertHeadToMCS method, InsertaRequisaMCS: " + ex.Message);
                return false;
            }
        }
    }

}