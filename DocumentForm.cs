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
                string Title = this.Text;
                DialogResult dr = MessageBox.Show(this, "文档已修改，是否保存?", Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Cancel)
                {
                    // Stop closing
                    e.Cancel = true;
                    return;
                }
                else if (dr == DialogResult.Yes)
                {
                    this.Save();
                    e.Cancel = false;
                    return;
                }
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

        private void DocumentForm_Load(object sender, EventArgs e)
        {

        }

        //关闭完了需要显示欢迎界面
        private void chek2ShowWelcome()
        {
            if (Attachment.DocCount == 1)
            {
                Attachment.frmMain.openWelcomePage();
            }
        }
        //关闭当前页签
        private void toolStripMenuItemTabClose_Click(object sender, EventArgs e)
        {
            this.Close();     
        }

        //除此之外全部关闭
        private void toolStripMenuItemTabCloseE_Click(object sender, EventArgs e)
        {
            IDockContent[] documents = DockPanel.DocumentsToArray();

            foreach (IDockContent content in documents)
            {
                if (!content.Equals(this))
                {
                    content.DockHandler.Close();
                }
            }
        }

        //全部关闭
        private void toolStripMenuItemTabCloseAll_Click(object sender, EventArgs e)
        {
            IDockContent[] documents = DockPanel.DocumentsToArray();

            foreach (IDockContent content in documents)
            {
                content.DockHandler.Close();
            }
        }

        //保存
        private void toolStripMenuItemTabSv_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        //全部保存
        private void toolStripMenuItemTabSvAll_Click(object sender, EventArgs e)
        {
            IDockContent[] documents = DockPanel.DocumentsToArray();

            foreach (WeCode1._0.DocumentForm Doc in documents)
            {
                Doc.Save();
            }
        }

        //只读
        private void toolStripMenuItemTabRdOnly_Click(object sender, EventArgs e)
        {
            toolStripMenuItemTabRdOnly.Checked = !toolStripMenuItemTabRdOnly.Checked;
            this.Scintilla.IsReadOnly = toolStripMenuItemTabRdOnly.Checked;
        }

        //加为书签
        private void toolStripMenuItemTabAddMark_Click(object sender, EventArgs e)
        {

            string NodeId = this.NodeId;
            string SQL = "update Ttree set marktime=" + PubFunc.time2TotalSeconds().ToString() + " where Nodeid=" + NodeId;
            AccessAdo.ExecuteNonQuery(SQL);
        }

        private void scintilla1_CharAdded(object sender, CharAddedEventArgs e)
        {
            string tmp = scintilla1.GetWordFromPosition(scintilla1.CurrentPos);
            string pretmp = scintilla1.GetWordFromPosition(scintilla1.CurrentPos - 1);
            if (string.IsNullOrEmpty(tmp) || tmp.Length < 1 || !tmp.Equals(pretmp))
                return;
            List<string> userList = scintilla1.AutoComplete.List.FindAll(
                delegate(string txt)
                {
                    if (txt.StartsWith(tmp, StringComparison.OrdinalIgnoreCase))
                        return true;
                    else
                        return false;
                });
            //Debug.Print("Word: " + tmp);
            if (userList.Count > 0)
            {
                scintilla1.AutoComplete.ShowUserList(-1, userList);
                //scintilla.AutoComplete.Show();
            }

            //scintilla.AutoComplete.Show();
        }

        private void DocumentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Attachment.DocCount = DockPanel.DocumentsCount;
            chek2ShowWelcome();
        }



    }
}
