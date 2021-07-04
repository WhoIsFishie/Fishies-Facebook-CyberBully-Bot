using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Girlfriend
{
    class Program
    {

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, int type);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            //hides the console
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);

            //shows a pop up msg saying ily
            MessageBox((IntPtr)0, "I Love You! <3", "Kuruzu's Girlfriend", 0);
        }
    }
}
