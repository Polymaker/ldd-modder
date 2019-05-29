using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    public static class StreamExtensions
    {
        public static float[] ReadSingles(this BinaryReader reader, int count)
        {
            var array = new float[count];
            for (int i = 0; i < count; i++)
                array[i] = reader.ReadSingle(); 
            return array;
        }

        public static int[] ReadInts(this BinaryReader reader, int count)
        {
            var array = new int[count];
            for (int i = 0; i < count; i++)
                array[i] = reader.ReadInt32();
            return array;
        }

        public static string ReadNullTerminatedString(this BinaryReader stream)
        {
            string str = "";
            char ch;
            while ((ch = stream.ReadChar()) != 0)
                str += ch;
            
            return str;
        }

        public static void Skip(this Stream stream, int count)
        {
            stream.Seek(count, SeekOrigin.Current);
        }
    }
}
