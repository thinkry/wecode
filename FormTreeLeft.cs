using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Data.OleDb;

namespace WeCode1._0
{
    public partial class FormTreeLeft : DockContent
    {
        public FormMain formParent;

        TreeNode dragDropTreeNode;
        DateTime startTime;

        public FormTreeLeft()
        {
            InitializeComponent();
            treeViewDir.AllowDrop = true;
        }

        public void frmTree_Reload()
        {
            treeViewDir.Nodes.Clear();
            //重新绑定
            treeViewDir.ImageList = imageList1;
            iniTreeDigui(GetTable("select * from ttree"), "NodeId", "ParentId", "Title", treeViewDir.Nodes, null);
        }
        private void FormTreeLeft_Load(object sender, EventArgs e)
        {
            //初始化绑定目录树(非递归)
            //iniTree();

            //递归方式一次性全部加载
            treeViewDir.ImageList = imageList1;
            iniTreeDigui(GetTable("select * from ttree"), "NodeId", "ParentId", "Title", treeViewDir.Nodes, null);

            //默认选中第一个节点
            if (treeViewDir.Nodes.Count > 0)
            {
                treeViewDir.SelectedNode = treeViewDir.Nodes[0];
            }
        }

        ///   <summary>   
        ///   加载树节点。建树的基本思路是：从根节点开始递归调用显示子树。   
        ///   </summary>   
        ///   <param   name="dt">是DataTable类型的保存树节点的数据表</param>   
        ///   <param   name="nodeID">数据表中保存节点的列名称</param>   
        ///   <param   name="parentID">数据表中保存节点父节点的列名称</param>   
        ///   <param   name="nodeName">数据表中保存节点名称的列名称</param>   
        ///   <param   name="treeNodeCollection">表示TreeView.Nodes对象的集合</param>   
        ///   <param   name="rootNodeTag">定义根节点的父节点的标记</param>   
        //法一：用DataView   
        public static void iniTreeDigui(DataTable dt, string id, string pid, string text, TreeNodeCollection treeNodeCollection, string rootNodeTag)
        {
            try
            {
                TreeNode tmpNode;
                DataView dv = new DataView(dt);
                //dv.Table = dt;
                if (rootNodeTag != null)
                    dv.RowFilter = pid + "=" + rootNodeTag + "";
                else
                    dv.RowFilter = pid + "=0";
                dv.Sort = "Turn ASC";
                foreach (DataRowView drv in dv)
                {
                    tmpNode = new TreeNode();
                    tmpNode.Text = drv[text].ToString();
                    tmpNode.Tag = drv[id];
                    if (drv["Type"].ToString() == "0")
                    {
                        tmpNode.ImageIndex = 0;
                        tmpNode.SelectedImageIndex = 0;
                    }
                    else
                    {
                        tmpNode.ImageIndex = 1;
                        tmpNode.SelectedImageIndex = 1;
                    }
                    string father = drv[id].ToString();
                    treeNodeCollection.Add(tmpNode);
                    iniTreeDigui(dt, id, pid, text, tmpNode.Nodes, father);
                }
            }
            catch (Exception te)
            {
                MessageBox.Show(te.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        #region 非递归方式加载树节点

        /// <summary>
        /// 初始化树，第一次加载绑定第一层子节点（无parentid）及孙节点（两层）
        /// 在展开节点之前再延迟加载节点
        /// </summary>
        private void iniTree()
        {
            string SQL = "SELECT * FROM TTree WHERE parentId=0 ORDER BY Turn ASC";
            DataTable RootTb = GetTable(SQL);
            int iLength = RootTb.Rows.Count;
            for (int i = 0; i < iLength; i++)
            {
                DirNode root = new DirNode(RootTb.Rows[i]["Title"].ToString());
                root.Name = RootTb.Rows[i]["NodeId"].ToString();
                treeViewDir.Nodes.Add(root);
                AddDirs(root);
            }

        }

        /// <summary>
        /// 加载下一级节点
        /// </summary>
        /// <param name="node">父节点</param>
        private void AddDirs(TreeNode node)
        {
            try
            {
                string strNodeId = node.Name;
                string sqlDir = string.Format("SELECT * FROM TTree WHERE parentId={0}  ORDER BY Turn ASC", strNodeId);

                //OleDbParameter[] ArrPara = new OleDbParameter[1];
                //OleDbParameter p1 = new OleDbParameter("@parentId", OleDbType.Integer);
                //p1.Value =Int32.Parse(strNodeId);
                //ArrPara[0] = p1;
                DataTable TbDir = GetTable(sqlDir);

                int DirCount = TbDir.Rows.Count;

                string name, text;

                //加载
                //for (int i = 0; i < DirCount; i++)
                //{
                //    DataRow dr = TbDir.Rows[i];
                //    name = dr["NodeId"].ToString();
                //    text = dr["Title"].ToString();
                //    DirNode tempNode = new DirNode(text);
                //    tempNode.Name = name;
                //    node.Nodes.Add(tempNode);
                //}

                foreach (DataRow dr in TbDir.Rows)
                {
                    name = dr["NodeId"].ToString();
                    text = dr["Title"].ToString();
                    DirNode tempNode = new DirNode(text);
                    tempNode.Name = name;
                    node.Nodes.Add(tempNode);
                }
            }
            catch
            {
            }
        }


        private DataTable GetTable(string SQL)
        {
            DataSet ds = AccessAdo.ExecuteDataSet(SQL);
            DataTable dt = ds.Tables[0];
            return dt;
        }

        /// <summary>
        /// 展开节点前动态加载孙节点（子节点已经加载），需要判断是否已加载
        /// 过节点，避免重复加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewDir_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            //DirNode nodeExpanding = (DirNode)e.Node;
            //if (!nodeExpanding.SubDirectoriesAdded)
            //{
            //    AddSubDirs(nodeExpanding);
            //}

        }


        private void AddSubDirs(DirNode node)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                AddDirs(node.Nodes[i]);
            }
            node.SubDirectoriesAdded = true;
        }

