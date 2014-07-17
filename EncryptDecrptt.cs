using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace WeCode1._0
{
    public static class EncryptDecrptt
    {

        //字符串转MD5
        public static string str2MD5(string str)
        {
            byte[] data = Encoding.GetEncoding("GB2312").GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] OutBytes = md5.ComputeHash(data);

            string OutString = "";
            for (int i = 0; i < OutBytes.Length; i++)
            {
                OutString += OutBytes[i].ToString("x2");
            }
            // return OutString.ToUpper();
            return OutString.ToLower();
        }

        //keyA转化为KeyD(先固定AES加密),keyD为加密秘钥
        public static string KeyA2KeyD(string KeyA)
        {
            string result = "";
            AesHelper ah1 = new AesHelper("herbertmarson1990loveoutcat");
            string keyD=ah1.EncryptoData(KeyA);
            result = keyD;
            return result;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str2encrypt">要加密的字符串</param>
        /// <param name="key">秘钥</param>
        /// <returns>加密后经过BASE64编码的字符串</returns>
        public static string EncrptyByKey(string str2encrypt, string key)
        {
            string result = "";
            AesHelper ah = new AesHelper(key);
            result = ah.EncryptoData(str2encrypt);

            return result;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str2decrypt">要解密的字符串</param>
        /// <param name="key">秘钥</param>
        /// <returns>解密并经过解码的字符串</returns>
        public static string DecrptyByKey(string str2decrypt, string key)
        {
            string result = "";
            try
            {
                AesHelper ah = new AesHelper(key);
                result = ah.DecryptoData(str2decrypt);
            }
            catch (Exception ex)
            {
                result = "";
            }

            return result;
        }

    }
}
