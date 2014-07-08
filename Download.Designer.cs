namespace WeCode1._0
{
    partial class Download
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Download));
            this.label1 = new System.Windows.Forms.Label();
            this.lblURL = new System.Windows.Forms.Label();
            this.lblSaveAs = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblDownLoad = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "下载链接：";
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(83, 13);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(41, 12);
            this.lblURL.TabIndex = 1;
            this.lblURL.Text = "label2";
            // 
            // lblSaveAs
            // 
            this.lblSaveAs.AutoSize = true;
            this.lblSaveAs.Location = new System.Drawing.Point(83, 40);
            this.lblSaveAs.Name = "lblSaveAs";
            this.lblSaveAs.Size = new System.Drawing.Size(41, 12);
            this.lblSaveAs.TabIndex = 3;
            this.lblSaveAs.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "保存到：";
            // 
            // lblDownLoad
            // 
            this.lblDownLoad.AutoSize = true;
            this.lblDownLoad.Location = new System.Drawing.Point(83, 69);
            this.lblDownLoad.Name = "lblDownLoad";
            this.lblDownLoad.Size = new System.Drawing.Size(41, 12);
            this.lblDownLoad.TabIndex = 5;
            this.lblDownLoad.Text = "label5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "当前下载：";
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(83, 95);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(41, 12);
            this.lblSpeed.TabIndex = 7;
            this.lblSpeed.Text = "label7";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 95);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 6;
            this.label8.Text = "下载速度：";
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 138);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(515, 20);
            this.progressBar1.TabIndex = 8;
            // 
            // Download
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(515, 158);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblDownLoad);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblSaveAs);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblURL);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Download";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "下载";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Download_FormClosing);
            this.Shown += new System.EventHandler(this.Download_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.Label lblSaveAs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDownLoad;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}