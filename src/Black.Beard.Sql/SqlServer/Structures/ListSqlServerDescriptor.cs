using System.Collections;

namespace Bb.SqlServer.Structures
{
    public class ListModelDescriptor<T> : SqlServerDescriptor, IList<T>, IList, IReadOnlyList<T>
        where T : SqlServerDescriptor
    {


        public ListModelDescriptor(int capacity)
        {
            this._items = new List<T>(capacity);
        }

        public ListModelDescriptor()
        {
            this._items = new List<T>();
        }

        public ListModelDescriptor(IEnumerable<T> collection)
        {
            this._items = new List<T>(collection);
        }

        public T this[int index] { get => _items[index]; set => _items[index] = value; }

        public int Count => _items.Count;

        public bool IsReadOnly => (_items as IList).IsReadOnly;

        public bool IsFixedSize => (_items as IList).IsFixedSize;

        public bool IsSynchronized => (_items as IList).IsSynchronized;

        public object SyncRoot => (_items as IList).SyncRoot;

        object? IList.this[int index] { get => (_items as IList)[index]; set => (_items as IList)[index] = value; }

        public bool Exists(Predicate<T> match) => _items.Exists(match);

        public T? Find(Predicate<T> match) => _items.Find(match);

        public List<T> FindAll(Predicate<T> match) => _items.FindAll(match);

        public int FindIndex(Predicate<T> match) => _items.FindIndex(match);

        public T? FindLast(Predicate<T> match) => _items.FindLast(match);

        public int FindLastIndex(Predicate<T> match) => _items.FindLastIndex(match);

        public int FindLastIndex(int startIndex, Predicate<T> match) => _items.FindLastIndex(startIndex, match);

        public int FindLastIndex(int startIndex, int count, Predicate<T> match) => _items.FindLastIndex(startIndex, count, match);

        public void ForEach(Action<T> action) => _items.ForEach(action);

        public List<T> GetRange(int index, int count) => _items.GetRange(index, count);

        public int IndexOf(T item) => _items.IndexOf(item);

        public void Add(T item) => _items.Add(item);

        public void AddRange(IEnumerable<T> items) => _items.AddRange(items);

        public void Clear() => _items.Clear();

        public bool Contains(T item) => _items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        public void Insert(int index, T item) => _items.Insert(index, item);

        public bool Remove(T item) => _items.Remove(item);

        public void RemoveAt(int index) => _items.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

        public int Add(object? value) => (_items as IList).Add(value);

        public bool Contains(object? value) => (_items as IList).Contains(value);

        public int IndexOf(object? value) => (_items as IList).IndexOf(value);

        public void Insert(int index, object? value) => (_items as IList).Insert(index, value);

        public void Remove(object? value) => (_items as IList).Remove(value);

        public void CopyTo(Array array, int index) => (_items as IList).CopyTo(array, index);

        private readonly List<T> _items;

    }

}