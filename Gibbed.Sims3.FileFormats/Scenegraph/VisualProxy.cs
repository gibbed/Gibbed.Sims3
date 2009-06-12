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
            this.Unknown0 = input.ReadU8();
            this.Unknown1 = new List<ResourceKey>();

            byte count = input.ReadU8();
            for (byte i = 0; i < count; i++)
            {
                if (parent.Version < 3)
                {
                    this.Unknown1.Add(input.ReadResourceKeyIGT());
                }
                else
                {
                    this.Unknown1.Add(parent.KeyTable.Keys[input.ReadS32()]);
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
        public UInt32 Unknown03;
        public UInt32 Unknown04;
        public UInt32 Unknown05;
        public UInt32 Unknown06;
        public UInt32 Unknown07;
        public UInt32 Unknown08;
        public UInt32 Unknown09;
        public bool Unknown10;
        public ResourceKey Unknown11;

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            this.Version = input.ReadU32();
            if (this.Version > 4)
            {
                throw new InvalidOperationException("bad version");
            }

            if (this.Version >= 3)
            {
                this.KeyTable = input.ReadKeyTable(4);
            }

            byte count = input.ReadU8();
            this.UnknownEntry0 = new List<VisualProxyEntry0>();
            this.UnknownEntry1 = new List<ResourceKey>();
            for (byte i = 0; i < count; i++)
            {
                byte type = input.ReadU8();

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
                        this.UnknownEntry1.Add(this.KeyTable.Keys[input.ReadS32()]);
                    }
                }
                else
                {
                    throw new InvalidOperationException("bad type");
                }
            }

            this.Unknown02 = input.ReadU8();
            this.Unknown03 = input.ReadU32();
            this.Unknown04 = input.ReadU32();
            this.Unknown05 = input.ReadU32();
            this.Unknown06 = input.ReadU32();
            this.Unknown07 = input.ReadU32();
            this.Unknown08 = input.ReadU32();

            if (this.Version >= 4)
            {
                this.Unknown09 = input.ReadU32();
            }

            if (this.Version >= 2)
            {
                this.Unknown10 = input.ReadBoolean();
                if (this.Unknown10 == true)
                {
                    if (this.Version < 3)
                    {
                        this.Unknown11 = input.ReadResourceKeyIGT();
                    }
                    else
                    {
                        this.Unknown11 = this.KeyTable.Keys[input.ReadS32()];
                    }
                }
            }
        }
    }
}
