using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WeCode1._0
{
    public partial class UserInfo : Form
    {
        public UserInfo(string info)
        {
            InitializeComponent();
            labelUInfo.Text = info;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //注销
        private void buttonlogOut_Click(object sender, EventArgs e)
        {
            this.Close() ;
        }
    }
}
