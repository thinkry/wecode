using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;
using System.Data.OleDb;
using System.Threading;
using System.Configuration;
using WeCode1._0.youdao;

namespace WeCode1._0
{
    public partial class YouDaoTree : DockContent
    {
        public FormMain formParent;

        TreeNode dragDropTreeNode;
        DateTime startTime;

        private Thread beginInvokeThread;
        private delegate void beginInvokeDelegate();

        private void StartMethod()
        {
            //授权校验
            //判断token是否有效
            string IsAuthor = AuthorAPI.GetIsAuthor();
            if (IsAuthor != "OK")
            {
                //禁用云目录，授权失败
                Attachment.IsTokeneffective = 0;
                this.BeginInvoke(new beginInvokeDelegate(showNoAuthor));
                formParent.SetAuthor(false);
            }
            else
            {
                //从云端拉取XML同步到本地
                XMLAPI.Yun2XML();
                Attachment.IsTokeneffective = 1;
                this.BeginInvoke(new beginInvokeDelegate(beginInvokeMethod));
                formParent.SetAuthor(true);
            }
        }

        private void showNoAuthor()
        {
            //MessageBox.Show("未授权有道云笔记或者授权已过期，请点击用户--登录以重新授权！");
            if (ConfigurationManager.AppSettings["authorAlert"] != "0")
            {
                if (MessageBox.Show("wecode笔记可以保存在有道云上，您可通过云笔记-登录有道云登录！\n\n点击“确定”不再提醒", "登录提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    PubFunc.SetConfiguration("authorAlert", "0");
                }
            }

        }

        //校验通过后通知本窗体加载树
        private void beginInvokeMethod()
        {
            MakeTree();
        }


        public YouDaoTree()
        {
            InitializeComponent();
            treeViewYouDao.AllowDrop = true;

        }

        //窗体加载
        private void YouDaoTree_Load(object sender, EventArgs e)
        {
            //20140719 将主窗口判断授权移至这里判断
            //另开线程，避免因网络原因造成的卡死，验证完成给主窗口发送消息，同时判断是否加载树
            beginInvokeThread = new Thread(new ThreadStart(StartMethod));
            beginInvokeThread.Start();


            MakeTree();
        }


        //初始化
        private void MakeTree()
        {
            //绑定树
            if (Attachment.IsTokeneffective == 1)
            {
                treeViewYouDao.Enabled = true;
                IniYoudaoTree();
            }
            else
            {
                treeViewYouDao.Enabled = false;
                toolStrip1.Enabled = false;
            }
        }




        #region 从xml加载生成树
        /// <summary>
        /// 从XML加载绑定树
        /// </summary>
        public void IniYoudaoTree()
        {
            this.treeViewYouDao.Enabled = true;
            try
            {
                this.Cursor = Cursors.WaitCursor;

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("TreeNodeLocal.xml");

                treeViewYouDao.Nodes.Clear();

                treeViewYouDao.ImageList = imageList1;

                XmlNode wecode = xDoc.DocumentElement;

                foreach (XmlNode cNode in wecode)
                {
                    //添加根节点
                    TreeNode tNode = new TreeNode();
                    tNode.ImageIndex = 0;
                    tNode.SelectedImageIndex = 0;
                    tNode.Text = cNode.Attributes["title"].Value.ToString();
                    tNode.Name = cNode.Attributes["id"].Value;
                    if (cNode.Name == "note")
                    {
                        treeTagNote tNoteTag = new treeTagNote();
                        tNoteTag.path = cNode.Attributes["path"].Value;
                        tNoteTag.createtime = cNode.Attributes["createtime"].Value;
                        tNoteTag.Language = cNode.Attributes["Language"].Value;
                        tNoteTag.isMark = cNode.Attributes["isMark"].Value;

                        tNode.ImageIndex = 1;
                        tNode.SelectedImageIndex = 1;
                        if (cNode.Attributes["IsLock"].Value == "1")
                        {
                            tNode.ImageIndex = 2;
                            tNode.SelectedImageIndex = 2;
                        }

                        tNode.Tag = tNoteTag;
                    }
                    else if (cNode.Name == "book")
                    {
                        treeTagBook tBookTag = new treeTagBook();
                        tBookTag.Language = cNode.Attributes["Language"].Value;
                        tNode.Tag = tBookTag;
                    }

                    treeViewYouDao.Nodes.Add(tNode);
                    addTreeNode(cNode, tNode);
                }

                //默认选中第一个节点
                if (treeViewYouDao.Nodes.Count > 0)
                {
                    treeViewYouDao.SelectedNode = treeViewYouDao.Nodes[0];
                }

                toolStrip1.Enabled = true;

            }
            catch (XmlException xExc) //Exception is thrown is there is an error in the Xml
            {
                MessageBox.Show(xExc.Message);
            }
            catch (Exception ex) //General exception
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default; //Change the cursor back
            }
        }

        //This function is called recursively until all nodes are loaded
        private void addTreeNode(XmlNode xmlNode, TreeNode treeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList xNodeList;

            if (xmlNode.HasChildNodes) //The current node has children
            {
                xNodeList = xmlNode.ChildNodes;

                for (int x = 0; x <= xNodeList.Count - 1; x++) //Loop through the child nodes
                {
                    xNode = xmlNode.ChildNodes[x];

                    TreeNode tempNode = new TreeNode();
                    tempNode.ImageIndex = 0;
                    tempNode.SelectedImageIndex = 0;
                    tempNode.Text = xNode.Attributes["title"].Value.ToString();
                    tempNode.Name = xNode.Attributes["id"].Value;
                    if (xNode.Name == "note")
                    {
                        treeTagNote tNoteTag = new treeTagNote();
                        tNoteTag.path = xNode.Attributes["path"].Value;
                        tNoteTag.createtime = xNode.Attributes["createtime"].Value;
                        tNoteTag.Language = xNode.Attributes["Language"].Value;
                        tempNode.ImageIndex = 1;
                        tempNode.SelectedImageIndex = 1;
                        if (xNode.Attributes["IsLock"].Value == "1")
                        {
                            tempNode.ImageIndex = 2;
                            tempNode.SelectedImageIndex = 2;
                        }

                        tempNode.Tag = tNoteTag;
                    }
                    else if (xNode.Name == "book")
                    {
                        treeTagBook tBookTag = new treeTagBook();
                        tBookTag.Language = xNode.Attributes["Language"].Value;
                        tempNode.Tag = tBookTag;
                    }

                    treeNode.Nodes.Add(tempNode);
                    tNode = treeNode.Nodes[x];
                    addTreeNode(xNode, tNode);
                }
            }
        }

        #endregion


        //双击打开文章
        private void treeViewYouDao_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeViewYouDao.SelectedNode == null)
                return;

