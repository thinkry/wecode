using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Data.OleDb;

namespace WeCode1._0
{
    public partial class UpLoad : Form
    {
        Thread t;
         private string _URL;
        private string _Filename;
        private string _Path;
        private string _length;
        private string _notepath;

        public UpLoad(string URL, string Path,string FileName,string NotePath)
        {
            InitializeComponent();
            this._URL = URL;
            this._Filename = FileName;
            this._Path = Path;
            this._notepath = NotePath;
        }

        private void UpLoad_Load(object sender, EventArgs e)
        {

        }

        private void UpLoad_Shown(object sender, EventArgs e)
        {
            //新线程下载
            Control.CheckForIllegalCrossThreadCalls = false;
            t = new Thread(new ThreadStart(ThFun));
            t.Start(); 
        }

        public void ThFun()
        {
            try
            {
                string JRtn = Upload_Request(_URL, _Path, _Filename, this.progressBar1);
                if (JRtn == "")
                {
                    MessageBox.Show("上传失败！");
                    return;
                }
                else
                {
                    JObject jo = JObject.Parse(JRtn);
                    string fileUrl = jo["url"].ToString();
                    //更新笔记
                    NoteAPI.UpdateRource(fileUrl, _length, _Filename, _notepath);

                    //更新缓存数据
                    OleDbConnection ExportConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db\\youdao.mdb");
                    FileStream fs = new FileStream(_Path, FileMode.Open, FileAccess.Read);
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, (int)fs.Length);

                    int Datalength = (int)fs.Length;
                    fs.Close();

                    string NewAffixid = AccessAdo.ExecuteScalar(ExportConn,"select max(affixid) from tattachment").ToString();
                    int intNewAffixid = NewAffixid == "" ? 1 : Convert.ToInt32(NewAffixid) + 1;

                    int Nodeid = (int)AccessAdo.ExecuteScalar(ExportConn, "select nodeid from ttree where path='" + _notepath + "'");
                    string Title = _Filename;
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
                    OleDbParameter p7 = new OleDbParameter("@gid", OleDbType.VarChar);
                    p7.Value = AccessAdo.ExecuteScalar(ExportConn,"select gid from ttree where path='"+_notepath+"'").ToString();

                    OleDbParameter[] arrPara = new OleDbParameter[7];
                    arrPara[0] = p1;
                    arrPara[1] = p2;
                    arrPara[2] = p3;
                    arrPara[3] = p4;
                    arrPara[4] = p5;
                    arrPara[5] = p6;
                    arrPara[6] = p7;
                    string SQL = "insert into tattachment values(@affixid,@nodeid,@title,@Data,@size,@time,@gid)";
                    AccessAdo.ExecuteNonQuery(ExportConn,SQL, arrPara);



                    MessageBox.Show("上传成功！");
                    Attachment.AttForm.ReFreshAttachGrid();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        ////ceshi
        // <summary> 
        /// 将本地文件上传到指定的服务器(HttpWebRequest方法) 
        /// </summary> 
        /// <param name="address">文件上传到的服务器</param> 
        /// <param name="fileNamePath">要上传的本地文件（全路径）</param> 
        /// <param name="saveName">文件上传后的名称</param> 
        /// <param name="progressBar">上传进度条</param> 
        /// <returns>成功返回Json，失败返回""</returns> 
        private string Upload_Request(string address, string fileNamePath, string saveName, ProgressBar progressBar)
        {
            string returnValue = "";

            // 要上传的文件 
            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);

            //时间戳 
            string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "--\r\n");

            //请求头部信息 
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append("file");
            sb.Append("\"; filename=\"");
            sb.Append(saveName);
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append("application/octet-stream");
            sb.Append("\r\n");
            sb.Append("\r\n");
            string strPostHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);

            // 根据uri创建HttpWebRequest对象 
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));
            httpReq.Method = "POST";
            httpReq.Headers.Add("Authorization", "OAuth oauth_token=\"" + ConfigurationManager.AppSettings["AccessToken"] + "\"");

            //对发送的数据不使用缓存 
            httpReq.AllowWriteStreamBuffering = false;

            //设置获得响应的超时时间（300秒） 
            httpReq.Timeout = 300000;
            httpReq.ContentType = "multipart/form-data; boundary=" + strBoundary;
            long length = fs.Length + postHeaderBytes.Length + boundaryBytes.Length;
            long fileLength = fs.Length;
            _length = fileLength.ToString();

            httpReq.ContentLength = length;
            try
            {
                progressBar.Maximum = int.MaxValue;
                progressBar.Minimum = 0;
                progressBar.Value = 0;

                //每次上传4k 
                int bufferLength = 4096;
                byte[] buffer = new byte[bufferLength];

                //已上传的字节数 
                long offset = 0;

                //开始上传时间 
                DateTime startTime = DateTime.Now;
                int size = r.Read(buffer, 0, bufferLength);
                Stream postStream = httpReq.GetRequestStream();

                //发送请求头部消息 
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                while (size > 0)
                {
                    postStream.Write(buffer, 0, size);
                    offset += size;
                    progressBar.Value = (int)(offset * (int.MaxValue / length));
                    TimeSpan span = DateTime.Now - startTime;
                    double second = span.TotalSeconds;
                    lblTime.Text = "已用时：" + second.ToString("F2") + "秒";
                    if (second > 0.001)
                    {
                        lblSpeed.Text = "平均速度：" + (offset / 1024 / second).ToString("0.00") + "KB/秒";
                    }
                    else
                    {
                        lblSpeed.Text = " 正在连接…";
                    }
                    lblState.Text = "已上传：" + (offset * 100.0 / length).ToString("F2") + "%";
                    lblSize.Text = (offset / 1048576.0).ToString("F2") + "M/" + (fileLength / 1048576.0).ToString("F2") + "M";
                    Application.DoEvents();
                    size = r.Read(buffer, 0, bufferLength);
                }
                //添加尾部的时间戳 
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                postStream.Close();

                //获取服务器端的响应 
                var webRespon = (HttpWebResponse)httpReq.GetResponse();
                Stream s = webRespon.GetResponseStream();
                StreamReader sr = new StreamReader(s);

                //读取服务器端返回的消息 
                String sReturnString = sr.ReadLine();
                s.Close();
                sr.Close();
                if (webRespon.StatusCode.ToString() == "OK")
                {
                    returnValue = sReturnString;
                    progressBar1.Value = int.MaxValue;
                    lblState.Text = "已上传：100%";
                }
                else
                {
                    returnValue = "";
                }

            }
            catch (Exception e)
            {
                returnValue = "";
            }
            finally
            {
                fs.Close();
                r.Close();
            }

            return returnValue;
        }

        private void UpLoad_FormClosing(object sender, FormClosingEventArgs e)
        {
            t.Abort();

            this.Dispose();

            this.Close();
        }
    }
}
