using System;
using System.Diagnostics;
using System.Windows.Automation;

namespace Metatrader_Autosaver
{
    class AutoSaver
    {
        ThreadManager threadManager = new ThreadManager();

        public AutoSaver()
        {
            AutomationFocusChangedEventHandler focusHandler = OnFocusChanged;
            Automation.AddAutomationFocusChangedEventHandler(focusHandler);
        }

        private void OnFocusChanged(object sender, AutomationFocusChangedEventArgs e)
        {
            AutomationElement focusedElement = sender as AutomationElement;
            if (focusedElement != null)
            {
                int processId = focusedElement.Current.ProcessId;
                using (Process process = Process.GetProcessById(processId))
                {
                    string getProccessName = process.ProcessName;
                    Console.WriteLine("Focusing on " + getProccessName);
                    if (getProccessName == "Code")
                    {
                        threadManager.CreateThread(KeyListener);
                    }
                    else
                    {
                        threadManager.KillAll();
                    }

                }
            }
        }

        private void KeyListener()
        {
            Console.WriteLine("Test");
        }


    }
}
