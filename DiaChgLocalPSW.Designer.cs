namespace WeCode1._0
{
    partial class DiaChgLocalPSW
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
            this.labelInput = new System.Windows.Forms.Label();
            this.textBoxOldpsw = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelConfirm = new System.Windows.Forms.Label();
            this.textBoxnewpsw = new System.Windows.Forms.TextBox();
            this.textBoxnewConfirm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelInput
            // 
            this.labelInput.AutoSize = true;
            this.labelInput.Location = new System.Drawing.Point(13, 13);
            this.labelInput.Name = "labelInput";
            this.labelInput.Size = new System.Drawing.Size(65, 12);
            this.labelInput.TabIndex = 0;
            this.labelInput.Text = "原始密码：";
            // 
            // textBoxOldpsw
            // 
            this.textBoxOldpsw.Location = new System.Drawing.Point(84, 10);
            this.textBoxOldpsw.Name = "textBoxOldpsw";
            this.textBoxOldpsw.Size = new System.Drawing.Size(156, 21);
            this.textBoxOldpsw.TabIndex = 1;
            this.textBoxOldpsw.UseSystemPasswordChar = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(84, 92);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(165, 92);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelConfirm
            // 
            this.labelConfirm.AutoSize = true;
            this.labelConfirm.Location = new System.Drawing.Point(13, 40);
            this.labelConfirm.Name = "labelConfirm";
            this.labelConfirm.Size = new System.Drawing.Size(53, 12);
            this.labelConfirm.TabIndex = 2;
            this.labelConfirm.Text = "新密码：";
            // 
            // textBoxnewpsw
            // 
            this.textBoxnewpsw.Location = new System.Drawing.Point(84, 37);
            this.textBoxnewpsw.Name = "textBoxnewpsw";
            this.textBoxnewpsw.Size = new System.Drawing.Size(156, 21);
            this.textBoxnewpsw.TabIndex = 3;
            this.textBoxnewpsw.UseSystemPasswordChar = true;
            // 
            // textBoxnewConfirm
            // 
            this.textBoxnewConfirm.Location = new System.Drawing.Point(84, 65);
            this.textBoxnewConfirm.Name = "textBoxnewConfirm";
            this.textBoxnewConfirm.Size = new System.Drawing.Size(156, 21);
            this.textBoxnewConfirm.TabIndex = 4;
            this.textBoxnewConfirm.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "确认密码：";
            // 
            // DiaChgLocalPSW
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 124);
            this.Controls.Add(this.textBoxnewConfirm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxnewpsw);
            this.Controls.Add(this.labelConfirm);
            this.Controls.Add(this.textBoxOldpsw);
            this.Controls.Add(this.labelInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DiaChgLocalPSW";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "密码";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInput;
        private System.Windows.Forms.TextBox textBoxOldpsw;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelConfirm;
        private System.Windows.Forms.TextBox textBoxnewpsw;
        private System.Windows.Forms.TextBox textBoxnewConfirm;
        private System.Windows.Forms.Label label1;
    }
}