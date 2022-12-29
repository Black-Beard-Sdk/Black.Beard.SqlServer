using System.Collections;

namespace Bb.SqlServerStructures
{
    public class ScriptItemList : IEnumerable<ScriptItems>
    {

        public ScriptItemList()
        {
            this._items = new List<ScriptItems>();
        }

        public IEnumerator<ScriptItems> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        internal void Add(ScriptItems current)
        {
            _items.Add(current);
        }

        private List<ScriptItems> _items;

    }

}