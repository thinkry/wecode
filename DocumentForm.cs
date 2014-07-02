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
using System.Configuration;
using System.Xml;

namespace WeCode1._0
{
    public partial class DocumentForm : DockContent
    {

        #region Fields

        private string _nodeId;

        private string _type;

        private bool _iniLexer;

        #endregion Fields

        #region Properties

        public string NodeId
        {
            get { return _nodeId; }
            set { _nodeId = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
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
            if (Scintilla.Modified&&Attachment.isDeleteClose=="0")
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
            if (this.Type == "local")
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
            else if (this.Type == "online")
            {
                string DocText = this.scintilla1.Text;
                if (DocText.Length == 0)
                    return;
                NoteAPI.UpdateNote(this.NodeId,DocText);

                scintilla1.Modified = false;
            }
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
            Attachment.ActiveDOCType = this.Type;
            Attachment.AttForm.GridEnable();
            Attachment.AttForm.ReFreshAttachGrid();

            try
            {
                int x = Scintilla.Caret.LineNumber + 1;
                int overAllPosition = Scintilla.Caret.Position;
                int lineStartPosition = Scintilla.Lines.Current.StartPosition;
                int y = overAllPosition - lineStartPosition;
                Attachment.frmMain.showLinePosition(x, y);
            }

            catch (Exception ex)
            {

            }
            
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
            if (this.Type == "local")
            {
                string NodeId = this.NodeId;
                string SQL = "update Ttree set marktime=" + PubFunc.time2TotalSeconds().ToString() + " where Nodeid=" + NodeId;
                AccessAdo.ExecuteNonQuery(SQL);
            }
            else if (this.Type == "online")
            {
                string NodeId = this.NodeId;
                XmlDocument doc = new XmlDocument();
                doc.Load("TreeNodeLocal.xml");
                XmlNode preNode = doc.SelectSingleNode("//note[@path='" + NodeId + "']");
                preNode.Attributes["isMark"].Value = "1";
                doc.Save("TreeNodeLocal.xml");
                XMLAPI.XML2Yun();
            }
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

        //导出为HTML
        public bool ExportAsHtml()
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                string fileName = (Text.EndsWith(" *") ? Text.Substring(0, Text.Length - 2) : Text);
                dialog.Filter = "HTML Files (*.html;*.htm)|*.html;*.htm|All Files (*.*)|*.*";
                dialog.FileName = fileName + ".html";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    scintilla1.Lexing.Colorize(); // Make sure the document is current
                    using (StreamWriter sw = new StreamWriter(dialog.FileName))
                        scintilla1.ExportHtml(sw, fileName, false);

                    return true;
                }
            }

            return false;
        }

