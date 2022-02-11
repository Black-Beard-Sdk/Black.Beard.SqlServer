
using Bb.ComponentModel;
using Bb.ComponentModel.Attributes;

namespace Bb.Sql
{

    [Configuration(ConfigurationKey = nameof(ConnectionSettings), TypeSerialisation = ConfigurationAttribute.TypeSerialisationToJson)]
    //[ExposeClass(ConstantsCore.Configuration, ExposedType = typeof(ConnectionSettings), LifeCycle = IocScopeEnum.Transiant)]
    public partial class ConnectionSettings
    {

        public ConnectionSettings()
        {
            this.ConnectionStringSettings = new ConnectionStringSettings();
        }

        public ConnectionStringSetting? this [string key] => ConnectionStringSettings.GetConnectionString(key);

        public ConnectionStringSettings ConnectionStringSettings { get;set; }

    }
}
