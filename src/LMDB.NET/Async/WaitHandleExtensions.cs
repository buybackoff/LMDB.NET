using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LMDB.Async {

    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/hh873178(v=vs.110).aspx#WaitHandles
    /// </summary>
    public static class WaitHandleExtensions {
        public static Task WaitOneAsync(this WaitHandle waitHandle) {
            if (waitHandle == null) throw new ArgumentNullException("waitHandle");

            var tcs = new TaskCompletionSource<bool>();
            var rwh = ThreadPool.RegisterWaitForSingleObject(waitHandle,
                delegate { tcs.TrySetResult(true); }, null, -1, true);
            var t = tcs.Task;
            t.ContinueWith(_ => rwh.Unregister(null));
            return t;
        }
    }
}
