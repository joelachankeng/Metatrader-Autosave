using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Metatrader_Autosaver
{
    /**
     * Reminder on how this works:
     * This class get the first process by name.
     * It uses PostMessage to send keys to the application
     * Applications have multiple handles. Each application has a main handle. The rest are child handles.
     * Sending keys to the main handle doesn't always worked because what you're trying change is a child handle
     * The WindowHandleInfo class has the functionality to get all child handles from a process
     * The ListAllChildClassNames() method iterates over the child handles and display classname
     * The sendKey() method iterates over the child handles and send keys to the chosen handle based on classname
     * get virtual key codes @ https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
     * **/
    class AppController
    {
        private string appName;
        public string AppName
        {
            get { return appName; }
            set
            {
                appName = value;
                getAppInfo();
            }
        }

        public List<IntPtr> childWindows;
        public AppController(string processName)
        {
            AppName = processName;
        }
        private void getAppInfo()
        {
            Process[] getApp = Process.GetProcessesByName(appName);
            if (getApp.Length == 0) return;
            if (getApp[0] != null)
            {
                childWindows = new WindowHandleInfo(getApp[0].MainWindowHandle).GetAllChildHandles();
            }
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);


        public void sendKey(string windowClassName, int VK_Code)
        {
            getAppInfo();
            foreach (var childWindow in childWindows)
            {
                int nRet;
                StringBuilder className = new StringBuilder(256);
                nRet = GetClassName(childWindow, className, className.Capacity);
                if (className.ToString() == windowClassName)
                {
                    Console.WriteLine("Found window with className - sending keys");
                    PostMessage(childWindow, 0x0100, VK_Code, 0);
                }
            }
        }

        /** 
         * This is the most optimized method for finding process and sending keys
         * However this does not work for some reason. 
         * This method works for Windows 7 and older versions. 
         * This issue seems to be caused by new versions of Windows internal configuration.
         * https://stackoverflow.com/questions/5241984/findwindowex-from-user32-dll-is-returning-a-handle-of-zero-and-error-code-of-127
         * **/
        [DllImport("user32.dll", EntryPoint = "FindWindowExW")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        public void sendKey(IntPtr winHandle, string windowClassName, int VK_Code)
        {
            getAppInfo();
            Process[] getApp = Process.GetProcessesByName(appName);
            if (getApp.Length == 0) return;
            if (getApp[0] != null)
            {
                IntPtr child = FindWindowEx(getApp[0].MainWindowHandle, IntPtr.Zero, windowClassName, null);
                if (child != IntPtr.Zero)
                {
                    PostMessage(child, 0x0100, VK_Code, 0);
                }
                else
                {
                    Console.WriteLine("FindWindowExW: Can't find process child window!");
                }
            }
        }

        public void ListAllChildClassNames()
        {
            getAppInfo();
            foreach (var childWindow in childWindows)
            {
                int nRet;
                StringBuilder className = new StringBuilder(256);
                nRet = GetClassName(childWindow, className, className.Capacity);
                Console.WriteLine(childWindow + " | className = " + className);
            }
        }

        public List<string> GetAllChildClassNames()
        {
            getAppInfo();
            List<string> classNames = new List<string>();
            foreach (var childWindow in childWindows)
            {
                int nRet;
                StringBuilder className = new StringBuilder(256);
                nRet = GetClassName(childWindow, className, className.Capacity);
                classNames.Add(className.ToString());
            }
            return classNames;
        }
    }
}
