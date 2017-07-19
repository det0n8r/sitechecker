using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace XDSiteChecker
{
    public class WorkerThreadPool
    {
        public WorkerThreadPool(int n, ManualResetEvent doneEvent)
        {
            _n = n;
            _doneEvent = doneEvent;
        }

        // Wrapper method for use with thread pool.
        public void ThreadPoolCallback(Object threadContext)
        {
            int threadIndex = (int)threadContext;
            //Console.WriteLine("thread {0} started...", threadIndex);
            _fibOfN = Calculate(_n);
            //Console.WriteLine("thread {0} result calculated...", threadIndex);
            _doneEvent.Set();
        }

        // Recursive method that calculates the Nth WorkerThreadPool number.
        public int Calculate(int n)
        {
            if (n <= 1)
            {
                return n;
            }

            return Calculate(n - 1) + Calculate(n - 2);
        }

        public int N { get { return _n; } }
        private int _n;

        public int FibOfN { get { return _fibOfN; } }
        private int _fibOfN;

        private ManualResetEvent _doneEvent;
    }

    public class ThreadPoolExample
    {
        static void WorkerThreadPoolMain()
        {
            const int WorkerThreadPoolCalculations = 10;

            // One event is used for each WorkerThreadPool object
            ManualResetEvent[] doneEvents = new ManualResetEvent[WorkerThreadPoolCalculations];
            WorkerThreadPool[] fibArray = new WorkerThreadPool[WorkerThreadPoolCalculations];
            Random r = new Random();

            // Configure and launch threads using ThreadPool:
            //Console.WriteLine("launching {0} tasks...", WorkerThreadPoolCalculations);
            for (int i = 0; i < WorkerThreadPoolCalculations; i++)
            {
                doneEvents[i] = new ManualResetEvent(false);
                WorkerThreadPool f = new WorkerThreadPool(r.Next(20,40), doneEvents[i]);
                fibArray[i] = f;
                ThreadPool.QueueUserWorkItem(f.ThreadPoolCallback, i);
            }

            // Wait for all threads in pool to calculation...
            WaitHandle.WaitAll(doneEvents);
            //Console.WriteLine("All calculations are complete.");

            // Display the results...
            for (int i= 0; i<WorkerThreadPoolCalculations; i++)
            {
                WorkerThreadPool f = fibArray[i];
                //Console.WriteLine("WorkerThreadPool({0}) = {1}", f.N, f.FibOfN);
            }
        }
   }
}

