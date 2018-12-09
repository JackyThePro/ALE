using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALE2
{
    public class Node
    {
        int id;
        int parId;
        int childOneId = 0;
        int childTwoId = 0;
        char param;
        bool letter = false;
        bool val;

        public int Id { get { return id; } set { id = value; } }
        public int ParId { get { return parId; } set { parId = value; } }
        public int ChildOneId { get { return childOneId; } set { childOneId = value; } }
        public int ChildTwoId { get { return childTwoId; } set { childTwoId = value; } }
        public char Param { get { return param; } set { param = value; } }
        public bool Letter { get { return letter; } set { letter = value; } }
        public bool Val { get { return val; } set { val = value; } }

        public Node(int id, int parId, char param)
        {
            this.id = id;
            this.parId = parId;
            this.param = param;
        }
    }
}
