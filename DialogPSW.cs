using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WeCode1._0
{
    public partial class DialogPSW : Form
    {
        public string ReturnVal { get; protected set; }

        public string strOpenType = "";

        public DialogPSW(string OpenType)
        {
            InitializeComponent();
            strOpenType = OpenType;
            if (OpenType == "0")
            { 
                //第一次设置密码
                this.Text = "第一次使用，请设置密码！";
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

            //TODO校验

            if (strOpenType == "0")
            {

                //密码转换，写入到内存以及数据库中
                string keyD = EncryptDecrptt.KeyA2KeyD(pswInput);
                string keyDmd5 = EncryptDecrptt.str2MD5(keyD);
                string keyE = EncryptDecrptt.EncrptyByKey(keyD, pswInput);

                //第一次设置密码，写入到数据库
                OleDbParameter p1 = new OleDbParameter("@KeyE", OleDbType.VarChar);
                p1.Value = keyE;
                OleDbParameter p2 = new OleDbParameter("@KeyD5", OleDbType.VarChar);
                p2.Value = keyDmd5;

                OleDbParameter[] ArrPara = new OleDbParameter[2];
                ArrPara[0] = p1;
                ArrPara[1] = p2;
                string SQL = "insert into MyKeys(KeyE,KeyD5) values(@KeyE,@KeyD5)";
                AccessAdo.ExecuteNonQuery(SQL, ArrPara);

                //临时存储keyD到内存，避免每次加密文章输入密码
                Attachment.KeyD = keyD;

                //返回keyD
                ReturnVal = keyD;
                DialogResult = DialogResult.OK;
                this.Close();
                
            }
            else if (strOpenType == "1" || strOpenType == "2"||strOpenType=="3")
            { 
                //加密,验证密码是否与数据库MD5匹配
                string d5 = AccessAdo.ExecuteScalar("select top 1 KeyD5 from MyKeys").ToString();
                //string KeyD = EncryptDecrptt.KeyA2KeyD(pswInput);
                //keyD需用keyE和keyA解密
                string KeyE=AccessAdo.ExecuteScalar("select top 1 KeyE from MyKeys").ToString();
                //keyD需从keyE和明文密码解密出来，修改了密码也不会改变
                //存储在数据库的keyD5会随着密码变化变化，目的是为了对比密码是否正确
                string KeyD = EncryptDecrptt.DecrptyByKey(KeyE, pswInput);
                string d51 = EncryptDecrptt.str2MD5(EncryptDecrptt.KeyA2KeyD(pswInput));
                if(d5!=d51)
                {
                    MessageBox.Show("密码错误！");
                    return;
                }

                //写入到内存
                Attachment.KeyD = KeyD;
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
