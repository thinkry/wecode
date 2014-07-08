using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Text;
using System.Xml;

namespace WeCode1._0
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            bool bRun = true;
            System.Threading.Mutex m = new System.Threading.Mutex(true, Application.ProductName, out bRun);

            if (bRun)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());//设置启动窗口为悬浮窗
                m.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("已经有一个此程序的实例在运行 ", "注意");
            }


            //bool initiallyOwned = true;
            //bool isCreated;
            //Mutex m = new Mutex(initiallyOwned, Application.ProductName, out isCreated);
            //if (!(initiallyOwned && isCreated))
            //{
            //    MessageBox.Show("抱歉，程序只能在一台机上运行一个实例！", "提示");
            //    Application.Exit();
            //}
            //else
            //{
            //    Application.EnableVisualStyles();
            //    Application.SetCompatibleTextRenderingDefault(false);
            //    Application.Run(new FormMain());
            //}  


            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormMain());
        }

        
    }
}
