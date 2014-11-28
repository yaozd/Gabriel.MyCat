using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gabriel.MyCat.Util
{
    public class  CatThreadLocal<T>
    {
        private readonly Hashtable _mValues = new Hashtable(1024);

        public T Value
        {
            get
            {
                int threadId = ManagedThreadId();
                object value = _mValues[threadId];

                return (T)value;
            }
            set
            {
                int threadId = ManagedThreadId();

                //Logger.TestWrite("current thread id {0};context id {1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentContext.ContextID);

                _mValues[threadId] = value;
            }
        }

        private static int ManagedThreadId()
        {
            //return Thread.CurrentContext.ContextID;
            return Thread.CurrentThread.ManagedThreadId;
        }

        public void Dispose()
        {
            int threadId = ManagedThreadId();
            if (_mValues.ContainsKey(threadId))
            {
                _mValues.Remove(threadId);
            }
        }
    }
}
