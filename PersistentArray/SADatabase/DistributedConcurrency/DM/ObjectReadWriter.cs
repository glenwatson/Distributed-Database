using System;
using DistributedConcurrency.Shared;

namespace DistributedConcurrency.DM
{
    class ObjectReadWriter
    {
        public byte Read(ObjectLocation objLocation)
        {

            byte returnValue = (byte) new Random(objLocation.Index + objLocation.ObjectPath.Length + (int) objLocation.Type).Next(256);
            //byte returnValue = 0;
            switch (objLocation.Type)
            {
                //open the appropriate type of object and read the data inside it
                case ObjectType.FileWithHeader:

                    break;
                case ObjectType.Array:

                    break;
                case ObjectType.LinkedList:

                    break;
                case ObjectType.HashTable:

                    break;
                case ObjectType.Heap:

                    break;
            }
            return returnValue;
        }

        public void Write(ObjectLocation objLocation, byte value)
        {
            //write the value to the appropriate object
        }
    }
}
