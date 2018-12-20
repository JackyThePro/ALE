using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALE2
{
    class PowersetConstructor
    {
        public List<State> subsets = new List<State>();
        public List<Transmission> transmissions = new List<Transmission>();

        public void PowersetTable (List<State> states,string alpha,out List<State> ss)
        {
            State stat = states[0];
            foreach (Transmission t in states[0].outgoing)
            {
                if (t.Value == "&")
                {
                    bool foo;
                    string s;
                    EpsilonRecursion("", stat, states[0].Final, out s, out foo);
                    s = s.Substring(1);
                    stat = new State(s, foo, true);
                }
            }
            subsets.Add(stat);
            bool sink = false;
            //this for is going through all subsets states
            for (int i = 0; i < subsets.Count; i++)
            {
                List<string> temp = subsets[i].Stat.Split(',').ToList();
                string stt = subsets[i].Stat.ToString();
                bool fooo;
                foreach (string stateName in temp)
                {
                    //going through every state in the normal list of states
                    foreach (State st in states)
                    {
                        if (st.Stat == stateName)
                        {
                            //going through every transition that is going out of the state
                            foreach (Transmission t in st.outgoing)
                            {
                                if (t.Value == "&")
                                {
                                    EpsilonRecursion(stt, t.Out, false, out stt, out fooo);
                                }
                            }
                        }
                    }
                }
                State stWithEpsilon = new State(stt, false, false);
                //going through every char in the alphabet
                foreach (char c in alpha)
                {
                    string newStateName = "";
                    bool firstCharInString = true;
                    List<string> stateNames = stWithEpsilon.Stat.Split(',').ToList();
                    bool finalS = false;
                    bool inList = false;
                    //going through every state in the subset of states
                    foreach (string stateName in stateNames)
                    {
                        //going through every state in the normal list of states
                        foreach (State st in states)
                        {
                            if (st.Stat == stateName)
                            {
                                //going through every transition that is going out of the state
                                foreach (Transmission t in st.outgoing)
                                {
                                    if (t.Value == c.ToString())
                                    {
                                        if (t.Out.Final)
                                        {
                                            finalS = true;
                                        }
                                        if(firstCharInString)
                                        {
                                            newStateName = t.Out.Stat;
                                            EpsilonRecursion(newStateName, t.Out, finalS, out newStateName, out finalS);
                                            firstCharInString = false;
                                        }
                                        else if (!newStateName.Contains(t.Out.Stat))
                                        {
                                            newStateName += "," + t.Out.Stat;
                                            EpsilonRecursion(newStateName, t.Out, finalS, out newStateName, out finalS);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    State sss;
                    if (NameChecker(newStateName, out sss))
                    {
                        inList = true;
                        Transmission transmission = new Transmission(subsets[i], sss, c.ToString());
                        transmissions.Add(transmission);
                    }
                    if (!inList)
                    {
                        if (newStateName == "" && !sink)
                        {
                            State sta = new State("sink", finalS, false);
                            Transmission transmission = new Transmission(subsets[i], sta, c.ToString());
                            sink = true;
                            subsets.Add(sta);
                            transmissions.Add(transmission);
                        }
                        else if (newStateName == "" && sink)
                        {
                            foreach (State s in subsets)
                            {
                                if (s.Stat == "sink")
                                {
                                    Transmission transmission = new Transmission(subsets[i], s, c.ToString());
                                    transmissions.Add(transmission);
                                }
                            }
                        }
                        else
                        {
                            State sta = new State(newStateName, finalS, false);
                            Transmission transmission = new Transmission(subsets[i], sta, c.ToString());
                            subsets.Add(sta);
                            transmissions.Add(transmission);
                        }
                    }
                }
            }
            ConnectStateTransitions();
            ss = subsets.ToList();
        }

        private void ConnectStateTransitions()
        {
            subsets[0].outgoing.Clear();
            subsets[0].incoming.Clear();
            foreach (Transmission transmission in transmissions)
            {
                foreach (State state in subsets)
                {
                    if (transmission.In == state)
                    {
                        state.outgoing.Add(transmission);
                    }
                    if (transmission.Out == state)
                    {
                        state.incoming.Add(transmission);
                    }
                }
            }
        }

        private void EpsilonRecursion(string name, State state, bool final, out string statename, out bool finall)
        {
            statename = "";
            finall = false;
            if (!name.Contains(state.Stat))
            {
                name += "," + state.Stat;
            }
            if (state.Final)
            {
                final = true;
            }
            statename = name;
            finall = final;
            foreach (Transmission t in state.outgoing)
            {
                if (t.Value == "&")
                {
                    EpsilonRecursion(name, t.Out, final, out statename, out finall);
                }
            }
        }

        public bool NameChecker(string s, out State st)
        {
            bool contains;
            foreach (State name in subsets)
            {
                contains = true;
                List<string> letters = name.Stat.Split(',').ToList();
                foreach (string letter in letters)
                {
                    if (!s.Contains(letter))
                    {
                        contains = false;
                        break;
                    }
                }
                if (contains)
                {
                    st = name;
                    return true;
                }
            }
            st = new State("", false, false);
            return false;
        }
    }
}
