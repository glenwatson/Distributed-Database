// Type: System.Collections.Generic.LinkedList`1
// Assembly: System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.dll

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Collections.Generic
{
    [DebuggerTypeProxy(typeof (System_CollectionDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
    [ComVisible(false)]
    [Serializable]
    public class LinkedList<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable, ISerializable, IDeserializationCallback
    {
        public LinkedList();
        public LinkedList(IEnumerable<T> collection);
        protected LinkedList(SerializationInfo info, StreamingContext context);
        public LinkedListNode<T> First { get; }
        public LinkedListNode<T> Last { get; }

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index);
        bool ICollection.IsSynchronized { get; }
        object ICollection.SyncRoot { get; }

        #endregion

        #region ICollection<T> Members

        void ICollection<T>.Add(T value);
        public void Clear();
        public bool Contains(T value);
        public void CopyTo(T[] array, int index);
        IEnumerator<T> IEnumerable<T>.GetEnumerator();
        public bool Remove(T value);
        IEnumerator IEnumerable.GetEnumerator();
        public int Count { get; }
        bool ICollection<T>.IsReadOnly { get; }

        #endregion

        #region IDeserializationCallback Members

        public virtual void OnDeserialization(object sender);

        #endregion

        #region ISerializable Members

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context);

        #endregion

        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value);
        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode);
        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value);
        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode);
        public LinkedListNode<T> AddFirst(T value);
        public void AddFirst(LinkedListNode<T> node);
        public LinkedListNode<T> AddLast(T value);
        public void AddLast(LinkedListNode<T> node);
        public LinkedListNode<T> Find(T value);
        public LinkedListNode<T> FindLast(T value);
        public LinkedList<T>.Enumerator GetEnumerator();
        public void Remove(LinkedListNode<T> node);
        public void RemoveFirst();
        public void RemoveLast();

        #region Nested type: Enumerator

        [Serializable]
        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator, ISerializable, IDeserializationCallback
        {
            #region IDeserializationCallback Members

            void IDeserializationCallback.OnDeserialization(object sender);

            #endregion

            #region IEnumerator<T> Members

            public bool MoveNext();
            void IEnumerator.Reset();
            public void Dispose();
            public T Current { get; }
            object IEnumerator.Current { get; }

            #endregion

            #region ISerializable Members

            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context);

            #endregion
        }

        #endregion
    }
}