        //撤销
        private void toolStripMenuItemUndo_Click(object sender, EventArgs e)
        {
            this.Scintilla.UndoRedo.Undo();
        }
        //反撤销
        private void toolStripMenuItemRedo_Click(object sender, EventArgs e)
        {
            this.Scintilla.UndoRedo.Redo();
        }
        //剪切
        private void toolStripMenuItemCut_Click(object sender, EventArgs e)
        {
            this.Scintilla.Clipboard.Cut();
        }
        //复制
        private void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            this.Scintilla.Clipboard.Copy();
        }
        //粘贴
        private void toolStripMenuItemPaste_Click(object sender, EventArgs e)
        {
           this.Scintilla.Clipboard.Paste();
        }
        //查找
        private void toolStripMenuItemSerch_Click(object sender, EventArgs e)
        {
            this.Scintilla.FindReplace.ShowFind();
        }
        //替换
        private void toolStripMenuItemReplace_Click(object sender, EventArgs e)
        {
            this.Scintilla.FindReplace.ShowReplace();
        }
        //全选
        private void toolStripMenuItemSeleAll_Click(object sender, EventArgs e)
        {
            this.Scintilla.Selection.SelectAll();
        }

        //右键菜单
        private void scintilla1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }
        //右键菜单
        private void contextMenuStripMouseClick_Opening(object sender, CancelEventArgs e)
        {
            this.toolStripMenuItemUndo.Enabled = this.Scintilla.UndoRedo.CanUndo;
            this.toolStripMenuItemRedo.Enabled = this.Scintilla.UndoRedo.CanRedo;
            this.toolStripMenuItemCut.Enabled = this.Scintilla.Clipboard.CanCut;
            this.toolStripMenuItemCopy.Enabled = this.Scintilla.Clipboard.CanCopy;
            this.toolStripMenuItemPaste.Enabled = this.Scintilla.Clipboard.CanPaste;

        }

        //光标改变
        private void scintilla1_CursorChanged(object sender, EventArgs e)
        {

        }

        //光标位置改变
        private void scintilla1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                int x = Scintilla.Caret.LineNumber + 1;
                int overAllPosition = Scintilla.Caret.Position;
                int lineStartPosition = Scintilla.Lines.Current.StartPosition;
                int y = overAllPosition - lineStartPosition;
                Attachment.frmMain.showLinePosition(x, y);
            }

            catch (Exception ex)
            { 
                
            }

        }

        private void DocumentForm_Shown(object sender, EventArgs e)
        {
            try
            {
                if (ConfigurationManager.AppSettings["defaultFont"] != "")
                { 
                    FontConverter fc = new FontConverter();
                    string sfont = ConfigurationManager.AppSettings["defaultFont"];
                    SetFont(this, (Font)fc.ConvertFromString(sfont));
                }
            }
            catch (Exception ex)
            { 
                
            }
        }

        public void SetFont(DocumentForm doc, Font xFont)
        {
            //ActiveDocument.Scintilla.Font = xFont;

            doc.Scintilla.Styles.Default.Font = xFont;

            doc.Scintilla.Styles[0].Font = xFont;               //'white space
            doc.Scintilla.Styles[1].Font = xFont;              //'comments-block
            doc.Scintilla.Styles[2].Font = xFont;              // 'comments-singlechar
            doc.Scintilla.Styles[3].Font = xFont;              // 'half-formed comment
            doc.Scintilla.Styles[4].Font = xFont;            //   'numbers
            doc.Scintilla.Styles[5].Font = xFont;              // 'keyword after complete
            doc.Scintilla.Styles[6].Font = xFont;              // 'quoted text-double
            doc.Scintilla.Styles[7].Font = xFont;              // 'quoted text-single
            doc.Scintilla.Styles[8].Font = xFont;               //'"table" keyword l
            doc.Scintilla.Styles[9].Font = xFont;               //'? knows
            doc.Scintilla.Styles[10].Font = xFont;              //'symbol (<>);=-
            doc.Scintilla.Styles[11].Font = xFont;              //'half-formed words
            doc.Scintilla.Styles[12].Font = xFont;              //'mixed quoted text 'text"  ?strange
            doc.Scintilla.Styles[14].Font = xFont;              //'sql-type keyword (still looks weird)
            doc.Scintilla.Styles[15].Font = xFont;              //'sql @symbol in a comment 
            doc.Scintilla.Styles[16].Font = xFont;              //'sql function returning INT
            doc.Scintilla.Styles[19].Font = xFont;              //'in/out
            doc.Scintilla.Styles[32].Font = xFont;              //'plain ordinary whitespace, that exists everywhere

            doc.Scintilla.Styles[StylesCommon.LineNumber].Font = xFont;
            doc.Scintilla.Styles[StylesCommon.BraceBad].Font = xFont;
            doc.Scintilla.Styles[StylesCommon.BraceLight].Font = xFont;
            doc.Scintilla.Styles[StylesCommon.CallTip].Font = xFont;
            doc.Scintilla.Styles[StylesCommon.ControlChar].Font = xFont;
            doc.Scintilla.Styles[StylesCommon.Default].Font = xFont;
            doc.Scintilla.Styles[StylesCommon.IndentGuide].Font = xFont;
            doc.Scintilla.Styles[StylesCommon.LastPredefined].Font = xFont;
            doc.Scintilla.Styles[StylesCommon.Max].Font = xFont;
        }

        //设置语言
        public void SetLanguageByDoc(string language)
        {
            if ("ini".Equals(language, StringComparison.OrdinalIgnoreCase))
            {
                // Reset/set all styles and prepare _scintilla for custom lexing
                this.IniLexer = true;
                //WeCode1._0.IniLexer.Init(this.Scintilla);
            }
            else
            {
                // Use a built-in lexer and configuration
                this.IniLexer = false;
                this.Scintilla.ConfigurationManager.Language = language;

                // Smart indenting...
                if ("cs".Equals(language, StringComparison.OrdinalIgnoreCase))
                    this.Scintilla.Indentation.SmartIndentType = ScintillaNET.SmartIndent.CPP;
                else
                    this.Scintilla.Indentation.SmartIndentType = ScintillaNET.SmartIndent.None;
            }
        }

    }
}
