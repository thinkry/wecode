using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Xml;
using System.Data.OleDb;

namespace WeCode1._0
{
    public partial class DocFind :DockContent
    {
        public FormMain formParent;
        private string serchType;

        public DocFind()
        {
            InitializeComponent();

            //添加搜索方式选项,默认选中按标题搜索
            comboBox2.SelectedIndex = 0;
        }

        private void DocFind_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
            dataGridViewSerch.DataSource = AccessAdo.ExecuteDataSet("select [title] as 标题 from ttree where 1=2").Tables[0];
        }

        //查找
        private void button1_Click(object sender, EventArgs e)
        {
            toolTip1.Hide(this);
            if (textBoxSerch.Text.Length == 0)
            {
                MessageBox.Show("请输入要搜索的关键字！");
                return;
            }
            else
            {
                serchDoc(textBoxSerch.Text);
            }
        }

        //根据标题进行查找
        private void serchDoc(string Title)
        {
            if (comboBox1.Text == "本地")
            {
                if (comboBox2.SelectedIndex == 0)
                {
                    //按标题搜索
                    serchType = "local";

                    OleDbParameter p1 = new OleDbParameter("@Title", OleDbType.VarChar);
                    p1.Value = Title;
                    OleDbParameter[] ArrPara = new OleDbParameter[1];
                    ArrPara[0] = p1;

                    string SQL = "select NodeId,[title] as 标题,islock from ttree where type=1 and title like '%'+@Title+'%'";
                    dataGridViewSerch.DataSource = AccessAdo.ExecuteDataSet(SQL,ArrPara).Tables[0];
                    dataGridViewSerch.Columns[0].Visible = false;
                    dataGridViewSerch.Columns[2].Visible = false;
                }
                else if (comboBox2.SelectedIndex == 1)
                { 
                    //全文搜索
                    serchType = "local";

                    OleDbParameter p1 = new OleDbParameter("@Title", OleDbType.VarChar);
                    p1.Value = Title;
                    OleDbParameter[] ArrPara = new OleDbParameter[1];
                    ArrPara[0] = p1;

                    string SQL = "SELECT NodeId,[Title] as 标题,isLock FROM Ttree WHERE type=1 AND [title] like '%'+@Title+'%' UNION SELECT NodeId,[Title] as 标题,isLock FROM Ttree where type=1 AND isLock=0 " +
                        "and NodeId in (select nodeid from tcontent where content like '%'+@Title+'%')";
                    dataGridViewSerch.DataSource = AccessAdo.ExecuteDataSet(SQL,ArrPara).Tables[0];
                    dataGridViewSerch.Columns[0].Visible = false;
                    dataGridViewSerch.Columns[2].Visible = false;
                }
                
            }
            else if (comboBox1.Text == "有道云" && Attachment.IsTokeneffective == 1)
            {
                serchType = "online";
                DataTable tempDt = new DataTable();
                tempDt.Columns.Add("path");
                tempDt.Columns.Add("标题");
                tempDt.Columns.Add("Language");
                tempDt.Columns.Add("islock");

                XmlDocument doc = new XmlDocument();
                doc.Load("TreeNodeLocal.xml");
                XmlNodeList xlist = doc.SelectNodes("//note[contains(@title,'" + Title + "')]");
                foreach (XmlNode xnode in xlist)
                {
                    DataRow dr = tempDt.NewRow();
                    dr["path"] = xnode.Attributes["path"].Value;
                    dr["标题"] = xnode.Attributes["title"].Value;
                    dr["Language"] = xnode.Attributes["Language"].Value;
                    dr["islock"] = xnode.Attributes["IsLock"].Value;
                    tempDt.Rows.Add(dr);
                }

                if (comboBox2.SelectedIndex == 1)
                {
                    //1.1.6增加全文搜索，只在本地缓存中搜索文章内容，添加到列表，标题还是按照XML搜索

                    OleDbParameter p1 = new OleDbParameter("@Title", OleDbType.VarChar);
                    p1.Value = Title;
                    OleDbParameter[] ArrPara = new OleDbParameter[1];
                    ArrPara[0] = p1;

                    OleDbConnection ExportConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PubFunc.GetYoudaoDBPath());
                    string SQL = "select path,title,SynId,islock from ttree where type=1 and isLock=0 and nodeid in (select nodeid from tcontent where content like '%'+@Title+'%')";
                    DataSet contentSet = AccessAdo.ExecuteDataSet(ExportConn, SQL,ArrPara);
                    for (int i = 0; i < contentSet.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = tempDt.NewRow();
                        dr["path"] = contentSet.Tables[0].Rows[i]["path"].ToString();
                        dr["标题"] = contentSet.Tables[0].Rows[i]["title"].ToString();
                        dr["Language"] = PubFunc.Synid2Language(contentSet.Tables[0].Rows[i]["SynId"].ToString());
                        dr["islock"] = contentSet.Tables[0].Rows[i]["islock"].ToString();
                        tempDt.Rows.Add(dr);
                    }
                }



                dataGridViewSerch.DataSource = tempDt;
                dataGridViewSerch.Columns[0].Visible = false;
                dataGridViewSerch.Columns[2].Visible = false;
                dataGridViewSerch.Columns[3].Visible = false;


            }
            else
            {
                serchType = "online";
                DataTable tempDt = new DataTable();
                tempDt.Columns.Add("path");
                tempDt.Columns.Add("标题");
                tempDt.Columns.Add("Language");

                dataGridViewSerch.DataSource = tempDt;
                dataGridViewSerch.Columns[0].Visible = false;
                dataGridViewSerch.Columns[2].Visible = false;
            }
        }

        //双击打开文章
        private void dataGridViewSerch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (serchType == "local")
                {
                    string nodeid = dataGridViewSerch.SelectedRows[0].Cells[0].Value.ToString();
                    int TotalSeconds = Convert.ToInt32(AccessAdo.ExecuteScalar("select updatetime from tcontent where nodeid=" + nodeid).ToString());
                    DateTime cTime = PubFunc.seconds2Time(TotalSeconds);
                    string UpdateTime = "最后更新时间： " + cTime.ToString();
                    string treeLocation = PubFunc.id2FullPath(nodeid);

                    int iType = Convert.ToInt16(dataGridViewSerch.SelectedRows[0].Cells[2].Value.ToString())+1;
                    if (iType == 2)
                    {
                        //加密,对content解密
                        string Mykeyd = "";
                        if (Attachment.KeyD != "")
                        {
                            //内存中已存在秘钥
                            Mykeyd = Attachment.KeyD;
                        }
                        else
                        {
                            //内存中不存在秘钥
                            DialogPSW dp = new DialogPSW("3");
                            DialogResult dr = dp.ShowDialog();
                            if (dr == DialogResult.OK)
                            {
                                Mykeyd = dp.ReturnVal;
                            }
                        }

                        if (Mykeyd == "")
                            return;

                    }

                    formParent.openNew(nodeid, treeLocation, UpdateTime,iType);

                    //打开后设置语言
                    string Language = AccessAdo.ExecuteScalar("select synid from ttree where nodeid=" + nodeid).ToString();
                    Language = PubFunc.Synid2LanguageSetLang(Language);
                    if (Attachment.isnewOpenDoc == "1")
                    {
                        formParent.SetLanguage(Language);
                    }
                }
                else if (serchType == "online")
                {
                    //双击文章，如果已经打开，则定位，否则新窗口打开
                    string sNodeId = dataGridViewSerch.SelectedRows[0].Cells[0].Value.ToString();
                    string sLang = dataGridViewSerch.SelectedRows[0].Cells[2].Value.ToString();
                    string title = dataGridViewSerch.SelectedRows[0].Cells[1].Value.ToString();
                    string treeLacation = PubFunc.id2FullPathYoudao(sNodeId);


                    int iType = Convert.ToInt16(dataGridViewSerch.SelectedRows[0].Cells[3].Value.ToString()) + 1;
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

                    formParent.openNewYouDao(sNodeId, title,treeLacation,iType);

                    ///打开后设置语言
                    string Language = PubFunc.Synid2LanguageSetLang(PubFunc.Language2Synid(sLang));
                    if (Attachment.isnewOpenDoc == "1")
                    {
                        formParent.SetLanguage(Language);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
           

        }

        //打开新的数据库时清空数据
        public void IniData()
        {
            dataGridViewSerch.DataSource = AccessAdo.ExecuteDataSet("select [title] as 标题 from ttree where 1=2").Tables[0];
            this.textBoxSerch.Text = "";
        }


        //设置查找的初始选项
        public void SetSerchType(string sType)
        {
            if (sType == "local")
            {
                this.comboBox1.SelectedIndex = 0;
            }
            else if (sType == "online")
            {
                this.comboBox1.SelectedIndex = 1;
            }
        }

        private void textBoxSerch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null,null);
            }
        }

        private void DocFind_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 1&&comboBox1.SelectedIndex==0)
            {
                toolTip1.ToolTipTitle = "本地全文搜索提示";
                toolTip1.Show("全文搜索不会搜索加密文章的内容", this, new Point(430, -62), 5000);
            }
            else if (comboBox2.SelectedIndex == 1 && comboBox1.SelectedIndex == 1)
            {
                toolTip1.ToolTipTitle = "有道全文搜索提示";
                toolTip1.Show("全文搜索不会搜索加密文章的内容，\n只搜索有道云本地缓存，如果需要，\n请先执行“云笔记同步到本地", this, new Point(430, -95), 5000);
            }
            else
            {
                toolTip1.Hide(this);
            }
        }
    }
}
