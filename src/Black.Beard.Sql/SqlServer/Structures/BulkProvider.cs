using Bb.SqlServer.Bulks;
using Bb.SqlServerStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bb.SqlServer.Structures
{

    public static class BulkProvider
    {

        public static BulkWriter GetBulkLoader(this ConnectionStringSetting setting)
        {
            BulkWriter loader = new BulkWriter(setting);
            return loader;
        }


    }

}
