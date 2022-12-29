using System.Collections;
using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Dacpacs
{

    public class DacListOfModel<T> : ModelBase, IEnumerable<T>
        where T : ModelBase
    {

        public DacListOfModel(string key) : base(key)
        {
            _list = new List<T>();
        }

        public T GetOrCreate<T1>(Func<T1, bool> predicate)
            where T1 : T
        {

            var item = _list.OfType<T1>().FirstOrDefault(predicate);
            if (item == null)
            {
                item = Create<T1>();
                Add(item);
            }

            return item;

        }

        protected virtual T1 Create<T1>()
            where T1 : T
        {
            throw new NotImplementedException();
        }

        public DacListOfModel<T> Add(T item)
        {
            _list.Add(item);
            return this;
        }

        public override XElement Serialize()
        {

            var xml = new XElement(XName.Get(Key));

            foreach (var item in this)
            {
                xml.Add(item.Serialize());
            }


            return xml;

        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        private List<T> _list;

    }



}
