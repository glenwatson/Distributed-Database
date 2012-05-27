using System;
using System.Runtime.Serialization;

namespace DistributedConcurrency.Shared
{
    [Serializable]
    public struct DataLocation : IComparable<DataLocation>
    {
        private DMLocation _dmLocation;
// ReSharper disable FieldCanBeMadeReadOnly.Local (breaks serialization)
        private ObjectLocation _objectLocation;
// ReSharper restore FieldCanBeMadeReadOnly.Local

        public DMLocation DmLocation
        {
            get { return _dmLocation; }
        }

        public ObjectLocation ObjectLocation
        {
            get { return _objectLocation; }
        }

        public DataLocation(DMLocation dmLocation, ObjectLocation objectLocation)
        {
            _dmLocation = dmLocation;
            _objectLocation = objectLocation;
        }

        public bool IsBefore(DataLocation endLocation)
        {
            bool isDmLocationSame = _dmLocation.IsBefore(endLocation._dmLocation);
            if (isDmLocationSame)
                return _objectLocation.IsBefore(endLocation._objectLocation);
            return false;
        }

        public bool IsAfter(DataLocation startLocation)
        {
            bool isDmLocationSame = _dmLocation.IsAfter(startLocation._dmLocation);
            if (isDmLocationSame)
                return _objectLocation.IsAfter(startLocation._objectLocation);
            return false;
        }

        public int CompareTo(DataLocation other)
        {
            int comparison = _dmLocation.CompareTo(other._dmLocation);
            if (comparison == 0)
            {
                return _objectLocation.CompareTo(other._objectLocation);
            }
            return comparison;
        }

        public override bool Equals(object obj)
        {
            if (obj is DataLocation)
                return Equals((DataLocation) obj);
            return false;
        }

        public bool Equals(DataLocation other)
        {
            return _dmLocation.Equals(other._dmLocation) && _objectLocation.Equals(other._objectLocation);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_dmLocation.GetHashCode()*397) ^ (_objectLocation != null ? _objectLocation.GetHashCode() : 0);
            }
        }
    }
}
