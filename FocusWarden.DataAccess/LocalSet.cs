using FocusWarden.DataAccess.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FocusWarden.DataAccess
{
    [Serializable]
    public class LocalSet<T> : ISetCollection<T> where T : ISetEntity
    {
        public List<T> Items { get; set; }

        public LocalSet()
        {
            Items = new List<T>();
        }

        public int Count => Items.Count;

        public bool IsReadOnly => false;

        public T this[int index] { get => Items[index]; set => Items[index]=value; }

        public void Add(T item)
        {
            Items.Add(item);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(T item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return Items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            Items.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return Items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }

        public void Update(T item)
        {
            var collectionItem = Items.Find(c => c.Id.Equals(item.Id));
            var collectionItemIndex = Items.IndexOf(collectionItem);
            Items.RemoveAt(collectionItemIndex);
            Items.Insert(collectionItemIndex, item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (Items as IEnumerable).GetEnumerator();
        }
    }
}