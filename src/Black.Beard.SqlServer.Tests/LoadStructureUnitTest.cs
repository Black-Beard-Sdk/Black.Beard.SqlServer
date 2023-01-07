using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;
using System.Diagnostics;
using System.Reflection;

namespace Black.Beard.SqlServer.Tests
{
    [TestClass]
    public class LoadStructureUnitTest
    {


        public LoadStructureUnitTest()
        {
        }


        [TestMethod]
        public void TestMethod1()
        {


            string Server = ".";
            string database = "TBase5";

            var setting = new Bb.SqlServerStructures.ConnectionStringSetting() { ConnectionString = $"Data Source={Server};Initial Catalog={database};Integrated Security=true;" };

            var db = DatabaseStructure.ResolveFromDatabase(setting);

        }

    }
}