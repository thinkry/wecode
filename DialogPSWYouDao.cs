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
    public partial class DialogPSWYouDao : Form
    {
        public string ReturnVal { get; protected set; }

        public string strOpenType = "";

        public DialogPSWYouDao(string OpenType)
        {
            InitializeComponent();
            strOpenType = OpenType;
            if (OpenType == "0")
            { 
                //第一次设置密码
                this.Text = "第一次使用有道云加密，请设置密码！";
            }
            else if (OpenType == "1")
            {
                //加密
                this.Text = "加密";
                labelConfirm.Visible = false;
                textBoxConfirm.Visible = false;
            }
            else if (OpenType == "2")
            {
                //取消加密
                this.Text = "解密";
                labelConfirm.Visible = false;
                textBoxConfirm.Visible = false;
            }
            else
            { 
                //查看时输入密码
                this.Text = "请输入密码";
                labelConfirm.Visible = false;
                textBoxConfirm.Visible = false;
            }

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {

            string pswInput = textBoxInput.Text;
            string pswConfirm = textBoxConfirm.Text;


            if (strOpenType == "0")
            {

                //校验密码长度以及两次输入是否一样
                if (pswInput != pswConfirm)
                {
                    MessageBox.Show("两次密码不一致，请重新输入！");
                    return;
                }

                if (pswInput.Length > 16 || pswInput.Length < 6)
                {
                    MessageBox.Show("密码长度需在6-16位之间！");
                    return;
                }

                //密码转换，写入到内存以及数据库中
                string keyD = EncryptDecrptt.KeyA2KeyD(pswInput);
                string keyDmd5 = EncryptDecrptt.str2MD5(keyD);
                string keyE = EncryptDecrptt.EncrptyByKey(keyD, pswInput);

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("TreeNodeLocal.xml");
                XmlNode preNode = xDoc.SelectSingleNode("//wecode");
                if (preNode != null)
                {
                    ((XmlElement)preNode).SetAttribute("KeyD5",keyDmd5);
                    ((XmlElement)preNode).SetAttribute("KeyE", keyE);

                }
                xDoc.Save("TreeNodeLocal.xml");
                XMLAPI.XML2Yun();

                //临时存储keyD到内存，避免每次加密文章输入密码
                Attachment.KeyDYouDao = keyD;

                //返回keyD
                ReturnVal = keyD;
                DialogResult = DialogResult.OK;
                this.Close();
                
            }
            else if (strOpenType == "1" || strOpenType == "2"||strOpenType=="3")
            { 
                //加密,验证密码是否与数据库MD5匹配
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("TreeNodeLocal.xml");
                XmlNode preNode = xDoc.SelectSingleNode("//wecode");
                string d5 = preNode.Attributes["KeyD5"].Value;
                //string KeyD = EncryptDecrptt.KeyA2KeyD(pswInput);
                //keyD需用keyE和keyA解密
                string KeyE = preNode.Attributes["KeyE"].Value; ;
                
                string d51 = EncryptDecrptt.str2MD5(EncryptDecrptt.KeyA2KeyD(pswInput));
                if(d5!=d51)
                {
                    MessageBox.Show("密码错误！");
                    return;
                }

                //keyD需从keyE和明文密码解密出来，修改了密码也不会改变
                //存储在数据库的keyD5会随着密码变化变化，目的是为了对比密码是否正确
                string KeyD = EncryptDecrptt.DecrptyByKey(KeyE, pswInput);

                //写入到内存
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

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            this.textBoxInput.PasswordChar = '●';
        }
    }
}
