using System;
using System.Data;
using System.Data.SqlClient;

namespace DOTP.Database
{
    public class Query
    {
        private SqlCommand m_command;

        public Query(string sql)
        {
            m_command = new SqlCommand(sql);
        }

        public SqlCommand Command
        {
            get
            {
                return m_command;
            }
        }

        public Query AddParam(string name, string value)
        {
            m_command.Parameters.Add(name, SqlDbType.VarChar);
            m_command.Parameters[name].Value = value;

            return this;
        }

        public Query AddParam(string name, int value)
        {
            m_command.Parameters.Add(name, SqlDbType.Int);
            m_command.Parameters[name].Value = value;

            return this;
        }

        public Query AddParam(string name, bool value)
        {
            m_command.Parameters.Add(name, SqlDbType.Bit);
            m_command.Parameters[name].Value = value;

            return this;
        }

        public Query AddParam(string name, DateTime value)
        {
            m_command.Parameters.Add(name, SqlDbType.Date);
            m_command.Parameters[name].Value = value.ToString();

            return this;
        }
    }
}
