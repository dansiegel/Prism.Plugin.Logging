using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace SampleApp.Collections
{
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private Dictionary<TKey, TValue> _collection = new Dictionary<TKey, TValue>();

        public TValue this[TKey key]
        {
            get => _collection[key];
            set => Add(key, value);
        }

        public ICollection<TKey> Keys => _collection.Keys;
        public ICollection<TValue> Values => _collection.Values;
        public int Count => _collection.Count;
        public bool IsReadOnly => false;

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(TKey key, TValue value)
        {
            _collection.Add(key, value);
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Count)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Keys)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Values)));
        }

        public void Add(KeyValuePair<TKey, TValue> item) =>
            Add(item.Key, item.Value);

        public void Clear()
        {
            _collection.Clear();
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Count)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Keys)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Values)));
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => _collection.Contains(item);

        public bool ContainsKey(TKey key) => _collection.ContainsKey(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _collection.GetEnumerator();

        public bool Remove(TKey key)
        {
            var result = _collection.Remove(key);

            if (result)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Count)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Keys)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Values)));
            }

            return result;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

        public bool TryGetValue(TKey key, out TValue value) => _collection.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();
    }
}
