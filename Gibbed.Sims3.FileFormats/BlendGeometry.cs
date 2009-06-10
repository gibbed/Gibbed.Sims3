using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public class BlendGeometry : IFormat
    {
        public bool LittleEndian;
        public UInt32 Version;

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            long baseOffset = input.Position;

            string magic = input.ReadASCII(4);
            if (magic != "BGEO" && magic != "OEGB")
            {
                throw new InvalidDataException("not a blend geometry");
            }

            this.LittleEndian = (magic == "BGEO");

            this.Version = input.ReadU32(this.LittleEndian);
            if (this.Version >= 0x400)
            {
                throw new InvalidOperationException("bad version");
            }

            UInt32 unknown1 = input.ReadU32(this.LittleEndian);
            UInt32 unknown2 = input.ReadU32(this.LittleEndian); // should always be 4
            UInt32 unknown3 = input.ReadU32(this.LittleEndian);
            UInt32 unknown4 = input.ReadU32(this.LittleEndian);
            UInt32 unknown5 = input.ReadU32(this.LittleEndian); // should always be 8
            UInt32 unknown6 = input.ReadU32(this.LittleEndian); // should always be 12
            UInt32 unknown7 = input.ReadU32(this.LittleEndian);
            UInt32 unknown8 = input.ReadU32(this.LittleEndian);
            UInt32 unknown9 = input.ReadU32(this.LittleEndian);

            if (unknown2 != 4)
            {
                throw new InvalidOperationException();
            }

            if (unknown5 != 8)
            {
                throw new InvalidOperationException();
            }

            if (unknown6 != 12)
            {
                throw new InvalidOperationException();
            }

            if (unknown1 > 0)
            {
                UInt32 _unknown3 = unknown3;
                UInt32 _unknown4 = unknown4;

                input.Seek(baseOffset + unknown7, SeekOrigin.Begin);

                for (int i = 0; i < unknown1; i++)
                {
                    UInt32 unknown10 = input.ReadU32(this.LittleEndian);
                    UInt32 unknown11 = input.ReadU32(this.LittleEndian);
                    // unknown5 represents previous reads size (8 bytes)

                    for (int j = 0; j < 4; j++) // unknown2 ?
                    {
                        UInt32 unknown12 = input.ReadU32(this.LittleEndian);
                        UInt32 unknown13 = input.ReadU32(this.LittleEndian);
                        UInt32 unknown14 = input.ReadU32(this.LittleEndian);
                        // unknown6 represents previous reads size (12 bytes)

                        if (unknown13 > _unknown3)
                        {
                            throw new InvalidOperationException();
                        }

                        if (unknown14 > _unknown4)
                        {
                            throw new InvalidOperationException();
                        }

                        _unknown3 -= unknown13;
                        _unknown4 -= unknown14;
                    }
                }
            }

            if (this.Version < 0x300)
            {
                input.Seek(baseOffset + unknown8, SeekOrigin.Begin);
                for (uint i = 0; i < unknown3; i++)
                {
                    input.ReadU32(this.LittleEndian);
                }
            }
            else
            {
                input.Seek(baseOffset + unknown8, SeekOrigin.Begin);
                for (uint i = 0; i < unknown3; i++)
                {
                    input.ReadU16(this.LittleEndian);
                }
            }

            if (this.Version == 0x100)
            {
                input.Seek(baseOffset + unknown9, SeekOrigin.Begin);
                for (uint i = 0; i < unknown4; i++)
                {
                    input.ReadF32(this.LittleEndian);
                    input.ReadF32(this.LittleEndian);
                    input.ReadF32(this.LittleEndian);
                }
            }
            else
            {
                input.Seek(baseOffset + unknown9, SeekOrigin.Begin);
                for (uint i = 0; i < unknown4; i++)
                {
                    input.ReadU16(this.LittleEndian);
                    input.ReadU16(this.LittleEndian);
                    input.ReadU16(this.LittleEndian);
                }
            }
        }
    }
}
