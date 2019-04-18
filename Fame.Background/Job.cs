using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;

namespace Fame.Background
{
    public static class Job
    {
        public static void Enqueue<T>(Expression<Action<T>> methodCall)
        {
            BackgroundJob.Enqueue(methodCall);
        }

        public static void Enqueue(Expression<Action> methodCall)
        {
            BackgroundJob.Enqueue(methodCall);
        }

        public static void Enqueue(Expression<Func<Task>> methodCall)
        {
            BackgroundJob.Enqueue(methodCall);
        }

        public static void Enqueue<T>(Expression<Func<T,Task>> methodCall)
        {
            BackgroundJob.Enqueue(methodCall);
        }
    }

}
