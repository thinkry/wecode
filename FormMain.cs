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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web;
using System.Xml;
using System.Net;
using System.Threading;

namespace WeCode1._0
{
    public partial class FormMain : Form
    {
        private FormTreeLeft frTree;
        private YouDaoTree frYoudaoTree;

        private FormAttachment frmAttchment;
        private DocMark frmMark;
        private DocFind FormFind;

        private DeserializeDockContent m_deserializeDockContent;


        #region Fields

        private int _newDocumentCount = 0;
        private string[] _args;
        private int _zoomLevel=0;
        private const int LINE_NUMBERS_MARGIN_WIDTH = 50;

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

        // 当收到第二个进程的通知时，显示窗体  
        private void OnProgramStarted(object state, bool timeout)
        {
            toolStripMenuItem2_Click(null,null);  
        }  

        public FormMain()
        {
            ThreadPool.RegisterWaitForSingleObject(Program.ProgramStarted, OnProgramStarted, null, -1, false); 

            InitializeComponent();

            //版本检查
            //DateTime d1 = DateTime.Now;
            //CheckVer();
            //DateTime d2 = DateTime.Now;
            //TimeSpan sp = d2 - d1;
            //MessageBox.Show(sp.TotalMilliseconds.ToString());

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

            //显示有道树窗口
            frYoudaoTree = new YouDaoTree();
            frYoudaoTree.formParent = this;
            //frYoudaoTree.Show(dockPanel1);

            //显示附件窗口
            Attachment.ActiveNodeId = "-1";
            frmAttchment = new FormAttachment();
            Attachment.AttForm = frmAttchment;
            //frmAttchment.Show(dockPanel1);

            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            IniPanel();

            //第一次打开界面，先判断token是否有效（为空或者过期）
            //有效，同步XML到本地，加载有道云树目录;无效，打开授权页面进行授权
            //授权成功后，云端创建两个目录以及配置文件，然后加载树目录
            IniYouDaoAuthor();


            //上报统计信息
            Thread t = new Thread(new ThreadStart(UpUerInfo));
            t.Start(); 
        }

        public void UpUerInfo()
        {
            try
            {
                string uuid = PubFunc.GetConfiguration("UUID");
                string ver = PubFunc.GetConfiguration("Version");
                string Token = PubFunc.GetConfiguration("AccessToken");
                string url = "http://wecode.thinkry.com/c/ping?u=" + uuid + "&v=" + ver + "&yd=" + Token;

                WebClient MyWebClient = new WebClient();

                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于对向Internet资源的请求进行身份验证的网络凭据。

                Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据

            }
            catch (WebException webEx)
            {

                Console.WriteLine(webEx.Message.ToString());

            }
            
        }

