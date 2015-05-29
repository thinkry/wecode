using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Xml;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using WeCode1._0.youdao;

namespace WeCode1._0
{
    class NoteAPI
    {
        private static string _bookPath = "";

        /// <summary>
        /// 获取wecodenote目录路径
        /// </summary>
        /// <returns>path</returns>
        private static string GetBOOKPath()
        {
            if (_bookPath != null && _bookPath != "")
            {
                return _bookPath;
            }

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
                        _bookPath = jo["path"].ToString();
                    }
                }
            }
            catch (Exception msg) //异常处理
            {
                _bookPath = "";
            }

            return _bookPath;
        }

        /// <summary>
        /// 发送消息Post方法
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="PostStr"></param>
        /// <returns></returns>
        private static string sendMessage(string strUrl, string PostStr)
        {
            try
            {
                //设置消息头
                CookieContainer objCookieContainer = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
                request.Method = "POST";
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

        private static bool CheckAccessToken()
        {
            return (ConfigurationManager.AppSettings["AccessToken"] != "");
        }

        /// <summary>
        /// 获取指定笔记
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static YouDaoNode2 GetNote(string path)
        {
            if (!CheckAccessToken())
            {
                return null;
            }

            YouDaoNode2 result = null;
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
                if (JNotebookListAll == null)
                {
                    return null;
                }

                //获取的数据装换为Json格式 此时返回的json格式的数据
                JObject o = JObject.Parse(JNotebookListAll);
                string data = o["content"].ToString();
                string createtime = o["create_time"].ToString();
                result = YouDaoNode2.FromString(path, data, createtime);
            }
            catch (Exception msg) //异常处理
            {
                Console.Write(msg);
            }

            return result;
        }

        /// <summary>
        /// 创建笔记
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static YouDaoNode2 CreateNote(string title)
        {
            if (!CheckAccessToken())
            {
                return null;
            }

            YouDaoNode2 result = null;
            try
            {
                result = new YouDaoNode2();
                result.SetTitle(title);
                result.SetUpdateTime(PubFunc.time2TotalSeconds().ToString());

                //获取AccessTokon构造URL
                StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/create.json"); //可变字符串

                NameValueCollection myCol = new NameValueCollection();
                myCol.Add("title", result.GetTitle());
                myCol.Add("content", result.ToString());
                myCol.Add("notebook", GetBOOKPath());

                string aaa = HttpPostData(url.ToString(), myCol);
                JObject jo = JObject.Parse(aaa);
                result.SetPath(jo["path"].ToString());
            }
            catch (Exception msg) //异常处理
            {

            }
            return result;
        }

        /// <summary>
        /// 修改笔记，仅修改内容
        /// </summary>
        /// <param name="note"></param>
        /// <param name="content"></param>
        /// <param name="updatetime"></param>
        /// <returns></returns>
        public static string UpdateContent(ref YouDaoNode2 note, string content, string updatetime)
        {
            if (!CheckAccessToken())
            {
                return "FAIL";
            }

            string result = "OK";
            try
            {
                note.SetContent(content);
                note.SetUpdateTime(updatetime);

                //获取AccessTokon构造URL
                StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/update.json"); //可变字符串

                NameValueCollection myCol = new NameValueCollection();
                myCol.Add("path", note.GetPath());
                myCol.Add("content", note.ToString());

                string aaa = HttpPostData(url.ToString(), myCol);
                result = "OK";
            }
            catch (Exception msg) //异常处理
            {
                result = "FAIL";
            }

            return result;
        }

        /// <summary>
        /// 修改笔记标题
        /// </summary>
        /// <param name="note"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string UpdateTitle(ref YouDaoNode2 note, string title)
        {
            if (!CheckAccessToken())
            {
                return "FAIL";
            }

            string result = "OK";
            try
            {
                note.SetUpdateTime(title);

                //获取AccessTokon构造URL
                StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/update.json"); //可变字符串

                NameValueCollection myCol = new NameValueCollection();
                myCol.Add("path", note.GetPath());
                myCol.Add("title", note.GetTitle());

                string aaa = HttpPostData(url.ToString(), myCol);
                result = "OK";
            }
            catch (Exception msg) //异常处理
            {
                result = "FAIL";
            }

            return result;
        }

        /// <summary>
        /// 删除笔记
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public static string DeleteNote(string path)
        {
            if (!CheckAccessToken())
            {
                return "FAIL";
            }

            string result = "FAIL";
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
                string JNotebookListAll = sendMessage(url.ToString(), urlPoststr.ToString());
                result = "OK";

                //获取的数据装换为Json格式 此时返回的json格式的数据
            }
            catch (Exception msg) //异常处理
            {

            }
            return result;
        }

        /// <summary>
        /// 增加附件
        /// </summary>
        /// <param name="note"></param>
        /// <param name="fileUrl"></param>
        /// <param name="fileLength"></param>
        /// <param name="fileName"></param>
        public static void AddAttachment(ref YouDaoNode2 note, string fileUrl, string fileLength, string fileName)
        {
            if (!CheckAccessToken())
            {
                return;
            }

            try
            {
                note.AddAttachment(fileUrl, fileLength, fileName);

                //获取AccessTokon构造URL
                StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/update.json"); //可变字符串

                NameValueCollection myCol = new NameValueCollection();
                myCol.Add("path", note.GetPath());
                myCol.Add("content", note.ToString());

                string aaa = HttpPostData(url.ToString(), myCol);
            }
            catch (Exception msg) //异常处理
            {

            }
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="note"></param>
        /// <param name="fileUrl"></param>
        public static void DelAttachment(ref YouDaoNode2 note, string fileUrl)
        {
            if (!CheckAccessToken())
            {
                return;
            }

            try
            {
                note.DelAttachment(fileUrl);

                //获取AccessTokon构造URL
                StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/update.json"); //可变字符串

                NameValueCollection myCol = new NameValueCollection();
                myCol.Add("path", note.GetPath());
                myCol.Add("content", note.ToString());

                string aaa = HttpPostData(url.ToString(), myCol);
            }
            catch (Exception msg) //异常处理
            {

            }
        }

        /// <summary>
        /// 重命名附件
        /// </summary>
        /// <param name="note"></param>
        /// <param name="fileUrl"></param>
        public static void RenameAttachment(ref YouDaoNode2 note, string fileUrl, string fileName)
        {
            if (!CheckAccessToken())
            {
                return;
            }

            try
            {
                note.RenameAttachment(fileUrl, fileName);

                //获取AccessTokon构造URL
                StringBuilder url = new StringBuilder("https://note.youdao.com/yws/open/note/update.json"); //可变字符串

                NameValueCollection myCol = new NameValueCollection();
                myCol.Add("path", note.GetPath());
                myCol.Add("content", note.ToString());

                string aaa = HttpPostData(url.ToString(), myCol);
            }
            catch (Exception msg) //异常处理
            {

            }
        }

        /// <summary>
        /// 修改笔记，仅修改内容
        /// 外部应该保存YouDaoNote2对象，不能每次保存都从网络上取数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content">sci文本框内的文本，未编码</param>
        /// <param name="updatetime">最后更新时间</param>
        public static string UpdateNote(string path, string content, string updatetime)
        {
            YouDaoNode2 note = GetNote(path);
            return UpdateContent(ref note, content, updatetime);
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

        //修改笔记，用于更新附件链接
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void UpdateRource(string fileUrl, string fileLength, string fileName, string path)
        {
            YouDaoNode2 note = GetNote(path);
            if (note != null)
            {
                AddAttachment(ref note, fileUrl, fileLength, fileName);
            }
        }

        //删除附件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notepath">笔记路径</param>
        /// <param name="url">附件URL</param>
        public static void DeleteRource(string notePath, string rsUrl)
        {
            YouDaoNode2 note = GetNote(notePath);
            if (note != null)
            {
                DelAttachment(ref note, rsUrl);
            }
        }

        //重命名附件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notepath">笔记路径</param>
        /// <param name="url">附件URL</param>
        public static void RenameRource(string notePath, string rsUrl, string title)
        {
            YouDaoNode2 note = GetNote(notePath);
            if (note != null)
            {
                RenameAttachment(ref note, rsUrl, title);
            }
        }

        public static string GetUserInfo()
        {
            if (!CheckAccessToken())
            {
                return "";
            }

            string result = "";

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
                Stream stream = response.GetResponseStream(); //GetResponseStream:获取应答流
                StreamReader sr = new StreamReader(stream); //从字节流中读取字符
                //从流当前位置读取到末尾并显示在WebBrower控件中 
                string content = sr.ReadToEnd();

                result = content;
                //关闭响应流
                stream.Close();
                sr.Close();
                response.Close();
            }
            catch (Exception msg) //异常处理
            {
                result = "";
            }

            return result;
        }
    }
}
