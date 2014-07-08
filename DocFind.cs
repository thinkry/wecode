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
    public partial class DocFind : DockContent
    {
        public FormMain formParent;
        private string serchType;

        public DocFind()
        {
            InitializeComponent();
        }

        private void DocFind_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
            dataGridViewSerch.DataSource = AccessAdo.ExecuteDataSet("select [title] as 标题 from ttree where 1=2").Tables[0];
        }

        //查找
        private void button1_Click(object sender, EventArgs e)
        {
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
                serchType = "local";
                string SQL = "select NodeId,[title] as 标题 from ttree where type=1 and title like '%" + Title + "%'";
                dataGridViewSerch.DataSource = AccessAdo.ExecuteDataSet(SQL).Tables[0];
                dataGridViewSerch.Columns[0].Visible = false;
            }
            else if (comboBox1.Text == "有道云" && Attachment.IsTokeneffective == 1)
            {
                serchType = "online";
                DataTable tempDt = new DataTable();
                tempDt.Columns.Add("path");
                tempDt.Columns.Add("标题");
                tempDt.Columns.Add("Language");

                XmlDocument doc = new XmlDocument();
                doc.Load("TreeNodeLocal.xml");
                XmlNodeList xlist = doc.SelectNodes("//note[contains(@title,'" + Title + "')]");
                foreach (XmlNode xnode in xlist)
                {
                    DataRow dr = tempDt.NewRow();
                    dr["path"] = xnode.Attributes["path"].Value;
                    dr["标题"] = xnode.Attributes["title"].Value;
                    dr["Language"] = xnode.Attributes["Language"].Value;
                    tempDt.Rows.Add(dr);
                }
                dataGridViewSerch.DataSource = tempDt;
                dataGridViewSerch.Columns[0].Visible = false;
                dataGridViewSerch.Columns[2].Visible = false;


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
                    formParent.openNew(nodeid);

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
                    formParent.openNewYouDao(sNodeId, title);

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
    }
}
