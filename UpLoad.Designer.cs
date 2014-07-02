namespace WeCode1._0
{
    partial class UpLoad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpLoad));
            this.lblTime = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(13, 13);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(41, 12);
            this.lblTime.TabIndex = 0;
            this.lblTime.Text = "label1";
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(13, 36);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(41, 12);
            this.lblSpeed.TabIndex = 1;
            this.lblSpeed.Text = "label2";
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(13, 83);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(41, 12);
            this.lblSize.TabIndex = 3;
            this.lblSize.Text = "label3";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(13, 60);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(41, 12);
            this.lblState.TabIndex = 2;
            this.lblState.Text = "label4";
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 110);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(496, 20);
            this.progressBar1.TabIndex = 4;
            // 
            // UpLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(496, 130);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.lblTime);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "UpLoad";
            this.Text = "上传附件";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UpLoad_FormClosing);
            this.Load += new System.EventHandler(this.UpLoad_Load);
            this.Shown += new System.EventHandler(this.UpLoad_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}