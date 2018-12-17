using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALE2
{
    class PowersetConstructor
    {
        List<State> subsets = new List<State>();
        List<Transmission> transmissions = new List<Transmission>();

        public void PowersetTable (List<State> states,string alpha,out List<State> ss)
        {
            State stat = states[0];
            foreach (Transmission t in states[0].outgoing)
            {
                if (t.Value == "&")
                {
                    bool foo = states[0].Final;
                    string s = states[0].Stat;
                    EpsilonRecursion(s, stat, foo);
                    stat = new State(s, foo, true);
                }
            }
            subsets.Add(stat);
            bool sink = false;
            //this for is going through all subsets states
            for (int i = 0; i < subsets.Count; i++)
            {
                //going through every char in the alphabet
                foreach (char c in alpha)
                {
                    string newStateName = "";
                    bool firstCharInString = true;
                    bool finalS = false;
                    List<string> stateNames = subsets[i].Stat.Split(',').ToList();
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
                                            firstCharInString = false;
                                            EpsilonRecursion(newStateName, t.Out, finalS);
                                        }
                                        else if (!newStateName.Contains(t.Out.Stat))
                                        {
                                            newStateName += "," + t.Out.Stat;
                                            EpsilonRecursion(newStateName, t.Out, finalS);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (State sss in subsets)
                    {
                        if (newStateName == sss.Stat)
                        {
                            inList = true;
                            Transmission transmission = new Transmission(subsets[i], sss, c.ToString());
                            transmissions.Add(transmission);
                        }
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

        private void EpsilonRecursion(string name, State state, bool final)
        {
            foreach (Transmission t in state.outgoing)
            {
                if (t.Value == "&")
                {
                    name += "," + t.Out.Stat;
                    if (t.Out.Final)
                    {
                        final = true;
                    }
                    EpsilonRecursion(name, t.Out, final);
                }
            }
        }
    }
}
