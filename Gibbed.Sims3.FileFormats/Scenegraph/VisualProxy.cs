using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats.Scenegraph
{
    public class VisualProxyEntry0
    {
        public byte Unknown0;
        public List<ResourceKey> Unknown1;

        public void Serialize(Stream output, VisualProxy parent)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, VisualProxy parent)
        {
            this.Unknown0 = input.ReadValueU8();
            this.Unknown1 = new List<ResourceKey>();

            byte count = input.ReadValueU8();
            for (byte i = 0; i < count; i++)
            {
                if (parent.Version < 3)
                {
                    this.Unknown1.Add(input.ReadResourceKeyIGT());
                }
                else
                {
                    this.Unknown1.Add(parent.KeyTable.Keys[input.ReadValueS32()]);
                }
            }
        }
    }

    public class VisualProxy : IFormat
    {
        public UInt32 Version;
        public KeyTable KeyTable;
        public List<VisualProxyEntry0> UnknownEntry0;
        public List<ResourceKey> UnknownEntry1;

        public byte Unknown02;
        public float Unknown03;
        public float Unknown04;
        public float Unknown05;
        public float Unknown06;
        public float Unknown07;
        public float Unknown08;
        public UInt32 Unknown09;
        public bool Unknown10;
        public ResourceKey Unknown11;

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            this.Version = input.ReadValueU32();
            if (this.Version > 4)
            {
                throw new InvalidOperationException("bad version");
            }

            if (this.Version >= 3)
            {
                this.KeyTable = input.ReadKeyTable(4);
            }

            byte count = input.ReadValueU8();
            this.UnknownEntry0 = new List<VisualProxyEntry0>();
            this.UnknownEntry1 = new List<ResourceKey>();
            for (byte i = 0; i < count; i++)
            {
                byte type = input.ReadValueU8();

                if (type == 0)
                {
                    VisualProxyEntry0 entry0 = new VisualProxyEntry0();
                    entry0.Deserialize(input, this);
                    this.UnknownEntry0.Add(entry0);
                }
                else if (type == 1)
                {
                    if (this.Version < 3)
                    {
                        this.UnknownEntry1.Add(input.ReadResourceKeyIGT());
                    }
                    else
                    {
                        this.UnknownEntry1.Add(this.KeyTable.Keys[input.ReadValueS32()]);
                    }
                }
                else
                {
                    throw new InvalidOperationException("bad type");
                }
            }

            this.Unknown02 = input.ReadValueU8();
            this.Unknown03 = input.ReadValueF32();
            this.Unknown04 = input.ReadValueF32();
            this.Unknown05 = input.ReadValueF32();
            this.Unknown06 = input.ReadValueF32();
            this.Unknown07 = input.ReadValueF32();
            this.Unknown08 = input.ReadValueF32();

            if (this.Version >= 4)
            {
                this.Unknown09 = input.ReadValueU32();
            }

            if (this.Version >= 2)
            {
                this.Unknown10 = input.ReadValueBoolean();
                if (this.Unknown10 == true)
                {
                    if (this.Version < 3)
                    {
                        this.Unknown11 = input.ReadResourceKeyIGT();
                    }
                    else
                    {
                        this.Unknown11 = this.KeyTable.Keys[input.ReadValueS32()];
                    }
                }
            }
        }
    }
}
