using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Configuration;

namespace MatechEssential
{
    public class SQLHelper
    {
        protected DbEngines _Engine = DbEngines.MSSQL;
        protected string _ConStr = "";
        public DataSet ExecuteQuery(string query)
        {
            return ExecuteQuery(query, null);
        }
        public void ExecuteNonQuery(string query)
        {
            ExecuteNonQuery(query, null, null);
        }
        public void ExecuteNonQuery(string query, SqlContext context)
        {
            ExecuteNonQuery(query, null,context);
        }
        public object ExecuteScalar(string query)
        {
            return ExecuteScalar(query, null);
        }
        public static SQLHelper Initialize()
        {
            return new SQLHelper(DbEngines.MySQL, ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
        }

        public static SqlContext GetNewContext(bool UseTransaction = false)
        {
            return new SqlContext(DbEngines.MySQL, ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString, UseTransaction);
        }

        public DataSet ExecuteQuery(string query, Dictionary<string, object> pars)
        {
            SqlContext context = new SqlContext(_Engine, _ConStr, true);
           
            DataSet ds = null;
            try
            {
                ds = ExecuteQuery(query, pars, context);
                context.Commit();
            }
            catch (Exception ex)
            {
                context.Rollback();
                throw ex;
            }
            context.Close();
            return ds;
        }
        public void ExecuteNonQuery(string query, Dictionary<string, object> pars)
        {
            SqlContext context = new SqlContext(_Engine, _ConStr, true);

            try
            {
                ExecuteNonQuery(query, pars, context);
                context.Commit();
            }
            catch (Exception ex)
            {
                context.Rollback();
                throw ex;
            }
            context.Close();
        }
        public object ExecuteScalar(string query, Dictionary<string, object> pars)
        {
            SqlContext context = new SqlContext(_Engine, _ConStr, true);
            object ret = null;
            try
            {
                ret = ExecuteScalar(query, pars, context);
                context.Commit();
            }
            catch (Exception ex)
            {
                context.Rollback();
                throw ex;
            }
            context.Close();
            return ret;
        }


        public DataSet ExecuteQuery(string query, Dictionary<string, object> pars, SqlContext context)
        {
            if (context == null)
                ExecuteQuery(query, pars);
            try
            {
                DbCommand cmd = CreateCommand(query);
                SetCmdPars(cmd, pars);
                cmd.Connection = context.Connection;
                cmd.Transaction = context.Transaction;
                cmd.CommandTimeout = 0;
                DataAdapter da = CreateAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ExecuteNonQuery(string query, Dictionary<string, object> pars, SqlContext context)
        {
            if (context == null)
            {
                ExecuteNonQuery(query, pars);
                return;
            }
            try
            {
                DbCommand cmd = CreateCommand(query);
                SetCmdPars(cmd, pars);
                cmd.Connection = context.Connection;
                cmd.Transaction = context.Transaction;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public object ExecuteScalar(string query, Dictionary<string, object> pars, SqlContext context)
        {
            if (context == null)
                ExecuteScalar(query, pars);
            try
            {
                DbCommand cmd = CreateCommand(query);
                SetCmdPars(cmd, pars);
                cmd.Connection = context.Connection;
                cmd.Transaction = context.Transaction;
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataSet ExecuteSP(string spName, Dictionary<string, object> pars)
        {
            SqlContext context = new SqlContext(_Engine, _ConStr, true);
            //context.Transaction = context.Connection.BeginTransaction();
            DataSet ds = null;
            try
            {
                ds = ExecuteSP(spName, pars, context);
                context.Commit();
            }
            catch (Exception ex)
            {
                context.Rollback();
            }
            context.Close();
            return ds;
        }
        public void ExecuteSPNonQuery(string spName, Dictionary<string, object> pars)
        {
            SqlContext context = new SqlContext(_Engine, _ConStr, true);
            try
            {
                ExecuteSPNonQuery(spName, pars, context);
                context.Commit();
            }
            catch (Exception ex)
            {
                context.Rollback();
                throw ex;
            }
            context.Close();
        }
        public object ExecuteSPScalar(string spName, Dictionary<string, object> pars)
        {
            SqlContext context = new SqlContext(_Engine, _ConStr, true);

            object ds = null;
            try
            {
                ds = ExecuteSPScalar(spName, pars, context);
                context.Commit();
            }
            catch (Exception ex)
            {
                context.Rollback();
                throw ex;
            }
            context.Close();
            return ds;
        }

        public DataSet ExecuteSP(string spName, Dictionary<string, object> pars, SqlContext context)
        {
            if (context == null)
                return ExecuteSP(spName, pars);
            try
            {
                DbCommand cmd = CreateCommand(spName, pars);
                cmd.Connection = context.Connection;
                cmd.CommandTimeout = 0;
                if (context.IsTransaction)
                {
                    cmd.Transaction = context.Transaction;
                }
                DataAdapter da = CreateAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExecuteSPNonQuery(string spName, Dictionary<string, object> pars, SqlContext context)
        {
            if (context == null)
                ExecuteSPNonQuery(spName, pars);
            try
            {
                DbCommand cmd = CreateCommand(spName, pars);
                cmd.Connection = context.Connection;
                cmd.CommandTimeout = 0;
                if (context.IsTransaction)
                {
                    cmd.Transaction = context.Transaction;
                }
                DataAdapter da = CreateAdapter(cmd);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object ExecuteSPScalar(string spName, Dictionary<string, object> pars, SqlContext context)
        {
            if (context == null)
                return ExecuteSPScalar(spName, pars);
            try
            {
                DbCommand cmd = CreateCommand(spName, pars);
                cmd.Connection = context.Connection;
                if (context.IsTransaction)
                {
                    cmd.Transaction = context.Transaction;
                }
                DataAdapter da = CreateAdapter(cmd);
                object ds = cmd.ExecuteScalar();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public SQLHelper(string ConStr)
        {
            _Engine = DbEngines.MSSQL;
            _ConStr = ConStr;

        }
        public SQLHelper(DbEngines Engine, string ConStr)
        {
            _Engine = Engine;
            _ConStr = ConStr;
        }


        private DataAdapter CreateAdapter(DbCommand cmd)
        {
            DataAdapter da;
            if (_Engine == DbEngines.MSSQL)
            {
                da = new SqlDataAdapter((SqlCommand)cmd);
            }
            else
                da = new MySqlDataAdapter((MySqlCommand)cmd);
            return da;

        }
        private DbCommand CreateCommand(string query)
        {
            DbCommand cmd;
            if (_Engine == DbEngines.MSSQL)
            {
                cmd = new SqlCommand(query);
            }
            else
            {
                cmd = new MySqlCommand(query);
            }
            return cmd;
        }
        private DbCommand CreateCommand(string spName, Dictionary<string, object> pars)
        {
            DbCommand cmd = CreateCommand(spName);
            cmd.CommandType = CommandType.StoredProcedure;
            SetCmdPars(cmd, pars);
            return cmd;
        }
        private void SetCmdPars(DbCommand cmd, Dictionary<string, object> pars)
        {
            if (pars != null)
            {
                foreach (KeyValuePair<string, object> p in pars)
                {
                    DbParameter dp;
                    if (_Engine == DbEngines.MSSQL)
                        dp = new SqlParameter();
                    else
                        dp = new MySqlParameter();
                    dp.ParameterName = (_Engine == DbEngines.MSSQL ? "@" : "") + p.Key;
                    dp.Value = (p.Value == null) ? DBNull.Value : p.Value;
                    cmd.Parameters.Add(dp);
                }
            }
        }

    }
    public class SqlContext
    {
        protected DbEngines _DBEngine;
        protected string _ConStr = "";
        private bool _IsTransaction = false;
        public bool IsTransaction { get { return _IsTransaction; } }
        public DbConnection Connection { get; set; }
        public DbTransaction Transaction { get; set; }
        public SqlContext(DbEngines engine, string ConStr, bool UseTransaction = false)
        {
            _DBEngine = engine;
            _ConStr = ConStr;
            Connection = GetConnection();
            if (UseTransaction)
            {
                StartTransaction();
            }
        }
        private void StartTransaction()
        {
            _IsTransaction = true;
            this.Transaction = Connection.BeginTransaction(IsolationLevel.ReadUncommitted);
        }
        private DbConnection GetConnection()
        {
            try
            {
                DbConnection dbCon;
                if (_DBEngine == DbEngines.MSSQL)
                    dbCon = new SqlConnection(_ConStr);
                else
                    dbCon = new MySqlConnection(_ConStr);
                dbCon.Open();
                return dbCon;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Close()
        {
            try
            {
                if (this.Connection.State != ConnectionState.Closed)
                    this.Connection.Close();
                this.Connection.Dispose();
                MySqlConnection.ClearPool((MySqlConnection)this.Connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Commit()
        {
            if (IsTransaction)
            {
                Transaction.Commit();
            }
        }
        public void Rollback()
        {
            if (IsTransaction)
            {
                try
                {
                    Transaction.Rollback();
                }catch(Exception ex)
                {
                    throw ex;
                }
            }
        }
    }


}
