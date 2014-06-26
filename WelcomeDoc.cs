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
    }
}
