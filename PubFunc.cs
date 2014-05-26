using System;
using System.Collections.Generic;
using System.Text;

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
                    Language = "Pascal/Delphi";
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
    }
}
