using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Xml;
using System.Web;
using System.IO;

namespace WeCode1._0
{
    public static class AuthorAPI
    {
        /// <summary>
        /// 判断是否授权或者授权是否有效
        /// </summary>
        /// <returns>结果</returns>
        public static string GetIsAuthor()
        {
            string result = "OK";

            if (ConfigurationManager.AppSettings["AccessToken"] == "")
            {
                result = "FAIL0";
            }
            else
            {
                //测试获取用户信息
                try
                {
                    //获取AccessTokon构造URL
                    StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/user/get.json"); //可变字符串
                    //追加组合格式字符串
                    url.AppendFormat("?oauth_token={0}&", ConfigurationManager.AppSettings["AccessToken"]);

                    string Input = url.ToString();
                    //HttpWebRequest对象实例:该类用于获取和操作HTTP请求
                    var request = (HttpWebRequest)WebRequest.Create(Input); //Create:创建WebRequest对象
                    //设置请求方法为GET
                    //request.Headers.Add("Authorization", "Bearer " + accessToken);
                    request.Method = "GET";
                    //HttpWebResponse对象实例:该类用于获取和操作HTTP应答 
                    var response = (HttpWebResponse)request.GetResponse(); //GetResponse:获取答复
                    //构造数据流对象实例
                    if (response.StatusCode.ToString() == "OK")
                    {
                        result = "OK";
                    }
                    else
                    {
                        result = "FAIL1";
                    }
                    response.Close();
                }
                catch (Exception msg) //异常处理
                {
                    result = "ERROR";
                }
            }

            return result;
        }

        //创建默认的wecode目录以及xml笔记
        public static string CreatewecodeConfig()
        {
            string result = "";
            //wecode笔记本路径
            string wecodepath = "";
            if (ConfigurationManager.AppSettings["AccessToken"] != "")
            {
                try
                {
                    //获取AccessTokon构造URL
                    StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/notebook/all.json"); //可变字符串

                    //获取access_token构造POST参数
                    StringBuilder urlPoststr = new StringBuilder(""); // 可变字符串
                    //追加组合格式字符串
                    urlPoststr.AppendFormat("oauth_token={0}&", ConfigurationManager.AppSettings["AccessToken"]);
                    //获取POST提交数据返回内容
                    string JNotebookListAll = sendMessage(url.ToString(), urlPoststr.ToString());
                    //获取的数据装换为Json格式 此时返回的json格式的数据
                    //JObject obj = JObject.Parse(AccessContent);

                    JArray ja = (JArray)JsonConvert.DeserializeObject(JNotebookListAll);
                    string isHavewecode = "false";
                    foreach (JObject jo in ja)
                    {
                        //如果已存在，返回笔记路径
                        if (jo["name"].ToString() == "wecode")
                        {
                            wecodepath = jo["path"].ToString();
                            isHavewecode = "true";
                            break;
                        }
                    }

                    if (isHavewecode == "false")
                    {
                        //不存在，先创建wecode目录，并返回路径,并且创建wecodenote目录
                        wecodepath = CreateWecodeDir();
                        CreateWecodenoteDir();

                        //创建配置文件
                        if (wecodepath != "")
                        {
                            result = CreateConfigNote(wecodepath);
                        }
                    }


                }
                catch (Exception msg) //异常处理
                {
                    result = "";
                }
            }
            return result;
        }


        //创建wecode目录
        public static string CreateWecodeDir()
        {
            string result = "";
            if (ConfigurationManager.AppSettings["AccessToken"] != "")
            {
                try
                {
                    //获取AccessTokon构造URL
                    StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/notebook/create.json"); //可变字符串

                    //获取access_token构造POST参数
                    StringBuilder urlPoststr = new StringBuilder(""); // 可变字符串
                    //追加组合格式字符串
                    urlPoststr.AppendFormat("oauth_token={0}&", ConfigurationManager.AppSettings["AccessToken"]);
                    urlPoststr.AppendFormat("name={0}", "wecode");
                    //获取POST提交数据返回内容
                    string JResult = sendMessage(url.ToString(), urlPoststr.ToString());
                    //获取的数据装换为Json格式 此时返回的json格式的数据
                    //JObject obj = JObject.Parse(AccessContent);

                    JObject obj = JObject.Parse(JResult);
                    result = obj["path"].ToString();

                }
                catch (Exception msg) //异常处理
                {
                    result = "";
                }
            }

            return result;
        }


