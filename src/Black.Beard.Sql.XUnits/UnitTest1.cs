using Bb.Sql;
using System.Data.SqlClient;
using Xunit;

namespace Black.Beard.Sql.XUnits
{
    public class UnitTest1
    {

        public UnitTest1()
        {
            var o = SqlClientFactory.Instance; // For reference in assembly



        }

        [Fact]
        public void Test1()
        {

            // Parse all loaded assemblies and resolve all DbProviderFactory
            var list = ResolveFactoryHelper.GetFactoriesFromLoadedAssemblies();
            // Register the factories
            list.RegisterFactories();


            var items = new ConnectionSettings()
            {
                ConnectionStringSettings = new ConnectionStringSettings()
                 {
                     new ConnectionStringSetting()
                     {
                         Name = "Name1",
                         ConnectionString = "Data Source=.",
                         ProviderName = "System.Data.SqlClient.SqlClientFactory"
                     }
                 }
            };

            ConnectionStringSetting? i = items["Name1"];

            Assert.NotNull(i);


            var provider = i.GetProvider();

            Assert.NotNull(i);


        }


        [Fact]
        public void Test2()
        {

        


        }

        [Fact]
        public void Test3()
        {

            // Parse all loaded assemblies and resolve all DbProviderFactory
            var list = ResolveFactoryHelper.GetFactoriesFromLoadedAssemblies();
            // Register the factories
            ResolveFactoryHelper.RegisterFactories(list);



        }
    }
}