        #endregion


        #region 上下移动操作
        //上移
        public void setNodeUp()
        {
            //树操作
            SetTreeNodeUp(this.treeViewDir.SelectedNode);

        }

        private void SetTreeNodeUp(System.Windows.Forms.TreeNode node)
        {
            if ((node == null) || (node.PrevNode) == null) return;
            System.Windows.Forms.TreeNode newNode = (System.Windows.Forms.TreeNode)node.Clone();

            //要交换次序的节点
            string NodeId1 = node.Tag.ToString();
            string NodeId2 = node.PrevNode.Tag.ToString();

            if (node.Parent != null)
                node.Parent.Nodes.Insert(node.PrevNode.Index, newNode);
            else
                node.TreeView.Nodes.Insert(node.PrevNode.Index, newNode);

            this.treeViewDir.Nodes.Remove(node);
            this.treeViewDir.SelectedNode = newNode;

            treeViewDir.Focus();

            //数据库更新
            ChangeTrunById(NodeId1, NodeId2);
        }


        //下移
        public void setNodeDown()
        {
            //树操作
            SetTreeNodeDown(this.treeViewDir.SelectedNode);

        }

        private void SetTreeNodeDown(System.Windows.Forms.TreeNode node)
        {
            if ((node == null) || (node.NextNode) == null) return;
            System.Windows.Forms.TreeNode newNode = (System.Windows.Forms.TreeNode)node.Clone();

            //要交换次序的节点
            string NodeId1 = node.Tag.ToString();
            string NodeId2 = node.NextNode.Tag.ToString();

            if (node.Parent != null)
                node.Parent.Nodes.Insert(node.NextNode.Index + 1, newNode);
            else
                node.TreeView.Nodes.Insert(node.NextNode.Index + 1, newNode);

            this.treeViewDir.Nodes.Remove(node);
            this.treeViewDir.SelectedNode = newNode;

            treeViewDir.Focus();

            //数据库更新
            ChangeTrunById(NodeId1, NodeId2);

        }


