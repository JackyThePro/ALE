using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALE2
{
    public class State
    {
        string state;
        bool final;
        bool init;
        public List<Transmission> outgoing = new List<Transmission>();
        public List<Transmission> incoming = new List<Transmission>();

        public string Stat { get { return state; } set { state = value; } }
        public bool Final { get { return final; } set { this.final = value; } }
        public bool Init { get { return init; } set { this.init = value; } }

        public State(string s, bool f,bool i)
        {
            Final = f;
            Stat = s;
            init = i;
        }
    }
}
