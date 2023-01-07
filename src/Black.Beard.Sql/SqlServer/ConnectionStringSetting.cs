using System.Data.SqlClient;
using System.ComponentModel;

namespace Bb.SqlServerStructures
{


    public class ConnectionStringSetting
    {

        static ConnectionStringSetting()
        {

        }

        [Description("p:ConnectionStringSetting,l:en-us,k:Name,d:Unique name of the connection string setting")]
        [DisplayName("p:ConnectionStringSetting,l:en-us,k:Name,d:Connection string name")]
        public string Name { get; set; }

        [Description("p:ConnectionStringSetting,l:en-us,k:ConnectionStringValueDescription,d:Connection string value for connecting data")]
        [DisplayName("p:ConnectionStringSetting,l:en-us,k:ConnectionStringValueDisplay,d:Connection string value")]
        public string ConnectionString { get; set; }


        public string ConnectionStringWithoutCatalog { get => GetBuilderWithoutCatalog().ConnectionString; }


        public SqlConnectionStringBuilder GetBuilder() => new SqlConnectionStringBuilder(this.ConnectionString);

        public SqlConnectionStringBuilder GetBuilderWithoutCatalog()
        {
            var o = new SqlConnectionStringBuilder(this.ConnectionString);
            o.Remove("Initial Catalog");
            return o;
        }

        public SqlProcessor CreateProcessor() => new SqlProcessor(this.GetBuilder());

        public SqlProcessor CreateProcessorWithoutCatalog() => new SqlProcessor(this.GetBuilderWithoutCatalog());

        public static implicit operator ConnectionStringSetting(string connectionString)
        {
            return new ConnectionStringSetting() { Name = "No name", ConnectionString = connectionString };
        }

        public static ConnectionStringSetting Create(string host, string catalog)
        {

            var cnx = new SqlConnectionStringBuilder();
            cnx.DataSource = host;
            cnx.InitialCatalog = catalog;
            cnx.IntegratedSecurity = true;

            return new ConnectionStringSetting() { ConnectionString = cnx.ConnectionString };

        }
    }
}
