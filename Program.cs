using System;
using System.Diagnostics;

namespace Metatrader_Autosaver
{
    class Program
    {
        ThreadManager threadManager = new ThreadManager();

        static void Main(string[] args)
        {
            Console.Title = "Metatrader Autosaver";
            Console.WriteLine("Metatrader Autosaver Initialized");

            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                Console.WriteLine("Metatrader Autosaver already running. Only one instance of this application is allowed");
                return;
            }

            new AutoSaver();

            Console.Read();
        }
    }
}
