using DistributedConcurrency;
using DistributedConcurrency.Shared;

namespace WCFDataManager
{
    public class Lock
    {
        public DataLocation StartLocation { get; set; }
        public DataLocation EndLocation { get; set; }

        public Lock(DataLocation location)
        {
            StartLocation = location;
            EndLocation = location;
        }

        public Lock(DataLocation startLocation, DataLocation endLocation)
        {
            StartLocation = startLocation;
            EndLocation = endLocation;
        }

        public override bool Equals(object obj)
        {
            if (obj is Lock)
            {
                Lock lock2 = obj as Lock;
                return StartLocation.Equals(lock2.StartLocation) &&
                       EndLocation.Equals(lock2.EndLocation);
            }
            return false;
        }

        public bool Overlap(Lock lock2)
        {
            return Overlap(lock2.StartLocation) || Overlap(lock2.EndLocation);
        }

        public bool Overlap(DataLocation location)
        {
            return StartLocation.IsBefore(location) && EndLocation.IsAfter(location);
        }

        public bool Equals(Lock other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.StartLocation.Equals(StartLocation) && other.EndLocation.Equals(EndLocation);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (StartLocation.GetHashCode()*397) ^ EndLocation.GetHashCode();
            }
        }
    }
}