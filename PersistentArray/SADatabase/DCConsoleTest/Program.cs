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

            DataManager.GetInstance().Start();

            Transaction t = new Transaction();
            t.Begin();
            DMLocation dmLocation = new DMLocation(@"db://[fe80::c00d:7562:2dc5:dcb7%24]:11000");
            ObjectLocation objectLocation0 = new ObjectLocation(ObjectType.FileWithHeader, @"C:\DB\arrayName.db", 0);
            ObjectLocation objectLocation1 = new ObjectLocation(ObjectType.FileWithHeader, @"C:\DB\arrayName.db", 1);
            DataLocation dataLocation0 = new DataLocation(dmLocation, objectLocation0);
            DataLocation dataLocation1 = new DataLocation(dmLocation, objectLocation1);

            
            t.Write(dataLocation0, 42);
            t.Write(dataLocation1, 77);
            Console.WriteLine("Read: " + t.Read(dataLocation0));
            Console.WriteLine("Read: " + t.Read(dataLocation1));

            t.End();

            Console.ReadLine();
        }
    }
}
