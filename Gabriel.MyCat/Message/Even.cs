using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gabriel.MyCat.Message
{
    public class Even : Message
    {
        /// <summary>
        /// 创建事件
        /// </summary>
        /// <param name="name">事件名称</param>
        /// <param name="description">事件描述</param>
        public Even(string name, string description)
        {
            // TODO: Complete member initialization
            this.Name = name;
            this.Description = description;
            this.IsException = false;
            this.Time = DateTime.Now.ToString("HH:mm:ss ffff");
        }
        /// <summary>
        /// 事件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 事件描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否发生异常
        /// </summary>
        public bool IsException { get; set; }
        /// <summary>
        /// 事件异常信息
        /// </summary>
        public string Exception { get; set; }
        /// <summary>
        /// 深度--嵌套操作计数
        /// </summary>
        public int Depth { get; set; }
        /// <summary>
        /// 操作执行时间（HH:mm:ss ffff）
        /// </summary>
        public string Time { get; set; }
        
        /// <summary>
        /// 事件中嵌套的事务集合
        /// </summary>
        public Stack<Transaction> Transactions = new Stack<Transaction>();
    }
}
