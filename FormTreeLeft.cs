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
        public FormTreeLeft()
        {
            InitializeComponent();
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
                string sNodeId = treeViewDir.SelectedNode.Tag.ToString();
                formParent.openNew(sNodeId);

                //打开后设置语言
                string Language = AccessAdo.ExecuteScalar("select synid from ttree where nodeid=" + sNodeId).ToString();
                Language = PubFunc.Synid2LanguageSetLang(Language);
                formParent.SetLanguage(Language);
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
                    string sql = string.Format("insert into ttree(NodeID,Title,ParentId,Type,CreateTime,SynId,Turn) values({0},'{1}',{2},{3},{4},{5},{6})", NewNodeId, Title, NewPid, 0, Seconds, SynId, NewTurn);
                    AccessAdo.ExecuteNonQuery(sql);

                    //插入树节点
                    TreeNode InsertNodeDir = new TreeNode(Title);
                    InsertNodeDir.Tag = NewNodeId;
                    InsertNodeDir.ImageIndex = 0;
                    InsertNodeDir.SelectedImageIndex = 0;
                    treeViewDir.Nodes.Insert(treeViewDir.Nodes.Count, InsertNodeDir);

                }

                else if (SeleNode != null)
                {
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
                    string Seconds=dt.Seconds.ToString();
                    //插入TTREE
                    string sql = string.Format("insert into ttree(NodeID,Title,ParentId,Type,CreateTime,SynId,Turn) values({0},'{1}',{2},{3},{4},{5},{6})", NewNodeId, Title, NewPid, 0, Seconds, SynId, NewTurn);
                    AccessAdo.ExecuteNonQuery(sql);

                    //插入树节点
                    TreeNode InsertNodeDir = new TreeNode(Title);
                    InsertNodeDir.Tag = NewNodeId;
                    InsertNodeDir.ImageIndex = 0;
                    InsertNodeDir.SelectedImageIndex = 0;
                    if (IsOnRoot == "True")
                    {
                        treeViewDir.Nodes.Insert(treeViewDir.Nodes.Count, InsertNodeDir);
                    }
                    else {
                        SeleNode.Nodes.Insert(SeleNode.Nodes.Count, InsertNodeDir);
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
            if (SeleNode == null)
                return;
            string ParLang = AccessAdo.ExecuteScalar("select SynId from ttree where NodeId=" + SeleNode.Tag.ToString()).ToString();
            ParLang = PubFunc.Synid2Language(ParLang);
            ProperDialog propDia = new ProperDialog("1", "", ParLang);
            DialogResult dr = propDia.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string Title = propDia.ReturnVal[0];
                string Language = propDia.ReturnVal[1];
                string IsOnRoot = propDia.ReturnVal[2];
                string SynId = PubFunc.Language2Synid(Language);

                if (SeleNode != null)
                {
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
                    string Seconds = dt.Seconds.ToString();
                    //插入TTREE
                    string sql = string.Format("insert into ttree(NodeID,Title,ParentId,Type,CreateTime,SynId,Turn) values({0},'{1}',{2},{3},{4},{5},{6})", NewNodeId, Title, NewPid, 1, Seconds, SynId, NewTurn);
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
                    }
                    else
                    {
                        SeleNode.Nodes.Insert(SeleNode.Nodes.Count, InsertNodeDoc);
                    }

                    //新窗口打开编辑界面
                    formParent.openNew(NewNodeId);

                    //打开后设置语言
                    Language = PubFunc.Synid2LanguageSetLang(SynId);
                    formParent.SetLanguage(Language);
                }
            }
        }

        //删除文章或目录
        private void toolStripMenuItemDel_Click(object sender, EventArgs e)
        {
            //获取选中节点
            TreeNode SeleNode = treeViewDir.SelectedNode;
            if (SeleNode == null)
                return;

            //删除前确认


            treeViewDir.Nodes.Remove(SeleNode);

            //删除数据库记录
            DelNodeData(SeleNode.Tag.ToString());
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


    }

}
