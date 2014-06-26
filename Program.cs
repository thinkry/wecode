using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

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

            bool initiallyOwned = true;
            bool isCreated;
            Mutex m = new Mutex(initiallyOwned, "WeCode1.0", out isCreated);
            if (!(initiallyOwned && isCreated))
            {
                MessageBox.Show("抱歉，程序只能在一台机上运行一个实例！", "提示");
                Application.Exit();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }  


            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormMain());
        }
    }
}
