﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALE2
{
    public class Transmission
    {
        State sin;
        State sout;
        string value;
        string sremove = "";
        string sadd = "";

        //coming from this state
        public State In { get { return sin; } set { sin = value; } }
        //going to this state
        public State Out { get { return sout; } set { sout = value; } }
        public string Value { get { return value; } set { this.value = value; } }
        public string Sremove { get { return sremove; } set { sremove = value; } }
        public string Sadd { get { return sadd; } set { sadd = value; } }

        public Transmission(State i, State o, string v)
        {
            Value = v;
            In = i;
            Out = o;
        }
    }
}
