using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace AxonifyIntegration.Dal.AdoDB
{
    public static class DataTableExtensions
    {
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            IList<System.Reflection.PropertyInfo> properties = typeof(T).GetProperties().ToList();
            List<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }

        private static T CreateItemFromRow<T>(DataRow row, IList<System.Reflection.PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                System.Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                if (row.Table.Columns.Contains(property.Name))
                {
                    if (property.PropertyType.IsGenericType && property.PropertyType.Name.Contains("Nullable"))
                    {
                        if (!string.IsNullOrEmpty(row[property.Name].ToString()))
                        {
                            property.SetValue(item, Convert.ChangeType(row[property.Name], propertyType, null));
                        }
                    }
                    else
                        property.SetValue(item, Convert.ChangeType(row[property.Name], propertyType), null);
                }
            }
            return item;
        }
    }

    public class AdoHelper : IDisposable
    {
        // Internal members
        protected string _connString = null;
        protected SqlConnection _conn = null;
        protected SqlTransaction _trans = null;
        protected bool _disposed = false;
        protected int _commandTimeout = 0;

        /// <summary>
        /// Returns the current SqlTransaction object or null if no transaction
        /// is in effect.
        /// </summary>
        public SqlTransaction Transaction { get { return _trans; } }

        /// <summary>
        /// Constructure using connection string override
        /// </summary>
        /// <param name="connString">Connection string for this instance</param>
        public AdoHelper(string connStringName)
        {
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings[connStringName];
            if (mySetting == null || string.IsNullOrEmpty(mySetting.ConnectionString))
                throw new System.Exception("Fatal error: missing connecting string (" + connStringName + ") in web.config file");
            _connString = mySetting.ConnectionString;
        }

        // Creates a SqlConnection using the current connection string
        public void Connect()
        {            
            _conn = new SqlConnection(_connString);
            _conn.Open();
        }

        /// <summary>
        /// Constructs a SqlCommand with the given parameters. This method is normally called
        /// from the other methods and not called directly. But here it is if you need access
        /// to it.
        /// </summary>
        /// <param name="qry">SQL query or stored procedure name</param>
        /// <param name="type">Type of SQL command</param>
        /// <param name="args">Query arguments. Arguments should be in pairs where one is the
        /// name of the parameter and the second is the value. The very last argument can
        /// optionally be a SqlParameter object for specifying a custom argument type</param>
        /// <returns></returns>
        public SqlCommand CreateCommand(string qry, CommandType type, params object[] args)
        {
            SqlCommand cmd = new SqlCommand(qry, _conn);
            cmd.CommandTimeout = _commandTimeout;

            // Associate with current transaction, if any
            if (_trans != null)
                cmd.Transaction = _trans;

            // Set command type
            cmd.CommandType = type;

            // Construct SQL parameters
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is string && i < (args.Length - 1))
                {
                    SqlParameter parm = new SqlParameter();
                    parm.ParameterName = (string)args[i];
                    parm.Value = args[++i];
                    if (parm.Value == null)
                        parm.Value = DBNull.Value;
                    cmd.Parameters.Add(parm);
                }
                else if (args[i] is SqlParameter)
                {
                    cmd.Parameters.Add((SqlParameter)args[i]);
                }
                else throw new ArgumentException("Invalid number or type of arguments supplied");
            }
            return cmd;
        }

        #region Exec Members

        /// <summary>
        /// Executes a query that returns no results
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>The number of rows affected</returns>
        public int ExecNonQuery(string qry, params object[] args)
        {
            try
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executes a stored procedure that returns no results
        /// </summary>
        /// <param name="proc">Name of stored proceduret</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>The number of rows affected</returns>
        public int ExecNonQueryProc(string proc, params object[] args)
        {
            try
            {
                using (SqlCommand cmd = CreateCommand(proc, CommandType.StoredProcedure, args))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executes a query that returns a single value
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Value of first column and first row of the results</returns>
        public object ExecScalar(string qry, params object[] args)
        {
            try
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
                {
                    return cmd.ExecuteScalar();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executes a query that returns a single value
        /// </summary>
        /// <param name="proc">Name of stored proceduret</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Value of first column and first row of the results</returns>
        public object ExecScalarProc(string qry, params object[] args)
        {
            try
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
                {
                    return cmd.ExecuteScalar();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executes a query and returns the results as a SqlDataReader
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a SqlDataReader</returns>
        public SqlDataReader ExecDataReader(string qry, params object[] args)
        {
            try
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
                {
                    return cmd.ExecuteReader();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the results as a SqlDataReader
        /// </summary>
        /// <param name="proc">Name of stored proceduret</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a SqlDataReader</returns>
        public SqlDataReader ExecDataReaderProc(string qry, params object[] args)
        {
            try
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
                {
                    return cmd.ExecuteReader();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executes a query and returns the results as a DataSet
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a DataSet</returns>
        public DataSet ExecDataSet(string qry, params object[] args)
        {
            try
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
                {
                    SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    return ds;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the results as a Data Set
        /// </summary>
        /// <param name="proc">Name of stored proceduret</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a DataSet</returns>
        public DataSet ExecDataSetProc(string qry, params object[] args)
        {
            try
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
                {
                    SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    return ds;
                }
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Transaction Members

        /// <summary>
        /// Begins a transaction
        /// </summary>
        /// <returns>The new SqlTransaction object</returns>
        public SqlTransaction BeginTransaction()
        {
            Rollback();
            _trans = _conn.BeginTransaction();
            return Transaction;
        }

        /// <summary>
        /// Commits any transaction in effect.
        /// </summary>
        public void Commit()
        {
            if (_trans != null)
            {
                _trans.Commit();
                _trans = null;
            }
        }

        /// <summary>
        /// Rolls back any transaction in effect.
        /// </summary>
        public void Rollback()
        {
            if (_trans != null)
            {
                _trans.Rollback();
                _trans = null;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // Need to dispose managed resources if being called manually
                if (disposing)
                {
                    if (_conn != null)
                    {
                        Rollback();
                        _conn.Dispose();
                        _conn = null;
                    }
                }
                _disposed = true;
            }
        }

        #endregion
    }
}
