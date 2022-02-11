using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using Bb.ComponentModel.Attributes;
using Bb.ComponentModel.DataAnnotations;

namespace Bb.Sql
{


    public class ConnectionStringSetting
    {             

        [Description("p:ConnectionStringSetting,l:en-us,k:Name,d:Name of the connection string")]
        [DisplayName("p:ConnectionStringSetting,l:en-us,k:Name,d:Connection string name")]
        public string Name { get; set; }

        [Description("p:ConnectionStringSetting,l:en-us,k:ConnectionStringValueDescription,d:Connection string value for connecting data")]
        [DisplayName("p:ConnectionStringSetting,l:en-us,k:ConnectionStringValueDisplay,d:Connection string value")]
        public string ConnectionString { get; set; }

        [Description("p:ConnectionStringSetting,l:en-us,k:Name,d:Name of the data provider")]
        [DisplayName("p:ConnectionStringSetting,l:en-us,k:ConnectionStringValue,d:Provider name")]
        [ListProvider(typeof(DbProviderListProvider))]
        public string ProviderName { get; set; }


        public DbProviderFactory GetProvider()
        {
            return DbProviderFactories.GetFactory(ProviderName);
        }

        public DbConnection GetConnection(bool open = false)
        {
            var provider = GetProvider();
            var cnx = provider.CreateConnection();
            cnx.ConnectionString = ConnectionString;
            if (open)
                cnx.Open();
            return cnx;
        }

    }
}
