using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CodeBase2
{
   public static class ByteArrays
    {
        /// <summary>
        /// This method takes a stream and converts it to a byte array.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] StreamToByteArray(Stream stream)
        {
            int streamLength = (int)stream.Length;
            byte[] byteArray = new byte[streamLength];
            stream.Read(byteArray, 0, streamLength);
            stream.Close();

            return byteArray;
        }

        public static string ByteArrayToString(byte[] bytes)
        {
                return new ASCIIEncoding().GetString(bytes); 
        }

        public static byte[] StringToByteArray(string s)
        {
            return new ASCIIEncoding().GetBytes(s); 
        }

    }
}
