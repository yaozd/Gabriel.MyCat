using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gabriel.MyCat
{

    #region Nested type: Context
    internal class Context
    {
        public Context()
        {
            this.IsException = false;
            this.BeginTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss ffff");
        }
        /// <summary>
        /// 事件--根
        /// </summary>
        public Message.Transaction Transaction { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Project { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 是否发生异常
        /// </summary>
        public bool IsException { get; set; }
        /// <summary>
        /// 执行的开始时间
        /// </summary>
        public string BeginTime { get; set; }
        /// <summary>
        /// 执行的结事时间
        /// </summary>
        public string EndTime { get; set; }
    }

    #endregion
}
