using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Xml;

namespace WeCode1._0
{
    public static class PubFunc
    {
        public static string Language2Synid(string language)
        {
            string SynId = "0";
            switch (language)
            {
                case "Text":
                    SynId = "0";
                    break;
                case "C/C++":
                    SynId = "101";
                    break;
                case "HTML":
                    SynId = "102";
                    break;
                case "Pascal/Delphi":
                    SynId = "103";
                    break;
                case "Java":
                    SynId = "104";
                    break;
                case "VB/VB.NET":
                    SynId = "105";
                    break;
                case "XML":
                    SynId = "106";
                    break;
                case "C#":
                    SynId = "108";
                    break;
                case "TSQL":
                    SynId = "109";
                    break;
                case "PostgreSQL":
                    SynId = "110";
                    break;
                case "Python":
                    SynId = "111";
                    break;
                case "AS3":
                    SynId = "112";
                    break;
                case "Lua":
                    SynId = "113";
                    break;
                case "JavaScript":
                    SynId = "114";
                    break;
                case "PHP":
                    SynId = "115";
                    break;
                case "Haxe":
                    SynId = "116";
                    break;
                default:
                    break;

            }

            return SynId;
        }


        public static string Synid2Language(string SynId)
        {
            string Language = "Text";
            switch (SynId)
            {
                case "0":
                    Language = "Text";
                    break;
                case "101":
                    Language = "C/C++";
                    break;
                case "102":
                    Language = "HTML";
                    break;
                case "103":
                    Language = "Pascal/Delphi";
                    break;
                case "104":
                    Language = "Java";
                    break;
                case "105":
                    Language = "VB/VB.NET";
                    break;
                case "106":
                    Language = "XML";
                    break;
                case "107":
                    Language = "TEXT";
                    break;
                case "108":
                    Language = "C#";
                    break;
                case "109":
                    Language = "TSQL";
                    break;
                case "110":
                    Language = "PostgreSQL";
                    break;
                case "111":
                    Language = "Python";
                    break;
                case "112":
                    Language = "AS3";
                    break;
                case "113":
                    Language = "Lua";
                    break;
                case "114":
                    Language = "JavaScript";
                    break;
                case "115":
                    Language = "PHP";
                    break;
                case "116":
                    Language = "Haxe";
                    break;
                default:
                    break;

            }

            return Language;
        }


        public static string Synid2LanguageSetLang(string SynId)
        {
            string Language = "default";
            switch (SynId)
            {
                case "0":
                    Language = "default";
                    break;
                case "101":
                    Language = "cpp";
                    break;
                case "102":
                    Language = "html";
                    break;
                case "103":
                    Language = "pascal";
                    break;
                case "104":
                    Language = "java";
                    break;
                case "105":
                    Language = "vbscript";
                    break;
                case "106":
                    Language = "xml";
                    break;
                case "107":
                    Language = "default";
                    break;
                case "108":
                    Language = "cs";
                    break;
                case "109":
                    Language = "mssql";
                    break;
                case "110":
                    Language = "pgsql";
                    break;
                case "111":
                    Language = "python";
                    break;
                case "112":
                    Language = "as3";
                    break;
                case "113":
                    Language = "lua";
                    break;
                case "114":
                    Language = "js";
                    break;
                case "115":
                    Language = "html";
                    break;
                case "116":
                    Language = "haxe";
                    break;
                default:
                    break;

            }

            return Language;
        }

        //当前时间转换成秒数
        public static int time2TotalSeconds()
        {
            DateTime d1 = DateTime.Parse("1970-01-01 08:00:00");
            DateTime d2 = DateTime.Now;
            TimeSpan dt = d2 - d1;
            //相差秒数
            int Seconds = (int)dt.TotalSeconds;
            return Seconds;
        }

        //当前时间转换成秒数
        public static DateTime seconds2Time(int totalSeconds)
        {
            DateTime d1 = DateTime.Parse("1970-01-01 08:00:00");
            DateTime d2 = new DateTime();
            d2 = d1.AddSeconds(totalSeconds);
            return d2;
        }

        //通过id找到树路径
        public static string id2FullPath(string id)
        {
            string result = "";
            try
            {
                DataTable tempDt = AccessAdo.ExecuteDataSet("select title,parentid from ttree where nodeid=" + id).Tables[0];
                while (tempDt.Rows.Count>0)
                {
                    string tempID = tempDt.Rows[0]["parentid"].ToString();
                    result += tempDt.Rows[0]["title"].ToString()+"\\";
                    tempDt = AccessAdo.ExecuteDataSet("select title,parentid from ttree where nodeid=" + tempID).Tables[0];

                }

                //倒置
                string[] arrTitle = result.Split('\\');
                result="";
                for (int i = arrTitle.Length - 2; i >=0; i--)
                {
                    result += arrTitle[i] + "\\";
                }
                result = result.Substring(0, result.Length - 1);
            }
            catch (Exception ex)
            {
                result = "";
            }
            return result;
        }

        //通过nodeid找到XML路径以及最后修改时间（云笔记）
        public static string id2FullPathYoudao(string nodeid)
        {
            string result = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("TreeNodeLocal.xml");
                XmlNode xnode = doc.SelectSingleNode("//note[@path='" + nodeid + "'" + "]");

                while (xnode!=null&&xnode.Name!="wecode")
                {
                    result += xnode.Attributes["title"].Value + "\\";
                    xnode = xnode.ParentNode;
                }

                //倒置
                string[] arrTitle = result.Split('\\');
                result = "";
                for (int i = arrTitle.Length - 2; i >= 0; i--)
                {
                    result += arrTitle[i] + "\\";
                }
                result = result.Substring(0, result.Length - 1);
            }
            catch (Exception ex)
            {
                result = "";
            }
            return result;
        }

        public static string GetLatUpdateTime(string nodeid)
        {
            string result = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("TreeNodeLocal.xml");
                XmlNode xnode = doc.SelectSingleNode("//note[@path='" + nodeid + "'" + "]");
                result = xnode.Attributes["updatetime"].Value;

            }
            catch (Exception ex)
            {
                result = "";
            }
            return result;
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


        //比较版本号大小
        public static int VersionCompare(string newVer, string oldVer)
        {
            int result = 0;
            try
            {
                string[] s1 = newVer.Split('.');
                string[] s2 = oldVer.Split('.');
                if (Convert.ToInt16(s1[0]) > Convert.ToInt16(s2[0]))
                {
                    result = 1;
                }
                else if (Convert.ToInt16(s1[0]) < Convert.ToInt16(s2[0]))
                {
                    result = -1;
                }
                else if (Convert.ToInt16(s1[1]) > Convert.ToInt16(s2[1]))
                {
                    result = 1;
                }
                else if (Convert.ToInt16(s1[1]) < Convert.ToInt16(s2[1]))
                {
                    result = -1;
                }
                else if (Convert.ToInt16(s1[2]) > Convert.ToInt16(s2[2]))
                {
                    result = 1;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }


        //获取当前有道用户缓存数据库路径
        public static string GetYoudaoDBPath()
        {
            string result = "";

            XmlDocument doc = new XmlDocument();
            doc.Load("TreeNodeLocal.xml");
            XmlNode RootNode = doc.DocumentElement;
            result = "db\\youdao_" + RootNode.Attributes["UserId"].Value + ".mdb";
            return result;
        }

    }
}
