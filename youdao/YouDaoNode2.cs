using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

//第2版本的有道云笔记的节点类
namespace WeCode1._0.youdao
{
    class YouDaoNode2
    {
        public const string typeBase64 = "base64";

        private string _path = ""; //该对象在有道云的path，可理解为key
        private string _title = ""; //标题
        private string _content = "";  //正文
        private Dictionary<string, string> _contentParams = new Dictionary<string, string>();  //正文的参数信息 updatetime/type等
        private List<Dictionary<string, string>> _attachments = new List<Dictionary<string, string>>();  //附件

        public YouDaoNode2()
        {
            _contentParams["type"] = typeBase64;
        }

        public string GetPath()
        {
            return _path;
        }

        public void SetPath(string value)
        {
            _path = value;
        }

        public string GetTitle()
        {
            return _title;
        }

        public void SetTitle(string value)
        {
            _title = value;
        }

        public string GetContent()
        {
            return _content;
        }

        public void SetContent(string value)
        {
            _content = value;
        }

        public string GetUpdateTime()
        {
            string result = _contentParams["updatetime"];
            return (result != null) ? result.Trim() : result;
        }

        public void SetUpdateTime(string updatetime)
        {
            _contentParams["updatetime"] = updatetime;
        }

        public List<Dictionary<string, string>> GetAttachments()
        {
            return _attachments;
        }

        public void AddAttachment(string fileUrl, string fileLength, string fileName)
        {
            Dictionary<string, string> img = new Dictionary<string, string>();
            img.Add("src", "");
            img.Add("alt", fileName);
            img.Add("title", fileName);
            img.Add("filename", "");
            img.Add("filelength", fileLength);
            img.Add("path", fileUrl);
            img.Add("id", "");
            _attachments.Add(img);
        }

        public bool DelAttachment(string fileUrl)
        {
            foreach (Dictionary<string, string> img in _attachments)
            {
                if (fileUrl == img["path"])
                {
                    _attachments.Remove(img);
                    return true;
                }
            }

            return false;
        }

        public bool RenameAttachment(string fileUrl, string fileName)
        {
            foreach (Dictionary<string, string> img in _attachments)
            {
                if (fileUrl == img["path"])
                {
                    img["title"] = fileName;
                    img["alt"] = fileName;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 解析data，返回YouDaoNode2
        /// </summary>
        /// <param name="data"></param>
        public static YouDaoNode2 FromString(string path, string data, string createTime)
        {
            YouDaoNode2 result = new YouDaoNode2();
            result.SetPath(path);

            //获取笔记的配置信息
            Dictionary<String, String> contentParams = utils.StringUtils.SplitXmlParams(data);

            //解析内容 第一个div标记
            int firstB = data.IndexOf('>') + 1;
            int firstE = data.IndexOf("</div>", firstB, StringComparison.OrdinalIgnoreCase);
            string first = data.Substring(firstB, firstE - firstB);

            if (contentParams.ContainsKey("type") && contentParams["type"].Trim() == typeBase64)
            {
                result.SetContent(Encoding.Default.GetString(Convert.FromBase64String(first)));
            }
            else
            {
                string t = HttpUtility.HtmlDecode(first);
                t = t.Replace("\n <br>", "\n");  //未采用base64编码前，有道云会把\n替换为\n <br>，所以这里要处理下
                result.SetContent(t);
            }

            //获取最后更新时间
            if (contentParams.ContainsKey("updatetime") && contentParams["updatetime"].Length > 0)
            {
                result.SetUpdateTime(contentParams["updatetime"]);
            }
            else
            {
                result.SetUpdateTime(createTime);
            }

            //复制剩下的contentParam， type和updatetime已在上面处理
            foreach (string key in contentParams.Keys)
            {
                if (key != "type" && key != "updatetime")
                {
                    result._contentParams[key] = contentParams[key];
                }
            }

            //解析附件第二个div标记
            int secondB = data.IndexOf("<div", firstE + 6, StringComparison.OrdinalIgnoreCase);
            if (secondB >= 0)  //有附件
            {
                secondB = data.IndexOf('>', secondB + 4) + 1;
                int secondE = data.LastIndexOf("</div>", StringComparison.OrdinalIgnoreCase);
                string second = data.Substring(secondB, secondE - secondB);
                result.ParseAttachments(second);
            }

            return result;
        }

        /// <summary>
        /// 拼装内容和附件组成一个字符串，用于提交到有道云
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            StringBuilder result = new StringBuilder();

            //拼装内容
            result.Append("<div");
            foreach (KeyValuePair<string, string> kv in _contentParams)
            {
                result.AppendFormat(" {0}=\"{1}\"", kv.Key, kv.Value);
            }
            result.AppendFormat(">\n{0}\n</div>\n", Convert.ToBase64String(Encoding.Default.GetBytes(_content)));

            //拼装附件
            if (_attachments.Count > 0)
            {
                result.Append("<div>\n");
                foreach (Dictionary<string, string> attachment in _attachments)
                {
                    result.Append("<img");
                    foreach (KeyValuePair<string, string> kv in attachment)
                    {
                        result.AppendFormat(" {0}=\"{1}\"", kv.Key, kv.Value);
                    }
                    result.Append(">\n");
                }
                result.Append("</div>\n");
            }

            return result.ToString();
        }

        private void ParseAttachments(string str)
        {
            str = str.Trim();
            string[] sep = new string[] { "<img" };
            string[] items = str.Split(sep, StringSplitOptions.RemoveEmptyEntries);

            char[] sep2 = new char[] { '>', '/', ' ', '\r', '\n', '\t' };
            foreach (string item in items)
            {
                string s = item.Trim(sep2);
                _attachments.Add(utils.StringUtils.SplitXmlParams(s));
            }
        }
    }
}