            int iType = treeViewYouDao.SelectedNode.ImageIndex;
            if (iType == 0)
            {
                //双击目录
            }
            else
            {
                //双击文章，如果已经打开，则定位，否则新窗口打开
                string sNodeId = ((treeTagNote)treeViewYouDao.SelectedNode.Tag).path;
                string sLang = ((treeTagNote)treeViewYouDao.SelectedNode.Tag).Language;
                //updateTime = "最后更新时间：" + PubFunc.seconds2Time(Convert.ToInt32(updateTime)).ToString();
                string treeLocation = treeViewYouDao.SelectedNode.FullPath;

                if (iType == 2)
                {
                    //加密,对content解密
                    string MykeydYd = "";
                    if (Attachment.KeyDYouDao != "")
                    {
                        //内存中已存在秘钥
                        MykeydYd = Attachment.KeyDYouDao;
                    }
                    else
                    {
                        //内存中不存在秘钥
                        DialogPSWYouDao dp = new DialogPSWYouDao("3");
                        DialogResult dr = dp.ShowDialog();
                        if (dr == DialogResult.OK)
                        {
                            MykeydYd = dp.ReturnVal;
                        }
                    }

                    if (MykeydYd == "")
                        return;

                }

                formParent.openNewYouDao(sNodeId, treeViewYouDao.SelectedNode.Text,treeLocation,iType);

                ///打开后设置语言
                string Language = PubFunc.Synid2LanguageSetLang(PubFunc.Language2Synid(sLang));
                if (Attachment.isnewOpenDoc == "1")
                {
                    formParent.SetLanguage(Language);
                }

            }
        }

        //新建目录
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //获取选中节点
            TreeNode SeleNode = treeViewYouDao.SelectedNode;
            string isCreateRoot = "False";
            string ParLang = "TEXT";

            //如果没有选中节点,则新建顶层目录
            if (SeleNode == null || treeViewYouDao.Nodes.Count == 0)
            {
                //没有节点
                isCreateRoot = "True";
            }
            else
            {
                if (SeleNode.ImageIndex == 0)
                {
                    ParLang = ((treeTagBook)SeleNode.Tag).Language;
                }
                else if (SeleNode.ImageIndex == 1 || SeleNode.ImageIndex == 2)
                {
                    ParLang = ((treeTagNote)SeleNode.Tag).Language;
                }
            }

            ProperDialog propDia = new ProperDialog("0", "", ParLang);
            DialogResult dr = propDia.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string Title = propDia.ReturnVal[0];
                string Language = propDia.ReturnVal[1];
                string IsOnRoot = propDia.ReturnVal[2];
                string sGUID = System.Guid.NewGuid().ToString();

                //新建根级目录
                if (IsOnRoot == "True" || isCreateRoot == "True")
                {
                    //插入树节点
                    TreeNode InsertNodeDir = new TreeNode(Title);
                    InsertNodeDir.Name = sGUID;
                    InsertNodeDir.ImageIndex = 0;
                    InsertNodeDir.SelectedImageIndex = 0;

                    treeTagBook tb = new treeTagBook();
                    tb.Language = Language;
                    InsertNodeDir.Tag = tb;

                    treeViewYouDao.Nodes.Insert(treeViewYouDao.Nodes.Count, InsertNodeDir);
                    treeViewYouDao.SelectedNode = InsertNodeDir;

                    //更新本地XML文档
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load("TreeNodeLocal.xml");
                    XmlNode xseleNode = xDoc.DocumentElement;

                    XmlElement appEle = xDoc.CreateElement("book");
                    appEle.SetAttribute("id", sGUID);
                    appEle.SetAttribute("title", Title);
                    appEle.SetAttribute("Language", Language);

                    xseleNode.AppendChild(appEle);
                    xDoc.Save("TreeNodeLocal.xml");

                    //同步到云端
                    XMLAPI.XML2Yun();


                    ////------同步到本地缓存数据库
                    OleDbConnection ExportConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+PubFunc.GetYoudaoDBPath());
                    string NewPid = "0";
                    string NewNodeId = "1";
                    string NewTurn = "1";
                    string SynId = PubFunc.Language2Synid(Language);

                    if (IsOnRoot == "True")
                    {
                        NewNodeId = AccessAdo.ExecuteScalar(ExportConn, "select max(NodeId) from ttree").ToString();
                        NewNodeId = NewNodeId == "" ? "1" : (Convert.ToInt32(NewNodeId) + 1).ToString();
                        NewTurn = AccessAdo.ExecuteScalar(ExportConn, "select max(Turn) from ttree where parentId=" + NewPid).ToString();
                        NewTurn = NewTurn == "" ? "1" : (Convert.ToInt32(NewTurn) + 1).ToString();
                    }


                    //插入数据库记录
                    DateTime d1 = DateTime.Parse("1970-01-01 08:00:00");
                    DateTime d2 = DateTime.Now;
                    TimeSpan dt = d2 - d1;
                    //相差秒数
                    string Seconds = dt.TotalSeconds.ToString();
                    //插入TTREE
                    string sql = string.Format("insert into ttree(NodeID,Title,ParentId,Type,CreateTime,SynId,Turn,MarkTime,IsLock,Gid) values({0},'{1}',{2},{3},{4},{5},{6},{7},{8},'{9}')", NewNodeId, Title, NewPid, 0, Seconds, SynId, NewTurn,0,0,sGUID);
                    AccessAdo.ExecuteNonQuery(ExportConn, sql);

                }

                else if (SeleNode != null)
                {
                    if ((SeleNode.ImageIndex == 1||SeleNode.ImageIndex == 2) && IsOnRoot == "False")
                    {
                        MessageBox.Show("不能在文章下新增节点！");
                        return;
                    }


                    //插入树节点
                    TreeNode InsertNodeDir = new TreeNode(Title);
                    InsertNodeDir.Name = sGUID;
                    InsertNodeDir.ImageIndex = 0;
                    InsertNodeDir.SelectedImageIndex = 0;

                    treeTagBook tb = new treeTagBook();
                    tb.Language = Language;
                    InsertNodeDir.Tag = tb;

                    SeleNode.Nodes.Insert(SeleNode.Nodes.Count, InsertNodeDir);
                    treeViewYouDao.SelectedNode = InsertNodeDir;


                    //更新本地XML文档
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load("TreeNodeLocal.xml");
                    XmlNode xseleNode = xDoc.SelectSingleNode("//book[@id='" + SeleNode.Name + "']");

                    XmlElement appEle = xDoc.CreateElement("book");
                    appEle.SetAttribute("id", sGUID);
                    appEle.SetAttribute("title", Title);
                    appEle.SetAttribute("Language", Language);

                    xseleNode.AppendChild(appEle);
                    xDoc.Save("TreeNodeLocal.xml");

                    //同步到云端
                    XMLAPI.XML2Yun();


                    ////------同步到本地缓存数据库
                    OleDbConnection ExportConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+PubFunc.GetYoudaoDBPath());

                    string NewPid = AccessAdo.ExecuteScalar(ExportConn, "select nodeid from ttree where gid='" + SeleNode.Name + "'").ToString();
                    string NewNodeId = AccessAdo.ExecuteScalar(ExportConn,"select max(NodeId) from ttree").ToString();
                    NewNodeId = NewNodeId == "" ? "1" : (Convert.ToInt32(NewNodeId) + 1).ToString();
                    string NewTurn = AccessAdo.ExecuteScalar(ExportConn,"select max(Turn) from ttree where parentId=" + NewPid).ToString();
                    NewTurn = NewTurn == "" ? "1" : (Convert.ToInt32(NewTurn) + 1).ToString();
                    string SynId = PubFunc.Language2Synid(Language);

                    //顶层
                    if (IsOnRoot == "True")
                    {
                        NewPid = "0";
                        NewTurn = AccessAdo.ExecuteScalar(ExportConn,"select max(Turn) from ttree where parentId=0").ToString();
                        NewTurn = NewTurn == "" ? "1" : (Convert.ToInt32(NewTurn) + 1).ToString();
                    }

                    //插入数据库记录
                    DateTime d1 = DateTime.Parse("1970-01-01 08:00:00");
                    DateTime d2 = DateTime.Now;
                    TimeSpan dt = d2 - d1;
                    //相差秒数
                    string Seconds = dt.TotalSeconds.ToString();
                    //插入TTREE
                    string sql = string.Format("insert into ttree(NodeID,Title,ParentId,Type,CreateTime,SynId,Turn,MarkTime,IsLock,Gid) values({0},'{1}',{2},{3},{4},{5},{6},{7},{8},'{9}')", NewNodeId, Title, NewPid, 0, Seconds, SynId, NewTurn, 0, 0, sGUID);
                    AccessAdo.ExecuteNonQuery(ExportConn,sql);

                }
            }
        }

        //右键菜单
        private void treeViewYouDao_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)//判断你点的是不是右键
            {
                Point ClickPoint = new Point(e.X, e.Y);
                TreeNode CurrentNode = treeViewYouDao.GetNodeAt(ClickPoint);
                if (CurrentNode != null)//判断你点的是不是一个节点
                {
                    switch (CurrentNode.SelectedImageIndex.ToString())//根据不同节点显示不同的右键菜单
                    {
                        case "0"://目录
                            CurrentNode.ContextMenuStrip = contextMenuStripYDdir;
                            break;
                        case "1":
                            CurrentNode.ContextMenuStrip = contextMenuStripYDtxt;
                            toolStripMenuItemEncrypt.Visible = true;
                            toolStripMenuItemDecrypt.Visible = false;
                            break;
                        case "2":
                            CurrentNode.ContextMenuStrip = contextMenuStripYDtxt;
                            toolStripMenuItemEncrypt.Visible = false;
                            toolStripMenuItemDecrypt.Visible = true;
                            break;
                        default:
                            break;
                    }
                    treeViewYouDao.SelectedNode = CurrentNode;//选中这个节点
                }

                else
                {
                    //右击空白区域
                    treeViewYouDao.ContextMenuStrip = contextMenuStripYDblank;
                }
            }
        }


        //新建文章
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //获取选中节点
            TreeNode SeleNode = treeViewYouDao.SelectedNode;
            string isHaveNodes = "True";
            string ParLang = "TEXT";
            //如果没有选中节点,则新建顶层目录
            if (SeleNode == null || treeViewYouDao.Nodes.Count == 0)
            {
                isHaveNodes = "False";
            }
            else
            {
                if (SeleNode.ImageIndex == 0)
                {
                    ParLang = ((treeTagBook)SeleNode.Tag).Language;
                }
                else if (SeleNode.ImageIndex == 1 || SeleNode.ImageIndex == 2)
                {
                    ParLang = ((treeTagNote)SeleNode.Tag).Language;
                }
            }

            ProperDialog propDia = new ProperDialog("1", "", ParLang);
            DialogResult dr = propDia.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string Title = propDia.ReturnVal[0];
                string Language = propDia.ReturnVal[1];
                string IsOnRoot = propDia.ReturnVal[2];
                string SynId = PubFunc.Language2Synid(Language);

                string sGUID = System.Guid.NewGuid().ToString();

                //云端创建笔记，返回路径
                YouDaoNode2 node = NoteAPI.CreateNote(Title);
                string Path = node.GetPath();

                //无节点，直接顶层创建
                if (isHaveNodes == "False")
                {
                    //插入树节点
                    TreeNode InsertNodeNote = new TreeNode(Title);
                    InsertNodeNote.Name = sGUID;
                    InsertNodeNote.ImageIndex = 1;
                    InsertNodeNote.SelectedImageIndex = 1;

                    treeTagNote tag = new treeTagNote();
                    tag.path = Path;
                    tag.createtime = PubFunc.time2TotalSeconds().ToString();
                    tag.Language = Language;
                    tag.isMark = "0";

                    InsertNodeNote.Tag = tag;
                    treeViewYouDao.Nodes.Insert(treeViewYouDao.Nodes.Count, InsertNodeNote);
                    treeViewYouDao.SelectedNode = InsertNodeNote;

                    //更新本地XML文档
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load("TreeNodeLocal.xml");
                    XmlNode xseleNode = xDoc.DocumentElement;

                    XmlElement appEle = xDoc.CreateElement("note");
                    appEle.SetAttribute("id", sGUID);
                    appEle.SetAttribute("title", Title);
                    appEle.SetAttribute("path", Path);
                    appEle.SetAttribute("createtime", tag.createtime);
                    appEle.SetAttribute("Language", Language);
                    appEle.SetAttribute("isMark", "0");
                    appEle.SetAttribute("IsLock", "0");

                    xseleNode.AppendChild(appEle);
                    xDoc.Save("TreeNodeLocal.xml");

                    //同步到云端
                    XMLAPI.XML2Yun();


                    //---------同步到缓存数据库
                    OleDbConnection ExportConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+PubFunc.GetYoudaoDBPath());
                    string NewPid = "0";
                    string NewNodeId = "1";
                    string NewTurn = "1";

                    //插入数据库记录
                    DateTime d1 = DateTime.Parse("1970-01-01 08:00:00");
                    DateTime d2 = DateTime.Now;
                    TimeSpan dt = d2 - d1;
                    //相差秒数
                    string Seconds = dt.TotalSeconds.ToString();
                    //插入TTREE
                    string sql = string.Format("insert into ttree(NodeID,Title,Path,ParentId,Type,CreateTime,SynId,Turn,MarkTime,IsLock,Gid) values({0},'{1}','{2}',{3},{4},{5},{6},{7},{8},{9},'{10}')", NewNodeId, Title,Path, NewPid, 1, Seconds, SynId, NewTurn,0,0,sGUID);
                    AccessAdo.ExecuteNonQuery(ExportConn,sql);
                    //插入TTcontent
                    sql = string.Format("insert into tcontent(NodeId,updatetime,Gid,Path,NeedSync) values({0},{1},'{2}','{3}',{4})", NewNodeId, Seconds,sGUID,Path,0);
                    AccessAdo.ExecuteNonQuery(ExportConn,sql);


                    //新窗口打开编辑界面
                    string lastTime="最后更新时间："+DateTime.Now.ToString();
                    formParent.openNewYouDao(Path, Title, treeViewYouDao.SelectedNode.FullPath,1);

                    //打开后设置语言
                    Language = PubFunc.Synid2LanguageSetLang(SynId);
                    formParent.SetLanguage(Language);
                }

                else if (SeleNode != null)
                {
                    if (Path != "")
                    {
                        //顶层创建文章
                        if (IsOnRoot == "True")
                        {
                            //插入树节点
                            TreeNode InsertNodeNote = new TreeNode(Title);
                            InsertNodeNote.Name = sGUID;
                            InsertNodeNote.ImageIndex = 1;
                            InsertNodeNote.SelectedImageIndex = 1;

                            treeTagNote tag = new treeTagNote();
                            tag.path = Path;
                            tag.createtime = PubFunc.time2TotalSeconds().ToString();
                            tag.Language = Language;
                            tag.isMark = "0";

                            InsertNodeNote.Tag = tag;
                            treeViewYouDao.Nodes.Insert(treeViewYouDao.Nodes.Count, InsertNodeNote);
                            treeViewYouDao.SelectedNode = InsertNodeNote;

                            //更新本地XML文档
                            XmlDocument xDoc = new XmlDocument();
                            xDoc.Load("TreeNodeLocal.xml");
                            XmlNode xseleNode = xDoc.DocumentElement;

                            XmlElement appEle = xDoc.CreateElement("note");
                            appEle.SetAttribute("id", sGUID);
                            appEle.SetAttribute("title", Title);
                            appEle.SetAttribute("path", Path);
                            appEle.SetAttribute("createtime", tag.createtime);
                            appEle.SetAttribute("Language", Language);
                            appEle.SetAttribute("isMark", "0");
                            appEle.SetAttribute("IsLock", "0");

                            xseleNode.AppendChild(appEle);
                            xDoc.Save("TreeNodeLocal.xml");

                            //同步到云端
                            XMLAPI.XML2Yun();

                            ////-------------同步到缓存数据库
                            OleDbConnection ExportConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+PubFunc.GetYoudaoDBPath());
                            string NewPid = AccessAdo.ExecuteScalar(ExportConn, "select nodeid from ttree where gid='" + SeleNode.Name + "'").ToString();
                            string NewNodeId = AccessAdo.ExecuteScalar(ExportConn,"select max(NodeId) from ttree").ToString();
                            NewNodeId = NewNodeId == "" ? "1" : (Convert.ToInt32(NewNodeId) + 1).ToString();
                            string NewTurn = AccessAdo.ExecuteScalar(ExportConn,"select max(Turn) from ttree where parentId=" + NewPid).ToString();
                            NewTurn = NewTurn == "" ? "1" : (Convert.ToInt32(NewTurn) + 1).ToString();

                            //顶层
                            if (IsOnRoot == "True")
                            {
                                NewPid = "0";
                                NewTurn = AccessAdo.ExecuteScalar(ExportConn,"select max(Turn) from ttree where parentId=0").ToString();
                                NewTurn = NewTurn == "" ? "1" : (Convert.ToInt32(NewTurn) + 1).ToString();
                            }

                            //插入数据库记录
                            DateTime d1 = DateTime.Parse("1970-01-01 08:00:00");
                            DateTime d2 = DateTime.Now;
                            TimeSpan dt = d2 - d1;
                            //相差秒数
                            string Seconds = dt.TotalSeconds.ToString();
                            //插入TTREE
                            string sql = string.Format("insert into ttree(NodeID,Title,path,ParentId,Type,CreateTime,SynId,Turn,MarkTime,IsLock,Gid) values({0},'{1}','{2}',{3},{4},{5},{6},{7},{8},{9},'{10}')", NewNodeId, Title,Path, NewPid, 1, Seconds, SynId, NewTurn,0,0,sGUID);
                            AccessAdo.ExecuteNonQuery(ExportConn,sql);
                            //插入TTcontent
                            sql = string.Format("insert into tcontent(NodeId,updatetime,Gid,Path,NeedSync) values({0},{1},'{2}','{3}',{4})", NewNodeId, Seconds, sGUID, Path, 0);
                            AccessAdo.ExecuteNonQuery(ExportConn,sql);



                            //新窗口打开编辑界面
                            string lastTime = "最后更新时间：" + DateTime.Now.ToString();
                            formParent.openNewYouDao(Path, Title, treeViewYouDao.SelectedNode.FullPath,1);

                            //打开后设置语言
                            Language = PubFunc.Synid2LanguageSetLang(SynId);
                            formParent.SetLanguage(Language);
                        }

                        else
                        {
                            if ((SeleNode.ImageIndex == 1||SeleNode.ImageIndex == 2) && IsOnRoot == "False")
                            {
                                MessageBox.Show("不能在文章下新增节点！");
                                return;
                            }
                            //插入树节点
                            TreeNode InsertNodeNote = new TreeNode(Title);

                            treeTagNote tag = new treeTagNote();
                            tag.path = Path;
                            tag.createtime = PubFunc.time2TotalSeconds().ToString();
                            tag.Language = Language;
                            tag.isMark = "0";

                            InsertNodeNote.Name = sGUID;
                            InsertNodeNote.ImageIndex = 1;
                            InsertNodeNote.SelectedImageIndex = 1;
                            InsertNodeNote.Tag = tag;

                            SeleNode.Nodes.Insert(SeleNode.Nodes.Count, InsertNodeNote);
                            treeViewYouDao.SelectedNode = InsertNodeNote;


                            //更新本地XML文档
                            XmlDocument xDoc = new XmlDocument();
                            xDoc.Load("TreeNodeLocal.xml");
                            XmlNode xseleNode = xDoc.SelectSingleNode("//book[@id='" + SeleNode.Name + "']");

                            XmlElement appEle = xDoc.CreateElement("note");
                            appEle.SetAttribute("id", sGUID);
                            appEle.SetAttribute("title", Title);
                            appEle.SetAttribute("path", Path);
                            appEle.SetAttribute("createtime", tag.createtime);
                            appEle.SetAttribute("Language", Language);
                            appEle.SetAttribute("isMark", "0");
                            appEle.SetAttribute("IsLock", "0");

                            xseleNode.AppendChild(appEle);
                            xDoc.Save("TreeNodeLocal.xml");

                            //同步到云端
                            XMLAPI.XML2Yun();


                            ////-------------同步到缓存数据库
                            OleDbConnection ExportConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+PubFunc.GetYoudaoDBPath());
                            string NewPid = AccessAdo.ExecuteScalar(ExportConn, "select nodeid from ttree where gid='" + SeleNode.Name + "'").ToString();
                            string NewNodeId = AccessAdo.ExecuteScalar(ExportConn, "select max(NodeId) from ttree").ToString();
                            NewNodeId = NewNodeId == "" ? "1" : (Convert.ToInt32(NewNodeId) + 1).ToString();
                            string NewTurn = AccessAdo.ExecuteScalar(ExportConn, "select max(Turn) from ttree where parentId=" + NewPid).ToString();
                            NewTurn = NewTurn == "" ? "1" : (Convert.ToInt32(NewTurn) + 1).ToString();

                            //顶层
                            if (IsOnRoot == "True")
                            {
                                NewPid = "0";
                                NewTurn = AccessAdo.ExecuteScalar(ExportConn, "select max(Turn) from ttree where parentId=0").ToString();
                                NewTurn = NewTurn == "" ? "1" : (Convert.ToInt32(NewTurn) + 1).ToString();
                            }

                            //插入数据库记录
                            DateTime d1 = DateTime.Parse("1970-01-01 08:00:00");
                            DateTime d2 = DateTime.Now;
                            TimeSpan dt = d2 - d1;
                            //相差秒数
                            string Seconds = dt.TotalSeconds.ToString();
                            //插入TTREE
                            string sql = string.Format("insert into ttree(NodeID,Title,Path,ParentId,Type,CreateTime,SynId,Turn,MarkTime,IsLock,Gid) values({0},'{1}','{2}',{3},{4},{5},{6},{7},{8},{9},'{10}')", NewNodeId, Title,Path, NewPid, 1, Seconds, SynId, NewTurn, 0, 0, sGUID);
                            AccessAdo.ExecuteNonQuery(ExportConn, sql);
                            //插入TTcontent
                            sql = string.Format("insert into tcontent(NodeId,updatetime,Gid,Path,NeedSync) values({0},{1},'{2}','{3}',{4})", NewNodeId, Seconds, sGUID, Path, 0);
                            AccessAdo.ExecuteNonQuery(ExportConn, sql);



                            //新窗口打开编辑界面
                            string lastTime = "最后更新时间：" + DateTime.Now.ToString();
                            formParent.openNewYouDao(Path, Title, treeViewYouDao.SelectedNode.FullPath,1);

                            //打开后设置语言
                            Language = PubFunc.Synid2LanguageSetLang(SynId);
                            formParent.SetLanguage(Language);

                        }
                    }

                }
            }
        }

        //删除目录或者文章
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

            //避免保存的提示
            Attachment.isDeleteClose = "1";
            //获取选中节点
            TreeNode SeleNode = treeViewYouDao.SelectedNode;
            if (SeleNode == null)
                return;

            //删除前确认
            if (MessageBox.Show("当前节点及其所有子节点都会被删除，继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            //先查找到所选节点下面所有的文章进行删除
            //删除云数据
            DelNodeData(SeleNode.Name);

            //更新缓存数据
            OleDbConnection ExportConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+PubFunc.GetYoudaoDBPath());
            DelLocalData(AccessAdo.ExecuteScalar(ExportConn,"select nodeid from ttree where gid='"+SeleNode.Name+"'").ToString());

            //移除树节点
            treeViewYouDao.Nodes.Remove(SeleNode);

            Attachment.isDeleteClose = "0";

            formParent.ReSetMarkFind();

        }

        //删除有道云数据，同时关闭已打开的文章,再更新本地XML并同步到云
        public void DelNodeData(string id)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load("TreeNodeLocal.xml");
            XmlNode seleNode = doc.SelectSingleNode("//node()[@id='" + id + "']");

            if (seleNode.Name == "note")
            {
                //选中的是文章
                string path = seleNode.Attributes["path"].Value;

                //关闭打开的文章
                formParent.CloseDoc(path);

                NoteAPI.DeleteNote(path);
            }
            else
            {
                XmlNodeList xlist = seleNode.SelectNodes("//node()[@id='" + id + "']//note");

                foreach (XmlNode xnode in xlist)
                {
                    string path = xnode.Attributes["path"].Value;
                    //删除笔记

                    //关闭打开的文章
                    formParent.CloseDoc(path);

                    NoteAPI.DeleteNote(path);
                }
            }

            //移除XML节点，更新XML到云

            seleNode.ParentNode.RemoveChild(seleNode);
            doc.Save("TreeNodeLocal.xml");
            XMLAPI.XML2Yun();

        }
        

        //删除本地缓存数据
        public void DelLocalData(string NodeId)
        {
            OleDbConnection ExportConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+PubFunc.GetYoudaoDBPath());
            string SQL = string.Format("select NodeId from Ttree where parentId={0}", NodeId);
            DataTable temp = AccessAdo.ExecuteDataSet(ExportConn,SQL).Tables[0];
            DataView dv = new DataView(temp);
            foreach (DataRowView drv in dv)
            {
                DelLocalData(drv["NodeId"].ToString());
            }

            ExportConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+PubFunc.GetYoudaoDBPath());
            string DelSQL = string.Format("Delete from TAttachment where NodeId={0}", NodeId);
            AccessAdo.ExecuteNonQuery(ExportConn,DelSQL);
            DelSQL = string.Format("Delete from Tcontent where NodeId={0}", NodeId);
            AccessAdo.ExecuteNonQuery(ExportConn,DelSQL);
            DelSQL = string.Format("Delete from Ttree where NodeId={0}", NodeId);
            AccessAdo.ExecuteNonQuery(ExportConn,DelSQL);


        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            toolStripMenuItem3_Click(sender, e);
        }


        #region 上下移动操作
        //上移
        public void setNodeUp()
        {
            //树操作
            SetTreeNodeUp(this.treeViewYouDao.SelectedNode);

        }

        private void SetTreeNodeUp(System.Windows.Forms.TreeNode node)
        {
            if ((node == null) || (node.PrevNode) == null) return;
            System.Windows.Forms.TreeNode newNode = (System.Windows.Forms.TreeNode)node.Clone();

            //要交换次序的节点
            string NodeId1 = node.Name.ToString();
            string NodeId2 = node.PrevNode.Name.ToString();

            if (node.Parent != null)
                node.Parent.Nodes.Insert(node.PrevNode.Index, newNode);
            else
                node.TreeView.Nodes.Insert(node.PrevNode.Index, newNode);

            this.treeViewYouDao.Nodes.Remove(node);
            this.treeViewYouDao.SelectedNode = newNode;

            treeViewYouDao.Focus();

            //xml移动
            xmlNodeMove(NodeId2, NodeId1);

            //xml同步
            XMLAPI.XML2Yun();
        }


        //下移
        public void setNodeDown()
        {
            //树操作
            SetTreeNodeDown(this.treeViewYouDao.SelectedNode);

        }

        private void SetTreeNodeDown(System.Windows.Forms.TreeNode node)
        {
            if ((node == null) || (node.NextNode) == null) return;
            System.Windows.Forms.TreeNode newNode = (System.Windows.Forms.TreeNode)node.Clone();

            //要交换次序的节点
            string NodeId1 = node.Name.ToString();
            string NodeId2 = node.NextNode.Name.ToString();

            if (node.Parent != null)
                node.Parent.Nodes.Insert(node.NextNode.Index + 1, newNode);
            else
                node.TreeView.Nodes.Insert(node.NextNode.Index + 1, newNode);

            this.treeViewYouDao.Nodes.Remove(node);
            this.treeViewYouDao.SelectedNode = newNode;

            treeViewYouDao.Focus();

            //xml移动
            xmlNodeMove(NodeId1, NodeId2);

            //xml同步
            XMLAPI.XML2Yun();
        }


        /// <summary>
        /// 交换xml节点的顺序
        /// </summary>
        /// <param name="id1">前一个节点id</param>
        /// <param name="id2">后一个节点id</param>
        private void xmlNodeMove(string id1, string id2)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("TreeNodeLocal.xml");
            XmlNode preNode = doc.SelectSingleNode("//node()[@id='" + id1 + "']");
            XmlNode parentNode = preNode.ParentNode;
            XmlNode nexNode = doc.SelectSingleNode("//node()[@id='" + id2 + "']");

            parentNode.InsertAfter(preNode, nexNode);
            doc.Save("TreeNodeLocal.xml");
        }

        #endregion


        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1_Click(sender, e);
        }

        //重命名以及语言
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            //获取选中节点
            TreeNode SeleNode = this.treeViewYouDao.SelectedNode;
            if (SeleNode == null)
                return;
            string ParLang, Type, DiaType;
            if (SeleNode.ImageIndex == 0)
            {
                //目录
                ParLang = ((treeTagBook)SeleNode.Tag).Language;
                DiaType = "2";
            }
            else
            {
                ParLang = ((treeTagNote)SeleNode.Tag).Language;
                DiaType = "3";
            }

            ProperDialog propDia = new ProperDialog(DiaType, SeleNode.Text, ParLang);
            DialogResult dr = propDia.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string Title = propDia.ReturnVal[0];
                string Language = propDia.ReturnVal[1];
                string SynId = PubFunc.Language2Synid(Language);

                if (SeleNode != null)
                {
                    //更新标题和语言
                    XmlDocument doc = new XmlDocument();
                    doc.Load("TreeNodeLocal.xml");
                    XmlNode preNode = doc.SelectSingleNode("//node()[@id='" + SeleNode.Name + "']");
                    preNode.Attributes["title"].Value = Title;
                    preNode.Attributes["Language"].Value = Language;
                    doc.Save("TreeNodeLocal.xml");
                    XMLAPI.XML2Yun();

                    //更新缓存数据
                    OleDbConnection ExportConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+PubFunc.GetYoudaoDBPath());
                    string SQL = "update ttree set title='" + Title + "',SynId=" + SynId + " where gid='" + SeleNode.Name + "'";
                    AccessAdo.ExecuteNonQuery(ExportConn, SQL);

                    //更新节点信息
                    SeleNode.Text = Title;
                    if (SeleNode.ImageIndex == 0)
                    {
                        treeTagBook tb = new treeTagBook();
                        tb.Language = Language;
                        SeleNode.Tag = tb;
                    }
                    else
                    {
                        treeTagNote tag = new treeTagNote();
                        tag.path = ((treeTagNote)SeleNode.Tag).path;
                        tag.createtime = ((treeTagNote)SeleNode.Tag).createtime;
                        tag.Language = Language;
                        tag.isMark = "0";
                        SeleNode.Tag = tag;
                    }
                    //打开后设置语言
                    Language = PubFunc.Synid2LanguageSetLang(SynId);
                    if (SeleNode.ImageIndex == 1 || SeleNode.ImageIndex == 2)
                    {
                        formParent.SetLanguageByDoc(Language, ((treeTagNote)SeleNode.Tag).path,Title,SeleNode.FullPath);
                    }

                }
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            toolStripMenuItem4_Click(sender, e);
        }

        //加为书签
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            //获取选中节点
            TreeNode SeleNode = treeViewYouDao.SelectedNode;
            if (SeleNode == null)
                return;
            XmlDocument doc = new XmlDocument();
            doc.Load("TreeNodeLocal.xml");
            XmlNode preNode = doc.SelectSingleNode("//node()[@id='" + SeleNode.Name + "']");
            preNode.Attributes["isMark"].Value = "1";
            doc.Save("TreeNodeLocal.xml");
            XMLAPI.XML2Yun();

            //更新缓存数据
            OleDbConnection ExportConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+PubFunc.GetYoudaoDBPath());
            string SQL = "update ttree set marktime=1 where gid='" + SeleNode.Name + "'";
            AccessAdo.ExecuteNonQuery(ExportConn, SQL);

        }

        private void YouDaoTree_Activated(object sender, EventArgs e)
        {

        }

        private void YouDaoTree_Deactivate(object sender, EventArgs e)
        {

        }

        //供主窗口调用
        public void NewDoc()
        {
            toolStripMenuItem2_Click(null, null);
        }

        //供主窗口调用
        public void NewDir()
        {
            toolStripMenuItem1_Click(null, null);
        }

        //供主窗口调用
        public void DelNode()
        {
            toolStripMenuItem3_Click(null, null);
        }

        //供主窗口调用
        public void ShowProp()
        {
            toolStripMenuItem4_Click(null, null);
        }

        private void treeViewYouDao_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        #region drag
        private void treeViewYouDao_DragDrop(object sender, DragEventArgs e)
        {
            if (this.dragDropTreeNode != null)
            {
                if (e.Data.GetDataPresent(typeof(TreeNode)))
                {

                    TreeNode tn = (TreeNode)e.Data.GetData(typeof(TreeNode));

                    if (tn.Text == this.dragDropTreeNode.Text || this.dragDropTreeNode.FullPath.IndexOf(tn.FullPath) == 0)
                    {
                        this.dragDropTreeNode.BackColor = SystemColors.Window;
                        this.dragDropTreeNode.ForeColor = SystemColors.WindowText;
                        this.dragDropTreeNode = null;
                        return;
                    }
                    tn.Remove();//从原父节点移除被拖得节点 
                    this.dragDropTreeNode.Nodes.Add(tn);//添加被拖得节点到新节点下面 

                    //更新XML
                    string id1 = tn.Name.ToString();  //拖动的节点
                    string id2 = this.dragDropTreeNode.Name.ToString();

                    XmlDocument doc = new XmlDocument();
                    doc.Load("TreeNodeLocal.xml");
                    XmlNode Node1 = doc.SelectSingleNode("//node()[@id='" + id1 + "']");
                    XmlNode Node2 = doc.SelectSingleNode("//node()[@id='" + id2 + "']");

                    Node2.AppendChild(Node1);
                    doc.Save("TreeNodeLocal.xml");
                    XMLAPI.XML2Yun();

                    if (this.dragDropTreeNode.IsExpanded == false)
                    {
                        this.dragDropTreeNode.Expand();//展开节点 
                    }

                }
                else if (e.Data.GetDataPresent(typeof(ListViewItem)))
                {
                    if (this.dragDropTreeNode.Parent != null)
                    {
                        //int categoryID = ((Category)this.dragDropTreeNode.Tag).CategoryID;
                        ListViewItem listViewItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                        //Item item = (Item)listViewItem.Tag;
                        //DocumentController.GetInstance().UpdateItemCategory(item.ItemID, categoryID);
                        listViewItem.Remove();
                    }
                }
                //取消被放置的节点高亮显示 
                this.dragDropTreeNode.BackColor = SystemColors.Window;
                this.dragDropTreeNode.ForeColor = SystemColors.WindowText;
                this.dragDropTreeNode = null;
            }
        }

        private void treeViewYouDao_DragLeave(object sender, EventArgs e)
        {
            if (this.dragDropTreeNode != null) //在按下{ESC}，取消被放置的节点高亮显示 
            {
                this.dragDropTreeNode.BackColor = SystemColors.Window;
                this.dragDropTreeNode.ForeColor = SystemColors.WindowText;
                this.dragDropTreeNode = null;
            }
        }

        private void treeViewYouDao_DragOver(object sender, DragEventArgs e)
        {
            //当光标悬停在 TreeView 控件上时，展开该控件中的 TreeNode 
            Point p = this.treeViewYouDao.PointToClient(Control.MousePosition);
            TreeNode tn = this.treeViewYouDao.GetNodeAt(p);
            if (tn != null)
            {
                if (this.dragDropTreeNode != tn) //移动到新的节点 
                {
                    if (tn.Nodes.Count > 0 && tn.IsExpanded == false)
                    {
                        this.startTime = DateTime.Now;//设置新的起始时间 
                    }
                }
                else
                {
                    if (tn.Nodes.Count > 0 && tn.IsExpanded == false && this.startTime != DateTime.MinValue)
                    {
                        TimeSpan ts = DateTime.Now - this.startTime;
                        if (ts.TotalMilliseconds >= 1000) //一秒 
                        {
                            tn.Expand();
                            this.startTime = DateTime.MinValue;
                        }
                    }
                }

            }
            //设置拖放标签Effect状态 
            if (tn != null)//&& (tn != this.treeView.SelectedNode)) //当控件移动到空白处时，设置不可用。 
            {
                if ((e.AllowedEffect & DragDropEffects.Move) != 0)
                {
                    e.Effect = DragDropEffects.Move;
                }
                if (((e.AllowedEffect & DragDropEffects.Copy) != 0) && ((e.KeyState & 0x08) != 0))//Ctrl key 
                {
                    e.Effect = DragDropEffects.Copy;
                }
                if (((e.AllowedEffect & DragDropEffects.Link) != 0) && ((e.KeyState & 0x08) != 0) && ((e.KeyState & 0x04) != 0))//Ctrl key + Shift key 
                {
                    e.Effect = DragDropEffects.Link;
                }
                if (e.Data.GetDataPresent(typeof(TreeNode)))//拖动的是TreeNode 
                {

                    TreeNode parND = tn;//判断是否拖到了子项 
                    bool isChildNode = false;
                    while (parND.Parent != null)
                    {
                        parND = parND.Parent;
                        if (parND == (TreeNode)e.Data.GetData(typeof(TreeNode)))
                        {
                            isChildNode = true;
                            break;
                        }
                    }
                    if (isChildNode || tn.ImageIndex == 1||tn.ImageIndex == 2)
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else if (e.Data.GetDataPresent(typeof(ListViewItem)))//拖动的是ListViewItem 
                {
                    if (tn.Parent == null)
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

            //设置拖放目标TreeNode的背景色 
            if (e.Effect == DragDropEffects.None)
            {
                if (this.dragDropTreeNode != null) //取消被放置的节点高亮显示 
                {
                    this.dragDropTreeNode.BackColor = SystemColors.Window;
                    this.dragDropTreeNode.ForeColor = SystemColors.WindowText;
                    this.dragDropTreeNode = null;
                }
            }
            else
            {
                if (tn != null)
                {
                    if (this.dragDropTreeNode != null)
                    {
                        if (this.dragDropTreeNode != tn)
                        {
                            this.dragDropTreeNode.BackColor = SystemColors.Window;//取消上一个被放置的节点高亮显示 
                            this.dragDropTreeNode.ForeColor = SystemColors.WindowText;
                            this.dragDropTreeNode = tn;//设置为新的节点 
                            this.dragDropTreeNode.BackColor = SystemColors.Highlight;
                            this.dragDropTreeNode.ForeColor = SystemColors.HighlightText;
                        }
                        //else
                        //{
                        //    return;
                        //}
                    }
                    else
                    {
                        this.dragDropTreeNode = tn;//设置为新的节点 
                        this.dragDropTreeNode.BackColor = SystemColors.Highlight;
                        this.dragDropTreeNode.ForeColor = SystemColors.HighlightText;
                    }
                }
            }

        }

        private void treeViewYouDao_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode tn = e.Item as TreeNode;
            dragDropTreeNode = tn;
            if ((e.Button == MouseButtons.Left) && (tn != null)) //根节点不允许拖放操作。 
            {
                this.treeViewYouDao.DoDragDrop(tn, DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link);
            }
        }
        #endregion

        //新建文章
        private void toolStripButtonNewYDDoc_Click(object sender, EventArgs e)
        {
            NewDoc();
        }

        private void toolStripButtonNewYDDir_Click(object sender, EventArgs e)
        {
            NewDir();
        }

        private void toolStripButtonYDDel_Click(object sender, EventArgs e)
        {
            DelNode();
        }

        private void toolStripButtonYDProp_Click(object sender, EventArgs e)
        {
            ShowProp();
        }

        private void toolStripButtonYDUp_Click(object sender, EventArgs e)
        {
            setNodeUp();
        }

        private void toolStripButtonYDDown_Click(object sender, EventArgs e)
        {
            setNodeDown();
        }

        private void toolStripButtonYDSerch_Click(object sender, EventArgs e)
        {
            formParent.OpenSerch("online");
        }

        //注销后，禁用控件，清空节点
        public void YdLogOut()
        { 
            //清空节点
            this.treeViewYouDao.Nodes.Clear();
            this.treeViewYouDao.Enabled = false;
            this.toolStrip1.Enabled = false;
        }


        //加密
        private void toolStripMenuItemEncrypt_Click(object sender, EventArgs e)
        {
            //获取选中节点
            TreeNode SeleNode = treeViewYouDao.SelectedNode;
            if (SeleNode == null)
                return;

            string sNodeId = ((treeTagNote)treeViewYouDao.SelectedNode.Tag).path;
            string MykeydYd = "";
            if (Attachment.KeyDYouDao != "")
            {
                //内存中已存在秘钥
                MykeydYd = Attachment.KeyDYouDao;
            }
            else
            {
                //内存中不存在秘钥
                //判断是否是第一次设置密码，如果是，则弹出设置密码
                XmlDocument doc = new XmlDocument();
                doc.Load("TreeNodeLocal.xml");
                XmlNode preNode = doc.SelectSingleNode("//wecode[@KeyD5]");
                string isExist = "1";
                if (preNode == null)
                {
                    isExist = "0";
                }

                string OpenType = "1";
                if (isExist=="0")
                {
                    //先设置密码
                    OpenType = "0";
                }
                else
                {
                    OpenType = "1";
                }

                DialogPSWYouDao dp = new DialogPSWYouDao(OpenType);
                DialogResult dr = dp.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    MykeydYd = dp.ReturnVal;
                }
            }

            //开始加密处理
            if (MykeydYd != "")
            {
                //获取文章信息
                youdao.YouDaoNode2 note = NoteAPI.GetNote(sNodeId);
                if (note == null)
                {
                    MessageBox.Show("操作失败！");
                    return;
                }

                string Content = note.GetContent();
                string EncrptyedContent = EncryptDecrptt.EncrptyByKey(Content, MykeydYd);
                //重新保存
                if (NoteAPI.UpdateContent(ref note, EncrptyedContent, note.GetUpdateTime()) == "OK")
                {
                    //更新配置文件
                    XmlDocument doc = new XmlDocument();
                    doc.Load("TreeNodeLocal.xml");
                    XmlNode preNode = doc.SelectSingleNode("//node()[@id='" + SeleNode.Name + "']");
                    preNode.Attributes["IsLock"].Value = "1";
                    doc.Save("TreeNodeLocal.xml");
                    XMLAPI.XML2Yun();

                    //---------更新缓存数据
                    OleDbConnection ExportConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+PubFunc.GetYoudaoDBPath());
                    OleDbParameter p1 = new OleDbParameter("@Content", OleDbType.VarChar);
                    p1.Value = EncrptyedContent;
                    OleDbParameter p2 = new OleDbParameter("@Gid", OleDbType.VarChar);
                    p2.Value = SeleNode.Name;

                    OleDbParameter[] ArrPara = new OleDbParameter[2];
                    ArrPara[0] = p1;
                    ArrPara[1] = p2;
                    string SQL = "update tcontent set content=@Content where Gid=@Gid";
                    AccessAdo.ExecuteNonQuery(ExportConn,SQL, ArrPara);
                    AccessAdo.ExecuteNonQuery(ExportConn,"update ttree set IsLock=1 where Gid='" + SeleNode.Name+"'");

                    SeleNode.ImageIndex = 2;
                    SeleNode.SelectedImageIndex = 2;

                    formParent.SetLock(sNodeId);
                }
                else
                {
                    MessageBox.Show("操作失败！");
                    return;
                }

            }
        }

        //取消加密
        private void toolStripMenuItemDecrypt_Click(object sender, EventArgs e)
        {
            //获取选中节点
            TreeNode SeleNode = treeViewYouDao.SelectedNode;
            if (SeleNode == null)
                return;

            string sNodeId = ((treeTagNote)treeViewYouDao.SelectedNode.Tag).path;
            string MykeydYd = "";
            if (Attachment.KeyDYouDao != "")
            {
                //内存中已存在秘钥
                MykeydYd = Attachment.KeyDYouDao;
            }
            else
            {
                //内存中不存在秘钥
                //判断是否是第一次设置密码，如果是，则弹出设置密码
                XmlDocument doc = new XmlDocument();
                doc.Load("TreeNodeLocal.xml");
                XmlNode preNode = doc.SelectSingleNode("//wecode[@KeyD5]");
                string isExist = "1";
                if (preNode == null)
                {
                    isExist = "0";
                }

                string OpenType = "2";
                if (isExist == "0")
                {
                    //先设置密码
                    OpenType = "0";
                }
                else
                {
                    OpenType = "2";
                }

                DialogPSWYouDao dp = new DialogPSWYouDao(OpenType);
                DialogResult dr = dp.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    MykeydYd = dp.ReturnVal;
                }
            }

            //开始解密处理
            if (MykeydYd != "")
            {
                //获取文章信息
                youdao.YouDaoNode2 note = NoteAPI.GetNote(sNodeId);
                if (note == null)
                {
                    MessageBox.Show("操作失败！");
                    return;
                }
                string DecrptyedContent = EncryptDecrptt.DecrptyByKey(note.GetContent(), MykeydYd);

                //重新保存
                if (NoteAPI.UpdateContent(ref note, DecrptyedContent, note.GetUpdateTime()) == "OK")
                {
                    //更新配置文件
                    XmlDocument doc = new XmlDocument();
                    doc.Load("TreeNodeLocal.xml");
                    XmlNode preNode = doc.SelectSingleNode("//node()[@id='" + SeleNode.Name + "']");
                    preNode.Attributes["IsLock"].Value = "0";
                    doc.Save("TreeNodeLocal.xml");
                    XMLAPI.XML2Yun();


                    //---------更新缓存数据
                    OleDbConnection ExportConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+PubFunc.GetYoudaoDBPath());
                    OleDbParameter p1 = new OleDbParameter("@Content", OleDbType.VarChar);
                    p1.Value = DecrptyedContent;
                    OleDbParameter p2 = new OleDbParameter("@Gid", OleDbType.VarChar);
                    p2.Value = SeleNode.Name;

                    OleDbParameter[] ArrPara = new OleDbParameter[2];
                    ArrPara[0] = p1;
                    ArrPara[1] = p2;
                    string SQL = "update tcontent set content=@Content where Gid=@Gid";
                    AccessAdo.ExecuteNonQuery(ExportConn, SQL, ArrPara);
                    AccessAdo.ExecuteNonQuery(ExportConn, "update ttree set IsLock=0 where Gid='" + SeleNode.Name + "'");


                    SeleNode.ImageIndex = 1;
                    SeleNode.SelectedImageIndex = 1;

                    formParent.UnsetLock(sNodeId);
                }
                else
                {
                    MessageBox.Show("操作失败！");
                    return;
                }

            }
        }
    }
}
