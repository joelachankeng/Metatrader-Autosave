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

        string previousProccessName;


        public AutoSaver()
        {
            //AutomationFocusChangedEventHandler focusHandler = OnFocusChanged;
            //Automation.AddAutomationFocusChangedEventHandler(focusHandler);
            Console.WriteLine("Metatrader Autosaver Initialized");
            do
            {
                Console.WriteLine("Foreground window is " + AppController.GetForegroundProcess().ProcessName);
                System.Threading.Thread.Sleep(1000);
                getProccessName = AppController.GetForegroundProcess().ProcessName;
                if (previousProccessName == getProccessName) continue;
                createThread(AppController.GetForegroundProcess().Id);
            } while (1 == 1);


        }

        private void OnFocusChanged(object sender, AutomationFocusChangedEventArgs e)
        {
            AutomationElement focusedElement = sender as AutomationElement;
            if (focusedElement != null)
            {
                // need to be wrapped with try/catch due to error - System.Windows.Automation.ElementNotAvailableException: 'ElementNotAvailable'
                // require further investigation - happens when open incognito window on chrome
                int processId = focusedElement.Current.ProcessId;
                createThread(processId);
            }
        }

        void createThread(int processId)
        {
            try
            {
                using (Process process = Process.GetProcessById(processId))
                {
                    getProccessName = process.ProcessName;
                    previousProccessName = getProccessName;
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
            catch
            {

            }
        } 

        private void KeyWatcher()
        {
            while (true)
            {
                if (getProccessName != "Code") break;
                int VK_LSHIFT = 0xA0;
                int S_Key = 0x53;
                int VK_LCONTROL = 0xA2;

                Console.WriteLine("Watching Keys on VSCode");
                if(KeyListener.CheckKeys(S_Key, VK_LCONTROL) || KeyListener.CheckKey(VK_LSHIFT))
                {
                    Console.WriteLine("Keys pressed!");
                    saveMetatrader();
                }
               System.Threading.Thread.Sleep(500);
            }
        }

        void saveMetatrader()
        {
            int VK_F7 = 0x76;
            //metaeditor.ListAllChildClassNames();

            foreach(string className in metaeditor.GetAllChildClassNames())
            {
                if (className.StartsWith("Afx:") && className.Length == 14)
                {
                    metaeditor.sendKey(className, VK_F7);
                }
            }
            
        }

        public void Dispose(object sender, EventArgs e)
        {
            Automation.RemoveAllEventHandlers();
            threadManager.KillAll();
            Debug.WriteLine("ThreadManager Killing All Threads!");
        }

    }
}
