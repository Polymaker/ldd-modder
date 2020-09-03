using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LDDModder.Utilities
{
    public static class TaskHelper
    {

        public static T ExecuteAsyncTask<T>(Task<T> task)
        {
            T result = default;
            var newTask = Task.Factory.StartNew(() =>
            {
                result = task.Result;
            });
            newTask.Wait();
            return result;
        }
    }
}
