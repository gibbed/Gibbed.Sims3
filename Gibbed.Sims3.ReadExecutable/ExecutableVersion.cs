using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using Gibbed.Helpers;

namespace Gibbed.Sims3.ReadExecutable
{
    [XmlRoot(ElementName = "executable")]
    public class ExecutableVersion
    {
        [XmlIgnore]
        public Version Version;

        [XmlAttribute(AttributeName = "version")]
        public string _Version
        {
            get
            {
                return this.Version.ToString();
            }

            set
            {
                this.Version = new Version(value);
            }
        }

        [XmlIgnore]
        public List<UInt32> ScriptTables = null;

        [XmlArray(ElementName = "script_tables")]
        [XmlArrayItem(ElementName = "script_table")]
        public string[] _ScriptTables
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                this.ScriptTables = new List<UInt32>();
                foreach (string table in value)
                {
                    this.ScriptTables.Add(table.ParseHex32());
                }
            }
        }

        [XmlIgnore]
        public UInt32 LocaleTable = 0;

        [XmlElement(ElementName = "local_table")]
        public string _LocaleTable
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                this.LocaleTable = value.ParseHex32();
            }
        }
    }
}
