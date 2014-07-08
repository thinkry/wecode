namespace WeCode1._0
{
    partial class UserInfo
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelUInfo = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonlogOut = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.labelUInfo);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(244, 108);
            this.panel1.TabIndex = 0;
            // 
            // labelUInfo
            // 
            this.labelUInfo.AutoSize = true;
            this.labelUInfo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelUInfo.Location = new System.Drawing.Point(24, 22);
            this.labelUInfo.Name = "labelUInfo";
            this.labelUInfo.Size = new System.Drawing.Size(55, 21);
            this.labelUInfo.TabIndex = 0;
            this.labelUInfo.Text = "label1";
            // 
            // buttonOK
            // 
            this.buttonOK.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonOK.Location = new System.Drawing.Point(26, 127);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonlogOut
            // 
            this.buttonlogOut.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.buttonlogOut.Location = new System.Drawing.Point(142, 127);
            this.buttonlogOut.Name = "buttonlogOut";
            this.buttonlogOut.Size = new System.Drawing.Size(75, 23);
            this.buttonlogOut.TabIndex = 2;
            this.buttonlogOut.Text = "注销";
            this.buttonlogOut.UseVisualStyleBackColor = true;
            this.buttonlogOut.Click += new System.EventHandler(this.buttonlogOut_Click);
            // 
            // UserInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 157);
            this.Controls.Add(this.buttonlogOut);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "有道云账号信息";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelUInfo;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonlogOut;
    }
}