        //数据库交换次序
        private void ChangeTrunById(string NodeId1, string NodeId2)
        { 
            string SQL="select turn from ttree where nodeid="+NodeId1;
            string Turn1 = AccessAdo.ExecuteScalar(SQL,null).ToString();
            SQL = "select turn from ttree where nodeid=" + NodeId2;
            string Turn2 = AccessAdo.ExecuteScalar(SQL, null).ToString();

            SQL = string.Format("update ttree set turn={0} where nodeid={1}", Turn2, NodeId1);
            try
            {
                AccessAdo.ExecuteNonQuery(SQL, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            SQL = string.Format("update ttree set turn={0} where nodeid={1}", Turn1, NodeId2);

            try
            {
                AccessAdo.ExecuteNonQuery(SQL, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        private void treeViewDir_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeViewDir.SelectedNode == null)
                return;

            int iType = treeViewDir.SelectedNode.ImageIndex;
            if (iType == 0)
            {
                //双击目录
            }
            else
            {
                //双击文章，如果已经打开，则定位，否则新窗口打开

                string treeLocation = treeViewDir.SelectedNode.FullPath;
                int TotalSeconds = Convert.ToInt32(AccessAdo.ExecuteScalar("select updatetime from ttree where nodeid=" + treeViewDir.SelectedNode.Tag.ToString()).ToString());
                DateTime cTime = PubFunc.seconds2Time(TotalSeconds);
                string UpdateTime = "最后更新时间： "+cTime.ToString();

                string sNodeId = treeViewDir.SelectedNode.Tag.ToString();
                formParent.openNew(sNodeId,treeLocation,UpdateTime);

                //打开后设置语言
                string Language = AccessAdo.ExecuteScalar("select synid from ttree where nodeid=" + sNodeId).ToString();
                Language = PubFunc.Synid2LanguageSetLang(Language);
                if (Attachment.isnewOpenDoc == "1")
                {
                    formParent.SetLanguage(Language);
                }
            }
        }

        //右键菜单
        private void treeViewDir_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)//判断你点的是不是右键
            {
                Point ClickPoint = new Point(e.X, e.Y);
                TreeNode CurrentNode = treeViewDir.GetNodeAt(ClickPoint);
                if (CurrentNode != null)//判断你点的是不是一个节点
                {
                    switch (CurrentNode.SelectedImageIndex.ToString())//根据不同节点显示不同的右键菜单
                    {
                        case "0"://目录
                            CurrentNode.ContextMenuStrip = contextMenuStripDir;
                            break;
                        default:
                            CurrentNode.ContextMenuStrip = contextMenuStripTxt;
                            break;
                    }
                    treeViewDir.SelectedNode = CurrentNode;//选中这个节点
                }

                else
                { 
                    //右击空白区域
                    treeViewDir.ContextMenuStrip = contextMenuStripTreeBlank;
                }
            }
        }

        //供主窗口调用
        public void NewDir()
        {
            toolStripMenuItemNewDir_Click(null, null);
        }

        //新建目录
        private void toolStripMenuItemNewDir_Click(object sender, EventArgs e)
        {
            //获取选中节点
            TreeNode SeleNode = treeViewDir.SelectedNode;
            string isHaveNodes = "True";
            string ParLang = "0";

            //如果没有选中节点,则新建顶层目录
            if (SeleNode == null || treeViewDir.Nodes.Count == 0)
            {
                isHaveNodes = "False";
            }
            else
            {
                ParLang = AccessAdo.ExecuteScalar("select SynId from ttree where NodeId=" + SeleNode.Tag.ToString()).ToString();
            }
            ParLang = PubFunc.Synid2Language(ParLang);

            ProperDialog propDia = new ProperDialog("0", "", ParLang);
            DialogResult dr= propDia.ShowDialog();
            if (dr == DialogResult.OK)    
            {
                string Title = propDia.ReturnVal[0];
                string Language = propDia.ReturnVal[1];
                string IsOnRoot=propDia.ReturnVal[2];


                string SynId=PubFunc.Language2Synid(Language);
                
                //没有节点时默认顶级目录
                if (isHaveNodes == "False")
                {
                    string NewPid = "0";
                    string NewNodeId = "1";
                    string NewTurn = "1";


                    //插入数据库记录
                    DateTime d1 = DateTime.Parse("1970-01-01 08:00:00");
                    DateTime d2 = DateTime.Now;
                    TimeSpan dt = d2 - d1;
                    //相差秒数
                    string Seconds = dt.Seconds.ToString();
                    //插入TTREE
                    string sql = string.Format("insert into ttree(NodeID,Title,ParentId,Type,CreateTime,SynId,Turn,Updatetime) values({0},'{1}',{2},{3},{4},{5},{6},{7})", NewNodeId, Title, NewPid, 0, Seconds, SynId, NewTurn,Seconds);
                    AccessAdo.ExecuteNonQuery(sql);

                    //插入树节点
                    TreeNode InsertNodeDir = new TreeNode(Title);
                    InsertNodeDir.Tag = NewNodeId;
                    InsertNodeDir.ImageIndex = 0;
                    InsertNodeDir.SelectedImageIndex = 0;
                    treeViewDir.Nodes.Insert(treeViewDir.Nodes.Count, InsertNodeDir);

                    treeViewDir.SelectedNode = InsertNodeDir;

                }

                else if (SeleNode != null)
                {
                    if (SeleNode.ImageIndex == 1 && IsOnRoot == "False")
                    {
                        MessageBox.Show("不能在文章下新增节点！");
                        return;
                    }
                    string NewPid = SeleNode.Tag.ToString();
                    string NewNodeId = AccessAdo.ExecuteScalar("select max(NodeId) from ttree").ToString();
                    NewNodeId = NewNodeId == "" ? "1" : (Convert.ToInt32(NewNodeId) + 1).ToString();
                    string NewTurn = AccessAdo.ExecuteScalar("select max(Turn) from ttree where parentId=" + NewPid).ToString();
                    NewTurn = NewTurn == "" ? "1" : (Convert.ToInt32(NewTurn) + 1).ToString();

                    //顶层
                    if (IsOnRoot == "True") {
                        NewPid = "0";
                        NewTurn = AccessAdo.ExecuteScalar("select max(Turn) from ttree where parentId=0").ToString();
                        NewTurn = NewTurn == "" ? "1" : (Convert.ToInt32(NewTurn) + 1).ToString();
                    }

                    //插入数据库记录
                    DateTime d1=DateTime.Parse("1970-01-01 08:00:00");
                    DateTime d2=DateTime.Now;
                    TimeSpan dt=d2-d1;
                    //相差秒数
                    string Seconds=dt.TotalSeconds.ToString();
                    //插入TTREE
                    string sql = string.Format("insert into ttree(NodeID,Title,ParentId,Type,CreateTime,SynId,Turn,UpdateTime) values({0},'{1}',{2},{3},{4},{5},{6},{7})", NewNodeId, Title, NewPid, 0, Seconds, SynId, NewTurn,Seconds);
                    AccessAdo.ExecuteNonQuery(sql);

                    //插入树节点
                    TreeNode InsertNodeDir = new TreeNode(Title);
                    InsertNodeDir.Tag = NewNodeId;
                    InsertNodeDir.ImageIndex = 0;
                    InsertNodeDir.SelectedImageIndex = 0;
                    if (IsOnRoot == "True")
                    {
                        treeViewDir.Nodes.Insert(treeViewDir.Nodes.Count, InsertNodeDir);
                        treeViewDir.SelectedNode = InsertNodeDir;
                    }
                    else {
                        SeleNode.Nodes.Insert(SeleNode.Nodes.Count, InsertNodeDir);
                        treeViewDir.SelectedNode = InsertNodeDir;
                    }

                }
             }

        }

        private void toolStripMenuItemBlankNewDir_Click(object sender, EventArgs e)
        {
            toolStripMenuItemNewDir_Click(sender, e);
        }

        //供主窗口调用
        public void NewDoc()
        {
            toolStripMenuItemNewTxt_Click(null, null);
        }

        //供主窗口调用
        public void DelNode()
        {
            toolStripMenuItemDel_Click(null, null);
        }

        //新建文章
        private void toolStripMenuItemNewTxt_Click(object sender, EventArgs e)
        {
            //获取选中节点
            TreeNode SeleNode = treeViewDir.SelectedNode;
            string isHaveNodes = "True";
            string ParLang = "0";
            //如果没有选中节点,则新建顶层目录
            if (SeleNode == null || treeViewDir.Nodes.Count == 0)
            {
                isHaveNodes = "False";
            }
            else{
                ParLang = AccessAdo.ExecuteScalar("select SynId from ttree where NodeId=" + SeleNode.Tag.ToString()).ToString();
            }
           
            ParLang = PubFunc.Synid2Language(ParLang);
            ProperDialog propDia = new ProperDialog("1", "", ParLang);
            DialogResult dr = propDia.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string Title = propDia.ReturnVal[0];
                string Language = propDia.ReturnVal[1];
                string IsOnRoot = propDia.ReturnVal[2];
                string SynId = PubFunc.Language2Synid(Language);

                //没有节点时默认顶级目录
                if (isHaveNodes == "False")
                {
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
                    string sql = string.Format("insert into ttree(NodeID,Title,ParentId,Type,CreateTime,SynId,Turn,UpdateTime) values({0},'{1}',{2},{3},{4},{5},{6},{7})", NewNodeId, Title, NewPid, 1, Seconds, SynId, NewTurn,Seconds);
                    AccessAdo.ExecuteNonQuery(sql);
                    //插入TTcontent
                    sql = string.Format("insert into tcontent(NodeId) values({0})", NewNodeId);
                    AccessAdo.ExecuteNonQuery(sql);

                    //插入树节点
                    TreeNode InsertNodeDoc = new TreeNode(Title);
                    InsertNodeDoc.Tag = NewNodeId;
                    InsertNodeDoc.ImageIndex = 1;
                    InsertNodeDoc.SelectedImageIndex = 1;
                    treeViewDir.Nodes.Insert(treeViewDir.Nodes.Count, InsertNodeDoc);

                    treeViewDir.SelectedNode = InsertNodeDoc;

                    //新窗口打开编辑界面
                    string lastTime="最后更新时间："+DateTime.Now.ToString();
                    formParent.openNew(NewNodeId, treeViewDir.SelectedNode.FullPath, lastTime);

                    //打开后设置语言
                    Language = PubFunc.Synid2LanguageSetLang(SynId);
                    formParent.SetLanguage(Language);
                }
                else if (SeleNode != null)
                {
                    if (SeleNode.ImageIndex == 1&&IsOnRoot=="False")
                    {
                        MessageBox.Show("不能在文章下新增节点！");
                        return;
                    }

                    string NewPid = SeleNode.Tag.ToString();
                    string NewNodeId = AccessAdo.ExecuteScalar("select max(NodeId) from ttree").ToString();
                    NewNodeId = NewNodeId == "" ? "1" : (Convert.ToInt32(NewNodeId) + 1).ToString();
                    string NewTurn = AccessAdo.ExecuteScalar("select max(Turn) from ttree where parentId=" + NewPid).ToString();
                    NewTurn = NewTurn == "" ? "1" : (Convert.ToInt32(NewTurn) + 1).ToString();

                    //顶层
                    if (IsOnRoot == "True")
                    {
                        NewPid = "0";
                        NewTurn = AccessAdo.ExecuteScalar("select max(Turn) from ttree where parentId=0").ToString();
                        NewTurn = NewTurn == "" ? "1" : (Convert.ToInt32(NewTurn) + 1).ToString();
                    }

                    //插入数据库记录
                    DateTime d1 = DateTime.Parse("1970-01-01 08:00:00");
                    DateTime d2 = DateTime.Now;
                    TimeSpan dt = d2 - d1;
                    //相差秒数
                    string Seconds = dt.TotalSeconds.ToString();
                    //插入TTREE
                    string sql = string.Format("insert into ttree(NodeID,Title,ParentId,Type,CreateTime,SynId,Turn,UpdateTime) values({0},'{1}',{2},{3},{4},{5},{6},{7})", NewNodeId, Title, NewPid, 1, Seconds, SynId, NewTurn,Seconds);
                    AccessAdo.ExecuteNonQuery(sql);
                    //插入TTcontent
                    sql = string.Format("insert into tcontent(NodeId) values({0})", NewNodeId);
                    AccessAdo.ExecuteNonQuery(sql);

                    //插入树节点
                    TreeNode InsertNodeDoc = new TreeNode(Title);
                    InsertNodeDoc.Tag = NewNodeId;
                    InsertNodeDoc.ImageIndex = 1;
                    InsertNodeDoc.SelectedImageIndex = 1;
                    if (IsOnRoot == "True")
                    {
                        treeViewDir.Nodes.Insert(treeViewDir.Nodes.Count, InsertNodeDoc);
                        treeViewDir.SelectedNode = InsertNodeDoc;
                    }
                    else
                    {
                        SeleNode.Nodes.Insert(SeleNode.Nodes.Count, InsertNodeDoc);
                        treeViewDir.SelectedNode = InsertNodeDoc;
                    }

                    //新窗口打开编辑界面
                    string lastTime = "最后更新时间：" + DateTime.Now.ToString();
                    formParent.openNew(NewNodeId, treeViewDir.SelectedNode.FullPath, lastTime);

                    //打开后设置语言
                    Language = PubFunc.Synid2LanguageSetLang(SynId);
                    formParent.SetLanguage(Language);
                }
            }
        }

