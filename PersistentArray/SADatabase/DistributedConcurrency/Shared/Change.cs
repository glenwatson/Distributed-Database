using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DistributedConcurrency.Shared
{
    [Serializable]
    public class Change : IComparer<Change>, IComparable<Change>
    {
        public DataLocation Location { get; set;}
        public byte Value { get; set; }
        public bool IsWrite { get; set; }
        public bool IsRead { get { return !IsWrite; } }

        public Change(DataLocation location, byte value, bool isWrite)
        {
            Location = location;
            Value = value;
            IsWrite = isWrite;
        }

        public int Compare(Change x, Change y)
        {
            return x.CompareTo(y);
        }

        public int CompareTo(Change other)
        {
            int locationComparison = Location.CompareTo(other.Location);
            if (locationComparison == 0)
                return IsWrite == other.IsWrite ? 0 : 1;
            return locationComparison;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Change)) return false;
            return Equals((Change) obj);
        }

        public bool Equals(Change other)
        {
            return CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Location.GetHashCode();
                result = (result*397) ^ Value.GetHashCode();
                result = (result*397) ^ IsWrite.GetHashCode();
                return result;
            }
        }
    }
}
