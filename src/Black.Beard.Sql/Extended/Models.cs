using Bb.SqlServer.Structures.Dacpacs;
using Bb.SqlServer.Structures;
using Bb.SqlServerStructures;
using System.Reflection;
using Bb.SqlServer.Queries;
using System;
using Bb.SqlServer.Bulks;

namespace Bb.Extended
{

    public class Models : ModelDescriptor
    {


        public Models(ConnectionStringSetting connectionStringSetting)
        {

            this._root = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;

            _cnxStringSetting = connectionStringSetting;
            var builder = _cnxStringSetting.GetBuilder();

            this.Host = builder.DataSource;
            this.Catalog = builder.InitialCatalog;

            Hosts = new HostModelListModel()
            {
                Parent = this,
            };
            Applications = new ApplicationListModel()
            {
                Parent = this,
            };

        }


        public HostModelListModel Hosts { get; set; }

        public string Host { get; }

        public string Catalog { get; }

        public string DefaultSchema { get; set; } = "dbo";



        public ApplicationListModel Applications { get; set; }

        public IEnumerable<(HostModel, ConnectionModel)> Connections 
        {
            get
            {
                foreach (var host in this.Hosts)
                    foreach (var connection in host)
                        yield return (host, connection);
            }
        }

        public Models AddHost(string host, Action<HostModel> action)
        {
            AddHost(host, TypeHostEnum.PhysicalServer, action);
            return this;
        }

        public Models AddHost(string host, TypeHostEnum hostType = TypeHostEnum.PhysicalServer)
        {
            AddHost(host, hostType);
            return this;
        }

        public Models AddHost(string host, TypeHostEnum hostType, Action<HostModel>? action = null)
        {

            if (this.Hosts.Any(c => c.Name == host))
                throw new InvalidDataException(host);

            var _host = new HostModel()
            {
                Name = host,
                HostKind = hostType,
            };

            this.Hosts.Add(_host);

            if (action != null)
                action(_host);

            return this;

        }



        public bool DatabaseExists()
        {

            var sql = _cnxStringSetting.CreateProcessorWithoutCatalog();
            HashSet<string> keys = new HashSet<string>();
            foreach (var item in sql.Read(TextQueries.SelectDatabases()))
                keys.Add(item.GetString(0));

            return keys.Contains(this.Catalog);

        }


        public Models InitializeDatabaseIfNotExists()
        {

            if (!this.DatabaseExists())
            {

                DatabaseStructure db = DatabaseStructure.GetStructure(this.Catalog, DefaultSchema);
                var checkCtx = new CheckContext();
                db.Check(checkCtx);

                var ctx = new ScriptContext(new Variables())    // new KeyValuePair<string, string>("tableQueue", "table1")
                {
                    CreateDatabase = true,
                    CreateAllTables = true,
                    // CurrentState = dbSource,
                };

                var script = db.GetScriptGenerator(ctx);

                var file = Path.Combine(this._root, "createDatabase.sql");
                script.WriteOnDisk(file, true);

                var settingNew = new ConnectionStringSetting() { ConnectionString = _cnxStringSetting.ConnectionStringWithoutCatalog };
                script.Execute(settingNew);


            }

            return this;

        }

        public Models AddApplication(string applicationName, Action<ApplicationModel>? action = null)
        {

            var application = new ApplicationModel()
            {
                Name = applicationName
            };

            this.Applications.Add(application);

            if (action != null)
                action(application);

            return this;
        }

        public HostModel? GetHost(string server)
        {
            return Hosts.FirstOrDefault(c => c.Name == server);
        }

        public ApplicationModel? GetApplication(string applicationName)
        {
            return Applications.FirstOrDefault(c => c.Name == applicationName);
        }

        public Models Save()
        {

            var sql = this._cnxStringSetting.CreateProcessor();

            using (var cnx = sql.GetConnexion())
            using (var transaction = cnx.BeginTransaction())
            {

                var bulk = this._cnxStringSetting.GetBulkLoader();
                if (bulk != null)
                {

                    var _hosts = new MemoryDbDataReader<HostModel>(
                          this.Hosts
                        , ("Id", c => c.Id)
                        , ("Name", c => c.Name)
                        , ("Instance", c => c.Instance)
                        , ("Kind", c => (int)c.HostKind)
                        );
                    bulk.Write(_hosts, this.DefaultSchema, "Hosts", sql, transaction);


                    //var _hosts2 = new DbDataReaderIdResolver<HostModel>(
                    //     this.Hosts
                    //   , ("Id", c => c.Id)
                    //   , ("Name", c => c.Name)
                    //   );
                    //bulk.ReloadIds(_hosts2, sql, this.DefaultSchema, "Hosts");

                    //var Connections = new MemoryDbDataReader<(HostModel, ConnectionModel)>(
                    //      this.Connections
                    //    , ("Id", c => c.Item2.Id)
                    //    , ("Name", c => c.Item2.Name)
                    //    , ("HostId", c => c.Item1.Id)
                    //    , ("Userid", c => c.Item2.UserId)
                    //    , ("Password", c => c.Item2.Password)
                    //    );
                    //bulk.Write(Connections, this.DefaultSchema, "Connections", sql, transaction);


                    transaction.Commit();

                }

            }
            return this;

        }

        private readonly string _root;
        private readonly ConnectionStringSetting _cnxStringSetting;

    }


}
