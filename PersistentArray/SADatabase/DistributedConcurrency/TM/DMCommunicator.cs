using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using DistributedConcurrency.Shared;

namespace DistributedConcurrency.TM
{
    class DMCommunicator : ITransactionManager
    {
        private readonly IDictionary<DMLocation, DataManagerClient> _dmClientLookup = new Dictionary<DMLocation,DataManagerClient>();
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
                string line = streamReader.ReadLine();
                if(!line.StartsWith("#"))
                    locations.Add(new DMLocation(line));
            }
            return locations;
        }

        private void InitDMClientSet(IEnumerable<DMLocation> locations)
        {
            foreach (DMLocation dmLocation in locations)
            {
                //try
                //{
                    _dmClientLookup.Add(dmLocation, new DataManagerClient(dmLocation));
                //}
                //catch (SocketException)
                //{
                //    Console.WriteLine("Could not connect to Data Manager at "+dmLocation.URI);
                //}
            }
        }

        private DataManagerClient GetDM(DMLocation location)
        {
            DataManagerClient client = _dmClientLookup[location];
            return client;
        }

        public ICollection<DMLocation> GetDMLocations()
        {
            return _dmClientLookup.Keys;
        }

        #region semi-proxy
        public byte Read(DataLocation dataLocation)
        {
            return GetDM(dataLocation.DmLocation).Read(dataLocation.ObjectLocation);
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

        public void StageChange(Change change)
        {
            GetDM(change.Location.DmLocation).StageChange(change);
        }

        public Vote EndStagingPhase(DMLocation dmLocation)
        {
            return GetDM(dmLocation).EndStagingPhase();
        }

        #endregion
    }

}
