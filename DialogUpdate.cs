using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WeCode1._0
{
    public partial class DialogUpdate : Form
    {
        string sVer, sTips, sdlUrl;
        public DialogUpdate(string Ver,string Tips,string dlUrl)
        {
            InitializeComponent();
            sVer = Ver;
            sTips = Tips;
            sdlUrl = dlUrl;
        }

        //打开下载地址,终止程序
        private void button1_Click(object sender, EventArgs e)
        {
            string target = sdlUrl;
            try
            {
                System.Diagnostics.Process.Start(target);
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
            finally {
                System.Environment.Exit(System.Environment.ExitCode);
                this.Dispose();
                this.Close();
            }
        }

        //以后再提醒
        private void button2_Click(object sender, EventArgs e)
        {
            PubFunc.SetConfiguration("checkVerAlert", "0");
            PubFunc.SetConfiguration("lastVerAlertTime",DateTime.Now.ToShortDateString());
            DialogResult = DialogResult.No;
            this.Close();

        }

        //取消，程序继续运行
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void DialogUpdate_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = sTips;
            this.label3.Text = "最新版本：" + sVer;

        }
    }
}