        //删除文章或目录
        private void toolStripMenuItemDel_Click(object sender, EventArgs e)
        {

            //避免保存的提示
            Attachment.isDeleteClose = "1";
            //获取选中节点
            TreeNode SeleNode = treeViewDir.SelectedNode;
            if (SeleNode == null)
                return;

            //删除前确认
            //删除前确认
            if (MessageBox.Show("当前节点及其所有子节点都会被删除，继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            } 

            treeViewDir.Nodes.Remove(SeleNode);

            //删除数据库记录
            DelNodeData(SeleNode.Tag.ToString());

            Attachment.isDeleteClose = "0";

            formParent.ReSetMarkFind();
        }

        //删除数据库记录，同时关闭已打开的文章
        public void DelNodeData(string NodeId)
        {
            string SQL = string.Format("select NodeId from Ttree where parentId={0}", NodeId);
            DataTable temp = AccessAdo.ExecuteDataSet(SQL).Tables[0];
            DataView dv = new DataView(temp);
            foreach (DataRowView drv in dv)
            {
                DelNodeData(drv["NodeId"].ToString());
            }

            //关闭打开的文章
            formParent.CloseDoc(NodeId);

            string DelSQL = string.Format("Delete from TAttachment where NodeId={0}", NodeId);
            AccessAdo.ExecuteNonQuery(DelSQL);
            DelSQL = string.Format("Delete from Tcontent where NodeId={0}", NodeId);
            AccessAdo.ExecuteNonQuery(DelSQL);
            DelSQL = string.Format("Delete from Ttree where NodeId={0}", NodeId);
            AccessAdo.ExecuteNonQuery(DelSQL);

            
        }

        private void toolStripMenuItemTxtDel_Click(object sender, EventArgs e)
        {
            toolStripMenuItemDel_Click(sender, e);
        }

        
        private void treeViewDir_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
        }

        //加入书签
        private void toolStripMenuItemTxtMark_Click(object sender, EventArgs e)
        {
            //获取选中节点
            TreeNode SeleNode = treeViewDir.SelectedNode;
            if (SeleNode == null)
                return;
            string NodeId = SeleNode.Tag.ToString();
            string SQL = "update Ttree set marktime=" + PubFunc.time2TotalSeconds().ToString() + " where Nodeid=" + NodeId;
            AccessAdo.ExecuteNonQuery(SQL);
        }

        //供主窗口调用
        public void ShowProp()
        {
            toolStripMenuItemProp_Click(null, null);
        }

        //调整、查看属性
        private void toolStripMenuItemProp_Click(object sender, EventArgs e)
        {
            //获取选中节点
            TreeNode SeleNode = treeViewDir.SelectedNode;
            if (SeleNode == null)
                return;
            string ParLang = AccessAdo.ExecuteScalar("select SynId from ttree where NodeId=" + SeleNode.Tag.ToString()).ToString();
            ParLang = PubFunc.Synid2Language(ParLang);
            string Type = AccessAdo.ExecuteScalar("select type from ttree where NodeId=" + SeleNode.Tag.ToString()).ToString();
            string DiaType = (Type == "1") ? "3" : "2";
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
                    AccessAdo.ExecuteNonQuery("update ttree set title='"+Title+"',synid="+SynId+",updatetime="+PubFunc.time2TotalSeconds()+" where nodeid="+SeleNode.Tag.ToString());
                    //更新节点信息
                    SeleNode.Text = Title;
                    //打开后设置语言
                    Language = PubFunc.Synid2LanguageSetLang(SynId);
                    if (SeleNode.ImageIndex == 1)
                    {
                        formParent.SetLanguageByDoc(Language, SeleNode.Tag.ToString(),Title,SeleNode.FullPath);
                    }
                    
                }
            }
        }

