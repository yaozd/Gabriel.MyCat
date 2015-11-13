using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gabriel.MyCat.Message;
using Gabriel.MyCat.Util;

namespace Gabriel.MyCat
{
    /// <summary>
    /// 规范：
    /// 每个方法有且仅有一个cat transaction
    /// 一个 cat transaction 中包含1或多个even 
    /// </summary>
    public class MyCat
    {
        private static readonly MyCat Cat = new MyCat();

        private MyCat()
        {
        }

        public static MyCat Instance
        {
            get
            {
                return Cat;
            }
        }

        private readonly MessageManager _manager = new MessageManager();
        /// <summary>
        /// 创建监测业务事务
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public void NewTransaction(string type, string name)
        {
            var ctx = _manager.GetContext();
            var t = new Transaction(type, name);
            var rootEven = new Even("root", "root");
            if (ctx.Transaction != null)
            {
                rootEven.Depth = 10000;
                rootEven.Transactions.Push(ctx.Transaction);
            }
            var newEven = AddTransction(rootEven, t);
            ctx.Transaction = newEven.Transactions.Peek();

        }

        private Even AddTransction(Even root, Transaction node)
        {
            if (root.Depth == 0)
            {
                root.Transactions.Push(node);
                return root;
            }
            root.Transactions.Peek().Depth += 1;
            var even= root.Transactions.Peek().Evens.Pop();
            var newEven = AddTransction(even, node);
            root.Transactions.Peek().Evens.Push(newEven);
            return root;
        }
        /// <summary>
        /// 创建详细业务事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public void LogEvent(string name, string description)
        {
            var ctx = _manager.GetContext();
            var e = new Even(name, description);
            ctx.Transaction = AddEvent(ctx.Transaction, e);
        }
        private Transaction AddEvent(Transaction root, Even node)
        {
            if (root.Depth == 0)
            {
                root.NewEven(node);
                return root;
            }
            root.Evens.Peek().Depth += 1;
            var T = root.Evens.Peek().Transactions.Pop();
            var newT = AddEvent(T, node);
            root.Evens.Peek().Transactions.Push(newT);
            return root;
        }
        /// <summary>
        /// 创建业务异常事件
        /// </summary>
        /// <param name="exception"></param>
        public void ExceptionEvent(string exception)
        {
            var ctx = _manager.GetContext();
            ctx.Transaction = AddExceptionEvent(ctx.Transaction, exception);
        }

        private Transaction AddExceptionEvent(Transaction root, string exception)
        {
            if (root.Depth == 0)
            {
                var ctx = _manager.GetContext();
                var even = root.Evens.Peek();
                if (!ctx.IsException)
                {
                    ctx.IsException = true;
                    even.IsException = true;
                }
                even.Exception = exception;
                return root;
            }
            var T = root.Evens.Peek().Transactions.Pop();
            var newT = AddExceptionEvent(T, exception);
            root.Evens.Peek().Transactions.Push(newT);
            return root;
        }

        public void Complete()
        {
            var ctx = _manager.GetContext();
            if (ctx.Transaction.Depth == 0)
            {
                ctx.EndTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss ffff");
                var jsonStr = FastJsonHelper.ToJsJson(ctx);
                _manager.Dispose();
                return;
            }
            var n = 0;
            ctx.Transaction = CompleteTransaction(ctx.Transaction,ref n);
        }

        private Transaction CompleteTransaction(Transaction root,ref int num)
        {
            var depth = root.Depth - 1;
            if (depth < 0)
            {
                num = root.Evens.Count;
                return root;
            }
            var T = root.Evens.Peek().Transactions.Pop();
            var newT = CompleteTransaction(T,ref num);
            root.Depth -= 1;
            if (root.Evens.Peek().Depth < num)
            {
                throw new Exception("MyCat Exception: Transaction is not close!");
            }
            root.Evens.Peek().Depth -= num;
            root.Evens.Peek().Transactions.Push(newT);
            return root;
        }
    }
}
