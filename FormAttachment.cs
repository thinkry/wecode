using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using System.Data.OleDb;
using System.Xml;
using System.Configuration;
using System.Collections.Specialized;
using System.Threading;

namespace WeCode1._0
{
    public partial class FormAttachment : DockContent
    {

        //上传参数
        private string _upUrl;
        private string _upFilePath;
        private string _upFileName;


        public FormAttachment()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FormAttachment_Load(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = AccessAdo.ExecuteDataSet("select '' as 序号,[title] as 标题,[size] as 大小,[time] as 加入日期 from tattachment where nodeid=-1").Tables[0];
            ReFreshAttachGrid();
        }

        //刷新附件
        public void ReFreshAttachGrid()
        {
            CheckForIllegalCrossThreadCalls = false;
            try { 
                //本地附件
            if (Attachment.ActiveDOCType == "local")
            {
                DataView dv = new DataView(AccessAdo.ExecuteDataSet("select affixid,nodeid,'' as 序号,[title] as 标题,format$([size]/1024,\"Fixed\")+'KB' as 大小,[time],' ' as 加入日期 from tattachment where nodeid=" + Attachment.ActiveNodeId).Tables[0]);
                dataGridView1.DataSource = dv;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[5].Visible = false;

                int count = dv.Count;
                for (int i = 0; i < count; i++)
                {
                    dataGridView1.Rows[i].Cells[2].Value = i + 1;
                    string TotalSeConds = dataGridView1.Rows[i].Cells[5].Value.ToString();
                    DateTime AttTime = PubFunc.seconds2Time(Convert.ToInt32(TotalSeConds));
                    dataGridView1.Rows[i].Cells[6].Value = AttTime.ToString();
                }
            }
            else if (Attachment.ActiveDOCType == "online")
            { 
                //有道附件
                XmlDocument xDoc = NoteAPI.GetResourceWithXML(Attachment.ActiveNodeId);
                DataTable tempDt=new DataTable();
                tempDt.Columns.Add("path");
                tempDt.Columns.Add("序号");
                tempDt.Columns.Add("标题");
                tempDt.Columns.Add("大小");
                tempDt.Columns.Add("加入日期");
                int i=1;
                foreach (XmlNode xNode in xDoc.DocumentElement.ChildNodes)
                {
                    DataRow dr = tempDt.NewRow();
                    dr["path"] = xNode.Attributes["path"].Value;
                    dr["序号"] = i;
                    dr["标题"] = xNode.Attributes["title"].Value;
                    dr["大小"] = (double.Parse(xNode.Attributes["filelength"].Value)/1024).ToString("0.00")+"KB";
                    dr["加入日期"] = "";
                    tempDt.Rows.Add(dr);
                    i++;
                }

                DataView dv = new DataView(tempDt);
                dataGridView1.DataSource = dv;
                dataGridView1.Columns[0].Visible = false;
            }
            }
            catch(Exception e)
            {
                
            }
            
        }

        //添加附件
        private void toolStripMenuItemAddZIP_Click(object sender, EventArgs e)
        {
            AddAttZip();
        }

        private void AddAttZip()
        {
            if (Attachment.ActiveNodeId == "-1")
                return;

            Stream myStream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //先校验文件大小以及文件格式
                FileInfo finfo = new FileInfo(openFileDialog1.FileName);
                if (finfo.Length >= 25000000)
                {
                    MessageBox.Show("附件最大大小必须小于25MB，请重试！");
                    return;
                }

                string fileName = openFileDialog1.FileName;
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    // Insert code to read the stream here.
                    FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, (int)fs.Length);

                    int Datalength = (int)fs.Length;
                    fs.Close();

                    string NewAffixid = AccessAdo.ExecuteScalar("select max(affixid) from tattachment").ToString();
                    int intNewAffixid = NewAffixid == "" ? 1 : Convert.ToInt32(NewAffixid) + 1;

                    int Nodeid = Convert.ToInt32(Attachment.ActiveNodeId);
                    string Title = openFileDialog1.SafeFileName;
                    int timeSeconds = PubFunc.time2TotalSeconds();

