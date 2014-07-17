using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace WeCode1._0
{
    //对称加密与解密类,
    public class AesHelper
    {
        private SymmetricAlgorithm mobjCryptoService;
        private string Key;

        //构造函数---------------------------------------------------------------------
        public AesHelper(string sKey)
        {
            //Rijndael由比利时计算机科学家Vincent Rijmen和Joan Daemen开发的"对称加密算法"
            mobjCryptoService = new RijndaelManaged();
            //下面的KEY需要保密,可以自己定义,不要被它人得到----------------------------
            Key = sKey;
        }
        //-----------------------------------------------------------------------------

        /// 获得密钥
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;

            if (sTemp.Length > KeyLength) sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength) sTemp = sTemp.PadRight(KeyLength, ' ');

            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// 获得初始向量IV
        private byte[] GetLegalIV()
        {
            string sTemp = "123456789";
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;

            if (sTemp.Length > IVLength) sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength) sTemp = sTemp.PadRight(IVLength, ' ');

            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// 加密方法
        public string EncryptoData(string Source)
        {
            //类UTF8Encoding使用 8 位形式的 UCS 转换格式 (UTF-8) 对 Unicode 字符进行编码
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }

        /// 解密方法
        public string DecryptoData(string Source)
        {
            byte[] bytIn = Convert.FromBase64String(Source);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
