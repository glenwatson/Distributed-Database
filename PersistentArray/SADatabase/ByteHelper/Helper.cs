using System;
using System.Linq;
using ByteHelper.Exceptions;

namespace ByteHelper
{
    public static class Helper
    {
        public static int ToInt(this byte[] bytes)
        {
            int result = 0;

            result += bytes[0];
            result = result << 8;
            result += bytes[1];
            result = result << 8;
            result += bytes[2];
            result = result << 8;
            result += bytes[3];

            return result;
        }

        public static byte[] ToBytes(this int i)
        {
            byte[] result = new byte[4];
            result[3] = (byte)i;
            i = i >> 8;
            result[2] = (byte)i;
            i = i >> 8;
            result[1] = (byte)i;
            i = i >> 8;
            result[0] = (byte)i;
            return result;
        }

        public static int GetHash(this byte[] source)
        {
            // set-up
            byte[] working = new byte[4];
            byte[] staging = new byte[working.Length];
            //loop through source in groups of HashCodeMax
            for (int sourceIdx = 0; sourceIdx < source.Length; sourceIdx += working.Length)
            {
                // copy as much as you can from source's current position into staging (up to HashCodeMax)
                for (int idx = sourceIdx; idx < staging.Length && sourceIdx < source.Length; idx++)
                    staging[idx] = source[idx];
                // XOR the staging into working
                for (int idx = 0; idx < staging.Length; idx++)
                    working[idx] ^= staging[idx];
            }

            int hash = working.ToInt();
            // hash can't be zero because zero designates empty space in my hash table
            hash = hash == 0 ? 1 : hash;
            //hash can't be negative
            hash = hash < 0 ? -1 * hash : hash;
            return hash;
        }

        public static byte[] Copy(this byte[] source)
        {
            byte[] copy = new byte[source.Length];
            Array.Copy(source, copy, source.Length);
            return copy;
        }

        public static byte[] ExtendTo(this byte[] source, int newLength)
        {
            if (source.Length > newLength)
                throw new SizeTooSmallException("The new length is less than the source's length");
            Array.Resize(ref source, newLength);
            return source;

            //if(source.Length == newLength)
            //{
            //    return source;
            //}
            //byte[] extended = new byte[newLength];
            //Array.Copy(source, extended, source.Length);
            //return extended;
        }

        public static byte[] Append(this byte[] bytes, byte[] b2)
        {
            byte[] result = new byte[bytes.Length + b2.Length];
            Array.Copy(bytes, 0, result, 0, bytes.Length);
            Array.Copy(b2, 0, result, bytes.Length, b2.Length);
            return result;
        }

        public static byte[] Append(this byte[] bytes, byte[] b2, byte[] b3)
        {
            byte[] result = new byte[bytes.Length + b2.Length + b3.Length];
            Array.Copy(bytes, 0, result, 0, bytes.Length);
            Array.Copy(b2, 0, result, bytes.Length, b2.Length);
            Array.Copy(b3, 0, result, bytes.Length + b2.Length, b3.Length);
            return result;
        }

        /// <summary>
        /// returns a sub-array of the byte array
        /// </summary>
        /// <param name="source">the original array to pull the sub array from</param>
        /// <param name="startIdx">the index in the source to start on, inclusive</param>
        /// <param name="endIdx">the index in the source to end on, exclusive</param>
        /// <returns></returns>
        public static byte[] SubArray(this byte[] source, int startIdx, int endIdx)
        {
            if (startIdx > endIdx)
                throw new InvalidRangeException("startIdx must be greater than endIdx");
            if (startIdx > source.Length)
                throw new IndexOutOfRangeException("startIdx is too large");
            if (endIdx > source.Length)
                throw new IndexOutOfRangeException("endIdx is too large");
            byte[] subArray = new byte[endIdx - startIdx];
            Array.Copy(source, startIdx, subArray, 0, subArray.Length);
            return subArray;
        }

        public static void CopyInto(this byte[] source, byte[] destination)
        {
            if (source.Length > destination.Length)
                throw new IndexOutOfRangeException();
            Array.Copy(source, destination, source.Length);
        }

        public static bool EqualsBytes(this byte[] original, byte[] candidate)
        {
            if (original.Length != candidate.Length)
                return false;

            return !original.Where((t, i) => t != candidate[i]).Any();
        }
    }
}
