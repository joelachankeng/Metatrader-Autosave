using System;
using System.Diagnostics;
using System.Windows.Automation;

namespace Metatrader_Autosaver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Metatrader Autosaver";
            Console.WriteLine("Metatrader Autosaver Initialized");

            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                Console.WriteLine("Metatrader Autosaver already running. Only one instance of this application is allowed");
                return;
            }

            AutomationFocusChangedEventHandler focusHandler = OnFocusChanged;
            Automation.AddAutomationFocusChangedEventHandler(focusHandler);

            Console.Read();

        }


        static void OnFocusChanged(object sender, AutomationFocusChangedEventArgs e)
        {
            AutomationElement focusedElement = sender as AutomationElement;
            if (focusedElement != null)
            {
                int processId = focusedElement.Current.ProcessId;
                using (Process process = Process.GetProcessById(processId))
                {
                    Console.WriteLine("Focusing on " + process.ProcessName);
                }
            }
        }


    }
}
