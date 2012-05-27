using System;

namespace DistributedConcurrency.Shared
{
    [Serializable]
    public struct DMLocation : IComparable<DMLocation>
    {
        //public string Address { get; private set; }
        //public int Port { get; private set; }
        public Uri URI { get; private set; }

        public DMLocation(string uri) : this()
        {
            URI = new Uri(uri);
        }

        public bool IsBefore(DMLocation endLocation)
        {
            return CompareTo(endLocation) < 0;
        }

        public bool IsAfter(DMLocation startLocation)
        {
            return CompareTo(startLocation) > 0;
        }

        public int CompareTo(DMLocation dmLocation)
        {
            int isAddrSame = String.CompareOrdinal(URI.AbsolutePath, dmLocation.URI.AbsolutePath);
            if (isAddrSame == 0)
            {
                return URI.Port - dmLocation.URI.Port;
            }
            return isAddrSame;
        }

        public override bool Equals(object obj)
        {
            if (obj is DMLocation)
                return Equals((DMLocation) obj);
            return false;
        }

        public bool Equals(DMLocation other)
        {
            return CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            return (URI != null ? URI.GetHashCode() : 0);
        }
    }
}
