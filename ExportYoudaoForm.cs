using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Threading;
using System.Xml;

namespace WeCode1._0
{
    public partial class ExportYoudaoForm : Form
    {
        Thread t;
        public OleDbConnection ExportConn;
        //每次插入数据库记录后，nodeid递增
        public int iNodeId = 1;

        public ExportYoudaoForm(OleDbConnection conn)
        {
            InitializeComponent();
            ExportConn = conn;
        }

        private void ExportYoudaoForm_Shown(object sender, EventArgs e)
        {
            //新线程下载
            Control.CheckForIllegalCrossThreadCalls = false;
            t = new Thread(new ThreadStart(ExportYoudao));
            t.Start(); 
        }

        //导出笔记本
        public void ExportYoudao()
        {
            //1、先导出秘钥信息
            XmlDocument doc = new XmlDocument();
            doc.Load("TreeNodeLocal.xml");
            XmlNode preNode = doc.SelectSingleNode("//wecode[@KeyD5]");
            if (preNode != null)
            {
                //存在秘钥信息
                string keyD5 = preNode.Attributes["KeyD5"].Value;
                string keyE = preNode.Attributes["KeyE"].Value;

                //第一次设置密码，写入到数据库
                OleDbParameter p1 = new OleDbParameter("@KeyE", OleDbType.VarChar);
                p1.Value = keyE;
                OleDbParameter p2 = new OleDbParameter("@KeyD5", OleDbType.VarChar);
                p2.Value = keyD5;

                OleDbParameter[] ArrPara = new OleDbParameter[2];
                ArrPara[0] = p1;
                ArrPara[1] = p2;
                string SQL = "insert into MyKeys(KeyE,KeyD5) values(@KeyE,@KeyD5)";
                AccessAdo.ExecuteNonQuery(ExportConn,SQL, ArrPara);

            }


            //2、生成ttree
            XmlNode wecode = doc.DocumentElement;

            //标记顺序的下标
            int iTurn = 1;
            foreach (XmlNode cNode in wecode)
            {
                //添加根节点
                if (cNode.Name == "note")
                {
                    string title = cNode.Attributes["title"].Value;
                    string Language = cNode.Attributes["Language"].Value;
                    string SysId = PubFunc.Language2Synid(Language);
                    string path = cNode.Attributes["path"].Value;
                    string createtime = cNode.Attributes["createtime"].Value;
                    string islock = cNode.Attributes["IsLock"].Value;
                    string ismark = cNode.Attributes["isMark"].Value;
                    int nodeid = this.iNodeId;

                    OleDbParameter p1 = new OleDbParameter("@NodeId", OleDbType.Integer);
                    p1.Value = nodeid;
                    OleDbParameter p2 = new OleDbParameter("@Title", OleDbType.VarChar);
                    p2.Value = title;
                    OleDbParameter p3 = new OleDbParameter("@path", OleDbType.VarChar);
                    p3.Value = path;
                    OleDbParameter p4 = new OleDbParameter("@parentid", OleDbType.Integer);
                    p4.Value = 0;
                    OleDbParameter p5 = new OleDbParameter("@Type", OleDbType.Integer);
                    p5.Value = 1;
                    OleDbParameter p6 = new OleDbParameter("@createtime", OleDbType.Integer);
                    p6.Value = createtime;
                    OleDbParameter p7 = new OleDbParameter("@SysId", OleDbType.Integer);
                    p7.Value = SysId;
                    OleDbParameter p8 = new OleDbParameter("@turn", OleDbType.Integer);
                    p8.Value = iTurn;
                    OleDbParameter p9 = new OleDbParameter("@marktime", OleDbType.Integer);
                    p9.Value = ismark;
                    OleDbParameter p10 = new OleDbParameter("@islock", OleDbType.Integer);
                    p10.Value = islock;

                    OleDbParameter[] ArrPara = new OleDbParameter[10];
                    ArrPara[0] = p1;
                    ArrPara[1] = p2;
                    ArrPara[2] = p3;
                    ArrPara[3] = p4;
                    ArrPara[4] = p5;
                    ArrPara[5] = p6;
                    ArrPara[6] = p7;
                    ArrPara[7] = p8;
                    ArrPara[8] = p9;
                    ArrPara[9] = p10;

                    string SQL = "insert into ttree(NodeID,Title,path,ParentId,Type,CreateTime,SynId,Turn,MarkTime,UpdateTime,IsLock) "+
                                " values(@NodeId,@Title,@path,@parentid,@Type,@createtime,@SysId,@turn,@marktime,@islock) ";
                    AccessAdo.ExecuteNonQuery(ExportConn, SQL, ArrPara);

                    this.iNodeId++;
                    iTurn++;

                    addTreeNode(cNode, nodeid);

                }
                else if (cNode.Name == "book")
                {
                    string title = cNode.Attributes["title"].Value;
                    string Language = cNode.Attributes["Language"].Value;
                    string SysId = PubFunc.Language2Synid(Language);
                    int nodeid = this.iNodeId;

                    OleDbParameter p1 = new OleDbParameter("@NodeId", OleDbType.Integer);
                    p1.Value = nodeid;
                    OleDbParameter p2 = new OleDbParameter("@Title", OleDbType.VarChar);
                    p2.Value = title;
                    OleDbParameter p3 = new OleDbParameter("@path", OleDbType.VarChar);
                    p3.Value = "";
                    OleDbParameter p4 = new OleDbParameter("@parentid", OleDbType.Integer);
                    p4.Value = 0;
                    OleDbParameter p5 = new OleDbParameter("@Type", OleDbType.Integer);
                    p5.Value = 0;
                    OleDbParameter p6 = new OleDbParameter("@createtime", OleDbType.Integer);
                    p6.Value = PubFunc.time2TotalSeconds();
                    OleDbParameter p7 = new OleDbParameter("@SysId", OleDbType.Integer);
                    p7.Value = SysId;
                    OleDbParameter p8 = new OleDbParameter("@turn", OleDbType.Integer);
                    p8.Value = iTurn;
                    OleDbParameter p9 = new OleDbParameter("@marktime", OleDbType.Integer);
                    p9.Value = 0;
                    OleDbParameter p10 = new OleDbParameter("@islock", OleDbType.Integer);
                    p10.Value = 0;

                    OleDbParameter[] ArrPara = new OleDbParameter[10];
                    ArrPara[0] = p1;
                    ArrPara[1] = p2;
                    ArrPara[2] = p3;
                    ArrPara[3] = p4;
                    ArrPara[4] = p5;
                    ArrPara[5] = p6;
                    ArrPara[6] = p7;
                    ArrPara[7] = p8;
                    ArrPara[8] = p9;
                    ArrPara[9] = p10;

                    string SQL = "insert into ttree(NodeID,Title,path,ParentId,Type,CreateTime,SynId,Turn,MarkTime,UpdateTime,IsLock) " +
                                " values(@NodeId,@Title,@path,@parentid,@Type,@createtime,@SysId,@turn,@marktime,@islock) ";
                    AccessAdo.ExecuteNonQuery(ExportConn, SQL, ArrPara);

                    this.iNodeId++;
                    iTurn++;

                    addTreeNode(cNode, nodeid);
                }

            }

        }

