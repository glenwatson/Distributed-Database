using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedConcurrency.Shared
{
    [Serializable]
    public class ObjectLocation : IComparable<ObjectLocation>
    {
        public ObjectType Type { get; set; }
        public string ObjectPath { get; set; }
        public int Index { get; set; }

        public ObjectLocation(ObjectType type, string objectPath, int index)
        {
            Type = type;
            ObjectPath = objectPath;
            Index = index;
        }

        public bool IsBefore(ObjectLocation objLocation)
        {
            return CompareTo(objLocation) < 0;            
        }

        public bool IsAfter(ObjectLocation objLocation)
        {
            return CompareTo(objLocation) > 0;
        }

        public int CompareTo(ObjectLocation objectLocation)
        {
            int isObjPathSame = String.CompareOrdinal(ObjectPath, objectLocation.ObjectPath);
            if (isObjPathSame == 0)
            {
                int isTypeSame = Type.CompareTo(objectLocation.Type);
                if (isTypeSame == 0)
                {
                    return Index - objectLocation.Index;                    
                }
                return isTypeSame;
            }
            return isObjPathSame;
        }

        public override bool Equals(object obj)
        {
            if (obj is ObjectLocation)
                return Equals((ObjectLocation) obj);
            return false;
        }

        public bool Equals(ObjectLocation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Type == Type && other.ObjectPath.Equals(ObjectPath) && other.Index == Index;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Type.GetHashCode();
                result = (result*397) ^ (ObjectPath != null ? ObjectPath.GetHashCode() : 0);
                result = (result*397) ^ Index;
                return result;
            }
        }
    }
    [Serializable]
    public enum ObjectType
    {
        FileWithHeader,
        Array,
        LinkedList,
        HashTable,
        Heap,
    }
}
