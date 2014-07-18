using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml;

namespace WeCode1._0
{
    public partial class DiaChgLocalPSW : Form
    {
        public string ReturnVal { get; protected set; }

        public string strOpenType = "";

        public DiaChgLocalPSW(string OpenType)
        {
            InitializeComponent();
            strOpenType = OpenType;
            if (OpenType == "0")
            { 
                //修改本地密码
                this.Text = "修改本地笔记本加密密码！";
            }
            else if (OpenType == "1")
            {
                //修改有道云密码
                this.Text = "修改有道云加密密码！";
            }

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {

            string oldpwd = textBoxOldpsw.Text;
            string newpwd = textBoxnewpsw.Text;
            string newpwdconfirm = textBoxnewConfirm.Text;


            if (strOpenType == "0")
            {
                string d5 = AccessAdo.ExecuteScalar("select top 1 KeyD5 from MyKeys").ToString();
                //keyD需用keyE和keyA解密
                string KeyE = AccessAdo.ExecuteScalar("select top 1 KeyE from MyKeys").ToString();
                string KeyD = EncryptDecrptt.DecrptyByKey(KeyE, oldpwd);
                string d51 = EncryptDecrptt.str2MD5(EncryptDecrptt.KeyA2KeyD(oldpwd));
                if (d5 != d51)
                {
                    MessageBox.Show("原密码错误！");
                    return;
                }

                //校验密码长度以及两次输入是否一样
                if (newpwd != newpwdconfirm)
                {
                    MessageBox.Show("新密码两次不一致，请重新输入！");
                    return;
                }

                if (newpwd.Length > 16 || newpwd.Length < 6)
                {
                    MessageBox.Show("新密码长度需在6-16位之间！");
                    return;
                }

                //新的密码转换，写入到内存以及数据库中
                string keyDmd5 = EncryptDecrptt.str2MD5(EncryptDecrptt.KeyA2KeyD(newpwd));
                KeyE = EncryptDecrptt.EncrptyByKey(KeyD, newpwd);

                //写入到数据库
                OleDbParameter p1 = new OleDbParameter("@KeyE", OleDbType.VarChar);
                p1.Value = KeyE;
                OleDbParameter p2 = new OleDbParameter("@KeyD5", OleDbType.VarChar);
                p2.Value = keyDmd5;

                OleDbParameter[] ArrPara = new OleDbParameter[2];
                ArrPara[0] = p1;
                ArrPara[1] = p2;
                string SQL = "update MyKeys set KeyE=@KeyE,KeyD5=@KeyD5";
                AccessAdo.ExecuteNonQuery(SQL, ArrPara);

                //临时存储keyD到内存，避免每次加密文章输入密码
                Attachment.KeyD = KeyD;

                //返回keyD
                ReturnVal = KeyD;
                DialogResult = DialogResult.OK;
                this.Close();
                
            }
            else if (strOpenType == "1")
            {
                //加密,验证密码是否与数据库MD5匹配
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("TreeNodeLocal.xml");
                XmlNode preNode = xDoc.SelectSingleNode("//wecode");
                string d5 = preNode.Attributes["KeyD5"].Value;
                string KeyE = preNode.Attributes["KeyE"].Value; ;
                
                string d51 = EncryptDecrptt.str2MD5(EncryptDecrptt.KeyA2KeyD(oldpwd));
                if (d5 != d51)
                {
                    MessageBox.Show("原密码错误！");
                    return;
                }

                //校验密码长度以及两次输入是否一样
                if (newpwd != newpwdconfirm)
                {
                    MessageBox.Show("新密码两次不一致，请重新输入！");
                    return;
                }

                if (newpwd.Length > 16 || newpwd.Length < 6)
                {
                    MessageBox.Show("新密码长度需在6-16位之间！");
                    return;
                }

                //keyD需从keyE和明文密码解密出来，修改了密码也不会改变
                //存储在数据库的keyD5会随着密码变化变化，目的是为了对比密码是否正确
                string KeyD = EncryptDecrptt.DecrptyByKey(KeyE, oldpwd);

                //新的密码转换，写入到内存以及数据库中
                string keyDmd5 = EncryptDecrptt.str2MD5(EncryptDecrptt.KeyA2KeyD(newpwd));
                KeyE = EncryptDecrptt.EncrptyByKey(KeyD, newpwd);

                //写入新的keyd5和KeyE
                XmlNode xnode = xDoc.SelectSingleNode("//wecode");
                if (xnode != null)
                {
                    ((XmlElement)xnode).SetAttribute("KeyD5", keyDmd5);
                    ((XmlElement)xnode).SetAttribute("KeyE", KeyE);

                }
                xDoc.Save("TreeNodeLocal.xml");
                XMLAPI.XML2Yun();

                //临时存储keyD到内存，避免每次加密文章输入密码
                Attachment.KeyDYouDao = KeyD;

                //返回keyD
                ReturnVal = KeyD;
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
