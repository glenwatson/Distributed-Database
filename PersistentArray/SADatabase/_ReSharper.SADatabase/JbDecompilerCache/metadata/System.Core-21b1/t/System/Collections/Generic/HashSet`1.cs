// Type: System.Collections.Generic.HashSet`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Collections.Generic
{
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof (HashSetDebugView<>))]
    [Serializable]
    [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
    public class HashSet<T> : ISerializable, IDeserializationCallback, ISet<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        public HashSet();
        public HashSet(IEqualityComparer<T> comparer);
        public HashSet(IEnumerable<T> collection);
        public HashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer);
        protected HashSet(SerializationInfo info, StreamingContext context);
        public IEqualityComparer<T> Comparer { get; }

        #region IDeserializationCallback Members

        public virtual void OnDeserialization(object sender);

        #endregion

        #region ISerializable Members

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context);

        #endregion

        #region ISet<T> Members

        void ICollection<T>.Add(T item);
        public void Clear();
        public bool Contains(T item);
        public void CopyTo(T[] array, int arrayIndex);
        public bool Remove(T item);
        IEnumerator<T> IEnumerable<T>.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator();

        public bool Add(T item);
        public void UnionWith(IEnumerable<T> other);

        [SecurityCritical]
        public void IntersectWith(IEnumerable<T> other);

        public void ExceptWith(IEnumerable<T> other);

        [SecurityCritical]
        public void SymmetricExceptWith(IEnumerable<T> other);

        [SecurityCritical]
        public bool IsSubsetOf(IEnumerable<T> other);

        [SecurityCritical]
        public bool IsProperSubsetOf(IEnumerable<T> other);

        public bool IsSupersetOf(IEnumerable<T> other);

        [SecurityCritical]
        public bool IsProperSupersetOf(IEnumerable<T> other);

        public bool Overlaps(IEnumerable<T> other);

        [SecurityCritical]
        public bool SetEquals(IEnumerable<T> other);

        public int Count { get; }
        bool ICollection<T>.IsReadOnly { get; }

        #endregion

        public HashSet<T>.Enumerator GetEnumerator();

        public void CopyTo(T[] array);
        public void CopyTo(T[] array, int arrayIndex, int count);
        public int RemoveWhere(Predicate<T> match);
        public void TrimExcess();
        public static IEqualityComparer<HashSet<T>> CreateSetComparer();

        #region Nested type: Enumerator

        [Serializable]
        [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            #region IEnumerator<T> Members

            public void Dispose();
            public bool MoveNext();
            void IEnumerator.Reset();
            public T Current { get; }
            object IEnumerator.Current { get; }

            #endregion
        }

        #endregion
    }
}
