using Bb.SqlServerStructures;

namespace Bb.Extended
{

    public class ConnectionModel : NamedModelDescriptor
    {


        public string? UserId { get; set; }

        public string? Password { get; set; }

        public ConnectionStringSetting GetConnectionStringSetting()
        {
            return ConnectionStringSetting.Create((this.Parent as HostModel).Name, Name);
        }
    }



    public enum TypeHostEnum
    {
        PhysicalServer = 0,
        VirtualServer,
        DockerisedServer,
    }

}
