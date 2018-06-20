using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsferixisLazyCSharp.Esferixis.Misc
{
    public static class TaskExtension
    {
        /**
         * @post Propagates the value to the specified TaskCompletionSource
         */
        public static void sendTo<T>(this Task<T> thisTask, TaskCompletionSource<T> taskCompletionSource)
        {
            Preconditions.checkNotNull(thisTask, "thisTask");
            Preconditions.checkNotNull(taskCompletionSource, "taskCompletionSource");

            thisTask.ContinueWith( task =>
            {
                if ( task.IsCompleted )
                {
                    taskCompletionSource.SetResult(task.Result);
                }
                else if ( task.IsFaulted )
                {
                    taskCompletionSource.SetException(task.Exception);
                }
                else if ( task.IsCanceled)
                {
                    taskCompletionSource.SetCanceled();
                }
            });
        }
    }
}
