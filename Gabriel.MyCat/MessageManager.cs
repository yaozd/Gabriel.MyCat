using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gabriel.MyCat.Util;

namespace Gabriel.MyCat
{
    public class MessageManager
    {
        private readonly CatThreadLocal<Context> _mContext = new CatThreadLocal<Context>();
        internal Context GetContext()
        {
            return _mContext.Value ?? (_mContext.Value = new Context());
        }

        internal void Dispose()
        {
          _mContext.Dispose();
        }
    }
}