        private void toolStripMenuItemTxtProp_Click(object sender, EventArgs e)
        {
            toolStripMenuItemProp_Click(sender, e);
        }

        //切换节点，状态栏显示路径
        private void treeViewDir_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //TreeNode seleNode = treeViewDir.SelectedNode;
            //if (seleNode == null)
            //    return;
            //if (seleNode.ImageIndex == 0)
            //    return;
            //string path = seleNode.FullPath;
            //int TotalSeconds = Convert.ToInt32(AccessAdo.ExecuteScalar("select updatetime from ttree where nodeid=" + seleNode.Tag.ToString()).ToString());
            //DateTime cTime = PubFunc.seconds2Time(TotalSeconds);
            //string createTime = "最后更新时间： "+cTime.ToString();
            //formParent.showFullPathTime(path,createTime);

            //修改为激活文档窗口时显示
        }



        #region drag
        private void treeViewDir_DragDrop(object sender, DragEventArgs e)
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
                    
                    //更新数据库记录
                    string id1 = tn.Tag.ToString();  //拖动的节点
                    string id2 = this.dragDropTreeNode.Tag.ToString();
                    string NewTurn = AccessAdo.ExecuteScalar("select max(Turn) from ttree where parentId=" + id2).ToString();
                    NewTurn = NewTurn == "" ? "1" : (Convert.ToInt32(NewTurn) + 1).ToString();

                    AccessAdo.ExecuteNonQuery("update ttree set parentid=" + id2 + ",turn=" + NewTurn + " where nodeid=" + id1);
                    
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

        private void treeViewDir_DragLeave(object sender, EventArgs e)
        {
            if (this.dragDropTreeNode != null) //在按下{ESC}，取消被放置的节点高亮显示 
            {
                this.dragDropTreeNode.BackColor = SystemColors.Window;
                this.dragDropTreeNode.ForeColor = SystemColors.WindowText;
                this.dragDropTreeNode = null;
            } 
        }

        private void treeViewDir_DragOver(object sender, DragEventArgs e)
        {
            //当光标悬停在 TreeView 控件上时，展开该控件中的 TreeNode 
            Point p = this.treeViewDir.PointToClient(Control.MousePosition);
            TreeNode tn = this.treeViewDir.GetNodeAt(p);
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
                    if (isChildNode||tn.ImageIndex==1)
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

        private void treeViewDir_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode tn = e.Item as TreeNode;
            dragDropTreeNode = tn;
            if ((e.Button == MouseButtons.Left) && (tn != null)) //根节点不允许拖放操作。 
            {
                this.treeViewDir.DoDragDrop(tn, DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link);
            }
        }

        #endregion

        //新建文章
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            NewDoc();
        }

        //新建目录
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            NewDir();
        }
        
        //删除
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            DelNode();
        }
        
        //属性
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            ShowProp();
        }

        //上移
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            setNodeUp();
        }

        //下移
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            setNodeDown();
        }

        //窗体激活时
        private void FormTreeLeft_Activated(object sender, EventArgs e)
        {
            
        }

        private void FormTreeLeft_Deactivate(object sender, EventArgs e)
        {

        }

        //搜索
        private void toolStripButtonSerch_Click(object sender, EventArgs e)
        {
            formParent.OpenSerch("local");
        }


    }

}
