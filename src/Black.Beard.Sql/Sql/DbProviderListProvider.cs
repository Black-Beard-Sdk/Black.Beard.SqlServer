using Bb.ComponentModel;
using Bb.ComponentModel.Attributes;
using Bb.ComponentModel.DataAnnotations;
using Bb.ComponentModel.Translations;
using System.ComponentModel;
using System.Data.Common;

namespace Bb.Sql
{


    [ExposeClass(ConstantsCore.Initialization, ExposedType = typeof(DbProviderListProvider), LifeCycle = IocScopeEnum.Transiant)]
    public class DbProviderListProvider : IListProvider
    {


        public object? Instance { get; set; }


        public PropertyDescriptor Property { get; set; }


        public IEnumerable<ListItem> GetItems()
        {
            foreach (var item in DbProviderFactories.GetProviderInvariantNames())
                yield return new ListItem() { Value = item, Name = item, Display = item };
        }

    }
}
