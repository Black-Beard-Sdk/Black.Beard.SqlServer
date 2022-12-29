using System.Collections;

namespace Bb.SqlServerStructures
{
    public class ScriptItems : IEnumerable<ScriptItem>
    {


        public ScriptItems()
        {
            this._items = new Queue<ScriptItem>();
        }


        public void Add(ScriptItem item)
        {

            this._items.Enqueue(item);

            if (item.IsCreateDatabase || item.IsAlterDatabase)
                this.UseTransaction = false;

        }


        public bool CanBeAdded(ScriptItem newItem)
        {

            if (this._items.Count == 0)
                return true;

            var last = _items.Last();

            if (last.IsUseDatabase)
                return true;

            else if (last.IsCreateDatabase)
            {

                if (newItem.IsCreateDatabase)
                    return true;

                if (newItem.IsAlterDatabase)
                    return true;

                return false;

            }

            //else if (last.IsAlterDatabase)
            if (newItem.IsUseDatabase)
                return false;

            return true;

        }

        public IEnumerator<ScriptItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        private Queue<ScriptItem> _items;

        public bool UseTransaction { get; private set; } = true;
    }

}