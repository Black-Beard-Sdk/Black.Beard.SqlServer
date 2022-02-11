using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Bb.Sql
{
    public class RegisterSqlServer
    {

        static RegisterSqlServer()
        {           
            string invariantName = nameof(SqlClientFactory);
            if (!DbProviderFactories.GetProviderInvariantNames().Any(c => c == invariantName))
                DbProviderFactories.RegisterFactory(invariantName, SqlClientFactory.Instance);
        }

    }

}
