using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Xml;

namespace WeCode1._0
{
    public partial class DocMark : DockContent
    {
        public FormMain formParent;
        private string markType="local";

        public DocMark()
        {
            InitializeComponent();
        }

        private void DocFind_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
            RefreshGrid("local");
        }


        //刷新表格
        public void RefreshGrid(string sType)
        {
            if (sType == "local")
            {
                string SQL = "select NodeId,[title] as 标题 from ttree where type=1 and marktime>0";
                dataGridViewSerch.DataSource = AccessAdo.ExecuteDataSet(SQL).Tables[0];
                dataGridViewSerch.Columns[0].Visible = false;
            }
            else if (sType == "online"&&Attachment.IsTokeneffective==1)
            {
                DataTable tempDt = new DataTable();
                tempDt.Columns.Add("path");
                tempDt.Columns.Add("标题");
                tempDt.Columns.Add("Language");
                tempDt.Columns.Add("id");

                XmlDocument doc = new XmlDocument();
                doc.Load("TreeNodeLocal.xml");
                XmlNodeList xlist = doc.SelectNodes("//note[@isMark='1']");
                foreach (XmlNode xnode in xlist)
                {
                    DataRow dr = tempDt.NewRow();
                    dr["path"] = xnode.Attributes["path"].Value;
                    dr["标题"] = xnode.Attributes["title"].Value;
                    dr["Language"] = xnode.Attributes["Language"].Value;
                    dr["id"] = xnode.Attributes["id"].Value;
                    tempDt.Rows.Add(dr);
                }
                dataGridViewSerch.DataSource = tempDt;
                dataGridViewSerch.Columns[0].Visible = false;
                dataGridViewSerch.Columns[2].Visible = false;
                dataGridViewSerch.Columns[3].Visible = false;
            }
            else
            {
                DataTable tempDt = new DataTable();
                tempDt.Columns.Add("path");
                tempDt.Columns.Add("标题");
                tempDt.Columns.Add("Language");
                tempDt.Columns.Add("id");
                dataGridViewSerch.DataSource = tempDt;
                dataGridViewSerch.Columns[0].Visible = false;
                dataGridViewSerch.Columns[2].Visible = false;
                dataGridViewSerch.Columns[3].Visible = false;
            }
        }

        //双击打开文章
        private void dataGridViewSerch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (markType == "local")
                {
                    string nodeid = dataGridViewSerch.SelectedRows[0].Cells[0].Value.ToString();
                    formParent.openNew(nodeid);

                    //打开后设置语言
                    string Language = AccessAdo.ExecuteScalar("select synid from ttree where nodeid=" + nodeid).ToString();
                    Language = PubFunc.Synid2LanguageSetLang(Language);
                    formParent.SetLanguage(Language);
                }
                else if (markType == "online")
                {
                    //双击文章，如果已经打开，则定位，否则新窗口打开
                    string sNodeId = dataGridViewSerch.SelectedRows[0].Cells[0].Value.ToString();
                    string sLang = dataGridViewSerch.SelectedRows[0].Cells[2].Value.ToString();
                    string title = dataGridViewSerch.SelectedRows[0].Cells[1].Value.ToString();
                    formParent.openNewYouDao(sNodeId, title);

                    ///打开后设置语言
                    string Language = PubFunc.Synid2LanguageSetLang(PubFunc.Language2Synid(sLang));
                    formParent.SetLanguage(Language);
                }
            }
            catch (Exception ex)
            {
                
            }
            
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "本地")
            {
                markType = "local";
                RefreshGrid("local");
            }
            else if (comboBox1.Text == "有道云")
            {
                markType = "online";
                RefreshGrid("online");
            }
        }

        //删除书签
        private void buttonDelMark_Click(object sender, EventArgs e)
        {
            if (markType == "local")
            {
                string nodeid = dataGridViewSerch.SelectedRows[0].Cells[0].Value.ToString();
                AccessAdo.ExecuteNonQuery("update ttree set marktime=0 where type=1 and nodeid=" + nodeid);
            }
            else if (markType == "online")
            {
                string id = dataGridViewSerch.SelectedRows[0].Cells[3].Value.ToString();
                XmlDocument doc = new XmlDocument();
                doc.Load("TreeNodeLocal.xml");
                XmlNode preNode = doc.SelectSingleNode("//node()[@id='" + id + "']");
                preNode.Attributes["isMark"].Value = "0";
                doc.Save("TreeNodeLocal.xml");
                XMLAPI.XML2Yun();
            }
            RefreshGrid(markType);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex == 0)
            {
                markType = "local";
                RefreshGrid("local");
            }
            else if (this.comboBox1.SelectedIndex == 1)
            {
                markType = "online";
                RefreshGrid("online"); 
            }
        }

    }
}
