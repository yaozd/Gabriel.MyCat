using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gabriel.MyCat.Message
{
    public class Even : Message
    {
        private string name;
        private string description;

        public Even(string name, string description)
        {
            // TODO: Complete member initialization
            this.name = name;
            this.description = description;
        }
        public Stack<Transaction> Transactions=new Stack<Transaction>();
        public int Depth { get; set; }

        public string Exception { get; set; }
    }
}
