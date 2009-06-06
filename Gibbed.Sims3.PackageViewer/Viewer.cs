using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.XPath;
using Gibbed.Helpers;
using Gibbed.Sims3.FileFormats;
using System.Text;
using Gibbed.RefPack;

namespace Gibbed.Sims3.PackageViewer
{
	public partial class Viewer : Form
	{
		public Viewer()
		{
			InitializeComponent();
		}

		private Font MonospaceFont = new Font(FontFamily.GenericMonospace, 9.0f);

    	private void LoadLookup()
		{
            Lookup.Reset();
            string basePath = Path.Combine(Application.StartupPath, "lists");

            string[] files;

            files = Directory.GetFiles(Path.Combine(basePath, "fnv32"), "*.txt", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                Lookup.LoadFiles32(file);
            }

            files = Directory.GetFiles(Path.Combine(basePath, "fnv64"), "*.txt", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                Lookup.LoadFiles64(file);
            }

            Lookup.LoadGroups(Path.Combine(basePath, "groups.xml"));
            Lookup.LoadTypes(Path.Combine(basePath, "types.xml"));
		}
		
		private void OnLoad(object sender, EventArgs e)
		{
			string path;
			path = (string)Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Sims\\The Sims 3", "Install Dir", null);
			if (path != null && path.Length > 0)
			{
				this.openDialog.InitialDirectory = path;
			}

            this.LoadLookup();
		}

        private void OnReloadFileLists(object sender, EventArgs e)
        {
            this.LoadLookup();
        }

        private void AddMapNames(DatabasePackedFile dbpf, Stream input)
        {
            foreach (DatabasePackedFile.Entry entry in dbpf.Entries.FindAll(entry => entry.Key.TypeId == 0x0166038C))
            {
                MemoryStream memory = new MemoryStream();

                input.Seek(entry.Offset, SeekOrigin.Begin);
                if (entry.Compressed)
                {
                    byte[] decompressed = input.RefPackDecompress();
                    memory.Write(decompressed, 0, decompressed.Length);
                }
                else
                {
                    input.Seek(entry.Offset, SeekOrigin.Begin);
                    int left = (int)entry.DecompressedSize;
                    byte[] data = new byte[4096];
                    while (left > 0)
                    {
                        int block = Math.Min(4096, left);
                        input.Read(data, 0, block);
                        memory.Write(data, 0, block);
                        left -= block;
                    }
                }

                memory.Seek(0, SeekOrigin.Begin);
                KeyNameMapFile keyNameMap = new KeyNameMapFile();
                keyNameMap.Read(memory);

                foreach (KeyValuePair<UInt64, string> value in keyNameMap.Map)
                {
                    if (value.Value.FNV64() == value.Key)
                    {
                        Lookup.Files[value.Key] = value.Value;
                    }
                }
            }
        }

        // A stupid way to do it but it's for the Save All function.
        private DatabasePackedFile.Entry[] DatabaseFiles;

