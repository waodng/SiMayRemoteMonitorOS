using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SiMay.Service.Core
{
    public class AwaitTaskSequence : ConcurrentQueue<ApplicationRemoteServiceBase>
    {
        public ApplicationRemoteServiceBase Dequeue()
        {
            base.TryDequeue(out var service);
            return service;
        }
    }
}
