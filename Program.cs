using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;

namespace WeCode1._0
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>

        static System.Threading.Mutex m;
        public static EventWaitHandle ProgramStarted; 
        //当前版本
        public static string sVer="1.1.2";


        private static void  CheckConfig()
        {
            //更新版本号
            string ConfigVer = PubFunc.GetConfiguration("Version");
            if (sVer != ConfigVer)
            {
                PubFunc.SetConfiguration("Version", sVer);
            }

        }


        [STAThread]
        static void Main()
        {
            //初始化上传信息
            if (PubFunc.GetConfiguration("UUID") == "")
            {
                string UUID = System.Guid.NewGuid().ToString();
                PubFunc.SetConfiguration("UUID", UUID);
            }


            //检查当前的appconfig
            CheckConfig();


            bool bRun = true;
            m = new System.Threading.Mutex(true, Application.ProductName, out bRun);

            bool createNew; 
            ProgramStarted = new EventWaitHandle(false, EventResetMode.AutoReset, "MyStartEvent", out createNew); 

            if (bRun)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());//设置启动窗口为悬浮窗
            }
            else
            {
                //MessageBox.Show("已经有一个此程序的实例在运行 ", "注意");
                //1.2 已经有一个实例在运行
                ProgramStarted.Set();
                return;
            }

        }


        //2.在进程中查找是否已经有实例在运行
        #region  确保程序只运行一个实例
        private static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //遍历与当前进程名称相同的进程列表 
            foreach (Process process in processes)
            {
                //如果实例已经存在则忽略当前进程 
                if (process.Id != current.Id)
                {
                        return process;
                }
            }
            return null;
        }

        //3.已经有了就把它激活，并将其窗口放置最前端
        private static void HandleRunningInstance(Process instance)
        {
            MessageBox.Show("已经在运行！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ShowWindowAsync(instance.MainWindowHandle, 1);  //调用api函数，正常显示窗口
            SetForegroundWindow(instance.MainWindowHandle); //将窗口放置最前端
        }
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(System.IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(System.IntPtr hWnd);
        #endregion

        
    }
}
