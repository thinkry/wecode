using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;
using System.Web;
using System.Collections.Specialized;

namespace WeCode1._0
{
    public static class XMLAPI
    {

        /// <summary>
        /// 本地XML同步到有道云笔记上
        /// </summary>
        public static void XML2Yun()
        {
            string sPath = "";
            try
            {
                //获取AccessTokon构造URL
                StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/notebook/list.json"); //可变字符串

                //获取access_token构造POST参数
                StringBuilder urlPoststr = new StringBuilder(""); // 可变字符串
                //追加组合格式字符串
                urlPoststr.AppendFormat("oauth_token={0}&", ConfigurationManager.AppSettings["AccessToken"]);
                urlPoststr.AppendFormat("&notebook={0}&", getWecodeDirPath());
                //获取POST提交数据返回内容
                string JNotebookListAll = sendMessage(url.ToString(), urlPoststr.ToString());
                //获取的数据装换为Json格式 此时返回的json格式的数据

                JArray ja = (JArray)JsonConvert.DeserializeObject(JNotebookListAll);
                //默认只有一篇笔记，因此直接返回路径
                JValue jv = (JValue)ja[0];
                sPath = jv.ToString();


                XmlDocument doc = new XmlDocument();
                doc.Load("TreeNodeLocal.xml");
                string content = doc.InnerXml;
                UpdateXML(sPath, content);

            }
            catch (Exception msg) //异常处理
            {

            }
        }


        //修改XML
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content">XML</param>
        public static void UpdateXML(string path, string content)
        {
            if (ConfigurationManager.AppSettings["AccessToken"] != "")
            {
                try
                {
                    //编码
                    string eContent = HttpUtility.HtmlEncode(content);


                    //获取AccessTokon构造URL
                    StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/update.json"); //可变字符串

                    NameValueCollection myCol = new NameValueCollection();
                    myCol.Add("path", path);//如果键值red相同结果合并 rojo,rouge  
                    myCol.Add("content", eContent);

                    string aaa = HttpPostData(url.ToString(), myCol);



                }
                catch (Exception msg) //异常处理
                {

                }
            }
        }


        


        /// <summary>
        /// 获取远端XML
        /// </summary>
        /// <param name="path">笔记路径</param>
        /// <returns>html解码之后的文本</returns>
        public static string GetXML(string path)
        {
            string result = "";
            if (ConfigurationManager.AppSettings["AccessToken"] != "")
            {
                try
                {
                    //获取AccessTokon构造URL
                    StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/get.json"); //可变字符串

                    //获取access_token构造POST参数
                    StringBuilder urlPoststr = new StringBuilder(""); // 可变字符串
                    //追加组合格式字符串
                    urlPoststr.AppendFormat("oauth_token={0}&", ConfigurationManager.AppSettings["AccessToken"]);
                    urlPoststr.AppendFormat("&path={0}&", path);
                    //获取POST提交数据返回内容
                    string JNotebookListAll = sendMessage(url.ToString(), urlPoststr.ToString());

                    //获取的数据装换为Json格式 此时返回的json格式的数据

                    JObject o = JObject.Parse(JNotebookListAll);
                    string oa = o["content"].ToString();

                    result = HttpUtility.HtmlDecode(oa);

                }
                catch (Exception msg) //异常处理
                {

                }
            }
            return result;
        }


        /// <summary>
        /// 获取笔记的content,转换成root为节点的XML
        /// </summary>
        /// <param name="path">笔记路径</param>
        /// <returns>xmlDocument对象</returns>
        public static XmlDocument GetNoteWithXML(string path)
        {
            XmlDocument xDoc = new XmlDocument();
            if (ConfigurationManager.AppSettings["AccessToken"] != "")
            {
                try
                {
                    //获取AccessTokon构造URL
                    StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/get.json"); //可变字符串

                    //获取access_token构造POST参数
                    StringBuilder urlPoststr = new StringBuilder(""); // 可变字符串
                    //追加组合格式字符串
                    urlPoststr.AppendFormat("oauth_token={0}&", ConfigurationManager.AppSettings["AccessToken"]);
                    urlPoststr.AppendFormat("&path={0}&", path);
                    //获取POST提交数据返回内容
                    string JNotebookListAll = sendMessage(url.ToString(), urlPoststr.ToString());

                    //获取的数据装换为Json格式 此时返回的json格式的数据

                    JObject o = JObject.Parse(JNotebookListAll);
                    string oa = o["content"].ToString();

                    xDoc.LoadXml("<root>" + oa + "</root>");

                }
                catch (Exception msg) //异常处理
                {

                }
            }
            return xDoc;
        }



        /// <summary>
        /// 有道云笔记的XML同步到本地XML
        /// </summary>
        public static void Yun2XML()
        {
            string result = "";
            string sPath = "";
            //先列出所有笔记
            try
            {
                //获取AccessTokon构造URL
                StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/notebook/list.json"); //可变字符串

                //获取access_token构造POST参数
                StringBuilder urlPoststr = new StringBuilder(""); // 可变字符串
                //追加组合格式字符串
                urlPoststr.AppendFormat("oauth_token={0}&", ConfigurationManager.AppSettings["AccessToken"]);
                urlPoststr.AppendFormat("&notebook={0}&", getWecodeDirPath());
                //获取POST提交数据返回内容
                string JNotebookListAll = sendMessage(url.ToString(), urlPoststr.ToString());
                //获取的数据装换为Json格式 此时返回的json格式的数据

                JArray ja = (JArray)JsonConvert.DeserializeObject(JNotebookListAll);
                //默认只有一篇笔记，因此直接返回路径
                JValue jv = (JValue)ja[0];
                sPath = jv.ToString();

                string XMLContent = GetXML(sPath);
                result = XMLContent;

                //写入到本地
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                doc.Save("TreeNodeLocal.xml");

                //拉取完了检查升级
                CheckDb.CheckUpdateXML();

            }
            catch (Exception msg) //异常处理
            {
                result = "";
            }

        }

        //获取wecode目录路径（仅存放目录配置一篇笔记）
        public static string getWecodeDirPath()
        {
            string result = "";
            //wecode笔记本路径
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
                foreach (JObject jo in ja)
                {
                    if (jo["name"].ToString() == "wecode")
                    {
                        result = jo["path"].ToString();
                    }
                }
            }
            catch (Exception msg) //异常处理
            {
                result = "";
            }
            return result;
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
        private static string HttpPostData(string url, NameValueCollection stringDict)
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
