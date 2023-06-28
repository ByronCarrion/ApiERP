using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace ApiERP.Services
{
    public class ConexionSQLServer
    {
        private SqlConnection conn;
        string connectionString = ConfigurationManager.ConnectionStrings["dbInventSql"].ConnectionString;

        public SqlConnection CrearConexion()
        {

            conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                return conn;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error al Ejecutar Conectar al BD: " + ex.Message);
                return null;
            }
        }

        public bool ExecuteQuery(string SqlQuery)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = SqlQuery;
                cmd.Connection = CrearConexion();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                return true;
            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {
                //logger.WriteLog("Error: " + ex.Message.ToString());
                return false;
            }
        }

        public bool ExecuteQuery(string SqlQuerry, ref SqlParameter[] SpParameters)
        {
            SqlCommand cmd = new SqlCommand();
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
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {
                return false;
            }
        }

        public bool ExecuteSP(string SqlName, ref SqlParameter[] SpParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SqlName;
                cmd.Connection = CrearConexion();
                for (var i = 0; i <= SpParameters.Length - 1; i++)
                    cmd.Parameters.Add(SpParameters[i]);
                cmd.ExecuteNonQuery();
                cmd.Parameters.CopyTo(SpParameters, 0);
                cmd.Connection.Close();
                return true;
            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {
                return false;
            }
        }

        public DataTable ExecuteTable(string SqlQuerry)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(SqlQuerry, CrearConexion());

                DataTable dtRes;
                SqlDataAdapter daQry;
                daQry = new SqlDataAdapter(cmd);
                dtRes = new DataTable();
                daQry.Fill(dtRes);
                daQry.Dispose();
                return (dtRes);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al Ejecutar Sentencia: " + ex.Message);
                return null/* TODO Change to default(_) if this is not a reference type */;
            }
        }

    }
}