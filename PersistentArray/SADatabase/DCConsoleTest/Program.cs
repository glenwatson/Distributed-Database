using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistributedConcurrency.DM;
using DistributedConcurrency.Shared;
using DistributedConcurrency.TM;

namespace DCConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {

            DataManager dm = new DataManager(11000);
            dm.Start();
            dm.GetLocation();

            DMLocation dmLocation = new DMLocation("127.0.0.1:11000");//(@"db://[fe80::a91e:3a94:e27a:9035%13]:11000");
            ObjectLocation objectLocation0 = new ObjectLocation(ObjectType.FileWithHeader, @"C:\DB\arrayName.db", 0);
            ObjectLocation objectLocation1 = new ObjectLocation(ObjectType.FileWithHeader, @"C:\DB\arrayName.db", 1);
            DataLocation dataLocation0 = new DataLocation(dmLocation, objectLocation0);
            DataLocation dataLocation1 = new DataLocation(dmLocation, objectLocation1);

            //First transaction
            Transaction t = new Transaction();
            t.Begin();

            t.Write(dataLocation0, 42);
            t.Write(dataLocation1, 77);
            Console.WriteLine("Read: " + t.Read(dataLocation0));
            Console.WriteLine("Read: " + t.Read(dataLocation1));

            //Second transaction
            Transaction t2 = new Transaction();
            t2.Begin();
            t2.Write(dataLocation1, 99);
            Console.WriteLine("Read: " + t2.Read(dataLocation1));

            t.End();

            t2.End();

            Console.ReadLine();
        }
    }
}