                    //affixid
                    OleDbParameter p1 = new OleDbParameter("@affixid", OleDbType.Integer);
                    p1.Value = intNewAffixid;
                    //nodeid
                    OleDbParameter p2 = new OleDbParameter("@nodeid", OleDbType.Integer);
                    p2.Value = Nodeid;
                    //title
                    OleDbParameter p3 = new OleDbParameter("@title", OleDbType.VarChar);
                    p3.Value = Title;
                    //二进制数据
                    OleDbParameter p4 = new OleDbParameter("@Data", OleDbType.Binary);
                    p4.Value = data;
                    //size
                    OleDbParameter p5 = new OleDbParameter("@size", OleDbType.Integer);
                    p5.Value = Datalength;
                    //time
                    OleDbParameter p6 = new OleDbParameter("@time", OleDbType.Integer);
                    p6.Value = timeSeconds;

                    OleDbParameter[] arrPara = new OleDbParameter[6];
                    arrPara[0] = p1;
                    arrPara[1] = p2;
                    arrPara[2] = p3;
                    arrPara[3] = p4;
                    arrPara[4] = p5;
                    arrPara[5] = p6;
                    string SQL = "insert into tattachment values(@affixid,@nodeid,@title,@Data,@size,@time)";
                    AccessAdo.ExecuteNonQuery(SQL, arrPara);

                    myStream.Close();

