﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ALE2
{
    public class Reader
    {
        string alpha;
        List<State> state = new List<State>();
        List<Transmission> transmissions = new List<Transmission>();
        bool trans = false;
        bool word = false;
        public List<Node> listerino;

        public string GetWord(string t)
        {
            int Start = t.IndexOf(':');
            int End = t.Length;
            return t.Substring(Start + 1, End - Start - 1);
        }

        public string ReadFile(string line)
        {
            bool empty = false;
            if (line.Contains("alphabet"))
            {
                //Get the alphabet
                alpha = GetWord(line);
            }
            else if (line.Contains("states"))
            {
                string s = GetWord(line);
                List<string> sta = new List<string>();
                sta = s.Split(',').ToList<string>();
                foreach (string sa in sta)
                {
                    int i = 0;
                    if (i == 0)
                    {
                    state.Add(new State(sa, false, true));
                    }
                    else
                    {
                        state.Add(new State(sa, false, false));
                    }
                    i++;
                }
            }
            else if (line.Contains("final"))
            {
                string f = GetWord(line);
                List<string> final = new List<string>();
                final = f.Split(',').ToList<string>();
                 
                foreach (string fi in final)
                {
                    foreach (State i in state)
                    {
                        if (fi == i.Stat)
                        {
                            i.Final = true;
                        }
                    }
                }
            }
            else if (line.Contains("transitions"))
            {
                trans = true;
                if (alpha == "")
                {
                    empty = true;
                }
            }
            else if (line.Contains("dfa"))
            {
                bool b = false;
                string f = GetWord(line);
                if (f == "y")
                {
                    b = true;
                }
                if (b == CheckDFA())
                {
                    return "\tRight";
                }
                else return "\tWring";
            }
            else if (line.Contains("words"))
            {
                word = true;
            }
            else if (line.Contains("end."))
            {
                trans = false;
                word = false;
                alpha = new String(alpha.Distinct().ToArray());
                //foreach (Transmission t in transmissions)
                //{
                //    Console.WriteLine(t.In.Stat + "," + t.Value + " --> " + t.Out.Stat);
                //}
            }
            else if (trans)
            {
                line = line.Replace("-->", ",");
                if (line != "")
                {
                    List<string> s = line.Split(',').ToList();
                    Regex r = new Regex("^[a-zA-Z_]*$");
                    if (r.IsMatch(s[1]))
                    {
                        State sin = null;
                        State sout = null;
                        foreach (State ss in state)
                        {
                            if (ss.Stat == s[0])
                            {
                                sin = ss;
                            }
                            if (ss.Stat == s[2])
                            {
                                sout = ss;
                            }
                        }
                        Transmission t = new Transmission(sin, sout, s[1]);
                        transmissions.Add(t);
                        foreach (State ss in state)
                        {
                            if (ss.Stat == s[0])
                            {
                                ss.outgoing.Add(t);
                            }
                            if (ss.Stat == s[2])
                            {
                                ss.incoming.Add(t);
                            }
                        }
                        if (empty && s[1] != "_")
                        {
                            alpha += s[1];
                        }
                    }
                }
            }
            else if (word)
            {
                List<string> s = line.Split(',').ToList();
                if (CheckWord(s[0], s[1]))
                {
                    return "\tRight";
                }
                else return "\tWring";
            }
            return "";
        }

        public bool CheckDFA()
        {
            foreach (Transmission r in transmissions)
            {
                if (r.Value == "_")
                {
                    return false;
                }
            }
            bool check2 = true;
            foreach (State st in state)
            {
                foreach (char c in alpha)
                {
                    bool check = false;
                    foreach (Transmission t in st.outgoing)
                    {
                        if (c.ToString() == t.Value)
                        {
                            check = true;
                        }
                    }
                    if (!check)
                    {
                        check2 = false;
                    }
                }
                if (!check2)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckWord(string w, string yn)
        {
            bool isword;
            if (yn == "y")
            {
                isword = true;
            }
            else isword = false;

            if (RecWord(w, state[0], 0) == isword)
            {
                return true;
            }
            else return false;
        }

        public bool RecWord(string s, State st, int con)
        {
            if (con == s.Length && st.Final)
            {
                return true;
            }
            if (con == s.Length && !st.Final)
            {
                foreach (Transmission t in st.outgoing)
                {
                    if (t.Value == "_")
                    {
                        if (RecWord(s, t.Out, con))
                        {
                            return true;
                        }
                    }
                }
                    return false;
            }
            char c = s[con];
            foreach (Transmission t in st.outgoing)
            {
                if (t.Value == c.ToString())
                {
                    con++;
                    if (RecWord(s, t.Out, con))
                    {
                        return true;
                    }
                }
                else if (t.Value == "_")
                {
                    if (RecWord(s, t.Out, con))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<Node> ReadFormula(string input)
        {
            listerino = new List<Node>();
            int id = 1;
            int cid = 1;
            bool foo = true;
            foreach (char c in input)
            {
                if (foo)
                {
                    listerino.Add(new Node(id, 0, c));
                    id++;
                    foo = false;
                }
                else if (c != ',')
                {
                    Node n = new Node(id, cid, c);
                    id++;
                    if (Char.IsLetter(c))
                    {
                        n.Letter = true;
                    }
                    foreach (Node i in listerino)
                    {
                        if (i.Id == n.ParId && i.ChildOneId == 0)
                        {
                            i.ChildOneId = n.Id;
                        }
                        else if (i.Id == n.ParId && i.ChildTwoId == 0)
                        {
                            i.ChildTwoId = n.Id;
                        }
                    }
                    listerino.Add(n);
                    cid = n.Id;
                }
                else if (c == ',')
                {
                    bool b = true;
                    while (b)
                    {
                        foreach (Node i in listerino)
                        {
                            if (cid == i.Id)
                            {
                                if (i.ChildOneId == 0 || i.ChildTwoId != 0 || i.Param == '~')
                                {
                                    cid = i.ParId;
                                }
                                else
                                {
                                    b = false;
                                }
                            }
                        }
                    }
                }
            }
            return listerino;
        }


    }
}