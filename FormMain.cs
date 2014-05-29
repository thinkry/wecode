using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Configuration;
using ScintillaNET;
using WeifenLuo.WinFormsUI.Docking;

namespace WeCode1._0
{
    public partial class FormMain : Form
    {
        private FormTreeLeft frTree;
        private FormAttachment frmAttchment;
        private DocMark frmMark;
        private DocFind FormFind;

        private DeserializeDockContent m_deserializeDockContent;


        #region Fields

        private int _newDocumentCount = 0;
        private string[] _args;
        private int _zoomLevel=0;
        private const int LINE_NUMBERS_MARGIN_WIDTH = 35;

        #endregion Fields

        #region Properties

        public DocumentForm ActiveDocument
        {
            get
            {
                return dockPanel1.ActiveDocument as DocumentForm;
            }
        }

        #endregion Properties


        public FormMain()
        {
            InitializeComponent();
            

            //书签
            frmMark = new DocMark();
            frmMark.formParent = this;
            //frmMark.Show(dockPanel1);

            //显示查找
            FormFind = new DocFind();
            FormFind.formParent = this;
            //FormFind.Show(dockPanel1);

            //显示树窗口
            frTree = new FormTreeLeft();
            frTree.formParent = this;
            //frTree.Show(dockPanel1);

            //显示附件窗口
            Attachment.ActiveNodeId = "-1";
            frmAttchment = new FormAttachment();
            Attachment.AttForm = frmAttchment;
            //frmAttchment.Show(dockPanel1);

            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

        }

        private void toolStripButtonUp_Click(object sender, EventArgs e)
        {
            frTree.setNodeUp();
        }

        private void toolStripButtonDown_Click(object sender, EventArgs e)
        {
            frTree.setNodeDown();
        }

        //关闭文章
        public void CloseDoc(string nodeId)
        {
            foreach (DocumentForm documentForm in dockPanel1.Documents)
            {
                if (nodeId.Equals(documentForm.NodeId, StringComparison.OrdinalIgnoreCase))
                {
                    documentForm.Close();
                    break;
                }
            }
        }

        //打开文章
        public void openNew(string nodeId)
        {
            //欢迎窗口是否打开，如果打开则关闭
            if (Attachment.isWelcomePageopen == "1")
            {
                IDockContent[] documents = dockPanel1.DocumentsToArray();

                foreach (IDockContent content in documents)
                {
                        content.DockHandler.Close();
                }
                Attachment.isWelcomePageopen = "0";
            }
            // 如果已经打开，则定位，否则新窗口打开
            bool isOpen = false;
            foreach (DocumentForm documentForm in dockPanel1.Documents)
            {
                if (nodeId.Equals(documentForm.NodeId, StringComparison.OrdinalIgnoreCase))
                {
                    documentForm.Select();
                    isOpen = true;
                    break;
                }
            }

            // Open the files
            if (!isOpen)
                OpenFile(nodeId);
        }

        ////打开文章
        //public void openNew(string nodeId)
        //{

        //        // 如果已经打开，则定位，否则新窗口打开
        //        bool isOpen = false;
        //        foreach (DocumentForm documentForm in dockPanel1.Documents)
        //        {
        //            if (nodeId.Equals(documentForm.NodeId, StringComparison.OrdinalIgnoreCase))
        //            {
        //                documentForm.Select();
        //                isOpen = true;
        //                break;
        //            }
        //        }

        //        // Open the files
        //        if (!isOpen)
        //            OpenFile(nodeId);
        //}


        private DocumentForm OpenFile(string nodeId)
        {

            //获取文章信息
            string SQL = "select Title,Content from TContent inner join TTree on TContent.NodeId=Ttree.NodeId where TContent.NodeId=" + nodeId;
            DataTable temp = AccessAdo.ExecuteDataSet(SQL, null).Tables[0];
            if (temp.Rows.Count == 0)
                return null;
            string Title = temp.Rows[0]["Title"].ToString();
            string Content = temp.Rows[0]["Content"].ToString();

            DocumentForm doc = new DocumentForm();
            SetScintillaToCurrentOptions(doc);
            doc.Scintilla.Text = Content;
            doc.Scintilla.UndoRedo.EmptyUndoBuffer();
            doc.Scintilla.Modified = false;
            doc.Text = Title;
            doc.NodeId = nodeId;
            doc.Show(dockPanel1);
            

            return doc;
        }

