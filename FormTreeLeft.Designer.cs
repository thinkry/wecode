namespace WeCode1._0
{
    partial class FormTreeLeft
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTreeLeft));
            this.treeViewDir = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStripDir = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemNewDir = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemNewTxt = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemProp = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripTxt = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemTxtDel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTxtProp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTxtMark = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripTreeBlank = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemBlankNewDir = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripDir.SuspendLayout();
            this.contextMenuStripTxt.SuspendLayout();
            this.contextMenuStripTreeBlank.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewDir
            // 
            this.treeViewDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDir.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeViewDir.HideSelection = false;
            this.treeViewDir.Location = new System.Drawing.Point(0, 0);
            this.treeViewDir.Name = "treeViewDir";
            this.treeViewDir.Size = new System.Drawing.Size(335, 509);
            this.treeViewDir.TabIndex = 0;
            this.treeViewDir.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewDir_BeforeExpand);
            this.treeViewDir.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewDir_ItemDrag);
            this.treeViewDir.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDir_AfterSelect);
            this.treeViewDir.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewDir_NodeMouseClick);
            this.treeViewDir.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewDir_NodeMouseDoubleClick);
            this.treeViewDir.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewDir_DragDrop);
            this.treeViewDir.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewDir_DragOver);
            this.treeViewDir.DragLeave += new System.EventHandler(this.treeViewDir_DragLeave);
            this.treeViewDir.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewDir_MouseDown);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "打开.png");
            this.imageList1.Images.SetKeyName(1, "文本.png");
            // 
            // contextMenuStripDir
            // 
            this.contextMenuStripDir.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemNewDir,
            this.toolStripMenuItemNewTxt,
            this.toolStripMenuItemDel,
            this.toolStripMenuItemProp});
            this.contextMenuStripDir.Name = "contextMenuStripDir";
            this.contextMenuStripDir.Size = new System.Drawing.Size(125, 92);
            // 
            // toolStripMenuItemNewDir
            // 
            this.toolStripMenuItemNewDir.Name = "toolStripMenuItemNewDir";
            this.toolStripMenuItemNewDir.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemNewDir.Text = "新建目录";
            this.toolStripMenuItemNewDir.Click += new System.EventHandler(this.toolStripMenuItemNewDir_Click);
            // 
            // toolStripMenuItemNewTxt
            // 
            this.toolStripMenuItemNewTxt.Name = "toolStripMenuItemNewTxt";
            this.toolStripMenuItemNewTxt.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemNewTxt.Text = "新建文章";
            this.toolStripMenuItemNewTxt.Click += new System.EventHandler(this.toolStripMenuItemNewTxt_Click);
            // 
            // toolStripMenuItemDel
            // 
            this.toolStripMenuItemDel.Name = "toolStripMenuItemDel";
            this.toolStripMenuItemDel.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemDel.Text = "删除";
            this.toolStripMenuItemDel.Click += new System.EventHandler(this.toolStripMenuItemDel_Click);
            // 
            // toolStripMenuItemProp
            // 
            this.toolStripMenuItemProp.Name = "toolStripMenuItemProp";
            this.toolStripMenuItemProp.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemProp.Text = "属性";
            this.toolStripMenuItemProp.Click += new System.EventHandler(this.toolStripMenuItemProp_Click);
            // 
            // contextMenuStripTxt
            // 
            this.contextMenuStripTxt.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemTxtDel,
            this.toolStripMenuItemTxtProp,
            this.toolStripMenuItemTxtMark});
            this.contextMenuStripTxt.Name = "contextMenuStripTxt";
            this.contextMenuStripTxt.Size = new System.Drawing.Size(125, 70);
            // 
            // toolStripMenuItemTxtDel
            // 
            this.toolStripMenuItemTxtDel.Name = "toolStripMenuItemTxtDel";
            this.toolStripMenuItemTxtDel.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemTxtDel.Text = "删除";
            this.toolStripMenuItemTxtDel.Click += new System.EventHandler(this.toolStripMenuItemTxtDel_Click);
            // 
            // toolStripMenuItemTxtProp
            // 
            this.toolStripMenuItemTxtProp.Name = "toolStripMenuItemTxtProp";
            this.toolStripMenuItemTxtProp.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemTxtProp.Text = "属性";
            this.toolStripMenuItemTxtProp.Click += new System.EventHandler(this.toolStripMenuItemTxtProp_Click);
            // 
            // toolStripMenuItemTxtMark
            // 
            this.toolStripMenuItemTxtMark.Name = "toolStripMenuItemTxtMark";
            this.toolStripMenuItemTxtMark.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemTxtMark.Text = "加为书签";
            this.toolStripMenuItemTxtMark.Click += new System.EventHandler(this.toolStripMenuItemTxtMark_Click);
            // 
            // contextMenuStripTreeBlank
            // 
            this.contextMenuStripTreeBlank.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemBlankNewDir});
            this.contextMenuStripTreeBlank.Name = "contextMenuStripTreeBlank";
            this.contextMenuStripTreeBlank.Size = new System.Drawing.Size(125, 26);
            // 
            // toolStripMenuItemBlankNewDir
            // 
            this.toolStripMenuItemBlankNewDir.Name = "toolStripMenuItemBlankNewDir";
            this.toolStripMenuItemBlankNewDir.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemBlankNewDir.Text = "新建目录";
            this.toolStripMenuItemBlankNewDir.Click += new System.EventHandler(this.toolStripMenuItemBlankNewDir_Click);
            // 
            // FormTreeLeft
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 509);
            this.Controls.Add(this.treeViewDir);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Name = "FormTreeLeft";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
            this.Text = "目录";
            this.Load += new System.EventHandler(this.FormTreeLeft_Load);
            this.contextMenuStripDir.ResumeLayout(false);
            this.contextMenuStripTxt.ResumeLayout(false);
            this.contextMenuStripTreeBlank.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewDir;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDir;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNewDir;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNewTxt;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDel;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemProp;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTxt;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTxtDel;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTxtProp;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTxtMark;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTreeBlank;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemBlankNewDir;
    }
}