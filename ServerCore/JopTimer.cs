using System;


namespace ServerCore
{
    struct TimeJob : IComparable<TimeJob>
    {
        public int excutantTime;
        public IJob job;

        public int CompareTo(TimeJob other)
        {
            return other.excutantTime - excutantTime;
        }
    }

    public class JopTimer
    {
        PriorityQueue<TimeJob> timeJobs = new PriorityQueue<TimeJob>();
        object lockObj = new object();

        public void Push(IJob job, int tickAfter = 0)
        {
            TimeJob timeJob;
            timeJob.excutantTime = Environment.TickCount + tickAfter;
            timeJob.job = job;

            lock (lockObj)
            {
                timeJobs.Push(timeJob);
            }
        }

        public void Flush()
        {
            while (true)
            {
                int now = Environment.TickCount;

                TimeJob timeJob;

                lock (lockObj)
                {
                    if (timeJobs.Count == 0)
                    {
                        break;
                    }

                    timeJob = timeJobs.Peek();
                    if (timeJob.excutantTime > now)
                    {
                        break;
                    }

                    timeJobs.Pop();
                }

                timeJob.job.Execute();

            }
        }
    }
}