        //配置相关显示参数
        private void SetScintillaToCurrentOptions(DocumentForm doc)
        {
            //// Turn on line numbers?
            //if (lineNumbersToolStripMenuItem.Checked)
                doc.Scintilla.Margins.Margin0.Width = LINE_NUMBERS_MARGIN_WIDTH;
            //else
            //    doc.Scintilla.Margins.Margin0.Width = 0;

            //// Turn on white space?
            //if (whitespaceToolStripMenuItem.Checked)
            //    doc.Scintilla.Whitespace.Mode = WhitespaceMode.VisibleAlways;
            //else
            //    doc.Scintilla.Whitespace.Mode = WhitespaceMode.Invisible;

            //// Turn on word wrap?
            //if (wordWrapToolStripMenuItem.Checked)
            //    doc.Scintilla.LineWrapping.Mode = LineWrappingMode.Word;
            //else
            //    doc.Scintilla.LineWrapping.Mode = LineWrappingMode.None;

            //// Show EOL?
            //doc.Scintilla.EndOfLine.IsVisible = endOfLineToolStripMenuItem.Checked;

            // Set the zoom
            doc.Scintilla.ZoomFactor = _zoomLevel;
        }


        private void toolStripButtonNewText_Click(object sender, EventArgs e)
        {
            frTree.NewDoc();
        }

        private void toolStripButtonNewDir_Click(object sender, EventArgs e)
        {
            frTree.NewDir();
        }

        //保存
        private void toolStripButtonSv_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Save();
        }


        //设置语言
        public void SetLanguage(string language)
        {
            if ("ini".Equals(language, StringComparison.OrdinalIgnoreCase))
            {
                // Reset/set all styles and prepare _scintilla for custom lexing
                ActiveDocument.IniLexer = true;
                IniLexer.Init(ActiveDocument.Scintilla);
            }
            else
            {
                // Use a built-in lexer and configuration
                ActiveDocument.IniLexer = false;
                ActiveDocument.Scintilla.ConfigurationManager.Language = language;

                // Smart indenting...
                if ("cs".Equals(language, StringComparison.OrdinalIgnoreCase))
                    ActiveDocument.Scintilla.Indentation.SmartIndentType = ScintillaNET.SmartIndent.CPP;
                else
                    ActiveDocument.Scintilla.Indentation.SmartIndentType = ScintillaNET.SmartIndent.None;
            }
        }
        
        //保存所有
        private void toolStripButtonSvAll_Click(object sender, EventArgs e)
        {
            if (Attachment.isWelcomePageopen == "1")
            {
                return;
            }
            foreach (DocumentForm doc in dockPanel1.Documents)
            {
                doc.Activate();
                doc.Save();
            }
        }

        //新建数据库
        private void toolStripMenuItemNewDB_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            string path = "";
            //设置文件类型
            sf.Filter = "数据文件(*.mdb)|*.mdb";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                path = sf.FileName;

                if (File.Exists(path)) //检查数据库是否已存在
                {
                    throw new Exception("目标数据库已存在,无法创建");
                }
                // 可以加上密码,这样创建后的数据库必须输入密码后才能打开
                path = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path;
                // 创建一个CatalogClass对象的实例,
                ADOX.CatalogClass cat = new ADOX.CatalogClass();
                // 使用CatalogClass对象的Create方法创建ACCESS数据库
                cat.Create(path);

                //创建表
                OleDbConnection conn = new OleDbConnection(path);
                string crtSQL=" CREATE TABLE TTree ( "+
				" [NodeId] INTEGER CONSTRAINT PK_TTree26 PRIMARY KEY, "+
				" [Title] VARCHAR, "+
				" [ParentId] INTEGER, "+
				" [Type] INTEGER, "+
				" [CreateTime] INTEGER, "+
				" [SynId] INTEGER, "+
				" [Turn] INTEGER,  "+
				" [MarkTime] INTEGER) ";
                AccessAdo.ExecuteNonQuery(conn, crtSQL);

                crtSQL=" CREATE TABLE TContent ( "+
				" [NodeId] INTEGER CONSTRAINT PK_TTree27 PRIMARY KEY, "+
				" [Content] MEMO, "+
				" [Note] MEMO, "+
				" [Link] MEMO) ";
                AccessAdo.ExecuteNonQuery(conn, crtSQL);

