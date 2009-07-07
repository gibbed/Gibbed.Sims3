using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Gibbed.Helpers;
using Gibbed.PortableExecutable;

namespace Gibbed.Sims3.ListNatives
{
    public class Executable
    {
        public ImageDosHeader DosHeader;
        public ImageNTHeaders32 NTHeaders;
        public List<ImageSectionHeader> Sections;

        public uint GetFileOffset(uint virtualAddress)
        {
            foreach (ImageSectionHeader section in this.Sections)
            {
                if (virtualAddress >= (this.NTHeaders.OptionalHeader.ImageBase + section.VirtualAddress) && virtualAddress < (this.NTHeaders.OptionalHeader.ImageBase + section.VirtualAddress + section.VirtualSize))
                {
                    return section.PointerToRawData + (virtualAddress - (this.NTHeaders.OptionalHeader.ImageBase + section.VirtualAddress));
                }
            }

            return 0;
        }

        public void Read(Stream input)
        {
            this.DosHeader = input.ReadStructure<ImageDosHeader>();
            if (this.DosHeader.Magic != 0x5A4D) // MZ
            {
                throw new FormatException("dos header has bad magic");
            }

            input.Seek(this.DosHeader.NewExeOffset, SeekOrigin.Begin);
            this.NTHeaders = input.ReadStructure<ImageNTHeaders32>();
            if (this.NTHeaders.Signature != 0x4550 || this.NTHeaders.FileHeader.SizeOfOptionalHeader != 0xE0) // PE
            {
                throw new FormatException("nt header has bad signature");
            }
            else if (this.NTHeaders.OptionalHeader.Magic != 0x10B) // IMAGE_NT_OPTIONAL_HDR32_MAGIC
            {
                throw new FormatException("optional header has bad magic");
            }

            this.Sections = new List<ImageSectionHeader>();
            for (int i = 0; i < this.NTHeaders.FileHeader.NumberOfSections; i++)
            {
                this.Sections.Add(input.ReadStructure<ImageSectionHeader>());
            }
        }
    }
}
