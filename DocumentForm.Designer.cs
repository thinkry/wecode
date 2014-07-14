namespace WeCode1._0
{
    partial class DocumentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentForm));
            this.scintilla1 = new ScintillaNET.Scintilla();
            this.contextMenuStripMouseClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemSerch = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemSeleAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripTab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemTabClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTabCloseE = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTabCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemTabSv = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTabSvAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTabRdOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemTabAddMark = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.scintilla1)).BeginInit();
            this.contextMenuStripMouseClick.SuspendLayout();
            this.contextMenuStripTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // scintilla1
            // 
            this.scintilla1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.scintilla1.ContextMenuStrip = this.contextMenuStripMouseClick;
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.scintilla1.LineWrapping.VisualFlags = ScintillaNET.LineWrappingVisualFlags.End;
            this.scintilla1.Location = new System.Drawing.Point(0, 0);
            this.scintilla1.Margins.Margin2.Width = 16;
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Selection.BackColorUnfocused = System.Drawing.SystemColors.Highlight;
            this.scintilla1.Size = new System.Drawing.Size(1444, 882);
            this.scintilla1.Styles.BraceBad.FontName = "Verd";
            this.scintilla1.Styles.BraceBad.Size = 9F;
            this.scintilla1.Styles.BraceLight.FontName = "Verd";
            this.scintilla1.Styles.BraceLight.Size = 9F;
            this.scintilla1.Styles.CallTip.FontName = "微软";
            this.scintilla1.Styles.ControlChar.FontName = "Verd";
            this.scintilla1.Styles.ControlChar.Size = 9F;
            this.scintilla1.Styles.Default.BackColor = System.Drawing.SystemColors.Window;
            this.scintilla1.Styles.Default.FontName = "Verd";
            this.scintilla1.Styles.Default.Size = 9F;
            this.scintilla1.Styles.IndentGuide.FontName = "Verd";
            this.scintilla1.Styles.IndentGuide.Size = 9F;
            this.scintilla1.Styles.LastPredefined.FontName = "Verd";
            this.scintilla1.Styles.LastPredefined.Size = 9F;
            this.scintilla1.Styles.LineNumber.FontName = "Verd";
            this.scintilla1.Styles.LineNumber.Size = 9F;
            this.scintilla1.Styles.Max.FontName = "Verd";
            this.scintilla1.Styles.Max.Size = 9F;
            this.scintilla1.TabIndex = 0;
            this.scintilla1.CharAdded += new System.EventHandler<ScintillaNET.CharAddedEventArgs>(this.scintilla1_CharAdded);
            this.scintilla1.ModifiedChanged += new System.EventHandler(this.scintilla1_ModifiedChanged);
            this.scintilla1.SelectionChanged += new System.EventHandler(this.scintilla1_SelectionChanged);
            this.scintilla1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scintilla1_MouseDown);
            // 
            // contextMenuStripMouseClick
            // 
            this.contextMenuStripMouseClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemUndo,
            this.toolStripMenuItemRedo,
            this.toolStripSeparator3,
            this.toolStripMenuItemCut,
            this.toolStripMenuItemCopy,
            this.toolStripMenuItemPaste,
            this.toolStripSeparator4,
            this.toolStripMenuItemSerch,
            this.toolStripMenuItemReplace,
            this.toolStripSeparator5,
            this.toolStripMenuItemSeleAll});
            this.contextMenuStripMouseClick.Name = "contextMenuStripMouseClick";
            this.contextMenuStripMouseClick.Size = new System.Drawing.Size(162, 198);
            this.contextMenuStripMouseClick.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripMouseClick_Opening);
            // 
            // toolStripMenuItemUndo
            // 
            this.toolStripMenuItemUndo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemUndo.Image")));
            this.toolStripMenuItemUndo.Name = "toolStripMenuItemUndo";
            this.toolStripMenuItemUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.toolStripMenuItemUndo.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItemUndo.Text = "撤消(&U)";
            this.toolStripMenuItemUndo.Click += new System.EventHandler(this.toolStripMenuItemUndo_Click);
            // 
            // toolStripMenuItemRedo
            // 
            this.toolStripMenuItemRedo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemRedo.Image")));
            this.toolStripMenuItemRedo.Name = "toolStripMenuItemRedo";
            this.toolStripMenuItemRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.toolStripMenuItemRedo.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItemRedo.Text = "重做(&R)";
            this.toolStripMenuItemRedo.Click += new System.EventHandler(this.toolStripMenuItemRedo_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(158, 6);
            // 
            // toolStripMenuItemCut
            // 
            this.toolStripMenuItemCut.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemCut.Image")));
            this.toolStripMenuItemCut.Name = "toolStripMenuItemCut";
            this.toolStripMenuItemCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemCut.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItemCut.Text = "剪切(&X)";
            this.toolStripMenuItemCut.Click += new System.EventHandler(this.toolStripMenuItemCut_Click);
            // 
            // toolStripMenuItemCopy
            // 
            this.toolStripMenuItemCopy.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemCopy.Image")));
            this.toolStripMenuItemCopy.Name = "toolStripMenuItemCopy";
            this.toolStripMenuItemCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemCopy.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItemCopy.Text = "复制(&C)";
            this.toolStripMenuItemCopy.Click += new System.EventHandler(this.toolStripMenuItemCopy_Click);
            // 
            // toolStripMenuItemPaste
            // 
            this.toolStripMenuItemPaste.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemPaste.Image")));
            this.toolStripMenuItemPaste.Name = "toolStripMenuItemPaste";
            this.toolStripMenuItemPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.toolStripMenuItemPaste.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItemPaste.Text = "粘贴(&P)";
            this.toolStripMenuItemPaste.Click += new System.EventHandler(this.toolStripMenuItemPaste_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(158, 6);
            // 
            // toolStripMenuItemSerch
            // 
            this.toolStripMenuItemSerch.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemSerch.Image")));
            this.toolStripMenuItemSerch.Name = "toolStripMenuItemSerch";
            this.toolStripMenuItemSerch.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.toolStripMenuItemSerch.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItemSerch.Text = "查找";
            this.toolStripMenuItemSerch.Click += new System.EventHandler(this.toolStripMenuItemSerch_Click);
            // 
            // toolStripMenuItemReplace
            // 
            this.toolStripMenuItemReplace.Name = "toolStripMenuItemReplace";
            this.toolStripMenuItemReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.toolStripMenuItemReplace.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItemReplace.Text = "替换";
            this.toolStripMenuItemReplace.Click += new System.EventHandler(this.toolStripMenuItemReplace_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(158, 6);
            // 
            // toolStripMenuItemSeleAll
            // 
            this.toolStripMenuItemSeleAll.Name = "toolStripMenuItemSeleAll";
            this.toolStripMenuItemSeleAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.toolStripMenuItemSeleAll.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItemSeleAll.Text = "全选(&A)";
            this.toolStripMenuItemSeleAll.Click += new System.EventHandler(this.toolStripMenuItemSeleAll_Click);
            // 
            // contextMenuStripTab
            // 
            this.contextMenuStripTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemTabClose,
            this.toolStripMenuItemTabCloseE,
            this.toolStripMenuItemTabCloseAll,
            this.toolStripSeparator1,
            this.toolStripMenuItemTabSv,
            this.toolStripMenuItemTabSvAll,
            this.toolStripMenuItemTabRdOnly,
            this.toolStripSeparator2,
            this.toolStripMenuItemTabAddMark});
            this.contextMenuStripTab.Name = "contextMenuStripTab";
            this.contextMenuStripTab.Size = new System.Drawing.Size(145, 170);
            // 
            // toolStripMenuItemTabClose
            // 
            this.toolStripMenuItemTabClose.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemTabClose.Image")));
            this.toolStripMenuItemTabClose.Name = "toolStripMenuItemTabClose";
            this.toolStripMenuItemTabClose.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuItemTabClose.Text = "关闭(&C)";
            this.toolStripMenuItemTabClose.Click += new System.EventHandler(this.toolStripMenuItemTabClose_Click);
            // 
            // toolStripMenuItemTabCloseE
            // 
            this.toolStripMenuItemTabCloseE.Name = "toolStripMenuItemTabCloseE";
            this.toolStripMenuItemTabCloseE.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuItemTabCloseE.Text = "关闭其他(&E)";
            this.toolStripMenuItemTabCloseE.Click += new System.EventHandler(this.toolStripMenuItemTabCloseE_Click);
            // 
            // toolStripMenuItemTabCloseAll
            // 
            this.toolStripMenuItemTabCloseAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemTabCloseAll.Image")));
            this.toolStripMenuItemTabCloseAll.Name = "toolStripMenuItemTabCloseAll";
            this.toolStripMenuItemTabCloseAll.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuItemTabCloseAll.Text = "全部关闭(&A)";
            this.toolStripMenuItemTabCloseAll.Click += new System.EventHandler(this.toolStripMenuItemTabCloseAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(141, 6);
            // 
            // toolStripMenuItemTabSv
            // 
            this.toolStripMenuItemTabSv.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemTabSv.Image")));
            this.toolStripMenuItemTabSv.Name = "toolStripMenuItemTabSv";
            this.toolStripMenuItemTabSv.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuItemTabSv.Text = "保存(&S)";
            this.toolStripMenuItemTabSv.Click += new System.EventHandler(this.toolStripMenuItemTabSv_Click);
            // 
            // toolStripMenuItemTabSvAll
            // 
            this.toolStripMenuItemTabSvAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemTabSvAll.Image")));
            this.toolStripMenuItemTabSvAll.Name = "toolStripMenuItemTabSvAll";
            this.toolStripMenuItemTabSvAll.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuItemTabSvAll.Text = "保存所有(&V)";
            this.toolStripMenuItemTabSvAll.Click += new System.EventHandler(this.toolStripMenuItemTabSvAll_Click);
            // 
            // toolStripMenuItemTabRdOnly
            // 
            this.toolStripMenuItemTabRdOnly.Name = "toolStripMenuItemTabRdOnly";
            this.toolStripMenuItemTabRdOnly.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuItemTabRdOnly.Text = "只读(&R)";
            this.toolStripMenuItemTabRdOnly.Click += new System.EventHandler(this.toolStripMenuItemTabRdOnly_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(141, 6);
            // 
            // toolStripMenuItemTabAddMark
            // 
            this.toolStripMenuItemTabAddMark.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemTabAddMark.Image")));
            this.toolStripMenuItemTabAddMark.Name = "toolStripMenuItemTabAddMark";
            this.toolStripMenuItemTabAddMark.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuItemTabAddMark.Text = "加为书签(&M)";
            this.toolStripMenuItemTabAddMark.Click += new System.EventHandler(this.toolStripMenuItemTabAddMark_Click);
            // 
            // DocumentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1444, 882);
            this.Controls.Add(this.scintilla1);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DocumentForm";
            this.TabPageContextMenuStrip = this.contextMenuStripTab;
            this.Activated += new System.EventHandler(this.DocumentForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DocumentForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DocumentForm_FormClosed);
            this.Load += new System.EventHandler(this.DocumentForm_Load);
            this.Shown += new System.EventHandler(this.DocumentForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.scintilla1)).EndInit();
            this.contextMenuStripMouseClick.ResumeLayout(false);
            this.contextMenuStripTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ScintillaNET.Scintilla scintilla1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTab;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTabClose;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTabCloseE;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTabCloseAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTabSv;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTabSvAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTabRdOnly;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTabAddMark;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMouseClick;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUndo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCut;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSerch;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemReplace;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSeleAll;

    }
}