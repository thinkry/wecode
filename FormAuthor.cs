using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace WeCode1._0
{
    public partial class FormAuthor : Form
    {
        public string ReturnVal { get; protected set; }


        //获取authorization_code
        public Uri GetAuthorizationCodeUri = new Uri("https://note.youdao.com/oauth/authorize2");
        //获取access_token
        public Uri GetAccessTokenUri = new Uri("https://note.youdao.com/oauth/access2");
        //浏览器返回autorization_code
        public string autorizationCode = "";
        public string accessToken = "";

        public FormAuthor()
        {
            InitializeComponent();
        }

        private void FormAuthor_Load(object sender, EventArgs e)
        {
            //获取authorization_code构造URL
            StringBuilder url = new StringBuilder(GetAuthorizationCodeUri.ToString()); //可变字符串
            //追加组合格式字符串
            url.AppendFormat("?client_id={0}&",ConfigurationManager.AppSettings["ConsumerKey"]);
            url.AppendFormat("redirect_uri={0}&", "http://note.youdao.com/test");
            url.AppendFormat("response_type={0}&", "code");
            url.AppendFormat("state={0}", "state");
            //url.AppendFormat("scope={0}", "scope=shuo_basic_r,shuo_basic_w");
            //显示输入URL
            string Input = url.ToString();
            //将指定URL加载到WebBrowser控件中
            webBrowser1.Navigate(Input);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //获取当前文档URL
            string reUrl = this.webBrowser1.Url.ToString();

            if (!string.IsNullOrEmpty(reUrl))
            {
                //获取问号后面字符串
                string LastUrl = reUrl.Substring(reUrl.LastIndexOf("?") + 1, (reUrl.Length - reUrl.LastIndexOf("?") - 1));
                //根据参数间的&号获取参数数组
                string[] urlParam = LastUrl.Split('&');
                foreach (string s in urlParam)
                {
                    //将参数名与参数值赋值给数组 value[0]参数名称 value[1]参数值
                    string[] value = s.Split('=');
                    //MessageBox.Show("参数名称为:" + value[0] + " 参数值为:" + value[1]);
                    if (value[0] == "code")
                    {
                        autorizationCode = value[1];
                        GetAccessToken(autorizationCode);
                    }
                }
            }
        }

        public void GetAccessToken(string Code)
        {
            try
            {
                //获取AccessTokon构造URL
                StringBuilder url = new StringBuilder(GetAccessTokenUri.ToString()); //可变字符串
                //追加组合格式字符串
                url.AppendFormat("?client_id={0}&", ConfigurationManager.AppSettings["ConsumerKey"]);
                url.AppendFormat("client_secret={0}&", ConfigurationManager.AppSettings["ConsumerSecret"]);
                url.AppendFormat("grant_type={0}&", "authorization_code");
                url.AppendFormat("redirect_uri={0}&", "http://note.youdao.com/test");
                url.AppendFormat("code={0}", autorizationCode);
                //url.AppendFormat("scope={0}", "scope=shuo_basic_r,shuo_basic_w");
                //显示输入URL
                string Input = url.ToString();
                //HttpWebRequest对象实例:该类用于获取和操作HTTP请求
                var request = (HttpWebRequest)WebRequest.Create(Input); //Create:创建WebRequest对象
                //设置请求方法为GET
                //request.Headers.Add("Authorization", "Bearer " + accessToken);
                request.Method = "GET";
                //HttpWebResponse对象实例:该类用于获取和操作HTTP应答 
                var response = (HttpWebResponse)request.GetResponse(); //GetResponse:获取答复
                //构造数据流对象实例
                Stream stream = response.GetResponseStream(); //GetResponseStream:获取应答流
                StreamReader sr = new StreamReader(stream); //从字节流中读取字符
                //从流当前位置读取到末尾并显示在WebBrower控件中 
                string content = sr.ReadToEnd();
                webBrowser1.DocumentText = content;

                //获取的数据装换为Json格式 此时返回的json格式的数据
                JObject obj = JObject.Parse(content);
                accessToken = (string)obj["accessToken"];   //accesstoken

                MessageBox.Show("授权成功!");

                //写入accessToken
                SetConfiguration("AccessToken", accessToken);

                //关闭响应流
                stream.Close();
                sr.Close();
                response.Close();

                //返回成功，关闭当前窗口
                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception msg) //异常处理
            {
                MessageBox.Show(msg.Message);
            }
        }

        /// <summary>  
        /// 写入值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <param name="value"></param>  
        public static void SetConfiguration(string key, string value)
        {
            //增加的内容写在appSettings段下 <add key="RegCode" value="0"/>  
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件   
        }

        /**/
        /// <summary>  
        /// 取得appSettings里的值  
        /// </summary>  
        /// <param name="key">键</param>  
        /// <returns>值</returns>  
        public static string GetConfiguration(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
