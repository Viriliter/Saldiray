using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FornoxGUI.ExternalScripts
{
    public class TaskScheduler
    {
        private Task previous = Task.FromResult(false);
        private object key = new object();

        public Task<T> Enqueue<T>(Func<Task<T>> taskGenerator)
        {
            lock (key)
            {
                var next = AddContinuation(taskGenerator);
                previous = next;
                return next;
            }
        }

        public Task<T> Enqueue<T>(Func<T> function)
        {
            return Enqueue(() => Task.Run(function));
        }

        public Task Enqueue(Func<Task> taskGenerator)
        {
            lock (key)
            {
                var next = AddContinuation(taskGenerator);
                previous = next;
                return next;
            }
        }

        public Task Enqueue(Action action)
        {
            return Enqueue(() => Task.Run(action));
        }

        private async Task<T> AddContinuation<T>(Func<Task<T>> taskGenerator)
        {
            await previous
                .ContinueWith(t => { }); //ignore errors of previous task here
            return await taskGenerator();
        }

        private async Task AddContinuation(Func<Task> taskGenerator)
        {
            await previous
                .ContinueWith(t => { }); //ignore errors of previous task here
            await taskGenerator();
        }
    }
}
