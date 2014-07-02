using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace WeCode1._0
{
    public static class PubFunc
    {
        public static string Language2Synid(string language)
        {
            string SynId = "0";
            switch (language)
            {
                case "TEXT":
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
                case "JAVA":
                    SynId = "104";
                    break;
                case "VB/VB.NET":
                    SynId = "105";
                    break;
                case "XML":
                    SynId = "106";
                    break;
                case "日记":
                    SynId = "107";
                    break;
                case "C#":
                    SynId = "108";
                    break;
                case "mssql":
                    SynId = "109";
                    break;
                case "pgsql":
                    SynId = "110";
                    break;
                case "python":
                    SynId = "111";
                    break;
                default:
                    break;

            }

            return SynId;
        }


        public static string Synid2Language(string SynId)
        {
            string Language = "TEXT";
            switch (SynId)
            {
                case "0":
                    Language = "TEXT";
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
                    Language = "JAVA";
                    break;
                case "105":
                    Language = "VB/VB.NET";
                    break;
                case "106":
                    Language = "XML";
                    break;
                case "107":
                    Language = "日记";
                    break;
                case "108":
                    Language = "C#";
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
