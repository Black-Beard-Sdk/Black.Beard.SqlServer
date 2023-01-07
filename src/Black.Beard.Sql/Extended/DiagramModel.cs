using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bb.Extended
{


    public class DiagramModel : NamedModelDescriptor
    {

        public DiagramModel()
        {
            DiskTables = new DiagramDiskTableListModel();
        }

        public DiagramDiskTableListModel DiskTables { get; }


    }


}
