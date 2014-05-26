using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using ScintillaNET;
using System.IO;
using System.Data.OleDb;

namespace WeCode1._0
{
    public partial class DocumentForm : DockContent
    {

        #region Fields

        private string _nodeId;

        private bool _iniLexer;

        #endregion Fields

        #region Properties

        public string NodeId
        {
            get { return _nodeId; }
            set { _nodeId = value; }
        }


        public bool IniLexer
        {
            get { return _iniLexer; }
            set { _iniLexer = value; }
        }


        public Scintilla Scintilla
        {
            get
            {
                return scintilla1;
            }
        }

        #endregion Properties


        public DocumentForm()
        {
            InitializeComponent();
        }

        private void DocumentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Scintilla.Modified)
            {
                MessageBox.Show("是否保存？");
            }
        }

        //保存当前文章
        public void Save()
        {
            string DocText = this.scintilla1.Text;
            if (DocText.Length == 0)
                return;

            OleDbParameter p1 = new OleDbParameter("@Content", OleDbType.VarChar);
            p1.Value = DocText;
            OleDbParameter p2 = new OleDbParameter("@NodeId", OleDbType.Integer);
            p2.Value = Convert.ToInt32(this.NodeId);

            OleDbParameter[] ArrPara = new OleDbParameter[2];
            ArrPara[0] = p1;
            ArrPara[1] = p2;
            string SQL = "update tcontent set content=@Content where NodeId=@NodeId";
            AccessAdo.ExecuteNonQuery(SQL, ArrPara);

            scintilla1.Modified = false;
        }

        private void scintilla1_ModifiedChanged(object sender, EventArgs e)
        {
            AddOrRemoveAsteric();
        }

        private void AddOrRemoveAsteric()
        {
            if (scintilla1.Modified)
            {
                if (!Text.EndsWith(" *"))
                    Text += " *";
            }
            else
            {
                if (Text.EndsWith(" *"))
                    Text = Text.Substring(0, Text.Length - 2);
            }
        }

        //编辑窗体激活
        private void DocumentForm_Activated(object sender, EventArgs e)
        {
            //设置附件
            Attachment.ActiveNodeId = this.NodeId;
            Attachment.AttForm.ReFreshAttachGrid();
        }
    }
}