                    ReFreshAttachGrid();
                }
            }
        }

        //另存为
        private void toolStripMenuItemSaveAs_Click(object sender, EventArgs e)
        {
            string Affixid = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string Title = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            byte[] TempData = (byte[])AccessAdo.ExecuteScalar("select data from tattachment where affixid=" + Affixid);

            string Path;
            SaveFileDialog sf=new SaveFileDialog();
            sf.FileName=Title;
            //设置文件类型
            sf.Filter = "All files(*.*)|*.*";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                Path = sf.FileName;
                FileStream fs = new FileStream(Path, FileMode.Create);
                fs.Write(TempData, 0, TempData.Length);
                fs.Close();
            }

        }

        //右键菜单
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1 && Attachment.ActiveDOCType == "local")
            {
                dataGridView1.CurrentRow.Selected = false;
                dataGridView1.Rows[e.RowIndex].Selected = true;
                contextMenuStripAtt1.Show(MousePosition.X, MousePosition.Y);
            }
            else if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1 && Attachment.ActiveDOCType == "online")
            {
                dataGridView1.CurrentRow.Selected = false;
                dataGridView1.Rows[e.RowIndex].Selected = true;
                contextMenuStripOL1.Show(MousePosition.X, MousePosition.Y);
            }
        }
        
        //右键菜单
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right&&Attachment.ActiveDOCType=="local")
            {
                contextMenuStripAtt2.Show(MousePosition.X, MousePosition.Y);
            }
            else if (e.Button == MouseButtons.Right && Attachment.ActiveDOCType == "online")
            {
                contextMenuStripOL2.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void 添加附件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddAttZip();
        }

        //打开附件
        private void toolStripMenuItemOpenZIP_Click(object sender, EventArgs e)
        {
            string Affixid = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string Title = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            byte[] TempData = (byte[])AccessAdo.ExecuteScalar("select data from tattachment where affixid=" + Affixid);

            string Path = Environment.CurrentDirectory;
            if (!Directory.Exists(@Path + "\\temp"))
            {
                Directory.CreateDirectory(@Path + "\\temp");
            }
            Path = @Path + "\\temp\\"+Title;

            FileStream fs = new FileStream(Path, FileMode.Create);
            fs.Write(TempData, 0, TempData.Length);
            fs.Close();
            System.Diagnostics.Process.Start(Path);
        }

        //删除附件
        private void toolStripMenuItemDelZIP_Click(object sender, EventArgs e)
        {
            string Affixid = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            AccessAdo.ExecuteNonQuery("delete from tattachment where affixid=" + Affixid);
            ReFreshAttachGrid();
        }

        //重命名
        private void toolStripMenuItemReName_Click(object sender, EventArgs e)
        {
            string Affixid = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string Title = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();

            AttRenameDialog ReNameDia = new AttRenameDialog(Title);
            DialogResult dr = ReNameDia.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Title = ReNameDia.ReturnVal[0];
                AccessAdo.ExecuteNonQuery("update tattachment set title='" + Title + "' where affixid=" + Affixid);
                ReFreshAttachGrid();
            }
        }

        //上传云附件
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["AccessToken"] != "")
            {
                try
                {
                    //获取AccessTokon构造URL
                    StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/resource/upload.json"); //可变字符串

                    NameValueCollection myCol = new NameValueCollection();
                    myCol.Add("nihao", "nihao");

                    OpenFileDialog op = new OpenFileDialog();
                    DialogResult dr = op.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        //先校验文件大小以及文件格式
                        FileInfo finfo = new FileInfo(op.FileName);
                        if (finfo.Length >= 25000000)
                        {
                            MessageBox.Show("附件最大大小必须小于25MB，请重试！");
                            return;
                        }
                        else if(finfo.Extension.ToUpper()==".EXE"||finfo.Extension.ToUpper()==".COM"||finfo.Extension.ToUpper()==".BAT"||finfo.Extension.ToUpper()==".CMD"||finfo.Extension.ToUpper()==".SYS")
                        {
                            MessageBox.Show("不允许上传exe、com、cmd、bat、sys格式的附件，请重试！");
                            return;
                        }
                        // string aaa = HttpPostFile(url.ToString(), op.SafeFileName,op.FileName, myCol);
                        //Upload_Request(url.ToString(), op.FileName, op.SafeFileName, progressBar1);
                        //新线程打开下载窗口
                        //_upUrl = url.ToString();
                        //_upFilePath = op.FileName;
                        //_upFileName = op.SafeFileName;

                        //UpLoad upForm = new UpLoad(_upUrl, _upFilePath, _upFileName,Attachment.ActiveNodeId);
                        //upForm.Show();

                        _upUrl = url.ToString();
                        _upFilePath = op.FileName;
                        _upFileName = op.SafeFileName;

                        UpLoad upForm = new UpLoad(_upUrl, _upFilePath, _upFileName, Attachment.ActiveNodeId);
                        upForm.Show();

                        //Thread t = new Thread(new ThreadStart(ThFun));
                        //t.Start(); 

                        
                    }

                }
                catch (Exception msg) //异常处理
                {
                    MessageBox.Show(msg.Message);
                }
            }
        }

        
        //上传云附件
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripMenuItem5_Click(sender, e);
        }

        public void GridDiable()
        {
            this.dataGridView1.Enabled = false;
        }

        public void GridEnable()
        {
            this.dataGridView1.Enabled = true;
        }

        //下载云附件
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                //下载路径
                string path = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string Title = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

                //获取AccessTokon构造URL
                StringBuilder url = new StringBuilder(path); //可变字符串
                //追加组合格式字符串
                url.AppendFormat("?oauth_token={0}&", ConfigurationManager.AppSettings["AccessToken"]);

                string Input = url.ToString();

                ////HttpWebRequest对象实例:该类用于获取和操作HTTP请求
                //var request = (HttpWebRequest)WebRequest.Create(Input); //Create:创建WebRequest对象
                ////设置请求方法为GET
                ////request.Headers.Add("Authorization", "Bearer " + accessToken);
                //request.Method = "GET";
                ////HttpWebResponse对象实例:该类用于获取和操作HTTP应答 
                //var response = (HttpWebResponse)request.GetResponse(); //GetResponse:获取答复
                ////构造数据流对象实例
                //Stream stream = response.GetResponseStream(); //GetResponseStream:获取应答流

                string Path;
                SaveFileDialog sf = new SaveFileDialog();
                sf.FileName = Title;
                //设置文件类型
                sf.Filter = "All files(*.*)|*.*";
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    Path = sf.FileName;
                    Download dl = new Download(Input, Path);
                    dl.Show();
                }

                //关闭响应流
                //stream.Close();
                //response.Close();
            }
            catch (Exception msg) //异常处理
            {
                MessageBox.Show(msg.Message);
            }
        }

        //删除附件
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //修改笔记
            string path = Attachment.ActiveNodeId;
            string rsurl=dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            NoteAPI.DeleteRource(path, rsurl);
            ReFreshAttachGrid();
        }

        //重命名附件
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            string path = Attachment.ActiveNodeId;
            string rsurl = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string Title = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

            AttRenameDialog ReNameDia = new AttRenameDialog(Title);
            DialogResult dr = ReNameDia.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Title = ReNameDia.ReturnVal[0];
                NoteAPI.RenameRource(path, rsurl,Title);
                ReFreshAttachGrid();
            }
            
        }
    }
}
