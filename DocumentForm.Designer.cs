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
            this.scintilla1 = new ScintillaNET.Scintilla();
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
            this.contextMenuStripTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // scintilla1
            // 
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.LineWrapping.VisualFlags = ScintillaNET.LineWrappingVisualFlags.End;
            this.scintilla1.Location = new System.Drawing.Point(0, 0);
            this.scintilla1.Margins.Margin2.Width = 16;
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(284, 262);
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
            this.contextMenuStripTab.Size = new System.Drawing.Size(188, 170);
            // 
            // toolStripMenuItemTabClose
            // 
            this.toolStripMenuItemTabClose.Name = "toolStripMenuItemTabClose";
            this.toolStripMenuItemTabClose.Size = new System.Drawing.Size(187, 22);
            this.toolStripMenuItemTabClose.Text = "关闭(&C)";
            this.toolStripMenuItemTabClose.Click += new System.EventHandler(this.toolStripMenuItemTabClose_Click);
            // 
            // toolStripMenuItemTabCloseE
            // 
            this.toolStripMenuItemTabCloseE.Name = "toolStripMenuItemTabCloseE";
            this.toolStripMenuItemTabCloseE.Size = new System.Drawing.Size(187, 22);
            this.toolStripMenuItemTabCloseE.Text = "除此之外全部关闭(&E)";
            this.toolStripMenuItemTabCloseE.Click += new System.EventHandler(this.toolStripMenuItemTabCloseE_Click);
            // 
            // toolStripMenuItemTabCloseAll
            // 
            this.toolStripMenuItemTabCloseAll.Name = "toolStripMenuItemTabCloseAll";
            this.toolStripMenuItemTabCloseAll.Size = new System.Drawing.Size(187, 22);
            this.toolStripMenuItemTabCloseAll.Text = "全部关闭(&A)";
            this.toolStripMenuItemTabCloseAll.Click += new System.EventHandler(this.toolStripMenuItemTabCloseAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(184, 6);
            // 
            // toolStripMenuItemTabSv
            // 
            this.toolStripMenuItemTabSv.Name = "toolStripMenuItemTabSv";
            this.toolStripMenuItemTabSv.Size = new System.Drawing.Size(187, 22);
            this.toolStripMenuItemTabSv.Text = "保存(&S)";
            this.toolStripMenuItemTabSv.Click += new System.EventHandler(this.toolStripMenuItemTabSv_Click);
            // 
            // toolStripMenuItemTabSvAll
            // 
            this.toolStripMenuItemTabSvAll.Name = "toolStripMenuItemTabSvAll";
            this.toolStripMenuItemTabSvAll.Size = new System.Drawing.Size(187, 22);
            this.toolStripMenuItemTabSvAll.Text = "保存所有(&V)";
            this.toolStripMenuItemTabSvAll.Click += new System.EventHandler(this.toolStripMenuItemTabSvAll_Click);
            // 
            // toolStripMenuItemTabRdOnly
            // 
            this.toolStripMenuItemTabRdOnly.Name = "toolStripMenuItemTabRdOnly";
            this.toolStripMenuItemTabRdOnly.Size = new System.Drawing.Size(187, 22);
            this.toolStripMenuItemTabRdOnly.Text = "只读(&R)";
            this.toolStripMenuItemTabRdOnly.Click += new System.EventHandler(this.toolStripMenuItemTabRdOnly_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(184, 6);
            // 
            // toolStripMenuItemTabAddMark
            // 
            this.toolStripMenuItemTabAddMark.Name = "toolStripMenuItemTabAddMark";
            this.toolStripMenuItemTabAddMark.Size = new System.Drawing.Size(187, 22);
            this.toolStripMenuItemTabAddMark.Text = "加为书签(&M)";
            this.toolStripMenuItemTabAddMark.Click += new System.EventHandler(this.toolStripMenuItemTabAddMark_Click);
            // 
            // DocumentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.scintilla1);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "DocumentForm";
            this.TabPageContextMenuStrip = this.contextMenuStripTab;
            this.Activated += new System.EventHandler(this.DocumentForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DocumentForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DocumentForm_FormClosed);
            this.Load += new System.EventHandler(this.DocumentForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.scintilla1)).EndInit();
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

    }
}