using Bb.ComponentModel.Accessors;
using System.Linq.Expressions;

namespace Bb.SqlServer.Bulks
{
    public class DbDataReaderIdResolver<T>
    {


        public DbDataReaderIdResolver(IEnumerable<T> items, (string, Expression<Func<T, object>>) id, params (string, Expression<Func<T, object>>)[] columns)
        {

            this._items = items;

            this.IdColumn = id.Item1;
            this.Columns = columns.Select(c => c.Item1).ToList();
            this._columns = new Dictionary<string, AccessorItem>();

            var accessors = AccessorItem.Get(typeof(T), true);
            Queue<AccessorItem> queue = new Queue<AccessorItem>();

            _idAccessor = accessors[id.Item2.GetProperty().Name];
            foreach (var item in columns)
            {
                var property = item.Item2.GetProperty();
                var a = property.Name;
                var col = accessors[item.Item2.GetProperty().Name];
                _columns.Add(item.Item1, col);
                queue.Enqueue(col);
            }

        }

        private IEnumerable<T> _items;

        public string IdColumn { get; }
        private readonly AccessorItem _idAccessor;


        public List<string> Columns { get; }
        private readonly Dictionary<string, AccessorItem> _columns;

        internal void Map(ContainerTree c)
        {

            foreach (var item in this._items)
                if (item != null)
                {

                    var o = _idAccessor.GetValue(item);
                    if (o == null)
                    {

                        var current = c;

                        foreach (var col in _columns)
                        {
                            current = current.Get(col.Value.GetValue(item));
                            if (current == null)
                                break;
                        }

                        if (current != null)
                            _idAccessor.SetValue(item, current.Id);

                    }
                }
            
            
        }

    }


}
