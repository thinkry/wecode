namespace WeCode1._0
{
    partial class YouDaoTree
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YouDaoTree));
            this.treeViewYouDao = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStripYDdir = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripYDtxt = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripYDblank = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripYDdir.SuspendLayout();
            this.contextMenuStripYDtxt.SuspendLayout();
            this.contextMenuStripYDblank.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewYouDao
            // 
            this.treeViewYouDao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewYouDao.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeViewYouDao.ImageIndex = 0;
            this.treeViewYouDao.ImageList = this.imageList1;
            this.treeViewYouDao.Location = new System.Drawing.Point(0, 0);
            this.treeViewYouDao.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeViewYouDao.Name = "treeViewYouDao";
            this.treeViewYouDao.SelectedImageIndex = 0;
            this.treeViewYouDao.Size = new System.Drawing.Size(372, 775);
            this.treeViewYouDao.TabIndex = 0;
            this.treeViewYouDao.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewYouDao_ItemDrag);
            this.treeViewYouDao.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewYouDao_AfterSelect);
            this.treeViewYouDao.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewYouDao_NodeMouseDoubleClick);
            this.treeViewYouDao.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewYouDao_DragDrop);
            this.treeViewYouDao.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewYouDao_DragOver);
            this.treeViewYouDao.DragLeave += new System.EventHandler(this.treeViewYouDao_DragLeave);
            this.treeViewYouDao.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewYouDao_MouseDown);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "打开.png");
            this.imageList1.Images.SetKeyName(1, "文本.png");
            // 
            // contextMenuStripYDdir
            // 
            this.contextMenuStripYDdir.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4});
            this.contextMenuStripYDdir.Name = "contextMenuStripYDdir";
            this.contextMenuStripYDdir.Size = new System.Drawing.Size(125, 92);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem1.Text = "新建目录";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem2.Text = "新建文章";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem3.Text = "删除";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem4.Text = "属性";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // contextMenuStripYDtxt
            // 
            this.contextMenuStripYDtxt.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem5});
            this.contextMenuStripYDtxt.Name = "contextMenuStripYDtxt";
            this.contextMenuStripYDtxt.Size = new System.Drawing.Size(125, 70);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem6.Text = "删除";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem7.Text = "属性";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem5.Text = "加为书签";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // contextMenuStripYDblank
            // 
            this.contextMenuStripYDblank.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem8});
            this.contextMenuStripYDblank.Name = "contextMenuStripYDblank";
            this.contextMenuStripYDblank.Size = new System.Drawing.Size(125, 26);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem8.Text = "新建目录";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
            // 
            // YouDaoTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 775);
            this.Controls.Add(this.treeViewYouDao);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "YouDaoTree";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
            this.Text = "有道云";
            this.Activated += new System.EventHandler(this.YouDaoTree_Activated);
            this.Deactivate += new System.EventHandler(this.YouDaoTree_Deactivate);
            this.Load += new System.EventHandler(this.YouDaoTree_Load);
            this.contextMenuStripYDdir.ResumeLayout(false);
            this.contextMenuStripYDtxt.ResumeLayout(false);
            this.contextMenuStripYDblank.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewYouDao;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripYDdir;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripYDtxt;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripYDblank;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
    }
}