        //递归方法
        private void addTreeNode(XmlNode xmlNode,int pid)
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
                    if (xNode.Name == "note")
                    {
                        string title = xNode.Attributes["title"].Value;
                        string Language = xNode.Attributes["Language"].Value;
                        string SysId = PubFunc.Language2Synid(Language);
                        string path = xNode.Attributes["path"].Value;
                        string createtime = xNode.Attributes["createtime"].Value;
                        string islock = xNode.Attributes["IsLock"].Value;
                        string ismark = xNode.Attributes["isMark"].Value;
                        int nodeid = this.iNodeId;

                        OleDbParameter p1 = new OleDbParameter("@NodeId", OleDbType.Integer);
                        p1.Value = nodeid;
                        OleDbParameter p2 = new OleDbParameter("@Title", OleDbType.VarChar);
                        p2.Value = title;
                        OleDbParameter p3 = new OleDbParameter("@path", OleDbType.VarChar);
                        p3.Value = path;
                        OleDbParameter p4 = new OleDbParameter("@parentid", OleDbType.Integer);
                        p4.Value = pid;
                        OleDbParameter p5 = new OleDbParameter("@Type", OleDbType.Integer);
                        p5.Value = 1;
                        OleDbParameter p6 = new OleDbParameter("@createtime", OleDbType.Integer);
                        p6.Value = createtime;
                        OleDbParameter p7 = new OleDbParameter("@SysId", OleDbType.Integer);
                        p7.Value = SysId;
                        OleDbParameter p8 = new OleDbParameter("@turn", OleDbType.Integer);
                        p8.Value = x+1;
                        OleDbParameter p9 = new OleDbParameter("@marktime", OleDbType.Integer);
                        p9.Value = ismark;
                        OleDbParameter p10 = new OleDbParameter("@islock", OleDbType.Integer);
                        p10.Value = islock;

                        OleDbParameter[] ArrPara = new OleDbParameter[10];
                        ArrPara[0] = p1;
                        ArrPara[1] = p2;
                        ArrPara[2] = p3;
                        ArrPara[3] = p4;
                        ArrPara[4] = p5;
                        ArrPara[5] = p6;
                        ArrPara[6] = p7;
                        ArrPara[7] = p8;
                        ArrPara[8] = p9;
                        ArrPara[9] = p10;

                        string SQL = "insert into ttree(NodeID,Title,path,ParentId,Type,CreateTime,SynId,Turn,MarkTime,UpdateTime,IsLock) " +
                                    " values(@NodeId,@Title,@path,@parentid,@Type,@createtime,@SysId,@turn,@marktime,@islock) ";
                        AccessAdo.ExecuteNonQuery(ExportConn, SQL, ArrPara);

                        this.iNodeId++;

                        addTreeNode(xNode, nodeid);
                    }
                    else if (xNode.Name == "book")
                    {
                        string title = xNode.Attributes["title"].Value;
                        string Language = xNode.Attributes["Language"].Value;
                        string SysId = PubFunc.Language2Synid(Language);
                        int nodeid = this.iNodeId;

                        OleDbParameter p1 = new OleDbParameter("@NodeId", OleDbType.Integer);
                        p1.Value = nodeid;
                        OleDbParameter p2 = new OleDbParameter("@Title", OleDbType.VarChar);
                        p2.Value = title;
                        OleDbParameter p3 = new OleDbParameter("@path", OleDbType.VarChar);
                        p3.Value = "";
                        OleDbParameter p4 = new OleDbParameter("@parentid", OleDbType.Integer);
                        p4.Value = pid;
                        OleDbParameter p5 = new OleDbParameter("@Type", OleDbType.Integer);
                        p5.Value = 0;
                        OleDbParameter p6 = new OleDbParameter("@createtime", OleDbType.Integer);
                        p6.Value = PubFunc.time2TotalSeconds();
                        OleDbParameter p7 = new OleDbParameter("@SysId", OleDbType.Integer);
                        p7.Value = SysId;
                        OleDbParameter p8 = new OleDbParameter("@turn", OleDbType.Integer);
                        p8.Value = x+1;
                        OleDbParameter p9 = new OleDbParameter("@marktime", OleDbType.Integer);
                        p9.Value = 0;
                        OleDbParameter p10 = new OleDbParameter("@islock", OleDbType.Integer);
                        p10.Value = 0;

                        OleDbParameter[] ArrPara = new OleDbParameter[10];
                        ArrPara[0] = p1;
                        ArrPara[1] = p2;
                        ArrPara[2] = p3;
                        ArrPara[3] = p4;
                        ArrPara[4] = p5;
                        ArrPara[5] = p6;
                        ArrPara[6] = p7;
                        ArrPara[7] = p8;
                        ArrPara[8] = p9;
                        ArrPara[9] = p10;

                        string SQL = "insert into ttree(NodeID,Title,path,ParentId,Type,CreateTime,SynId,Turn,MarkTime,UpdateTime,IsLock) " +
                                    " values(@NodeId,@Title,@path,@parentid,@Type,@createtime,@SysId,@turn,@marktime,@islock) ";
                        AccessAdo.ExecuteNonQuery(ExportConn, SQL, ArrPara);

                        this.iNodeId++;

                        addTreeNode(xNode, nodeid);
                    }
                    
                }
            }
        }

    }
}
