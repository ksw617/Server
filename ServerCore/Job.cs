using System;

namespace ServerCore
{
    public interface IJob
    {
        void Execute();
    }
    class Job : IJob
    {
        readonly Action callback;
        public Job(Action action)
        {
            callback = action;
        }

        public void Execute()
        {
            callback.Invoke();
        }
    }

    class Job<T1> : IJob
    {
        readonly Action<T1> callback;
        readonly T1 t1;
        public Job(Action<T1> action, T1 obj)
        {
            callback = action;
            t1 = obj;
        }

        public void Execute()
        {
            callback.Invoke(t1);
        }
    }

    class Job<T1, T2> : IJob
    {
        readonly Action<T1, T2> callback;
        readonly T1 t1;
        readonly T2 t2;
        public Job(Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            callback = action;
            t1 = arg1;
            t2 = arg2;
        }

        public void Execute()
        {
            callback.Invoke(t1, t2);
        }
    }

    class Job<T1, T2, T3> : IJob
    {
        readonly Action<T1, T2, T3> callback;
        readonly T1 t1;
        readonly T2 t2;
        readonly T3 t3;
        public Job(Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            callback = action;
            t1 = arg1;
            t2 = arg2;
            t3 = arg3;
        }

        public void Execute()
        {
            callback.Invoke(t1, t2, t3);
        }
    }

    class Job<T1, T2, T3, T4> : IJob
    {
        readonly Action<T1, T2, T3, T4> callback;
        readonly T1 t1;
        readonly T2 t2;
        readonly T3 t3;
        readonly T4 t4;
        public Job(Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            callback = action;
            t1 = arg1;
            t2 = arg2;
            t3 = arg3;
            t4 = arg4;
        }

        public void Execute()
        {
            callback.Invoke(t1, t2, t3, t4);
        }
    }

    class Job<T1, T2, T3, T4, T5> : IJob
    {
        readonly Action<T1, T2, T3, T4, T5> callback;
        readonly T1 t1;
        readonly T2 t2;
        readonly T3 t3;
        readonly T4 t4;
        readonly T5 t5;
        public Job(Action<T1, T2, T3, T4, T5> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            callback = action;
            t1 = arg1;
            t2 = arg2;
            t3 = arg3;
            t4 = arg4;
            t5 = arg5;
        }

        public void Execute()
        {
            callback.Invoke(t1, t2, t3, t4, t5);
        }
    }
}
