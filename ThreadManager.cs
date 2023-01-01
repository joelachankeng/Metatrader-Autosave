using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Metatrader_Autosaver
{
    class ThreadManager
    {
        private List<Thread> threadsList;

        public ThreadManager()
        {
            threadsList = new List<Thread>();
        }

        public bool IsAnyThreadRunning()
        {
            foreach (Thread threadItem in threadsList)
            {
                if (threadItem.IsAlive) return true;
            }
            return false;
        }

        public void CreateThread(Action threadFunction)
        {
            if (IsAnyThreadRunning()) return;
            Thread backgroundThread = new Thread(() => threadFunction());
            threadsList.Add(backgroundThread);

            backgroundThread.Start();
        }

        public void KillAll()
        {
            foreach (Thread threadItem in threadsList)
            {
                try
                {
                    if (threadItem.IsAlive) threadItem.Abort();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("KillAll Exception: " +  e.Message);
                }
            }
        }


    }
}
