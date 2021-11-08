using System;
using System.Collections.Generic;


namespace ServerCore
{
    class JobQueue
    {
        readonly Queue<IJob> jobs = new Queue<IJob>();
        readonly JopTimer jopTimer = new JopTimer();
        readonly object lockObj = new object();
        
        #region Push
        public void Push(Action action) { Push(new Job(action)); }
        public void Push<T1>(Action<T1> action, T1 t1) { Push(new Job<T1>(action, t1)); }
        public void Push<T1, T2>(Action<T1, T2> action, T1 t1, T2 t2) { Push(new Job<T1, T2>(action, t1, t2)); }
        public void Push<T1, T2, T3>(Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3) { Push(new Job<T1, T2, T3>(action, t1, t2, t3)); }
        public void Push<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4) { Push(new Job<T1, T2, T3, T4>(action, t1, t2, t3, t4)); }
        public void Push<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) { Push(new Job<T1, T2, T3, T4, T5>(action, t1, t2, t3, t4, t5)); }
     
        void Push(IJob job)
        {
            lock (lockObj)
            {
                jobs.Enqueue(job);
            }
        }
        #endregion

        #region PushAfter
        public void PushAfter(int time, Action action) { PushAfter(time, new Job(action)); }
        public void PushAfter<T1>(int time, Action<T1> action, T1 t1) { PushAfter(time, new Job<T1>(action, t1)); }
        public void PushAfter<T1, T2>(int time, Action<T1, T2> action, T1 t1, T2 t2) { PushAfter(time, new Job<T1, T2>(action, t1, t2)); }
        public void PushAfter<T1, T2, T3>(int time, Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3) { PushAfter(time, new Job<T1, T2, T3>(action, t1, t2, t3)); }
        public void PushAfter<T1, T2, T3, T4>(int time, Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4) { PushAfter(time, new Job<T1, T2, T3, T4>(action, t1, t2, t3, t4)); }
        public void PushAfter<T1, T2, T3, T4, T5>(int time, Action<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) { PushAfter(time, new Job<T1, T2, T3, T4, T5>(action, t1, t2, t3, t4, t5)); }

        public void PushAfter(int tickAfter, IJob job)
        {
            jopTimer.Push(job, tickAfter);
        }

        #endregion

        public void Flush()
        {
            jopTimer.Flush();

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
