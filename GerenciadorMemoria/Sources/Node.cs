using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorMemoria.Sources
{
    public class Node
    {
        public Node Next { get; set; }

        public Requisition Req { get; set; }

        public Node(Requisition req)
        {
            this.Req = req;
            this.Next = null;
            
        }
    }
}
