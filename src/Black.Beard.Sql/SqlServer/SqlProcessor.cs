using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Bb.SqlServerStructures
{

    public class SqlProcessor : IDisposable
    {

        public SqlProcessor(ConnectionStringSetting connectionStringSetting)
            : this(connectionStringSetting.GetBuilder()) // ?? throw new NullReferenceException(nameof(connectionStringSetting)
        {

        }

        public SqlProcessor(string? connexionString) : this(new SqlConnectionStringBuilder(connexionString))
        {

        }

        public SqlProcessor(SqlConnectionStringBuilder connexionBuilder)
        {

            if (connexionBuilder == null)
                throw new NullReferenceException(nameof(connexionBuilder));

            this._connexionString = connexionBuilder.ConnectionString;

            if (string.IsNullOrEmpty(this._connexionString))
                throw new NullReferenceException(nameof(_connexionString));

            this.ConnectionBuilder = connexionBuilder;
        }



        public SqlProcessorResult ExecuteNonQuery(string commandText, params SqlParameter[] parameters)
        {

            SqlProcessorResult result = new SqlProcessorResult();
            result.Success = true;

            SqlTransaction transaction = null;
            bool useTransaction = false;
            Exception ex = null;


            var scripts = ScriptConvert.Get(commandText);



            foreach (var item in scripts)
            {

                useTransaction = item.UseTransaction;

                using (var cnx = GetConnexion())
                {

                    if (useTransaction)
                        transaction = cnx.BeginTransaction();

                    ScriptItem currentScript = null;
                    try
                    {

                        foreach (var script in item)
                            if (!script.IsEmpty)
                            {
                                currentScript = script;
                                using (var cmd = Getcommand(cnx, script.ToString(), parameters))
                                {
                                    cmd.Connection = cnx;
                                    if (transaction != null)
                                        cmd.Transaction = transaction;

                                    var i = cmd.ExecuteNonQuery();
                                    if (i > 0)
                                        result.CountInpactedObjects += i;

                                    result.Success = true;

                                }
                            }
                    }
                    catch (Exception e)
                    {

                        ex = e;
                        if (Debugger.IsAttached)
                            Debugger.Launch();

                        result.Exception = e;
                        result.Success = false;
                        Trace.TraceError($"error sql at line {currentScript.Position}.\r\n {e.Message}.\r\n " + currentScript.ToString() + "\r\n");

                        if (useTransaction && transaction != null)
                            transaction.Rollback();

                    }
                    finally
                    {

                        if (useTransaction && transaction != null)
                        {
                            if (ex == null)
                                transaction.Commit();
                            transaction.Dispose();
                        }

                    }

                }

            }
            return result;

        }

        public IEnumerable<SqlProcessorResult1<T>> ExecuteNonQueryInBlock<T>(string commandText, SqlParameter[] parameters, IEnumerable<T> items, Action<T> action)
        {

            using (var cnx = GetConnexion())
                return ExecuteNonQueryInBlock<T>(cnx, commandText, parameters, items, action);

        }

        public IEnumerable<SqlProcessorResult1<T>> ExecuteNonQueryInBlock<T>(SqlConnection cnx, string commandText, SqlParameter[] parameters, IEnumerable<T> items, Action<T> action)
        {

            List<SqlProcessorResult1<T>> results = new List<SqlProcessorResult1<T>>();

            using (var cmd = Getcommand(cnx, commandText, parameters))
            {

                cmd.Connection = cnx;

                foreach (var item in items)
                {

                    var i = new SqlProcessorResult1<T>();
                    results.Add(i);

                    action(item);

                    try
                    {

                        var r = cmd.ExecuteNonQuery();
                        i.Success = true;

                        if (r > 0)
                            i.CountInpactedObjects += r;

                    }
                    catch (Exception e)
                    {
                        i.Exception = e;
                    }

                }

            }

            return results;

        }

        public bool Exists(string queryString, params SqlParameter[] arguments)
        {
            return Read(queryString, arguments).Any();
        }

        public IEnumerable<SqlDataReader> Read(string queryString, params SqlParameter[] arguments)
        {

            using (var cnx = GetConnexion())
            using (var query = Getcommand(cnx, queryString, arguments))
            using (var reader = query.ExecuteReader())

                while (reader.Read())
                    yield return reader;


        }

        public IEnumerable<Reader<T>> Read<T>(string queryString, params SqlParameter[] arguments)
            where T : Enum
        {

            using (var cnx = GetConnexion())
            using (var query = Getcommand(cnx, queryString, arguments))
            using (var reader = query.ExecuteReader())
            {
                var r = new Reader<T>(reader);
                while (reader.Read())
                    yield return r;
            }

        }

        public SqlProcessorResult1<T> ExecuteScalar<T>(string commandText, params SqlParameter[] parameters)
        {

            using (var cnx = GetConnexion())
            using (var cmd = Getcommand(cnx, commandText, parameters))
            {

                cmd.Connection = cnx;
                var result = new SqlProcessorResult1<T>();

                try
                {
                    var r = cmd.ExecuteScalar();
                    result.Item = (T)Convert.ChangeType(r, typeof(T));
                    result.Success = true;
                }
                catch (Exception e)
                {
                    result.Exception = e;
                }

                return result;

            }
        }

        public SqlParameter GetParameterReturnValue(string parameterName, object? value, DbType? dbType = null, int? size = null, byte? scale = null)
        {
            var p = GetParameter(parameterName, value, dbType, size, scale);
            p.Direction = ParameterDirection.ReturnValue;
            return p;
        }

        public SqlParameter GetParameterOut(string parameterName, object? value, DbType? dbType = null, int? size = null, byte? scale = null)
        {
            var p = GetParameter(parameterName, value, dbType, size, scale);
            p.Direction = ParameterDirection.Output;
            return p;
        }

        public SqlParameter GetParameter(string parameterName, object? value, DbType? dbType = null, int? size = null, byte? scale = null)
        {

            if (_factory == null)
                throw new NullReferenceException("no connexion string initialized");

            var parameter = _factory.CreateParameter() as SqlParameter;

            parameter.ParameterName = parameterName;
            parameter.Value = value;

            if (dbType.HasValue)
                parameter.DbType = dbType.Value;

            if (size.HasValue)
                parameter.Size = size.Value;

            if (scale.HasValue)
                parameter.Scale = scale.Value;

            return parameter;

        }

        public SqlConnection GetConnexion()
        {

            if (_factory == null)
                throw new NullReferenceException("no connexion string initialized");

            var cnx = _factory.CreateConnection() as SqlConnection;

            cnx.ConnectionString = this._connexionString
                                    ?? this._builder?.ConnectionString
                                    ?? throw new NullReferenceException("no connexion string initialized");
            cnx.Open();

            return cnx;

        }

        public ISqlServerWatcher? SqlServerWatcher { get; private set; }

        public SqlConnectionStringBuilder ConnectionBuilder { get; }

        public SqlProcessor InitializeWatcher(int refreshInterval, Action action)
        {
            SqlServerWatcher = new SqlServerPeriodicalWatcher(TimeSpan.FromSeconds(refreshInterval))
                .Subscribe(action);
            return this;

        }


        public void WriteBulk(DbDataReader reader, string targetTable, params (string, string)[] mappings)
        {

            using (var cnx = GetConnexion())
            using (var transaction = cnx.BeginTransaction())
            {
                WriteBulk(transaction, reader, targetTable, mappings);
                transaction.Commit();
            }

        }

        public void WriteBulk(SqlTransaction transaction, DbDataReader reader, string targetTable, params (string, string)[] mappings)
        {

            using (var bulkCopy = new SqlBulkCopy(transaction.Connection, SqlBulkCopyOptions.Default, transaction)
            {
                DestinationTableName = targetTable,
                BulkCopyTimeout = 60,
                BatchSize = 50000,
                EnableStreaming = true,
            })
            {

                foreach (var item in mappings)
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(item.Item1, item.Item2));

                bulkCopy.WriteToServer(reader);

            }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SqlServerWatcher?.Dispose();
                }

                disposedValue = true;
            }
        }

        // // TODO: substituer le finaliseur uniquement si 'Dispose(bool disposing)' a du code pour libérer les ressources non managées
        // ~SqlProcessor()
        // {
        //     // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private SqlCommand Getcommand(SqlConnection cnx, string commandText, SqlParameter[] parameters)
        {

            var cmd = this._factory.CreateCommand() as SqlCommand;
            cmd.CommandText = commandText;
            cmd.Connection = cnx;

            if (parameters != null)
                foreach (var param in parameters)
                    if (param != null)
                    {
                        var p = Clone(param);
                        cmd.Parameters.Add(p);
                    }

            return cmd;

        }

        private SqlParameter Clone(SqlParameter param)
        {

            var result = _factory.CreateParameter() as SqlParameter;

            result.ParameterName = param.ParameterName;
            result.SourceColumn = param.SourceColumn;
            result.SourceColumnNullMapping = param.SourceColumnNullMapping;
            result.Precision = param.Precision;
            result.Scale = param.Scale;
            result.Size = param.Size;
            result.DbType = param.DbType;
            result.Value = param.Value;
            result.Direction = param.Direction;

            return result;

        }

        private SqlConnectionStringBuilder _builder;
        private readonly SqlClientFactory _factory = SqlClientFactory.Instance;
        private readonly string? _connexionString;

        private bool disposedValue;

    }

}