using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;
using System.Diagnostics;
using System.Reflection;

namespace Black.Beard.SqlServer.Tests
{
    [TestClass]
    public class StructureUnitTest
    {

        public StructureUnitTest()
        {
            this._root = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
        }

        [TestMethod]
        public void GenerateDacpac()
        {

            string databaseName = "BaseModelSqlServer";
            var schema = "dbo";
            var checkCtx = new CheckContext();
            DatabaseStructure db = DatabaseStructure.GetStructure(databaseName, schema);
            db.Check(checkCtx);

            var ctx = new DacpacContext(new Variables())    // new KeyValuePair<string, string>("tableQueue", "table1")
            {
                CreateFileGroupPolicy = PolicyEnum.Create,
                CreateSchemaPolicy = PolicyEnum.Create,

                TargetState = null,

            };
            var dacpack = db.GenerateDacpac(databaseName, ctx);
            var file = Path.Combine(this._root, "test.dacpac");
            dacpack.Write(file, true);

            string Server = ".";


            //var result = DotnetCommand.DacpacPublishWithCreateDatabase(this._root, "test.dacpac", Server, "baseTest1", new DacpacArguments() { CreateNewDatabase = false });
            // dotnet dacpac publish --dacpath=C:\Src\Black.Beard.Roslyn\Src\Black.Beard.Dacpac.Tests\bin\Debug\net6.0 --namePattern=test.dacpac --server=. --databaseNames=BaseModelSqlServer --UseSspi=true --createNewDatabase=true
            // dotnet dacpac publish --dacpath=C:\Src\Black.Beard.Roslyn\Src\Black.Beard.Dacpac.Tests\bin\Debug\net6.0 --namePattern=test.dacpac --server=. --databaseNames=baseTest1 --UseSspi=true --createNewDatabase=false
            // var processInfo = new ProcessStartInfo(result.Item1, result.Item2);
            // var thread = System.Diagnostics.Process.Start(processInfo);

        }

        [TestMethod]
        public void GenerateScriptCreateDatabase()
        {

            string Server = ".";
            string databaseName = "TBase5";
            var schema = "dbo";

            var settingCurrent = new Bb.SqlServerStructures.ConnectionStringSetting() { ConnectionString = $"Data Source={Server};Initial Catalog={databaseName};Integrated Security=true;" };
            //DatabaseStructure dbSource = DatabaseStructure.Load(settingCurrent);

            DatabaseStructure db = DatabaseStructure.GetStructure(databaseName, schema);
            var checkCtx = new CheckContext();
            db.Check(checkCtx);

            var ctx = new ScriptContext(new Variables())    // new KeyValuePair<string, string>("tableQueue", "table1")
            {
                CreateDatabase = true,
                CreateAllTables = true,
                // CurrentState = dbSource,
            };

            var script = db.GetScriptGenerator(ctx);

            var file = Path.Combine(this._root, "test.sql");
            script.WriteOnDisk(file, true);

            var settingNew = new Bb.SqlServerStructures.ConnectionStringSetting() { ConnectionString = $"Data Source={Server};Integrated Security=true;" };
            script.Execute(settingNew);


            db.Save(settingCurrent);


        }

        private readonly string _root;

    }
}