        //创建wecodenote目录
        public static string CreateWecodenoteDir()
        {
            string result = "";
            if (ConfigurationManager.AppSettings["AccessToken"] != "")
            {
                try
                {
                    //获取AccessTokon构造URL
                    StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/notebook/create.json"); //可变字符串

                    //获取access_token构造POST参数
                    StringBuilder urlPoststr = new StringBuilder(""); // 可变字符串
                    //追加组合格式字符串
                    urlPoststr.AppendFormat("oauth_token={0}&", ConfigurationManager.AppSettings["AccessToken"]);
                    urlPoststr.AppendFormat("name={0}", "wecodenote");
                    //获取POST提交数据返回内容
                    string JResult = sendMessage(url.ToString(), urlPoststr.ToString());
                    //获取的数据装换为Json格式 此时返回的json格式的数据
                    //JObject obj = JObject.Parse(AccessContent);

                    JObject obj = JObject.Parse(JResult);
                    result = obj["path"].ToString();

                }
                catch (Exception msg) //异常处理
                {
                    result = "";
                }
            }

            return result;
        }


        //创建默认目录笔记
        public static string CreateConfigNote(string wecodebookpath)
        {
            string result = "";

            if (ConfigurationManager.AppSettings["AccessToken"] != "")
            {
                try
                {
                    //获取AccessTokon构造URL
                    StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/create.json"); //可变字符串

                    NameValueCollection myCol = new NameValueCollection();
                    myCol.Add("notebook", wecodebookpath);//如果键值red相同结果合并 rojo,rouge  
                    myCol.Add("content", getConfigtext());
                    myCol.Add("title", "wecodeconfig");


                    string JResult = HttpPostData(url.ToString(), "", myCol);


                    //获取的数据装换为Json格式 此时返回的json格式的数据
                    //JObject obj = JObject.Parse(AccessContent);

                    JObject obj = JObject.Parse(JResult);
                    result = obj["path"].ToString();

                }
                catch (Exception msg) //异常处理
                {
                    result = "";
                }
            }
            return result;

        }

        //获取本地xml内容，转码
        public static string getConfigtext()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("TreeNodeLocal.xml");
            string inTXT = doc.InnerXml;
            inTXT = HttpUtility.HtmlEncode(inTXT);
            return inTXT;

        }

        //发送消息Post方法
        public static string sendMessage(string strUrl, string PostStr)
        {
            try
            {
                //设置消息头
                CookieContainer objCookieContainer = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Referer = strUrl;
                if (objCookieContainer == null)
                    objCookieContainer = new CookieContainer();

                request.CookieContainer = objCookieContainer;
                byte[] byteData = Encoding.UTF8.GetBytes(PostStr.ToString().TrimEnd('&'));
                request.ContentLength = byteData.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(byteData, 0, byteData.Length);
                }

                //Response应答流获取数据
                string strResponse = "";
                using (HttpWebResponse res = (HttpWebResponse)request.GetResponse())
                {
                    objCookieContainer = request.CookieContainer;
                    using (Stream resStream = res.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(resStream, Encoding.UTF8)) //UTF8格式 
                        {
                            strResponse = sr.ReadToEnd();
                        }
                    }
                    // res.Close();
                }
                return strResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Read();
            }
            return null;
        }

        //post数据，ContentType = "multipart/form-data"
        private static string HttpPostData(string url, string filePath, NameValueCollection stringDict)
        {
            //字符串参数
            string Pararesult = "";

            string responseContent;
            var memStream = new MemoryStream();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            // 边界符
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            // 边界符
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            // 最后的结束符
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

            // 设置属性
            webRequest.Method = "POST";
            webRequest.Headers.Add("Authorization", "OAuth oauth_token=\"" + ConfigurationManager.AppSettings["AccessToken"] + "\"");
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;


            //写入字符串的Key
            string stringKeyHeader = "--" + boundary + "\r\n" + "Content-Disposition: form-data; name=\"{0}\"" +
                                   "\r\n\r\n{1}\r\n";
            foreach (string key in stringDict.Keys)
            {
                Pararesult += string.Format(stringKeyHeader, key, stringDict[key]);
            }
            var paraByte = Encoding.UTF8.GetBytes(Pararesult);
            memStream.Write(paraByte, 0, paraByte.Length);


            // 写入最后的结束边界符
            memStream.Write(endBoundary, 0, endBoundary.Length);

            webRequest.ContentLength = memStream.Length;

            var requestStream = webRequest.GetRequestStream();

            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

            using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(),
                                                            Encoding.GetEncoding("utf-8")))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }

            httpWebResponse.Close();
            webRequest.Abort();

            return responseContent;
        }



    }
}
