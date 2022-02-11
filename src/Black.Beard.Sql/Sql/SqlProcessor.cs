using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace Bb.Sql
{

    public class SqlProcessor : IDisposable
    {

        public static SqlProcessor GetSqlProcessor(ConnectionStringSetting connectionStringSetting)
        {
            return new SqlProcessor(connectionStringSetting ?? throw new NullReferenceException(nameof(connectionStringSetting)));
        }

        public static SqlProcessor GetSqlProcessor(string connexionString, DbProviderFactory factory)
        {
            return new SqlProcessor(connexionString, factory);
        }

        public static SqlProcessor GetSqlProcessor(ConnectionSettings settings, string connectionStringName)
        {
            return new SqlProcessor(settings, connectionStringName);
        }

        protected SqlProcessor(ConnectionStringSetting connectionStringSetting)
            : this(connectionStringSetting?.ConnectionString, connectionStringSetting.GetProvider())
        {

        }

        protected SqlProcessor(ConnectionSettings settings, string connectionStringName)
            : this(settings.ConnectionStringSettings?.GetConnectionString(connectionStringName))
        {

        }

        protected SqlProcessor(string connexionString, DbProviderFactory factory)
        {
            this._connexionString = connexionString ?? throw new NullReferenceException(nameof(connexionString));
            this._factory = factory ?? throw new NullReferenceException(nameof(factory));
        }

        public SqlProcessor(DbConnectionStringBuilder builder, DbProviderFactory factory)
        {
            this._builder = builder ?? throw new NullReferenceException(nameof(builder));
            this._factory = factory ?? throw new NullReferenceException(nameof(factory));
        }




        public SqlProcessorResult ExecuteNonQuery(string commandText, params DbParameter[] parameters)
        {

            SqlProcessorResult result = new SqlProcessorResult();
            result.Success = true;

            DbTransaction transaction = null;
            bool inTransaction = false;
            Exception ex = null;

            using (var cnx = GetConnexion())
            {

                var scripts = commandText.Split(new string[] { "GO", "go", "Go", "gO" }, StringSplitOptions.RemoveEmptyEntries);

                inTransaction = scripts.Length > 1;

                if (inTransaction)
                    transaction = cnx.BeginTransaction();

                try
                {

                    foreach (var script in scripts)
                        using (var cmd = Getcommand(cnx, script, parameters))
                        {
                            cmd.Connection = cnx;
                            if (transaction != null)
                                cmd.Transaction = transaction;

                            var i = cmd.ExecuteNonQuery();
                            if (i > 0)
                                result.InpactedObject += i;

                            result.Success = true;

                        }

                }
                catch (Exception e)
                {

                    ex = e;
                    if (Debugger.IsAttached)
                        Debugger.Launch();

                    result.Exception = e;
                    result.Success = false;
                    Trace.TraceError(e.Message);

                    if (inTransaction && transaction != null)
                        transaction.Rollback();

                }
                finally
                {

                    if (inTransaction && transaction != null)
                    {
                        if (ex == null)
                            transaction.Commit();
                        transaction.Dispose();
                    }

                }

            }

            return result;

        }

        public IEnumerable<SqlProcessorResult1<T>> ExecuteNonQueryInBlock<T>(string commandText, DbParameter[] parameters, IEnumerable<T> items, Action<T> action)
        {

            using (var cnx = GetConnexion())
                return ExecuteNonQueryInBlock<T>(cnx, commandText, parameters, items, action);

        }

        public IEnumerable<SqlProcessorResult1<T>> ExecuteNonQueryInBlock<T>(DbConnection cnx, string commandText, DbParameter[] parameters, IEnumerable<T> items, Action<T> action)
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
                            i.InpactedObject += r;

                    }
                    catch (Exception e)
                    {
                        i.Exception = e;
                    }

                }

            }

            return results;

        }

        public IEnumerable<IDataReader> Read(string queryString, params DbParameter[] arguments)
        {

            using (var cnx = GetConnexion())
            using (var query = Getcommand(cnx, queryString, arguments))
            {

                using (var reader = query.ExecuteReader())
                    while (reader.Read())
                        yield return reader;

            }

        }

        public SqlProcessorResult1<T> ExecuteScalar<T>(string commandText, params DbParameter[] parameters)
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

        public DbParameter GetParameterReturnValue(string parameterName, object? value, DbType? dbType = null, int? size = null, byte? scale = null)
        {
            var p = GetParameter(parameterName, value, dbType, size, scale);
            p.Direction = ParameterDirection.ReturnValue;
            return p;
        }

        public DbParameter GetParameterOut(string parameterName, object? value, DbType? dbType = null, int? size = null, byte? scale = null)
        {
            var p = GetParameter(parameterName, value, dbType, size, scale);
            p.Direction = ParameterDirection.Output;
            return p;
        }

        public DbParameter GetParameter(string parameterName, object? value, DbType? dbType = null, int? size = null, byte? scale = null)
        {

            if (_factory == null)
                throw new NullReferenceException("no connexion string initialized");

            var parameter = _factory.CreateParameter();

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

        public DbConnection GetConnexion()
        {

            if (_factory == null)
                throw new NullReferenceException("no connexion string initialized");

            var cnx = _factory.CreateConnection();

            cnx.ConnectionString = this._connexionString ?? this._builder?.ConnectionString ?? throw new NullReferenceException("no connexion string initialized");
            cnx.Open();

            return cnx;

        }

        public ISqlServerWatcher? SqlServerWatcher { get; private set; }

        public SqlProcessor InitializeWatcher(int refreshInterval, Action action)
        {
            SqlServerWatcher = new SqlServerPeriodicalWatcher(TimeSpan.FromSeconds(refreshInterval))
                .Subscribe(action);
            return this;

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

        private DbCommand Getcommand(DbConnection cnx, string commandText, DbParameter[] parameters)
        {

            var cmd = this._factory.CreateCommand();
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

        private DbParameter Clone(DbParameter param)
        {

            var result = _factory.CreateParameter();

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

        private DbConnectionStringBuilder? _builder;
        private readonly DbProviderFactory? _factory;
        private readonly string? _connexionString;

        private bool disposedValue;

    }


}


