using Bb.ComponentModel.DataAnnotations;
using Bb.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bb.ComponentModel.Attributes
{

    [ExposeClass(ConstantsCore.Initialization, ExposedType = typeof(ConnectionStringListProvider), LifeCycle = IocScopeEnum.Transiant)]
    public class ConnectionStringListProvider : IListProvider
    {

        public ConnectionStringListProvider(ConnectionSettings settings)
        {
            this._settings = settings;
        }

        public PropertyDescriptor Property { get; set; }

        public object Instance { get; set; }

        public IEnumerable<ListItem> GetItems()
        {

            foreach (var item in _settings.ConnectionStringSettings)
                yield return new ListItem() { Value = item.Name, Name = item.Name, Display = item.Name };

        }

        private readonly ConnectionSettings _settings;


    }

}