        //初始化相关
        private void IniPanel()
        {
            try
            {
                Attachment.frmMain = this;

                //加载布局
                string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");

                if (File.Exists(configFile))
                    dockPanel1.LoadFromXml(configFile, m_deserializeDockContent);

                //打开欢迎界面
                openWelcomePage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void setSkin()
        {
            this.dockPanel1.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.StartColor = Color.FromArgb(228, 226, 213);
            this.dockPanel1.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.EndColor = Color.FromArgb(228, 226, 213);

            this.dockPanel1.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.StartColor = Color.FromArgb(204, 199, 186);
            this.dockPanel1.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.EndColor = Color.FromArgb(204, 199, 186);

            //this.dockPanel1.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.StartColor = Color.FromArgb(204, 199, 186);
            //this.dockPanel1.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.EndColor = Color.FromArgb(204, 199, 186);
        }

        //初始化授权相关
        public void IniYouDaoAuthor()
        {
            //判断token是否有效
            string IsAuthor = AuthorAPI.GetIsAuthor();
            if (IsAuthor != "OK")
            {
                //禁用云目录
                this.toolStripMenuItemLogin.Visible = true;
                this.toolStripMenuItemUinfo.Visible = false;
                Text = "WeCode--未登录";

                Attachment.IsTokeneffective = 0;
                this.Load += new System.EventHandler(this.showNoAuthor);
            }
            else
            {
                //从云端拉取XML同步到本地
                XMLAPI.Yun2XML();
                Attachment.IsTokeneffective = 1;
                //获取用户信息并禁用登陆按钮
                this.toolStripMenuItemLogin.Visible = false;
                this.toolStripMenuItemUinfo.Visible = true;
                Text = "WeCode--已登录";
            }
            
        }

        private void showNoAuthor(object sender, System.EventArgs e)
        {
            //MessageBox.Show("未授权有道云笔记或者授权已过期，请点击用户--登录以重新授权！");
            if (ConfigurationManager.AppSettings["authorAlert"] != "0")
            {
                if (MessageBox.Show("未授权有道云笔记或者授权已过期，点击菜单用户--登录以重新授权！\n\n点击“确定”不再提醒", "登录提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    PubFunc.SetConfiguration("authorAlert", "0");
                }
            }

        }
        
        //上移
        private void toolStripButtonUp_Click(object sender, EventArgs e)
        {
            if(dockPanel1.ActiveContent.GetType()==typeof(FormTreeLeft))
            {
                frTree.setNodeUp();
            }
            else if (dockPanel1.ActiveContent.GetType() == typeof(YouDaoTree))
            {
                frYoudaoTree.setNodeUp();
            }
            else
            {
                return;
            }
        }

        //下移
        private void toolStripButtonDown_Click(object sender, EventArgs e)
        {
            if (dockPanel1.ActiveContent.GetType() == typeof(FormTreeLeft))
            {
                frTree.setNodeDown();
            }
            else if (dockPanel1.ActiveContent.GetType() == typeof(YouDaoTree))
            {
                frYoudaoTree.setNodeDown();
            }
            else
            {
                return;
            }
        }

        //关闭文章
        public void CloseDoc(string nodeId)
        {
            if (Attachment.isWelcomePageopen == "0")
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
        }

        //打开文章
        public void openNew(string nodeId,string treeLocation,string updateTime)
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
                    Attachment.isnewOpenDoc = "0";
                    isOpen = true;
                    break;
                }
            }

            // Open the files
            if (!isOpen)
                OpenFile(nodeId,treeLocation,updateTime);
        }

