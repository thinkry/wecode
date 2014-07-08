namespace WeCode1._0
{
    partial class FormAttachment
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStripAtt1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemOpenZIP = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAddZIP = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDelZIP = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemReName = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripAtt2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加附件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripOL1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripOL2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStripAtt1.SuspendLayout();
            this.contextMenuStripAtt2.SuspendLayout();
            this.contextMenuStripOL1.SuspendLayout();
            this.contextMenuStripOL2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeight = 20;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 10;
            this.dataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(691, 364);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            this.dataGridView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseClick);
            // 
            // contextMenuStripAtt1
            // 
            this.contextMenuStripAtt1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOpenZIP,
            this.toolStripMenuItemAddZIP,
            this.toolStripMenuItemDelZIP,
            this.toolStripMenuItemSaveAs,
            this.toolStripMenuItemReName});
            this.contextMenuStripAtt1.Name = "contextMenuStrip1";
            this.contextMenuStripAtt1.Size = new System.Drawing.Size(125, 114);
            // 
            // toolStripMenuItemOpenZIP
            // 
            this.toolStripMenuItemOpenZIP.Name = "toolStripMenuItemOpenZIP";
            this.toolStripMenuItemOpenZIP.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemOpenZIP.Text = "打开附件";
            this.toolStripMenuItemOpenZIP.Click += new System.EventHandler(this.toolStripMenuItemOpenZIP_Click);
            // 
            // toolStripMenuItemAddZIP
            // 
            this.toolStripMenuItemAddZIP.Name = "toolStripMenuItemAddZIP";
            this.toolStripMenuItemAddZIP.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemAddZIP.Text = "添加附件";
            this.toolStripMenuItemAddZIP.Click += new System.EventHandler(this.toolStripMenuItemAddZIP_Click);
            // 
            // toolStripMenuItemDelZIP
            // 
            this.toolStripMenuItemDelZIP.Name = "toolStripMenuItemDelZIP";
            this.toolStripMenuItemDelZIP.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemDelZIP.Text = "删除附件";
            this.toolStripMenuItemDelZIP.Click += new System.EventHandler(this.toolStripMenuItemDelZIP_Click);
            // 
            // toolStripMenuItemSaveAs
            // 
            this.toolStripMenuItemSaveAs.Name = "toolStripMenuItemSaveAs";
            this.toolStripMenuItemSaveAs.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemSaveAs.Text = "另存为";
            this.toolStripMenuItemSaveAs.Click += new System.EventHandler(this.toolStripMenuItemSaveAs_Click);
            // 
            // toolStripMenuItemReName
            // 
            this.toolStripMenuItemReName.Name = "toolStripMenuItemReName";
            this.toolStripMenuItemReName.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItemReName.Text = "重命名";
            this.toolStripMenuItemReName.Click += new System.EventHandler(this.toolStripMenuItemReName_Click);
            // 
            // contextMenuStripAtt2
            // 
            this.contextMenuStripAtt2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加附件ToolStripMenuItem});
            this.contextMenuStripAtt2.Name = "contextMenuStripAtt2";
            this.contextMenuStripAtt2.Size = new System.Drawing.Size(125, 26);
            // 
            // 添加附件ToolStripMenuItem
            // 
            this.添加附件ToolStripMenuItem.Name = "添加附件ToolStripMenuItem";
            this.添加附件ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.添加附件ToolStripMenuItem.Text = "添加附件";
            this.添加附件ToolStripMenuItem.Click += new System.EventHandler(this.添加附件ToolStripMenuItem_Click);
            // 
            // contextMenuStripOL1
            // 
            this.contextMenuStripOL1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4});
            this.contextMenuStripOL1.Name = "contextMenuStripOL1";
            this.contextMenuStripOL1.Size = new System.Drawing.Size(125, 92);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem1.Text = "上传附件";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem2.Text = "删除附件";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem3.Text = "下载附件";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem4.Text = "重命名";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // contextMenuStripOL2
            // 
            this.contextMenuStripOL2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5});
            this.contextMenuStripOL2.Name = "contextMenuStrip1";
            this.contextMenuStripOL2.Size = new System.Drawing.Size(125, 26);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem5.Text = "上传附件";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // FormAttachment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(691, 364);
            this.Controls.Add(this.dataGridView1);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Name = "FormAttachment";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
            this.Text = "附件";
            this.Load += new System.EventHandler(this.FormAttachment_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStripAtt1.ResumeLayout(false);
            this.contextMenuStripAtt2.ResumeLayout(false);
            this.contextMenuStripOL1.ResumeLayout(false);
            this.contextMenuStripOL2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripAtt1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpenZIP;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddZIP;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDelZIP;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveAs;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemReName;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripAtt2;
        private System.Windows.Forms.ToolStripMenuItem 添加附件ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOL1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOL2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
    }
}