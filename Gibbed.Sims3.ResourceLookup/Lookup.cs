using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.XPath;
using Gibbed.Helpers;
using Gibbed.Sims3.FileFormats;
using System.Text;

namespace Gibbed.Sims3.ResourceLookup
{
    public class Lookup
    {
        public static Dictionary<UInt64, string> Files;
        public static Dictionary<uint, string> Groups;
        public static Dictionary<uint, TypeLookup> Types;

        static Lookup()
        {
            Reset();
        }

        public static void Reset()
        {
            Files = new Dictionary<ulong, string>();
            Groups = new Dictionary<uint, string>();
            Types = new Dictionary<uint, TypeLookup>();
        }

        public static void LoadFiles32(string path)
        {
            if (File.Exists(path) == false)
            {
                return;
            }

            TextReader reader = new StreamReader(path);
            while (true)
            {
                string line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }

                line = line.Trim();
                if (line.Length == 0)
                {
                    continue;
                }

                UInt64 hash = line.HashFNV32();
                Files[hash] = line;
            }

            reader.Close();
        }

        public static void LoadFiles64(string path)
        {
            if (File.Exists(path) == false)
            {
                return;
            }

            TextReader reader = new StreamReader(path);
            while (true)
            {
                string line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }

                line = line.Trim();
                if (line.Length == 0)
                {
                    continue;
                }

                UInt64 hash = line.HashFNV64();
                Files[hash] = line;
            }

            reader.Close();
        }

        public static void LoadGroups(string path)
        {
            if (File.Exists(path) == false)
            {
                return;
            }

            XPathDocument document = new XPathDocument(path);
            XPathNavigator navigator = document.CreateNavigator();
            XPathNodeIterator nodes = navigator.Select("/names/name");

            while (nodes.MoveNext())
            {
                uint id;
                string key = nodes.Current.GetAttribute("id", "");
                string value = nodes.Current.Value;

                if (key.StartsWith("(hash(") && key.EndsWith("))"))
                {
                    string tmp = key.Substring(6, key.Length - 8);
                    id = tmp.HashFNV32();
                }
                else
                {
                    id = key.ParseHex32();
                }

                Groups[id] = value;
            }
        }

        private static string GetNodeValue(XPathNavigator node, string xpath, string defaultValue)
        {
            node = node.SelectSingleNode(xpath);
            if (node == null)
            {
                return defaultValue;
            }

            string value = node.Value.Trim();
            if (value.Length == 0)
            {
                return defaultValue;
            }

            return value;
        }

        public static void LoadTypes(string path)
        {
            if (File.Exists(path) == false)
            {
                return;
            }

            XPathDocument document = new XPathDocument(path);
            XPathNavigator navigator = document.CreateNavigator();
            XPathNodeIterator nodes = navigator.Select("/types/type");

            while (nodes.MoveNext())
            {
                XPathNavigator node = nodes.Current;

                string key = node.GetAttribute("id", "").Trim();
                uint id = key.ParseHex32();

                if (Types.ContainsKey(id) == true)
                {
                    throw new InvalidOperationException("duplicate type 0x" + id.ToString("X8"));
                }

                TypeLookup type;
                type.Category = GetNodeValue(node, "category", "unknown");
                type.FourCC = GetNodeValue(node, "fourcc", "UNKN");
                type.Extension = GetNodeValue(node, "extension", null);
                type.Directory = GetNodeValue(node, "directory", "#" + id.ToString("X8"));
                type.Description = GetNodeValue(node, "description", null);

                if (Types.Count(check => (check.Value.Category == type.Category && check.Value.Directory == type.Directory)) > 0)
                {
                    throw new InvalidOperationException("duplicate directory " + type.Directory + " for category " + type.Category);
                }

                Types[id] = type;
            }
        }
    }
}
