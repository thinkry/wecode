using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace WeCode1._0
{
    public partial class DocMark : DockContent
    {
        public FormMain formParent;

        public DocMark()
        {
            InitializeComponent();
        }

        private void DocFind_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }


        //刷新表格
        private void RefreshGrid()
        {
            string SQL = "select NodeId,[title] as 标题 from ttree where type=1 and marktime>0";
            dataGridViewSerch.DataSource = AccessAdo.ExecuteDataSet(SQL).Tables[0];
            dataGridViewSerch.Columns[0].Visible = false;
        }

        //双击打开文章
        private void dataGridViewSerch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string nodeid = dataGridViewSerch.SelectedRows[0].Cells[0].Value.ToString();
            formParent.openNew(nodeid);

            //打开后设置语言
            string Language = AccessAdo.ExecuteScalar("select synid from ttree where nodeid=" + nodeid).ToString();
            Language = PubFunc.Synid2LanguageSetLang(Language);
            formParent.SetLanguage(Language);
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }
    }
}
