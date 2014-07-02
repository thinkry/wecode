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
            string target = "https://github.com/thinkry/wecode";
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
            string target = "mailto:herbertmarson1990@gmail.com";
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
    }
}