        //打开云笔记
        public void openNewYouDao(string nodeId,string title,string treeLocation,string updatetime)
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
                    Attachment.isnewOpenDoc = "0";
                    isOpen = true;
                    break;
                }
            }

            // Open the files
            if (!isOpen)
                OpenFileYouDao(nodeId,title,treeLocation,updatetime);
        }

        private DocumentForm OpenFileYouDao(string nodeId, string title, string treeLocation, string updatetime)
        {
            Attachment.isnewOpenDoc = "1";
            //获取文章信息

            string Content = NoteAPI.GetNote(nodeId);

            DocumentForm doc = new DocumentForm();
            doc.TreeLocation = treeLocation;
            doc.LastUpdateTime = updatetime;

            SetScintillaToCurrentOptions(doc);
            doc.Scintilla.Text = Content;
            doc.Scintilla.UndoRedo.EmptyUndoBuffer();
            doc.Scintilla.Modified = false;
            doc.Text = title;
            doc.NodeId = nodeId;
            doc.Type = "online";
            doc.Show(dockPanel1);

            return doc;
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


        private DocumentForm OpenFile(string nodeId,string treeLocation,string updateTime)
        {
            Attachment.isnewOpenDoc = "1";
            //获取文章信息
            string SQL = "select Title,Content from TContent inner join TTree on TContent.NodeId=Ttree.NodeId where TContent.NodeId=" + nodeId;
            DataTable temp = AccessAdo.ExecuteDataSet(SQL, null).Tables[0];
            if (temp.Rows.Count == 0)
                return null;
            string Title = temp.Rows[0]["Title"].ToString();
            string Content = temp.Rows[0]["Content"].ToString();

            DocumentForm doc = new DocumentForm();
            doc.TreeLocation = treeLocation;
            doc.LastUpdateTime = updateTime;

            SetScintillaToCurrentOptions(doc);
            doc.Scintilla.Text = Content;
            doc.Scintilla.UndoRedo.EmptyUndoBuffer();
            doc.Scintilla.Modified = false;
            doc.Text = Title;
            doc.NodeId = nodeId;
            doc.Type = "local";
            doc.Show(dockPanel1);
            

            return doc;
        }

        //配置相关显示参数
        private void SetScintillaToCurrentOptions(DocumentForm doc)
        {
            //// Turn on line numbers?
            if (toolStripMenuItemLn.Checked)
                doc.Scintilla.Margins.Margin0.Width = LINE_NUMBERS_MARGIN_WIDTH;
            else
                doc.Scintilla.Margins.Margin0.Width = 0;

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
            doc.Scintilla.ZoomFactor = 0;
        }


        private void toolStripButtonNewText_Click(object sender, EventArgs e)
        {
            if (dockPanel1.ActiveContent.GetType() == typeof(FormTreeLeft))
            {
                frTree.NewDoc();
            }
            else if (dockPanel1.ActiveContent.GetType() == typeof(YouDaoTree))
            {
                frYoudaoTree.NewDoc();
            }
        }

        private void toolStripButtonNewDir_Click(object sender, EventArgs e)
        {
            if (dockPanel1.ActiveContent.GetType() == typeof(FormTreeLeft))
            {
                frTree.NewDir();
            }
            else if (dockPanel1.ActiveContent.GetType() == typeof(YouDaoTree))
            {
                frYoudaoTree.NewDir();
            }
        }

        //保存
        private void toolStripButtonSv_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Save();
        }


        //设置路径，修改时间
        public void SetTreeLocat(string treeLocation,string UpdateTime)
        {
            ActiveDocument.TreeLocation = treeLocation;
            ActiveDocument.LastUpdateTime = UpdateTime;
        }

        //设置语言(激活文档)
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

        //设置语言
        public void SetLanguageByDoc(string language,string id,string newTitle,string newTreeLocation)
        {
            //根据id设置语言
            if (Attachment.isWelcomePageopen == "0")
            {
                foreach (DocumentForm documentForm in dockPanel1.Documents)
                {
                    if (id.Equals(documentForm.NodeId, StringComparison.OrdinalIgnoreCase))
                    {
                        documentForm.SetLanguageByDoc(language,newTitle,newTreeLocation);
                        break;
                    }
                }
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
                string crtSQL = " CREATE TABLE TTree ( " +
                " [NodeId] INTEGER CONSTRAINT PK_TTree26 PRIMARY KEY, " +
                " [Title] VARCHAR, " +
                " [ParentId] INTEGER, " +
                " [Type] INTEGER, " +
                " [CreateTime] INTEGER, " +
                " [SynId] INTEGER, " +
                " [Turn] INTEGER,  " +
                " [MarkTime] INTEGER, " +
                " [UpdateTime] INTEGER, "+
                " [IsLock] INTEGER DEFAULT 0 ) ";
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

                crtSQL = " CREATE TABLE MyKeys ( " +
                " [KeyE] MEMO, " +
                " [KeyD5] MEMO) ";
                AccessAdo.ExecuteNonQuery(conn, crtSQL);

            }

        }

        private bool closeAll()
        {
            //关闭所有打开的文档
                string IsDocModi = "false";
                if (Attachment.isWelcomePageopen == "0")
                {
                    foreach (DocumentForm doc in dockPanel1.Documents)
                    {
                        if (doc.Scintilla.Modified)
                            IsDocModi = "true";
                    }
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
            if (Attachment.isWelcomePageopen == "0")
            {
                IDockContent[] documents = dockPanel1.DocumentsToArray();

                foreach (IDockContent content in documents)
                {
                    content.DockHandler.Close();
                }
                
            }
        }

        //打开数据库
        private void toolStripMenuItemOpenDB_Click(object sender, EventArgs e)
        {
           
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "数据文件(*.mdb)|*.mdb";
            openFileDialog1.RestoreDirectory = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //关闭所有打开的文档
                if (closeAll() == false)
                    return;
                //刷新附件列表数据
                if (Attachment.isWelcomePageopen == "0")
                {
                    openWelcomePage();
                }
                Attachment.ActiveNodeId = "-1";
                Attachment.AttForm.ReFreshAttachGrid();


                string fileName = openFileDialog1.FileName;
               //修改连接字符串，并重新加载
                string conStr = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + fileName;
                UpdateConnectionStringsConfig("DBConn",conStr);
                
                //重新加载所有资源
                AccessAdo.strConnection = conStr;
                
                //升级数据库
                CheckDb.UpdateDB();

                frTree.frmTree_Reload();
                ReSetMarkFind();
            }
        }


        public void ReSetMarkFind()
        {
            //清空搜索重新加载书签
            FormFind.IniData();
            frmMark.RefreshGrid("local");
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
            sf.Filter = "数据文件(*.mdb)|*.mdb";
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
            if (ActiveDocument != null)
                 ActiveDocument.Scintilla.FindReplace.ShowFind();
        }

        private void toolStripMenuItemReplace_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.FindReplace.ShowReplace();
        }

        private void toolStripMenuItemSelAll_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Selection.SelectAll();
        }

        private void toolStripButtonDel_Click(object sender, EventArgs e)
        {
            if (dockPanel1.ActiveContent.GetType() == typeof(FormTreeLeft))
            {
                frTree.DelNode();
            }
            else if (dockPanel1.ActiveContent.GetType() == typeof(YouDaoTree))
            {

                frYoudaoTree.DelNode();
            }
        }

        //自动换行
        private void toolStripMenuItemAutoWrap_Click(object sender, EventArgs e)
        {
            //// 所有打开的文档自动换行
            //toolStripMenuItemAutoWrap.Checked = !toolStripMenuItemAutoWrap.Checked;
            //if (Attachment.isWelcomePageopen == "0")
            //{
            //    foreach (DocumentForm doc in dockPanel1.Documents)
            //    {
            //        if (toolStripMenuItemAutoWrap.Checked)
            //        {
            //            doc.Scintilla.LineWrapping.Mode = LineWrappingMode.Word;
            //        }
            //        else
            //        {
            //            doc.Scintilla.LineWrapping.Mode = LineWrappingMode.None;
            //        }
            //    }
            //}

            //if (toolStripMenuItemAutoWrap.Checked)
            //{
            //    PubFunc.SetConfiguration("AutoLineWrap", "1");
            //}
            //else
            //{
            //    PubFunc.SetConfiguration("AutoLineWrap", "0");
            //}
            
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripMenuItemFont_Click(object sender, EventArgs e)
        {
            FontConverter fc = new FontConverter();

            FontDialog fontDialog = new FontDialog();
            fontDialog.AllowScriptChange = true;
            fontDialog.ShowEffects = false;
            fontDialog.AllowVerticalFonts = false;
            if (PubFunc.GetConfiguration("defaultFont") != "")
            {
                fontDialog.Font = (Font)fc.ConvertFromString(PubFunc.GetConfiguration("defaultFont"));
            }
            if (fontDialog.ShowDialog() != DialogResult.Cancel)
            {
                string sfont = fc.ConvertToString(fontDialog.Font);

                if (Attachment.isWelcomePageopen == "1")
                {
                    return;
                }
                foreach (DocumentForm doc in dockPanel1.Documents)
                {
                    SetFont(doc,(Font)fc.ConvertFromString(sfont));
                }

                PubFunc.SetConfiguration("defaultFont", sfont);
            }
        }

        public void SetFont(DocumentForm doc,Font xFont)
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

        //显示隐藏工作区
        private void toolStripMenuItemIsShowLeft_Click(object sender, EventArgs e)
        {
            //toolStripMenuItemIsShowLeft.Checked = !toolStripMenuItemIsShowLeft.Checked;
            //if (toolStripMenuItemIsShowLeft.Checked == true)
            //{
            frmMark.Show(dockPanel1);
            //FormFind.Show(dockPanel1);
            frYoudaoTree.Show(dockPanel1);
            frTree.Show(dockPanel1);
            //}
            //else { 
            //    //隐藏工作区
            //    this.frTree.Hide();
            //    this.frYoudaoTree.Hide();
            //    //this.FormFind.Close();
            //    this.frmMark.Hide();
            //}
        }

        //显示隐藏附件
        private void toolStripMenuItemIsShowAtt_Click(object sender, EventArgs e)
        {
            //toolStripMenuItemIsShowAtt.Checked = !toolStripMenuItemIsShowAtt.Checked;
            //if (toolStripMenuItemIsShowAtt.Checked == true)
            //{
                frmAttchment.Show(dockPanel1);

            //}
            //else
            //{
            //    frmAttchment.Hide();
            //}
        }

        private void toolStripButtonProp_Click(object sender, EventArgs e)
        {
            if (dockPanel1.ActiveContent.GetType() == typeof(FormTreeLeft))
            {
                frTree.ShowProp();
            }
            else if (dockPanel1.ActiveContent.GetType() == typeof(YouDaoTree))
            {

                frYoudaoTree.ShowProp();
            }
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
            CheckVer();

            setSkin();

            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            //设置菜单栏勾选状态
            SetMenuCheck();
        }

        public void SetMenuCheck()
        {
            if (PubFunc.GetConfiguration("AutoLineWrap") == "1")
            {
                toolStripMenuItemAutoWrap1.Checked = true;
            }
            else
            {
                toolStripMenuItemAutoWrap1.Checked = false;
            }
        }

        //清空状态栏信息
        public void clearStatus()
        {
            this.toolStripStatusLabelTitle.Text = "";
            this.toolStripStatusLabelDocTime.Text = "";
            this.toolStripStatusLabelRowCol.Text = "";
        }

        //状态栏标题
        public void showFullPathTime(string path,string crtTime)
        {
            this.toolStripStatusLabelTitle.Text = path;
            this.toolStripStatusLabelDocTime.Text = crtTime;
        }

        public void showLinePosition(int x, int y)
        {
            this.toolStripStatusLabelRowCol.Text = "行 " + x.ToString() + "        列 " + y.ToString();
        }

        //删除所有书签
        private void toolStripMenuItemDelAllMark_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("删除所有书签（包括本地笔记本和有道云）？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            } 
            AccessAdo.ExecuteNonQuery("update ttree set marktime=0");
            //删除有道云书签
            if (Attachment.IsTokeneffective == 1)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("TreeNodeLocal.xml");
                XmlNodeList xlist = doc.SelectNodes("//note[@isMark='1']");
                foreach (XmlNode xnode in xlist)
                {
                    xnode.Attributes["isMark"].Value = "0";
                }
                doc.Save("TreeNodeLocal.xml");
                XMLAPI.XML2Yun();
            }

            ReSetMarkFind();
        }

        //退出
        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {

            //先判断查找是否显示，显示先隐藏
            if (this.FormFind.IsHidden == false)
                FormFind.Hide();

            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
                dockPanel1.SaveAsXml(configFile);

            this.notifyIcon1.Visible=false;
            System.Environment.Exit(System.Environment.ExitCode);
            this.Dispose();
            this.Close();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            //先判断查找是否显示，显示先隐藏
            if (this.FormFind.IsHidden == false)
                FormFind.Hide();
            this.Visible = false;
            //气泡提示
            if (PubFunc.GetConfiguration("FirstRun") == "1")
            {
                //仅第一次弹出提示
                notifyIcon1.ShowBalloonTip(100, "提示", "wecode已最小化至托盘", ToolTipIcon.Info);
                PubFunc.SetConfiguration("FirstRun", "0");
            }
            //string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            //    dockPanel1.SaveAsXml(configFile);
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(FormTreeLeft).ToString())
                return frTree;
            else if (persistString == typeof(YouDaoTree).ToString())
                return frYoudaoTree;
            else if (persistString == typeof(FormAttachment).ToString())
                return frmAttchment;
            else if (persistString == typeof(DocMark).ToString())
                return frmMark;
            else
            {
                return null;
            }
        }

        //关闭
        private void toolStripButtonCloseTab_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Close();
        }

        //关闭所有
        private void toolStripButtonCloseTabAll_Click(object sender, EventArgs e)
        {
            if (Attachment.isWelcomePageopen == "0")
            {
                IDockContent[] documents = dockPanel1.DocumentsToArray();

                foreach (IDockContent content in documents)
                {
                    content.DockHandler.Close();
                }
            }
        }

        private void toolStripMenuItemExportHTML_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.ExportAsHtml();
        }

        private void toolStripMenuItemPrint_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Printing.Print();
        }

        private void toolStripMenuItemPv_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Printing.PrintPreview();
        }


        //授权，并且初始化用户信息
        private void toolStripMenuItemAuthor_Click(object sender, EventArgs e)
        {
            
        }


        //在目录窗口激活或者失去焦点时禁用菜单按钮
        public void HideShowMenu(Boolean sMode)
        {
              
              this.toolStripButtonNewText.Enabled = sMode;
              this.toolStripButtonNewDir.Enabled = sMode;
              this.toolStripButtonDel.Enabled = sMode;
              this.toolStripButtonProp.Enabled = sMode;
              this.toolStripButtonUp.Enabled = sMode;
              this.toolStripButtonDown.Enabled = sMode;

        }

        //窗体发生变化时触发
        private void dockPanel1_ActiveContentChanged(object sender, EventArgs e)
        {
            if (dockPanel1.ActiveContent != null)
            {
                try
                {
                    if (dockPanel1.ActiveContent.GetType() == typeof(FormTreeLeft))
                    {
                        HideShowMenu(true);
                    }
                    else if (dockPanel1.ActiveContent.GetType() == typeof(YouDaoTree) && Attachment.IsTokeneffective == 1)
                    {
                        HideShowMenu(true);
                    }
                    else
                    {
                        HideShowMenu(false);
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            //IniYouDaoAuthor();
        }

        //关于
        private void toolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            About ab = new About();
            ab.ShowDialog();
        }

        //保存
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Save();
        }

        //撤销
        private void toolStripButtonUndo_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.UndoRedo.Undo();
        }

        //反撤销
        private void toolStripButtonRedo_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.UndoRedo.Redo();
        }

        //剪切
        private void toolStripButtonCut_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Clipboard.Cut();
        }

        //复制
        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Clipboard.Copy();
        }

        //粘贴
        private void toolStripButtonPaste_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Clipboard.Paste();
        }

        //授权
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            FormAuthor frAuthor = new FormAuthor();
            DialogResult dr = frAuthor.ShowDialog();
            if (dr == DialogResult.OK)
            {
                MessageBox.Show("授权成功!");
                //授权成功，禁用登陆按钮，初始化目录以及配置信息，显示有道云树
                this.toolStripMenuItemLogin.Visible = false;
                this.toolStripMenuItemUinfo.Visible = true;
                Text = "WeCode--已登录";
                //初始化目录以及配置信息
                AuthorAPI.CreatewecodeConfig();
                //从云端拉取目录配置到本地
                XMLAPI.Yun2XML();

                Attachment.IsTokeneffective = 1;
                //加载树
                frYoudaoTree.IniYoudaoTree();
                HideShowMenu(true);
            }
        }

        //查看用户信息
        private void toolStripMenuItemUinfo_Click(object sender, EventArgs e)
        {
            string sUserInfo = NoteAPI.GetUserInfo();
            if (sUserInfo=="")
                return;
            JObject jo = JObject.Parse(sUserInfo);
            float userdsize=(float)jo["used_size"];
            float totalsize = (float)jo["total_size"];
            string user = jo["user"].ToString();
            string info = "当前用户：" + user + "\n";
            info += "总共空间：" + totalsize / 1048576 + "MB\n";
            info += "已用空间：" + userdsize / 1048576 + "MB\n";
            //MessageBox.Show(info,"用户信息");
            UserInfo uInfo = new UserInfo(info);
            uInfo.ShowDialog();
            if (uInfo.DialogResult == DialogResult.Abort)
            {
                LoginOut();
            }

        }

        //注销操作
        public void LoginOut()
        {
            //修改配置
            PubFunc.SetConfiguration("AccessToken", "");
            //禁用云目录
            this.toolStripMenuItemLogin.Visible = true;
            this.toolStripMenuItemUinfo.Visible = false;
            Text = "WeCode--未登录";

            Attachment.IsTokeneffective = 0;
            frYoudaoTree.YdLogOut();
            //关闭所有已打开的有道文章
            //避免保存的提示
            Attachment.isDeleteClose = "1";

            if (Attachment.isWelcomePageopen == "0")
            {
                IDockContent[] documents = dockPanel1.DocumentsToArray();

                foreach (IDockContent content in documents)
                {
                    if (((DocumentForm)content).Type == "online")
                    {
                        content.DockHandler.Close();
                    }
                }
            }

            Attachment.isDeleteClose = "0";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //DocumentForm doc = new DocumentForm();
            //SetScintillaToCurrentOptions(doc);
            //doc.Text = "aaa";
            //doc.Show(dockPanel1);
            ////toolIncremental.Searcher.Scintilla = doc.Scintilla;
            ////return doc;
        }

        //搜索
        public void OpenSerch(string sType)
        {
            if (sType == "local")
            {
                FormFind.Show();
                FormFind.SetSerchType("local");
            }
            else if (sType == "online")
            {
                FormFind.Show();
                FormFind.SetSerchType("online");
            }
        }

        //显示主窗口
        public void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                this.Visible = true;
            }
            else
            {
                this.WindowState = Attachment.WinState;
            }
        }

        //退出
        private void toolStripMenuItem3_Click_1(object sender, EventArgs e)
        {
            //先判断查找是否显示，显示先隐藏
            if (this.FormFind.IsHidden == false)
                FormFind.Hide();

            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            dockPanel1.SaveAsXml(configFile);

            this.notifyIcon1.Visible = false;
            System.Environment.Exit(System.Environment.ExitCode);
            this.Dispose();
            this.Close();
        }

        //双击托盘区显示窗口
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible == false)
            {
                this.Visible = true;
            }
            else
            {
                this.WindowState = Attachment.WinState;
            }
        }

        //查找文章
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            OpenSerch("local");
        }


        #region 版本检查
        //定义一个委托 
        public delegate void MyInvoke();

        //线程方法 
        private void DoWork() 
        { 
        //其他操作  
        MyInvoke mi = new MyInvoke(Check); 
        this.BeginInvoke(mi); 
        } 

        private void CheckVer()
        {
            bool bResult = true;
            bool IsCheck = false;
            if (PubFunc.GetConfiguration("checkVerAlert") == "1")
            {
                IsCheck = true;
            }
            else if (PubFunc.GetConfiguration("checkVerAlert") == "0")
            {
                string lastTime = PubFunc.GetConfiguration("lastVerAlertTime");
                if (lastTime != "")
                {
                    DateTime d1 = DateTime.Now;
                    DateTime d2 = DateTime.Parse(lastTime);
                    TimeSpan tp = d1 - d2;
                    if (tp.TotalDays > 15)
                    {
                        IsCheck = true;
                    }
                }
            }

            if (IsCheck == true)
            {
                //检查新版本，新线程中检查
                Thread thread = new Thread(new ThreadStart(Check));
                thread.Start(); 
                //Check();
            }
        }

        private void Check()
        {
            string pageHtml;
            try
            {
                WebClient MyWebClient = new WebClient();

                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于对向Internet资源的请求进行身份验证的网络凭据。

                Byte[] pageData = MyWebClient.DownloadData("http://thinkry.github.io/wecode/version.xml"); //从指定网站下载数据

                pageHtml = Encoding.UTF8.GetString(pageData);
                pageHtml = pageHtml.Trim();
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(pageHtml);
                //xDoc.Load("version.xml");
                string newVer = xDoc.DocumentElement.FirstChild.Attributes["latestver"].Value;
                string oldVer = PubFunc.GetConfiguration("Version");
                int rsCompare = PubFunc.VersionCompare(newVer, oldVer);

                if (rsCompare==1)//有新版本
                {
                    string tips = "";
                    XmlNode ntips = xDoc.SelectSingleNode("//tips");
                    foreach (XmlNode node in ntips)
                    {
                        tips += node.InnerText + "\r\n";
                    }
                    string sdlUrl = xDoc.SelectSingleNode("//download").Attributes["url"].Value;

                    DialogUpdate upDia = new DialogUpdate(newVer, tips, sdlUrl);
                    DialogResult dr = upDia.ShowDialog();

                    if (dr == DialogResult.No||dr==DialogResult.Cancel)
                    {
                        //继续运行
                    }
                }

            }
            catch (WebException webEx)
            {

                Console.WriteLine(webEx.Message.ToString());

            }
        }
        #endregion

        //主页
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            string target = "http://thinkry.github.io/wecode";
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

        //意见反馈
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
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

        //放大
        private void toolStripButtonZoomOut_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null&&_zoomLevel<=19)
            {
                _zoomLevel++;
                ActiveDocument.Scintilla.ZoomFactor = _zoomLevel;
            }
        }

        private void toolStripButtonZoomIn_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null && _zoomLevel >=-9)
            {
                _zoomLevel--;
                ActiveDocument.Scintilla.ZoomFactor = _zoomLevel;
            }
        }

        private void toolStripButtonResetZoom_Click(object sender, EventArgs e)
        {
            _zoomLevel=0;
            if (ActiveDocument != null)
            {
                ActiveDocument.Scintilla.ZoomFactor = _zoomLevel;
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            //this.WindowState = System.Windows.Forms.FormWindowState.Normal;
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                Attachment.WinState = this.WindowState;
            }
        }

        private void toolStripMenuItemZoomOut_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null && _zoomLevel <= 19)
            {
                _zoomLevel++;
                ActiveDocument.Scintilla.ZoomFactor = _zoomLevel;
            }
        }

        private void toolStripMenuItemZoomIn_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null && _zoomLevel >= -9)
            {
                _zoomLevel--;
                ActiveDocument.Scintilla.ZoomFactor = _zoomLevel;
            }
        }

        private void toolStripMenuItemResetZoom_Click(object sender, EventArgs e)
        {
            _zoomLevel = 0;
            if (ActiveDocument != null)
            {
                ActiveDocument.Scintilla.ZoomFactor = _zoomLevel;
            }
        }

        //显示行号
        private void toolStripMenuItemShowLn_Click(object sender, EventArgs e)
        {
            //if (Attachment.isWelcomePageopen == "0")
            //{
            //    toolStripMenuItemShowLn.Checked = !toolStripMenuItemShowLn.Checked;
            //    foreach (DocumentForm docForm in dockPanel1.Documents)
            //    {
            //        if (toolStripMenuItemShowLn.Checked)
            //            docForm.Scintilla.Margins.Margin0.Width = LINE_NUMBERS_MARGIN_WIDTH;
            //        else
            //            docForm.Scintilla.Margins.Margin0.Width = 0;
            //    }
            //}
        }

        //查找
        private void toolStripMenuItemSerch_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.FindReplace.ShowFind();
        }
        //替换
        private void toolStripMenuItemRp_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.FindReplace.ShowReplace();
        }

        private void toolStripButtonPrint_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Printing.Print();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            // 所有打开的文档自动换行
            toolStripMenuItemAutoWrap1.Checked = !toolStripMenuItemAutoWrap1.Checked;
            if (Attachment.isWelcomePageopen == "0")
            {
                foreach (DocumentForm doc in dockPanel1.Documents)
                {
                    if (toolStripMenuItemAutoWrap1.Checked)
                    {
                        doc.Scintilla.LineWrapping.Mode = LineWrappingMode.Word;
                    }
                    else
                    {
                        doc.Scintilla.LineWrapping.Mode = LineWrappingMode.None;
                    }
                }
            }

            if (toolStripMenuItemAutoWrap1.Checked)
            {
                PubFunc.SetConfiguration("AutoLineWrap", "1");
            }
            else
            {
                PubFunc.SetConfiguration("AutoLineWrap", "0");
            }
        }


        private void toolStripMenuItemLn_Click(object sender, EventArgs e)
        {
            if (Attachment.isWelcomePageopen == "0")
            {
                toolStripMenuItemLn.Checked = !toolStripMenuItemLn.Checked;
                foreach (DocumentForm docForm in dockPanel1.Documents)
                {
                    if (toolStripMenuItemLn.Checked)
                        docForm.Scintilla.Margins.Margin0.Width = LINE_NUMBERS_MARGIN_WIDTH;
                    else
                        docForm.Scintilla.Margins.Margin0.Width = 0;
                }
            }
        }

        private void toolStripMenuItemZo_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null && _zoomLevel <= 19)
            {
                _zoomLevel++;
                ActiveDocument.Scintilla.ZoomFactor = _zoomLevel;
            }
        }

        private void toolStripMenuItemZi_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null && _zoomLevel >= -9)
            {
                _zoomLevel--;
                ActiveDocument.Scintilla.ZoomFactor = _zoomLevel;
            }
        }

        private void toolStripMenuItemResZoom_Click(object sender, EventArgs e)
        {
            _zoomLevel = 0;
            if (ActiveDocument != null)
            {
                ActiveDocument.Scintilla.ZoomFactor = _zoomLevel;
            }
        }

        
    }
}
