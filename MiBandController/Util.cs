using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace MiBandController
{
    public static class Util
    {
        public static void DebugIBuff(IBuffer buff)
        {
            DataReader dataReader = DataReader.FromBuffer(buff);
            byte[] data = new byte[dataReader.UnconsumedBufferLength];
            dataReader.ReadBytes(data);
            foreach (var item in data)
            {
                Console.Write(item.ToString() + " ");
            }
            Console.WriteLine("");
        }
    }
}
