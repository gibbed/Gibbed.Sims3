using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public class PropertyStream
    {
        private static class Hashes
        {
            public const UInt32 Array = 0x555CCDf4;
            public const UInt32 Child = 0xFFF75A95;
            public const UInt32 Bool = 0x68FE5F59;
            public const UInt32 Int32 = 0x0415642B;
            public const UInt32 UInt32 = 0xF1288606;
            public const UInt32 Int64 = 0x071568E6;
            public const UInt32 UInt64 = 0xEE28814F;
            public const UInt32 String = 0x15196597; // what is this?!
        }

        private short Version = 2;
        private long DebugOffset = 0;
        private Stream Data = new MemoryStream();
        private Dictionary<UInt32, int> Items;
        
        public void OutputItems()
        {
            foreach (KeyValuePair<UInt32, int> item in this.Items)
            {
                Console.WriteLine("{0:X8} @ {1}", item.Key, this.DebugOffset + item.Value);
            }
        }

        public void Read(Stream input)
        {
            this.Read(input, 0);
        }

        public void Read(Stream input, long debugOffset)
        {
            this.DebugOffset = debugOffset + input.Position;

            this.Version = input.ReadS16();
            if (this.Version < 1 || this.Version > 2)
            {
                throw new Exception();
            }
            int size = input.ReadS32();

            this.Data = new MemoryStream();
            this.Data.WriteS16(this.Version);
            this.Data.WriteS32(size);

            byte[] data = new byte[size - (4 + 2)];
            input.Read(data, 0, data.Length);
            this.Data.Write(data, 0, data.Length);

            this.Items = new Dictionary<UInt32, int>();
            short count = input.ReadS16();
            for (short i = 0; i < count; i++)
            {
                UInt32 hash = input.ReadU32();
                int offset = input.ReadS32();
                this.Items.Add(hash, offset);
            }
        }

        public void Write(Stream input)
        {
            throw new NotImplementedException();
        }

        // Items
        private bool SeekToItem(UInt32 hash)
        {
            if (this.Items.ContainsKey(hash) == false)
            {
                throw new Exception("hash not found");
                //return false;
            }

            this.Data.Seek(this.Items[hash], SeekOrigin.Begin);
            return true;
        }

        public PropertyStream GetChild(UInt32 hash)
        {
            this.SeekToItem(hash ^ Hashes.Child);
            PropertyStream result = new PropertyStream();
            result.Read(this.Data, this.DebugOffset);
            return result;
        }

        public bool GetBoolean(UInt32 hash)
        {
            this.SeekToItem(hash ^ Hashes.Bool);
            return this.Data.ReadBoolean();
        }

        public Int32 GetS32(UInt32 hash)
        {
            this.SeekToItem(hash ^ Hashes.Int32);
            return this.Data.ReadS32();
        }

        public Int32[] GetS32s(UInt32 hash)
        {
            this.SeekToItem(hash ^ Hashes.Int32 ^ Hashes.Array);

            int count = this.Data.ReadS32();
            Int32[] rez = new Int32[count];
            for (int i = 0; i < count; i++)
            {
                rez[i] = this.Data.ReadS32();
            }
            return rez;
        }

        public Int64 GetS64(UInt32 hash)
        {
            this.SeekToItem(hash ^ Hashes.Int64);
            return this.Data.ReadS64();
        }

        public UInt32 GetU32(UInt32 hash)
        {
            this.SeekToItem(hash ^ Hashes.UInt32);
            return this.Data.ReadU32();
        }

        public UInt64 GetU64(UInt32 hash)
        {
            this.SeekToItem(hash ^ Hashes.UInt64);
            return this.Data.ReadU64();
        }

        public string GetString(UInt32 hash)
        {
            this.SeekToItem(hash ^ Hashes.String);

            if (this.Version < 2)
            {
                throw new Exception("don't know how to handle this");
            }

            uint length = this.Data.ReadU32();
            return this.Data.ReadUTF16(length * 2);
        }
    }
}
