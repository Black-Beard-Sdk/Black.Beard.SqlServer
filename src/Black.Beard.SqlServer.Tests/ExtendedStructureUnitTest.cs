using Bb.Extended;
using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;
using Bb.SqlServerStructures;
using System.Diagnostics;
using System.Reflection;

namespace Black.Beard.SqlServer.Tests
{
    [TestClass]
    public class ExtendedStructureUnitTest
    {


        public ExtendedStructureUnitTest()
        {
            this._root = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
        }


        [TestMethod]
        public void GenerateScriptCreateDatabase()
        {

            string Server = ".";
            string databaseName = "BaseModels";


            var settingCurrent = new ConnectionStringSetting()
            {
                ConnectionString = $"Data Source={Server};Initial Catalog={databaseName};Integrated Security=true;"
            };


            var models = new Models(settingCurrent)
                .InitializeDatabaseIfNotExists()
                ;

            models
                .AddHost(Server,
                    c => c.AddCatalog(databaseName)
                    )
                .AddApplication("ApplicationModeler",
                    c => c.AddEnvironment("Prod",                        
                        c => c.SetConnection(Server, databaseName)
                              .ResolveDatabase()
                    )
                )
                .Save()
            ;




        }

        private readonly string _root;

    }
}