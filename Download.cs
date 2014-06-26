using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WeCode1._0
{
    public partial class Download : Form
    {
        private string _URL;
        private string _Filename;

        public Download(string URL, string Filename)
        {
            InitializeComponent();
            this._URL = URL;
            this._Filename = Filename;
        }

        private void Download_Shown(object sender, EventArgs e)
        {
            lblSaveAs.Text = _Filename;
            lblURL.Text = _URL;
            lblDownLoad.Text = "0/0";
            lblSpeed.Text = "O KB/S";
            DownloadFile(_URL, _Filename, this.progressBar1);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="URL">下载文件地址</param>
        /// <param name="Filename">下载后的存放地址</param>
        /// <param name="Prog">用于显示的进度条</param>
        public void DownloadFile(string URL, string filename, System.Windows.Forms.ProgressBar prog)
        {
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;

                lblDownLoad.Text = "0字节/" + totalBytes.ToString() + "字节";

                if (prog != null)
                {
                    prog.Maximum = (int)totalBytes;
                }

                System.IO.Stream st = myrp.GetResponseStream();
                System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);

                //已上传的字节数 
                long offset = 0;

                //开始上传时间 
                DateTime startTime = DateTime.Now;

                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    System.Windows.Forms.Application.DoEvents();
                    so.Write(by, 0, osize);

                    TimeSpan span = DateTime.Now - startTime;
                    double second = span.TotalSeconds;
                    offset += osize;
                    if (second > 0.001)
                    {
                        lblSpeed.Text = " 平均速度：" + (offset / 1024 / second).ToString("0.00") + "KB/秒";
                    }

                    lblDownLoad.Text = totalDownloadedByte.ToString() + "字节/" + totalBytes.ToString() + "字节";
                    if (prog != null)
                    {
                        prog.Value = (int)totalDownloadedByte;
                    }
                    osize = st.Read(by, 0, (int)by.Length);
                }
                so.Close();
                st.Close();
                MessageBox.Show("下载完成！");
                this.Close();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

    }
}
