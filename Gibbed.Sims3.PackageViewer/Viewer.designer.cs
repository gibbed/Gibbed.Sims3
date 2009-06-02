namespace Gibbed.Sims3.PackageViewer
{
    partial class Viewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Viewer));
            this.openDialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.typeList = new System.Windows.Forms.TreeView();
            this.fileList = new System.Windows.Forms.ListView();
            this.columnName = new System.Windows.Forms.ColumnHeader();
            this.columnGroup = new System.Windows.Forms.ColumnHeader();
            this.columnSize = new System.Windows.Forms.ColumnHeader();
            this.filesToolStrip = new System.Windows.Forms.ToolStrip();
            this.saveFileButton = new System.Windows.Forms.ToolStripButton();
            this.saveTheseButtons = new System.Windows.Forms.ToolStripSplitButton();
            this.saveAllTheseStripButton = new System.Windows.Forms.ToolStripMenuItem();
            this.saveKnownTheseStripButton = new System.Windows.Forms.ToolStripMenuItem();
            this.saveUnknownTheseStripButton = new System.Windows.Forms.ToolStripMenuItem();
            this.copyKnownNamesButton = new System.Windows.Forms.ToolStripButton();
            this.saveAllFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.openButton = new System.Windows.Forms.ToolStripButton();
            this.saveAllButton = new System.Windows.Forms.ToolStripButton();
            this.reloadButton = new System.Windows.Forms.ToolStripButton();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.filesToolStrip.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // openDialog
            // 
            this.openDialog.Filter = resources.GetString("openDialog.Filter");
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 25);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.typeList);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.fileList);
            this.splitContainer.Panel2.Controls.Add(this.filesToolStrip);
            this.splitContainer.Size = new System.Drawing.Size(792, 427);
            this.splitContainer.SplitterDistance = 240;
            this.splitContainer.TabIndex = 1;
            // 
            // typeList
            // 
            this.typeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typeList.Location = new System.Drawing.Point(0, 0);
            this.typeList.Name = "typeList";
            this.typeList.Size = new System.Drawing.Size(240, 427);
            this.typeList.TabIndex = 0;
            this.typeList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnSelectType);
            // 
            // fileList
            // 
            this.fileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnGroup,
            this.columnSize});
            this.fileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileList.Font = new System.Drawing.Font("Courier New", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileList.FullRowSelect = true;
            this.fileList.Location = new System.Drawing.Point(0, 25);
            this.fileList.Name = "fileList";
            this.fileList.Size = new System.Drawing.Size(548, 402);
            this.fileList.TabIndex = 0;
            this.fileList.UseCompatibleStateImageBehavior = false;
            this.fileList.View = System.Windows.Forms.View.Details;
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 340;
            // 
            // columnGroup
            // 
            this.columnGroup.Text = "Group";
            this.columnGroup.Width = 100;
            // 
            // columnSize
            // 
            this.columnSize.Text = "Size";
            this.columnSize.Width = 80;
            // 
            // filesToolStrip
            // 
            this.filesToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveFileButton,
            this.saveTheseButtons,
            this.copyKnownNamesButton});
            this.filesToolStrip.Location = new System.Drawing.Point(0, 0);
            this.filesToolStrip.Name = "filesToolStrip";
            this.filesToolStrip.Size = new System.Drawing.Size(548, 25);
            this.filesToolStrip.TabIndex = 1;
            this.filesToolStrip.Text = "toolStrip2";
            // 
            // saveFileButton
            // 
            this.saveFileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveFileButton.Image = global::Gibbed.Sims3.PackageViewer.Properties.Resources.Floppy;
            this.saveFileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveFileButton.Name = "saveFileButton";
            this.saveFileButton.Size = new System.Drawing.Size(23, 22);
            this.saveFileButton.Text = "Save";
            this.saveFileButton.ToolTipText = "Save file from package.";
            this.saveFileButton.Click += new System.EventHandler(this.OnSave);
            // 
            // saveTheseButtons
            // 
            this.saveTheseButtons.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveTheseButtons.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAllTheseStripButton,
            this.saveKnownTheseStripButton,
            this.saveUnknownTheseStripButton});
            this.saveTheseButtons.Image = global::Gibbed.Sims3.PackageViewer.Properties.Resources.Lightning;
            this.saveTheseButtons.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveTheseButtons.Name = "saveTheseButtons";
            this.saveTheseButtons.Size = new System.Drawing.Size(32, 22);
            this.saveTheseButtons.Text = "Save these.";
            this.saveTheseButtons.ToolTipText = "Save all files in package of this type.";
            this.saveTheseButtons.ButtonClick += new System.EventHandler(this.OnSaveTheseAll);
            // 
            // saveAllTheseStripButton
            // 
            this.saveAllTheseStripButton.Name = "saveAllTheseStripButton";
            this.saveAllTheseStripButton.Size = new System.Drawing.Size(152, 22);
            this.saveAllTheseStripButton.Text = "Save All";
            this.saveAllTheseStripButton.Click += new System.EventHandler(this.OnSaveTheseAll);
            // 
            // saveKnownTheseStripButton
            // 
            this.saveKnownTheseStripButton.Name = "saveKnownTheseStripButton";
            this.saveKnownTheseStripButton.Size = new System.Drawing.Size(152, 22);
            this.saveKnownTheseStripButton.Text = "Save Known";
            this.saveKnownTheseStripButton.Click += new System.EventHandler(this.OnSaveTheseKnown);
            // 
            // saveUnknownTheseStripButton
            // 
            this.saveUnknownTheseStripButton.Name = "saveUnknownTheseStripButton";
            this.saveUnknownTheseStripButton.Size = new System.Drawing.Size(152, 22);
            this.saveUnknownTheseStripButton.Text = "Save Unknown";
            this.saveUnknownTheseStripButton.Click += new System.EventHandler(this.OnSaveTheseUnknown);
            // 
            // copyKnownNamesButton
            // 
            this.copyKnownNamesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyKnownNamesButton.Image = global::Gibbed.Sims3.PackageViewer.Properties.Resources.Clipboard;
            this.copyKnownNamesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyKnownNamesButton.Name = "copyKnownNamesButton";
            this.copyKnownNamesButton.Size = new System.Drawing.Size(23, 22);
            this.copyKnownNamesButton.Text = "Copy known names in this view to the clipboard.";
            this.copyKnownNamesButton.Click += new System.EventHandler(this.OnCopyKnownList);
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openButton,
            this.saveAllButton,
            this.reloadButton});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(792, 25);
            this.mainToolStrip.TabIndex = 2;
            this.mainToolStrip.Text = "toolStrip1";
            // 
            // openButton
            // 
            this.openButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openButton.Image = global::Gibbed.Sims3.PackageViewer.Properties.Resources.Folder;
            this.openButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(23, 22);
            this.openButton.Text = "Open";
            this.openButton.ToolTipText = "Open package.";
            this.openButton.Click += new System.EventHandler(this.OnOpen);
            // 
            // saveAllButton
            // 
            this.saveAllButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveAllButton.Image = global::Gibbed.Sims3.PackageViewer.Properties.Resources.Lightning;
            this.saveAllButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveAllButton.Name = "saveAllButton";
            this.saveAllButton.Size = new System.Drawing.Size(23, 22);
            this.saveAllButton.Text = "Save All";
            this.saveAllButton.ToolTipText = "Save all files in package.";
            this.saveAllButton.Click += new System.EventHandler(this.OnSaveAll);
            // 
            // reloadButton
            // 
            this.reloadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.reloadButton.Image = global::Gibbed.Sims3.PackageViewer.Properties.Resources.Refresh;
            this.reloadButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.Size = new System.Drawing.Size(23, 22);
            this.reloadButton.Text = "Reload file lists";
            this.reloadButton.ToolTipText = "Reload file lists.";
            this.reloadButton.Click += new System.EventHandler(this.OnReloadFileLists);
            // 
            // Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 452);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.mainToolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Viewer";
            this.Text = "Package Viewer";
            this.Load += new System.EventHandler(this.OnLoad);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            this.splitContainer.ResumeLayout(false);
            this.filesToolStrip.ResumeLayout(false);
            this.filesToolStrip.PerformLayout();
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.OpenFileDialog openDialog;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.TreeView typeList;
		private System.Windows.Forms.ListView fileList;
		private System.Windows.Forms.ColumnHeader columnName;
		private System.Windows.Forms.ColumnHeader columnSize;
		private System.Windows.Forms.ColumnHeader columnGroup;
        private System.Windows.Forms.FolderBrowserDialog saveAllFolderDialog;
		private System.Windows.Forms.ToolStrip filesToolStrip;
		private System.Windows.Forms.ToolStripButton saveFileButton;
		private System.Windows.Forms.ToolStrip mainToolStrip;
		private System.Windows.Forms.ToolStripButton openButton;
        private System.Windows.Forms.ToolStripButton saveAllButton;
        private System.Windows.Forms.ToolStripButton reloadButton;
        private System.Windows.Forms.ToolStripButton copyKnownNamesButton;
        private System.Windows.Forms.ToolStripSplitButton saveTheseButtons;
        private System.Windows.Forms.ToolStripMenuItem saveAllTheseStripButton;
        private System.Windows.Forms.ToolStripMenuItem saveKnownTheseStripButton;
        private System.Windows.Forms.ToolStripMenuItem saveUnknownTheseStripButton;
    }
}

