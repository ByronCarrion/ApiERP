using ApiERP.Services;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace ApiERP.Models.ModelExtensions
{
    public class Biomasa
    {
        public int TCKCOMPANY { get; set; }
        public int TCKSEQUENCE { get; set; }
        public int TCKIDEXTERNO { get; set; }
        public DateTime TCKDATETIME { get; set; }
        public string TCKCONDUCT { get; set; }
        public string TCKMATRIC { get; set; }
        public string TCKNUMBASCULA { get; set; }
        public Decimal TCKWGHTBT { get; set; }
        public Decimal TCKWGHTTR { get; set; }
        public Decimal TCKWGHTNT { get; set; }
        public string WHO { get; set; }
        public string IP { get; set; }
        public string ACTION { get; set; }

        public bool Insert()
        {
            ConexionMySQL con = new ConexionMySQL();
            string SpName;
            MySqlParameter[] SpParameters = new MySqlParameter[25];
            bool Answer;
            int i = 0;
            try
            {
                SpName = "APCNTTICK_ADD_UPD";
                SpParameters[i] = new MySqlParameter("@p_TCKCOMPANY", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = TCKCOMPANY;
                //SpParameters[i].Value = 2;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKSEQUENCE", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = TCKSEQUENCE;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKIDEXTERNO", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKDATETIME", MySqlDbType.Timestamp);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = TCKDATETIME;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKCONDUCT", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = TCKCONDUCT;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKMATRIC", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = TCKMATRIC;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKNUMBASCULA", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = "";
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKWGHTTR", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = TCKWGHTTR;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKWGHTBT", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = TCKWGHTBT;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKWGHTNT", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = TCKWGHTNT;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKWGHTNTEXT", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKIDCTC", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKIDCTM", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKIDCTT", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKFROM", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = "";
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKFINCAID", MySqlDbType.Int32);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKDISTKM", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKHUMDET", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKSRDET", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKCVDET", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKCENDET", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_TCKLNDDET", MySqlDbType.Decimal);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = 0;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_WHO", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = WHO;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_IP", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = IP;
                i = i + 1;
                SpParameters[i] = new MySqlParameter("@p_action", MySqlDbType.VarChar);
                SpParameters[i].Direction = ParameterDirection.Input;
                SpParameters[i].Value = ACTION;

                Answer = con.ExecuteSP(SpName, ref SpParameters);
                return Answer;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al Ejecutar Conectar al BD: " + ex.Message);
                return false;
            }
        }
    }

}