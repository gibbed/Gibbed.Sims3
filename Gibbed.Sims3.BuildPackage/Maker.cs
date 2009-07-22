using System;
using System.IO;
using System.Xml.XPath;
using Gibbed.Helpers;
using Gibbed.Sims3.FileFormats;

namespace Gibbed.Sims3.BuildPackage
{
	public class Maker
	{
		public void Build(string filesPath, string outputPath)
		{
			string filesBasePath = Path.GetDirectoryName(filesPath);
			Stream output = new FileStream(outputPath, FileMode.Create, FileAccess.ReadWrite);

            Database db = new Database(output, false);

			XPathDocument document = new XPathDocument(filesPath);
			XPathNavigator navigator = document.CreateNavigator();
			XPathNodeIterator nodes = navigator.Select("/files/file");

			while (nodes.MoveNext())
			{
				string groupText = nodes.Current.GetAttribute("groupid", "");
				string instanceText = nodes.Current.GetAttribute("instanceid", "");
				string typeText = nodes.Current.GetAttribute("typeid", "");

				if (groupText == null || instanceText == null || typeText == null)
				{
					throw new InvalidDataException("file missing attributes");
				}

                ResourceKey key;
                key.GroupId = groupText.ParseHex32();
                key.InstanceId = instanceText.ParseHex64();
                key.TypeId = typeText.ParseHex32();

				string inputPath;
				if (Path.IsPathRooted(nodes.Current.Value) == false)
				{
					// relative path, it should be relative to the XML file
					inputPath = Path.Combine(filesBasePath, nodes.Current.Value);
				}
				else
				{
					inputPath = nodes.Current.Value;
				}

				if (File.Exists(inputPath) == false)
				{
					Console.WriteLine(inputPath + " does not exist");
                    continue;
				}

				Stream input = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
                byte[] data = new byte[input.Length];
                input.Read(data, 0, data.Length);
                db.SetResource(key, data);
                input.Close();
			}

            db.Commit(true);
			output.Close();
		}
	}
}
