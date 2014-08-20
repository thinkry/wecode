using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace WeCode1._0
{
    public partial class WelcomeDoc :DockContent
    {

        #region Fields

        private string _nodeId;

        #endregion Fields

        #region Properties

        public string NodeId
        {
            get { return _nodeId; }
            set { _nodeId = value; }
        }
        #endregion


        public WelcomeDoc()
        {
            InitializeComponent();
            label5.Text = PubFunc.GetConfiguration("Version");
        }

        private void WelcomeDoc_Activated(object sender, EventArgs e)
        {
            Attachment.ActiveNodeId = "-1";
            Attachment.ActiveDOCType = "local";
            Attachment.AttForm.ReFreshAttachGrid();
            Attachment.AttForm.GridDiable();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = "http://wecode.thinkry.com";
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
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = "https://github.com/thinkry/wecode/issues";
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
        }

        private void WelcomeDoc_Load(object sender, EventArgs e)
        {
            //设置随机语录
            int i = 0;
            string[] sWord = new string[5];
            sWord[0] = "程序员的知识管理软件";
            sWord[1] = "我们是程序员，我们改变世界";
            sWord[2] = "总结  分享  提升";
            sWord[3] = "自己用心又有计划去做事，是很难失败的";
            sWord[4] = "一段写得好的代码，一用好多年，拷贝好带身边";

            int random = new Random().Next(0, 2);
            label2.Text = sWord[random];
        }
    }
}
