using ApiERP.Services.Utility;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace ApiERP.Services
{
    public class ConexionMySQL
    {
        private MySqlConnection conn;
        string connectionString = ConfigurationManager.ConnectionStrings["dbMySql"].ConnectionString;

        public MySqlConnection CrearConexion()
        {

            conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                return conn;
            }
            catch (MySqlException ex)
            {
                MyLogger.GetInstance().Error($"Error al Conetar con la BD: {ex.Message}");
                return null;
            }
        }

        public bool ExecuteQuery(string SqlQuery)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = SqlQuery;
                cmd.Connection = CrearConexion();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MyLogger.GetInstance().Error($"Error al ejecutar consulta: {ex.Message}");
                return false;
            }
        }

        public bool ExecuteQuery(string SqlQuerry, ref MySqlParameter[] SpParameters)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = SqlQuerry;
                cmd.Connection = CrearConexion();
                for (var i = 0; i <= SpParameters.Length - 1; i++)
                    cmd.Parameters.Add(SpParameters[i]);
                cmd.ExecuteNonQuery();
                cmd.Parameters.CopyTo(SpParameters, 0);
                cmd.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MyLogger.GetInstance().Error($"Error al ejecutar Procedimiento almacenado: {ex.Message}");
                return false;
            }
        }

        public bool ExecuteSP(string SqlName, ref MySqlParameter[] SpParameters)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SqlName;
                cmd.Connection = CrearConexion();
                for (var i = 0; i <= SpParameters.Length - 1; i++)
                    cmd.Parameters.Add(SpParameters[i]);
                MyLogger.GetInstance().Info("Requisa Clase. SP a Ejecutar: " + cmd.ToString());
                cmd.ExecuteNonQuery();
                cmd.Parameters.CopyTo(SpParameters, 0);
                cmd.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MyLogger.GetInstance().Error($"Error al ejecutar Procedimiento almacenado: {ex.Message}");
                return false;
            }
        }

        public DataTable ExecuteTable(string SqlQuerry)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(SqlQuerry, CrearConexion());

                DataTable dtRes;
                MySqlDataAdapter daQry;
                daQry = new MySqlDataAdapter(cmd);
                dtRes = new DataTable();
                daQry.Fill(dtRes);
                daQry.Dispose();
                return (dtRes);
            }
            catch (Exception ex)
            {
                MyLogger.GetInstance().Error($"Error al ejecutar Consulta: {ex.Message}");
                return null;
            }
        }

    }

}