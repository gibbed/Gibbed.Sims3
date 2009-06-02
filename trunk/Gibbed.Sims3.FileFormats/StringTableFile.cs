using System;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Sims3.FileFormats
{
    public class StringTableFile
    {
        public short Version = 2;
        #region private bool LittleEndian
        private bool LittleEndian
        {
            get
            {
                if (this.Version <= 1)
                {
                    return false;
                }

                return true;
            }
        }
        #endregion

        public void Read(Stream input)
        {
            if (input.ReadASCII(4) != "STBL")
            {
                throw new Exception();
            }

            this.Version = input.ReadS16();
            
            byte unk1 = input.ReadU8();
            if (unk1 > 1)
            {
                throw new Exception();
            }
            else if (unk1 == 1)
            {
                UInt64 unk2 = input.ReadU64(LittleEndian);
                byte unk3 = input.ReadU8();
                byte unk4 = input.ReadU8();
            }
        }
    }
}