                crtSQL = " CREATE TABLE TAttachment ( " +
                " [AffixId] INTEGER CONSTRAINT PK_TTree28 PRIMARY KEY, " +
                " [NodeId] INTEGER, " +
                " [Title] VARCHAR, " +
                " [Data] IMAGE , " +
                " [Size] INTEGER, " +
                " [Time] INTEGER)";
                AccessAdo.ExecuteNonQuery(conn, crtSQL);

            }

        }

        private bool closeAll()
        {
            //关闭所有打开的文档
                string IsDocModi = "false";
                foreach (DocumentForm doc in dockPanel1.Documents)
                {
                    if (doc.Scintilla.Modified)
                        IsDocModi = "true";
                }
                if (IsDocModi == "true")
                {
                    DialogResult dr = MessageBox.Show(this, "是否保存所有文档?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.Cancel)
                    {
                        return false;
                    }
                    else if (dr == DialogResult.No)
                    {
                        CloseAllDoment();
                        return true;
                    }
                    else
                    {
                        foreach (DocumentForm doc in dockPanel1.Documents)
                        {
                            doc.Save();
                        }
                        CloseAllDoment();
                        return true;
                    }
                }

                else
                {
                    CloseAllDoment();
                    return true;
                }
        }

        //关闭所有文档
        private void CloseAllDoment()
        {
            IDockContent[] documents = dockPanel1.DocumentsToArray();

            foreach (IDockContent content in documents)
            {
                content.DockHandler.Close();
            }
        }

        //打开数据库
        private void toolStripMenuItemOpenDB_Click(object sender, EventArgs e)
        {
           //关闭所有打开的文档
            if (closeAll() == false)
                return;
            //刷新附件列表数据
            Attachment.ActiveNodeId = "-1";
            Attachment.AttForm.ReFreshAttachGrid();
            //TODO
            //清空搜索以及书签列表

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "数据文件(*.mdb)|*.mdb";
            openFileDialog1.RestoreDirectory = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
               //修改连接字符串，并重新加载
                string conStr = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + fileName;
                UpdateConnectionStringsConfig("DBConn",conStr);
                
                //重新加载所有资源
                AccessAdo.strConnection = conStr;
                

                frTree.frmTree_Reload();
            }
        }


        ///<summary> 
        ///更新连接字符串  
        ///</summary> 
        ///<param name="newName">连接字符串名称</param> 
        ///<param name="newConString">连接字符串内容</param> 
        private static void UpdateConnectionStringsConfig(string newName,
            string newConString)
        {
            bool isModified = false;    //记录该连接串是否已经存在  
            //如果要更改的连接串已经存在  
            if (ConfigurationManager.ConnectionStrings[newName] != null)
            {
                isModified = true;
            }
            //新建一个连接字符串实例  
            ConnectionStringSettings mySettings =
                new ConnectionStringSettings(newName, newConString);
            // 打开可执行的配置文件*.exe.config  
            Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // 如果连接串已存在，首先删除它  
            if (isModified)
            {
                config.ConnectionStrings.ConnectionStrings.Remove(newName);
            }
            // 将新的连接串添加到配置文件中.  
            config.ConnectionStrings.ConnectionStrings.Add(mySettings);
            // 保存对配置文件所作的更改  
            config.Save(ConfigurationSaveMode.Modified);
            // 强制重新载入配置文件的ConnectionStrings配置节  
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }

        //压缩数据库
        private void toolStripMenuItemZipDB_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "数据文件(*.mdb)|*.mdb";
            openFileDialog1.RestoreDirectory = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                //压缩
                Compact(fileName);
            }
        }

        ///压缩修复ACCESS数据库,mdbPath为数据库绝对路径
        public void Compact(string mdbPath)
        {
            if (!File.Exists(mdbPath)) //检查数据库是否已存在
            {
                throw new Exception("目标数据库不存在,无法压缩");
            }
            //声明临时数据库的名称
            string temp = DateTime.Now.Year.ToString();
            temp += DateTime.Now.Month.ToString();
            temp += DateTime.Now.Day.ToString();
            temp += DateTime.Now.Hour.ToString();
            temp += DateTime.Now.Minute.ToString();
            temp += DateTime.Now.Second.ToString() + ".bak";
            temp = mdbPath.Substring(0, mdbPath.LastIndexOf("\\") + 1) + temp;
            //定义临时数据库的连接字符串
            string temp2 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + temp;
            //定义目标数据库的连接字符串
            string mdbPath2 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbPath;
            //创建一个JetEngineClass对象的实例
            JRO.JetEngineClass jt = new JRO.JetEngineClass();
            //使用JetEngineClass对象的CompactDatabase方法压缩修复数据库
            jt.CompactDatabase(mdbPath2, temp2);
            //拷贝临时数据库到目标数据库(覆盖)
            File.Copy(temp, mdbPath, true);
            //最后删除临时数据库
            File.Delete(temp);
        }

        //备份当前数据库
        private void toolStripMenuItemBackUpDB_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection(AccessAdo.strConnection);
            string Path1 = conn.DataSource;

            SaveFileDialog sf = new SaveFileDialog();
            //设置文件类型
            sf.Filter = "数据文件(*.mdb)|*..mdb";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                string Path2 = sf.FileName;
                Backup(Path1, Path2);
            }
        }

        /// 备份数据库,mdb1,源数据库绝对路径; mdb2: 目标数据库绝对路径 
        public void Backup(string mdb1, string mdb2)
        {
            if (!File.Exists(mdb1))
            {
                throw new Exception("源数据库不存在");
            }
            try
            {
                File.Copy(mdb1, mdb2, true);
            }
            catch (IOException ixp)
            {
                throw new Exception(ixp.ToString());
            }
        }

        private void toolStripMenuItemUndo_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.UndoRedo.Undo();
        }

        private void toolStripMenuItemRedo_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.UndoRedo.Redo();
        }

        private void toolStripMenuItemCut_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Clipboard.Cut();
        }

        private void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Clipboard.Copy();
        }

        private void toolStripMenuItempaste_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Clipboard.Paste();
        }

        private void toolStripMenuItemFind_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.FindReplace.ShowFind();
        }

        private void toolStripMenuItemReplace_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.FindReplace.ShowReplace();
        }

        private void toolStripMenuItemSelAll_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Selection.SelectAll();
        }

        private void toolStripButtonDel_Click(object sender, EventArgs e)
        {
            frTree.DelNode();
        }

        //自动换行
        private void toolStripMenuItemAutoWrap_Click(object sender, EventArgs e)
        {
            // 所有打开的文档自动换行
            toolStripMenuItemAutoWrap.Checked = !toolStripMenuItemAutoWrap.Checked;
            foreach (DocumentForm doc in dockPanel1.Documents)
            {
                if (toolStripMenuItemAutoWrap.Checked)
                    doc.Scintilla.LineWrapping.Mode = LineWrappingMode.Word;
                else
                    doc.Scintilla.LineWrapping.Mode = LineWrappingMode.None;
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripMenuItemFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.AllowScriptChange = true;
            fontDialog.ShowEffects = false;
            if (fontDialog.ShowDialog() != DialogResult.Cancel)
            {
                ActiveDocument.Scintilla.Font = fontDialog.Font;//将当前选定的文字改变字体
            }
        }

        private void toolStripMenuItemIsShowTB_Click(object sender, EventArgs e)
        {
            toolStripMenuItemIsShowTB.Checked = !toolStripMenuItemIsShowTB.Checked;

            toolStripMain.Visible = toolStripMenuItemIsShowTB.Checked;
        }

        private void toolStripMenuItemIsShowSb_Click(object sender, EventArgs e)
        {
            toolStripMenuItemIsShowSb.Checked = !toolStripMenuItemIsShowSb.Checked;
            statusStripMain.Visible = toolStripMenuItemIsShowSb.Checked;
        }

        private void toolStripMenuItemIsShowLeft_Click(object sender, EventArgs e)
        {
            frTree.Show(dockPanel1);
            frmMark.Show(dockPanel1);
            FormFind.Show(dockPanel1);
        }

        private void toolStripMenuItemIsShowAtt_Click(object sender, EventArgs e)
        {
            frmAttchment.Show(dockPanel1);
        }

        private void toolStripButtonProp_Click(object sender, EventArgs e)
        {
            frTree.ShowProp();
        }

        //打开欢迎界面
        public void openWelcomePage()
        {
            WelcomeDoc welDoc = new WelcomeDoc();
            welDoc.NodeId = "-1";
            welDoc.Show(dockPanel1);
            Attachment.isWelcomePageopen = "1";
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Attachment.frmMain = this;

            //加载布局
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");

            if (File.Exists(configFile))
                dockPanel1.LoadFromXml(configFile, m_deserializeDockContent);

            //打开欢迎界面
            openWelcomePage();
        }

        //状态栏标题
        public void showFullPathTime(string path,string crtTime)
        {
            this.toolStripStatusLabelTitle.Text = path;
            this.toolStripStatusLabelDocTime.Text = crtTime;
        }

        //删除所有书签
        private void toolStripMenuItemDelAllMark_Click(object sender, EventArgs e)
        {
            AccessAdo.ExecuteNonQuery("update ttree set marktime=0");
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
                dockPanel1.SaveAsXml(configFile);
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(FormTreeLeft).ToString())
                return frTree;
            else if (persistString == typeof(FormAttachment).ToString())
                return frmAttchment;
            else if (persistString == typeof(DocFind).ToString())
                return FormFind;
            else if (persistString == typeof(DocMark).ToString())
                return frmMark;
            else
            {
                return null;
            }
        }

        private void toolStripButtonCloseTab_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonCloseTabAll_Click(object sender, EventArgs e)
        {

        }
    }
}
