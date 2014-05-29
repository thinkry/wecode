using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WeCode1._0
{
    public partial class AttRenameDialog : Form
    {
        public string[] ReturnVal { get; protected set; }

        public AttRenameDialog(string Attname)
        {
            InitializeComponent();
            this.textBoxReName.Text = Attname;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxReName.Text))
            {
                MessageBox.Show("不能为空！");
                return;
            }
            ReturnVal = new string[1];
            ReturnVal[0] = this.textBoxReName.Text;

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