		private void OnOpen(object sender, EventArgs e)
		{
			if (this.openDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			if (this.openDialog.InitialDirectory != null)
			{
				this.openDialog.InitialDirectory = null;
			}

			Stream input = this.openDialog.OpenFile();

			DatabasePackedFile dbpf = new DatabasePackedFile();
			dbpf.Read(input);

            this.AddMapNames(dbpf, input);

			this.DatabaseFiles = dbpf.Entries.ToArray();

            Dictionary<string, TreeNode> categoryNodes = new Dictionary<string, TreeNode>();
			Dictionary<uint, TreeNode> unknownTypeNodes = new Dictionary<uint, TreeNode>();

			this.typeList.Nodes.Clear();
			this.typeList.BeginUpdate();

			TreeNode knownNode = new TreeNode("Known");
			TreeNode unknownNode = new TreeNode("Unknown");

			for (int i = 0; i < this.DatabaseFiles.Length; i++)
			{
                DatabasePackedFile.Entry index = this.DatabaseFiles[i];

				TreeNode typeNode = null;

                if (Lookup.Types.ContainsKey(index.Key.TypeId) == true)
                {
                    TypeLookup type = Lookup.Types[index.Key.TypeId];
                    
                    TreeNode categoryNode;

                    if (categoryNodes.ContainsKey(type.Category) == false)
                    {
                        categoryNode = new TreeNode();
                        categoryNode.Text = type.Category;
                        categoryNode.Tag = new Dictionary<uint, TreeNode>();
						knownNode.Nodes.Add(categoryNode);
                        categoryNodes[type.Category] = categoryNode;
                    }
                    else
                    {
                        categoryNode = categoryNodes[type.Category];
                    }

                    Dictionary<uint, TreeNode> typeNodes = categoryNode.Tag as Dictionary<uint, TreeNode>;

                    if (typeNodes.ContainsKey(index.Key.TypeId) == false)
				    {
                        typeNode = new TreeNode();
                        typeNode.Text = type.Description == null ? type.Directory : type.Description;
                        typeNode.Tag = new List<DatabasePackedFile.Entry>();
						typeNode.NodeFont = this.MonospaceFont;
						categoryNode.Nodes.Add(typeNode);
                        typeNodes[index.Key.TypeId] = typeNode;
                    }
                    else
                    {
                        typeNode = typeNodes[index.Key.TypeId];
                    }
                }
                else
                {
				    if (unknownTypeNodes.ContainsKey(index.Key.TypeId) == false)
				    {
                        typeNode = new TreeNode();
                        typeNode.Text = "#" + index.Key.TypeId.ToString("X8");
                        typeNode.Tag = new List<DatabasePackedFile.Entry>();
						typeNode.NodeFont = this.MonospaceFont;
						unknownNode.Nodes.Add(typeNode);
                        unknownTypeNodes[index.Key.TypeId] = typeNode;
                    }
                    else
                    {
                        typeNode = unknownTypeNodes[index.Key.TypeId];
                    }
                }

                List<DatabasePackedFile.Entry> files = typeNode.Tag as List<DatabasePackedFile.Entry>;
				files.Add(index);
			}

			if (knownNode.Nodes.Count > 0)
			{
				this.typeList.Nodes.Add(knownNode);
			}

			if (unknownNode.Nodes.Count > 0)
			{
				this.typeList.Nodes.Add(unknownNode);
			}

			this.typeList.Sort();
			this.typeList.EndUpdate();

			if (knownNode.Nodes.Count > 0)
			{
				knownNode.Expand();
			}
			else if (unknownNode.Nodes.Count > 0)
			{
				unknownNode.Expand();
			}
		}

		private void OnSelectType(object sender, TreeViewEventArgs e)
		{
			if (e.Node == null || e.Node.Tag == null)
			{
				return;
			}

            if (!(e.Node.Tag is List<DatabasePackedFile.Entry>))
            {
                return;
            }

            List<DatabasePackedFile.Entry> files = e.Node.Tag as List<DatabasePackedFile.Entry>;

			this.fileList.BeginUpdate();
			this.fileList.Items.Clear();
			this.fileList.Sorting = SortOrder.None;

            foreach (DatabasePackedFile.Entry file in files)
			{
				ListViewItem listViewItem = new ListViewItem("");

                if (Lookup.Files.ContainsKey(file.Key.InstanceId))
				{
                    listViewItem.Text = Lookup.Files[file.Key.InstanceId];
				}
				else
				{
                    listViewItem.Text = "#" + file.Key.InstanceId.ToString("X16");
				}

                if (Lookup.Groups.ContainsKey(file.Key.GroupId))
				{
                    listViewItem.SubItems.Add(Lookup.Groups[file.Key.GroupId]);
				}
				else
				{
                    listViewItem.SubItems.Add("#" + file.Key.GroupId.ToString("X8")).Font = this.MonospaceFont;
				}

				listViewItem.SubItems.Add(file.DecompressedSize.ToString());
				listViewItem.Tag = file;

				this.fileList.Items.Add(listViewItem);
			}

			this.fileList.Sorting = SortOrder.Ascending;
			this.fileList.Sort();
			this.fileList.EndUpdate();
		}

		private void OnSave(object sender, EventArgs e)
		{

		}

		private void OnSaveAll(object sender, EventArgs e)
		{
			if (this.saveAllFolderDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			Stream input = this.openDialog.OpenFile();

			if (input == null)
			{
				return;
			}

			string basePath = this.saveAllFolderDialog.SelectedPath;

			SaveAllProgress progress = new SaveAllProgress();
			progress.ShowSaveProgress(this, input, this.DatabaseFiles, basePath);

			input.Close();
		}

        private void OnSaveTheseAll(object sender, EventArgs e)
        {
            if (this.saveAllFolderDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Stream input = this.openDialog.OpenFile();

            if (input == null)
            {
                return;
            }

            string basePath = this.saveAllFolderDialog.SelectedPath;

            DatabasePackedFile.Entry[] files = new DatabasePackedFile.Entry[this.fileList.Items.Count];
            for (int i = 0; i < this.fileList.Items.Count; i++)
            {
                files[i] = (DatabasePackedFile.Entry)(this.fileList.Items[i].Tag);
            }

            SaveAllProgress progress = new SaveAllProgress();
            progress.ShowSaveProgress(this, input, files, basePath);

            input.Close();
        }

        private void OnSaveTheseKnown(object sender, EventArgs e)
        {
            if (this.saveAllFolderDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Stream input = this.openDialog.OpenFile();

            if (input == null)
            {
                return;
            }

            string basePath = this.saveAllFolderDialog.SelectedPath;

            List<DatabasePackedFile.Entry> files = new List<DatabasePackedFile.Entry>();
            for (int i = 0; i < this.fileList.Items.Count; i++)
            {
                DatabasePackedFile.Entry index = (DatabasePackedFile.Entry)(this.fileList.Items[i].Tag);

                if (Lookup.Files.ContainsKey(index.Key.InstanceId) == true)
                {
                    files.Add(index);
                }
            }

            SaveAllProgress progress = new SaveAllProgress();
            progress.ShowSaveProgress(this, input, files.ToArray(), basePath);

            input.Close();
        }

        private void OnSaveTheseUnknown(object sender, EventArgs e)
        {
            if (this.saveAllFolderDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Stream input = this.openDialog.OpenFile();

            if (input == null)
            {
                return;
            }

            string basePath = this.saveAllFolderDialog.SelectedPath;

            List<DatabasePackedFile.Entry> files = new List<DatabasePackedFile.Entry>();
            for (int i = 0; i < this.fileList.Items.Count; i++)
            {
                DatabasePackedFile.Entry index = (DatabasePackedFile.Entry)(this.fileList.Items[i].Tag);

                if (Lookup.Files.ContainsKey(index.Key.InstanceId) == false)
                {
                    files.Add(index);
                }
            }

            SaveAllProgress progress = new SaveAllProgress();
            progress.ShowSaveProgress(this, input, files.ToArray(), basePath);

            input.Close();
        }

        private void OnCopyKnownList(object sender, EventArgs e)
        {
            List<string> names = new List<string>();

            for (int i = 0; i < this.fileList.Items.Count; i++)
            {
                DatabasePackedFile.Entry index = (DatabasePackedFile.Entry)(this.fileList.Items[i].Tag);
                if (Lookup.Files.ContainsKey(index.Key.InstanceId))
                {
                    names.Add(Lookup.Files[index.Key.InstanceId]);
                }
            }

            names.Sort();

            StringBuilder output = new StringBuilder();
            foreach (string name in names)
            {
                output.AppendLine(name);
            }

            Clipboard.SetText(output.ToString(), TextDataFormat.UnicodeText);
        }
	}
}
