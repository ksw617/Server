using System;
using System.Collections.Generic;


namespace ServerCore
{
    class JobQueue
    {
        Queue<IJob> jobs = new Queue<IJob>();
        object lockObj = new object();

        #region Push
        public void Push(Action action) { Push(new Job(action)); }
        public void Push<T1>(Action<T1> action, T1 t1) { Push(new Job<T1>(action, t1)); }
        public void Push<T1, T2>(Action<T1, T2> action, T1 t1, T2 t2) { Push(new Job<T1, T2>(action, t1, t2)); }
        public void Push<T1, T2, T3>(Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3) { Push(new Job<T1, T2, T3>(action, t1, t2, t3)); }
        public void Push<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4) { Push(new Job<T1, T2, T3, T4>(action, t1, t2, t3, t4)); }
        public void Push<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) { Push(new Job<T1, T2, T3, T4, T5>(action, t1, t2, t3, t4, t5)); }
        #endregion

        void Push(IJob job)
        {
            lock (lockObj)
            {
                jobs.Enqueue(job);
            }
        }

        public void Flush()
        {

            while (true)
            {
                IJob job = Pop();
                if (job == null)
                    return;

                job.Execute();
            }
        }

        IJob Pop()
        {
            lock (lockObj)
            {
                if (jobs.Count == 0)
                {
                    return null;
                }
                return jobs.Dequeue();
            }
        }
    }

}
