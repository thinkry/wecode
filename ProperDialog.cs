﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WeCode1._0
{
    public partial class ProperDialog : Form
    {

        public string[] ReturnVal { get; protected set; }

        public ProperDialog(string OpenType,string Title,string Language)
        {
            InitializeComponent();
            this.comboBoxLanguageType.Text = Language;

            switch (OpenType)
            {
                case "0":
                    this.Text = "新建目录";
                    this.labelTitle.Text="输入目录标题：";
                    this.labelTip.Text = "新建目录的默认语法高亮和父目录相同";
                    this.checkBoxIsOnRootCreate.Text = "在顶层新建目录";
                    break;
                case "1":
                    this.Text="新建文章";
                    this.labelTitle.Text="输入文章标题：";
                    this.labelTip.Text = "新建文章的默认语法高亮和父目录相同";
                    this.checkBoxIsOnRootCreate.Text = "在顶层新建文章";
                    break;
                case "2":
                    this.Text = "目录属性";
                    this.labelTitle.Text="输入目录标题：";
                    this.labelTip.Text = "目录属性";
                    this.textBoxTitle.Text = Title;
                    this.checkBoxIsOnRootCreate.Visible = false;
                    break;
                case "3":
                    this.Text = "文章属性";
                    this.labelTitle.Text="输入文章标题：";
                    this.labelTip.Text = "文章属性";
                    this.textBoxTitle.Text = Title;
                    this.checkBoxIsOnRootCreate.Visible = false;
                    break;
                default:
                    break;

            }



        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxTitle.Text))
            {
                MessageBox.Show("标题不能为空！");
                return;
            }
            else if (textBoxTitle.Text.Length > 50)
            {
                MessageBox.Show("标题长度不能超过50字节！");
                return;
            }
            ReturnVal = new string[3];
            ReturnVal[0] = this.textBoxTitle.Text;
            ReturnVal[1] = this.comboBoxLanguageType.Text;
            ReturnVal[2]=this.checkBoxIsOnRootCreate.Checked.ToString();

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
