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


        public SqlConnectionStringBuilder GetBuilder() => new SqlConnectionStringBuilder(this.ConnectionString);

        public SqlProcessor CreateProcessor() => new SqlProcessor(this.GetBuilder());

        public static implicit operator ConnectionStringSetting(string connectionString)
        {
            return new ConnectionStringSetting() { Name = "No name", ConnectionString = connectionString };
        }

    }
}
