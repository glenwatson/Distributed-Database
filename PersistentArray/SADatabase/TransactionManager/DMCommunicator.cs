using System;
using System.Collections.Generic;
using System.IO;
using DistributedConcurrency;

namespace TransactionManager
{
    class DMCommunicator : ITransactionManager
    {
        private readonly IDictionary<DMLocation, DataManagerClient> _dmClientLookup = new SortedDictionary<DMLocation,DataManagerClient>();
        private const string ConfigFile = @"DMList.txt";
        public DMCommunicator()
        {
            FileStream fileStream = File.OpenRead(ConfigFile);
            StreamReader streamReader = new StreamReader(fileStream);
            List<DMLocation> locations = ParseStream(streamReader);
            InitDMClientSet(locations);
            fileStream.Close();
        }

        private List<DMLocation> ParseStream(StreamReader streamReader)
        {
            List<DMLocation> locations = new List<DMLocation>();
            while (!streamReader.EndOfStream)
            {
                string[] parts = streamReader.ReadLine().Split(',');
                string endpoint = parts[0];
                string remoteAddress = parts[1];
                locations.Add(new DMLocation(endpoint, remoteAddress));
            }
            return locations;
        }

        private void InitDMClientSet(List<DMLocation> locations)
        {
            foreach (DMLocation dmLocation in locations)
            {
                _dmClientLookup.Add(dmLocation, new DataManagerClient(dmLocation.EndpointName, dmLocation.RemoteAddress));
            }
        }

        private DataManagerClient GetDM(DMLocation location)
        {
            //TODO: lookup client in dictionary
            //return _dmClientLookup[location];
            DataManagerClient client = new DataManagerClient("Endpoint1", "localhost:5432");
            return client;
        }

        public ICollection<DMLocation> GetDMLocations()
        {
            return _dmClientLookup.Keys;
        }

        #region semi-proxy
        public byte Read(TMProxy.DataLocation dataLocation)
        {
            return GetDM(dataLocation._dmLocation).Read(dataLocation);
        }

        public void Write(TMProxy.DataLocation dataLocation, byte value)
        {
            GetDM(dataLocation.DmLocation).Write(dataLocation, value);
        }

        public void Begin(DMLocation dmLocation)
        {
            GetDM(dmLocation).Begin();
        }

        public void End(DMLocation dmLocation)
        {
            GetDM(dmLocation).End();
        }

        public void Abort(DMLocation dmLocation)
        {
            GetDM(dmLocation).Abort();
        }

        public void Restart(DMLocation dmLocation)
        {
            GetDM(dmLocation).Restart();
        }
        #endregion
    }
}
