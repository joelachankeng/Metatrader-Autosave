using System;
using System.Diagnostics;
using System.Windows.Automation;

namespace Metatrader_Autosaver
{
    class AutoSaver
    {
        ThreadManager threadManager = new ThreadManager();
        AppController metaeditor = new AppController("metaeditor");
        string getProccessName;
        public AutoSaver()
        {
            AutomationFocusChangedEventHandler focusHandler = OnFocusChanged;
            Automation.AddAutomationFocusChangedEventHandler(focusHandler);
            Console.WriteLine("Metatrader Autosaver Initialized");
        }

        private void OnFocusChanged(object sender, AutomationFocusChangedEventArgs e)
        {
            AutomationElement focusedElement = sender as AutomationElement;
            if (focusedElement != null)
            {
                int processId = focusedElement.Current.ProcessId;
                using (Process process = Process.GetProcessById(processId))
                {
                    getProccessName = process.ProcessName;
                    Console.WriteLine("Focusing on " + getProccessName);
                    if (getProccessName == "Code")
                    {
                        threadManager.CreateThread(KeyWatcher);
                    }
                    else
                    {
                        threadManager.KillAll();
                    }

                }
            }
        }

        private void KeyWatcher()
        {
            while (true)
            {
                if (getProccessName != "Code") break;
                
                int S_Key = 0x53;
                int VK_LCONTROL = 0xA2;

                Console.WriteLine("Watching Keys on VSCode");
                if(KeyListener.CheckKeys(S_Key, VK_LCONTROL))
                {
                    Console.WriteLine("Keys pressed!");
                    saveMetatrader();
                }
               System.Threading.Thread.Sleep(500);
            }
        }

        void saveMetatrader()
        {
            int VK_F7 = 0x53;
            metaeditor.ListAllChildClassNames();
            //metaeditor.sendKey("Afx:002F0000:8", VK_F7);
        }

        public void Dispose(object sender, EventArgs e)
        {
            threadManager.KillAll();
            Debug.WriteLine("ThreadManager Killing All Threads!");
        }

    }
}
