using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DistributedConcurrency.Shared
{
    public class PrivateWorkspace : IEnumerable<Change>
    {
        private readonly ISet<Change> _changes = new SortedSet<Change>();

        public void AddChange(Change change)
        {
            _changes.Add(change);
        }

        public void RemoveAll()
        {
            _changes.Clear();
        }

        #region IEnumerable
        public IEnumerator<Change> GetEnumerator()
        {
            return _changes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        public Change GetDataLocation(DataLocation location)
        {
            return this.SingleOrDefault(c => c.Location.Equals(location));
        }
    }
}
