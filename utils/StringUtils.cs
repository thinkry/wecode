using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace WeCode1._0.utils
{
    class StringUtils
    {
        /// <summary>
        /// 把&lt;div upatetime="xxxx" ver="2"&gt;或upatetime="xxxx" ver="2"这一样的语句分割到Dict中
        /// </summary>
        /// <param name="xmlParams"></param>
        /// <returns></returns>
        public static Dictionary<string, string> SplitXmlParams(string xmlParams)
        {
            char[] spaces = new char[] {' ', '\t', '\r', '\n', '>'};

            //先跳过<div这个node name
            int tagB = xmlParams.IndexOf('<');
            int tagE = -1;
            if (tagB >= 0) //是<打头的params，例如<div upatetime="xxxx" ver="2"> 这种
            {
                tagB = xmlParams.IndexOfAny(spaces, tagB + 1);
                tagE = xmlParams.IndexOf('>', tagB);
            }
            else
            {
                tagB = 0;
                tagE = xmlParams.Length;
            }

            char[] seps = new char[] {' ', '\t'};
            string t = xmlParams.Substring(tagB, tagE - tagB);
            string[] items = t.Split(seps, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, string> ret = new Dictionary<string, string>();

            char[] seps2 = new char[] { '=' };
            char[] seps3 = new char[] { '"', ' ', '\n', '\r', '\t' };
            foreach (string item in items)
            {
                string[] values = item.Split(seps2, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length != 2)
                {
                    continue;
                }

                ret.Add(values[0].Trim(), values[1].Trim(seps3));
            }

            return ret;
        }
    }
}
