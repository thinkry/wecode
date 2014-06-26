using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Xml;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace WeCode1._0
{
    class NoteAPI
    {
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


        /// <summary>
        /// 获取笔记的附件,转换成<div>为节点的XML，用于绑定附件列表
        /// </summary>
        /// <param name="path">笔记路径</param>
        /// <returns>xmlDocument对象</returns>
        public static XmlDocument GetResourceWithXML(string path)
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
                    string sResourceDIV = xDoc.DocumentElement.LastChild.OuterXml;
                    xDoc.LoadXml(sResourceDIV);

                }
                catch (Exception msg) //异常处理
                {

                }
            }
            return xDoc;
        }

        /// <summary>
        /// 创建笔记
        /// </summary>
        /// <param name="title">笔记标题</param>
        /// <returns>笔记路径</returns>
        public static string CreateNote(string title)
        {
            string result = "";
            string wecodeNotePath = GetBOOKPath();

            if (ConfigurationManager.AppSettings["AccessToken"] != "")
            {
                try
                {
                    //获取AccessTokon构造URL
                    StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/create.json"); //可变字符串

                    NameValueCollection myCol = new NameValueCollection();
                    myCol.Add("title", title);//如果键值red相同结果合并 rojo,rouge  
                    myCol.Add("content", "<DIV></DIV><DIV></DIV>");
                    myCol.Add("notebook", wecodeNotePath);

                    string aaa = HttpPostData(url.ToString(), myCol);
                    JObject jo = JObject.Parse(aaa);
                    result = jo["path"].ToString();
                }
                catch (Exception msg) //异常处理
                {

                }
            }

            return result;

        }

        /// <summary>
        /// 获取wecodenote目录路径
        /// </summary>
        /// <returns>path</returns>
        public static string GetBOOKPath()
        {
            string result = "";
            //wecodenote笔记本路径
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
                    if (jo["name"].ToString() == "wecodenote")
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



        //修改笔记，仅修改内容
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content">sci文本框内的文本，未编码</param>
        public static void UpdateNote(string path, string content)
        {
            if (ConfigurationManager.AppSettings["AccessToken"] != "")
            {
                try
                {
                    //编码
                    XmlDocument doc = GetNoteWithXML(path);
                    if (doc == null)
                    {
                        return;
                    }
                    string eContent = HttpUtility.HtmlEncode(content);
                    doc.DocumentElement.FirstChild.InnerXml = eContent;


                    //获取AccessTokon构造URL
                    StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/update.json"); //可变字符串

                    NameValueCollection myCol = new NameValueCollection();
                    myCol.Add("path", path);//如果键值red相同结果合并 rojo,rouge  
                    myCol.Add("content", doc.DocumentElement.InnerXml);

                    string aaa = HttpPostData(url.ToString(), myCol);



                }
                catch (Exception msg) //异常处理
                {

                }
            }
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
        /// 获取笔记的正文
        /// </summary>
        /// <param name="path">笔记路径</param>
        /// <returns>html解码之后的文本</returns>
        public static string GetNote(string path)
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

                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml("<root>" + oa + "</root>");
                    result = xDoc.DocumentElement.FirstChild.InnerXml;
                    result = HttpUtility.HtmlDecode(result);

                }
                catch (Exception msg) //异常处理
                {

                }
            }
            return result;
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


        /// <summary>
        /// 删除笔记
        /// </summary>
        /// <param name="path">笔记路径</param>
        /// <returns>删除结果 FAIL/OK</returns>
        public static string DeleteNote(string path)
        {
            string result = "FAIL";
            if (ConfigurationManager.AppSettings["AccessToken"] != "")
            {
                try
                {
                    //获取AccessTokon构造URL
                    StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/delete.json"); //可变字符串

                    //获取access_token构造POST参数
                    StringBuilder urlPoststr = new StringBuilder(""); // 可变字符串
                    //追加组合格式字符串
                    urlPoststr.AppendFormat("oauth_token={0}&", ConfigurationManager.AppSettings["AccessToken"]);
                    urlPoststr.AppendFormat("&path={0}&", path);
                    //获取POST提交数据返回内容
                    string JNotebookListAll=sendMessage(url.ToString(), urlPoststr.ToString());
                    result = "OK";
                    //获取的数据装换为Json格式 此时返回的json格式的数据



                }
                catch (Exception msg) //异常处理
                {

                }
            }
            return result;
        }



        //修改笔记，用于更新附件链接
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void UpdateRource(string fileUrl, string filelenth,string filename,string path)
        {
            if (ConfigurationManager.AppSettings["AccessToken"] != "")
            {
                try
                {
                    //编码
                    XmlDocument doc = GetNoteWithXML(path);
                    if (doc == null)
                    {
                        return;
                    }
                    XmlElement xNode = doc.CreateElement("img");
                    xNode.SetAttribute("src", "");
                    xNode.SetAttribute("alt", filename);
                    xNode.SetAttribute("title", filename);
                    xNode.SetAttribute("filename", "");
                    xNode.SetAttribute("filelength", filelenth);
                    xNode.SetAttribute("path", fileUrl);
                    xNode.SetAttribute("id", "");

                    doc.DocumentElement.LastChild.AppendChild(xNode);

                    //获取AccessTokon构造URL
                    StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/update.json"); //可变字符串

                    NameValueCollection myCol = new NameValueCollection();
                    myCol.Add("path", path);//如果键值red相同结果合并 rojo,rouge  
                    myCol.Add("content", doc.DocumentElement.InnerXml);

                    string aaa = HttpPostData(url.ToString(), myCol);



                }
                catch (Exception msg) //异常处理
                {

                }
            }
        }


        //删除附件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notepath">笔记路径</param>
        /// <param name="url">附件URL</param>
        public static void DeleteRource(string notepath, string RsUrl)
        {
            if (ConfigurationManager.AppSettings["AccessToken"] != "")
            {
                try
                {
                    //编码
                    XmlDocument doc = GetNoteWithXML(notepath);
                    if (doc == null)
                    {
                        return;
                    }
                    //查找附件节点
                    XmlNodeList xlist = doc.DocumentElement.LastChild.ChildNodes;
                    
                    foreach (XmlNode xnode in xlist)
                    {
                        if (xnode.Attributes["path"].Value == RsUrl)
                        {
                            doc.DocumentElement.LastChild.RemoveChild(xnode);
                            break;
                        }
                    }

                    //获取AccessTokon构造URL
                    StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/update.json"); //可变字符串

                    NameValueCollection myCol = new NameValueCollection();
                    myCol.Add("path", notepath);//如果键值red相同结果合并 rojo,rouge  
                    myCol.Add("content", doc.DocumentElement.InnerXml);

                    string aaa = HttpPostData(url.ToString(), myCol);



                }
                catch (Exception msg) //异常处理
                {

                }
            }
        }

        //重命名附件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notepath">笔记路径</param>
        /// <param name="url">附件URL</param>
        public static void RenameRource(string notepath, string RsUrl,string Title)
        {
            if (ConfigurationManager.AppSettings["AccessToken"] != "")
            {
                try
                {
                    //编码
                    XmlDocument doc = GetNoteWithXML(notepath);
                    if (doc == null)
                    {
                        return;
                    }
                    //查找附件节点
                    XmlNodeList xlist = doc.DocumentElement.LastChild.ChildNodes;

                    foreach (XmlNode xnode in xlist)
                    {
                        if (xnode.Attributes["path"].Value == RsUrl)
                        {
                            xnode.Attributes["title"].Value = Title;
                            break;
                        }
                    }

                    //获取AccessTokon构造URL
                    StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/update.json"); //可变字符串

                    NameValueCollection myCol = new NameValueCollection();
                    myCol.Add("path", notepath);//如果键值red相同结果合并 rojo,rouge  
                    myCol.Add("content", doc.DocumentElement.InnerXml);

                    string aaa = HttpPostData(url.ToString(), myCol);



                }
                catch (Exception msg) //异常处理
                {

                }
            }
        }

    }
}
