using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gabriel.MyCat.Message
{
    public class Transaction : Message
    {
        private string _type;
        private string _name;

        public Transaction(string type, string name)
        {
            // TODO: Complete member initialization
            this._type = type;
            this._name = name;
        }
        public readonly Stack<Even> Evens=new Stack<Even>();
        public void NewEven(Even e)
        {
            Evens.Push(e);
        }
        public int Depth { get; set; }

    }
}
