using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScanUtilityLibraryVer2.LivoxSdk2
{
    /// <summary>
    /// DLL加载器
    /// <para/>在Windows平台下通过调用系统API的方式设置DLL路径
    /// <para/>在跨平台框架下（.NET Core 3.0 或 .NET 5+ 环境）需使用NativeLibrary.SetDllImportResolver方法
    /// </summary>
    public class DllLoader
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);

        /// <summary>
        /// 是否执行过ConfigureDllPath方法
        /// </summary>
        public static bool DllDirSet { get; set; } = false;

        /// <summary>
        /// 调用系统API，根据程序架构，设置优先搜索并加载DLL的路径，程序启动时需调用此方法
        /// <para/>调用后，在DllImport中可直接使用DLL名称而无需完整路径，系统会根据设置好的路径查找DLL
        /// </summary>
        public static void ConfigureDllPath()
        {
            //TODO 跨平台框架下（.NET Core 3.0 或 .NET 5+ 环境）需使用NativeLibrary.SetDllImportResolver方法
            //参见 https://www.yuque.com/yuyuyu-lbiwy/dgpcg0/kvv385nzws7ynfcf#sPoKC
            string architectureFolder = Environment.Is64BitProcess ? "x64" : "x86";
            string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, architectureFolder);

            // 设置 DLL 搜索路径
            SetDllDirectory(dllPath);
            DllDirSet = true;
        }
    }
}
