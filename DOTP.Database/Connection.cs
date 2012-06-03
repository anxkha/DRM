using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace DOTP.Database
{
    public delegate void SqlCallback(SqlDataReader reader);

    public static class Connection
    {
        private static SqlConnection m_sqlConnection;
        private static string m_connectionString;

        private static void EnsureConnected()
        {
            if ((null != m_sqlConnection) && (System.Data.ConnectionState.Open == m_sqlConnection.State)) return;

            var webConfig = WebConfigurationManager.OpenWebConfiguration("/Web.config");

            if (0 == webConfig.ConnectionStrings.ConnectionStrings.Count)
                throw new Exception();

            var connectionStringSettings = webConfig.ConnectionStrings.ConnectionStrings["DefaultConnection"];

            if (null == connectionStringSettings)
                throw new Exception();

            m_connectionString = connectionStringSettings.ConnectionString;

            m_sqlConnection = new SqlConnection(m_connectionString);
            m_sqlConnection.Open();
        }

        public static void ExecuteSql(Query command, SqlCallback callback)
        {
            EnsureConnected();

            command.Command.Connection = m_sqlConnection;

            var reader = command.Command.ExecuteReader();

            callback(reader);

            reader.Close();
        }

        public static object ExecuteSqlScalar(Query command)
        {
            EnsureConnected();

            command.Command.Connection = m_sqlConnection;

            return command.Command.ExecuteScalar();
        }
    }
}
