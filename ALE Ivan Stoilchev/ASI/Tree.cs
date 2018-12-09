using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI
{
    public class Tree
    {
        List<Node> listerino;
        Calculate cal = new Calculate();
        
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

        public List<char> GetLetters()
        {
            List<char> t = new List<char>();
            foreach (Node i in listerino)
            {
                if (i.Letter && !t.Contains(i.Param))
                {
                    t.Add(i.Param);
                }
            }
            return t;
        }

        public bool[,] CreateTable(int col)
        {
            int row = Convert.ToInt32(Math.Pow(2, col));
            int half = row;
            bool[,] table = new bool[row, col];
            bool value = false;

            for (int c = 0; c < col; c++)
            {
                half /= 2;
                for (int r = 0; r < row; r++)
                {
                    table[r, c] = value;
                    if ((r + 1) % half == 0 || half == 1)
                    {
                        value = !value;
                    }
                }
            }
            return table;
        }

        public bool BM(int idd)
        {
            bool x = false;
            if (listerino[idd - 1].Param != '~')
            {
                if (listerino[listerino[idd - 1].ChildOneId - 1].Letter == false && listerino[listerino[idd - 1].ChildTwoId - 1].Letter == false)
                {
                    x = cal.GetSign(listerino[idd - 1].Param, BM(listerino[idd - 1].ChildOneId), BM(listerino[idd - 1].ChildTwoId));
                }
                else if (listerino[listerino[idd - 1].ChildOneId - 1].Letter == true && listerino[listerino[idd - 1].ChildTwoId - 1].Letter == false)
                {
                    x = cal.GetSign(listerino[idd - 1].Param, listerino[listerino[idd - 1].ChildOneId - 1].Val, BM(listerino[idd - 1].ChildTwoId));
                }
                else if (listerino[listerino[idd - 1].ChildOneId - 1].Letter == false && listerino[listerino[idd - 1].ChildTwoId - 1].Letter == true)
                {
                    x = cal.GetSign(listerino[idd - 1].Param, BM(listerino[idd - 1].ChildOneId), listerino[listerino[idd - 1].ChildTwoId - 1].Val);
                }
                else if (listerino[listerino[idd - 1].ChildOneId - 1].Letter == true && listerino[listerino[idd - 1].ChildTwoId - 1].Letter == true)
                {
                    x = cal.GetSign(listerino[idd - 1].Param, listerino[listerino[idd - 1].ChildOneId - 1].Val, listerino[listerino[idd - 1].ChildTwoId - 1].Val);
                }
            }
            else
            {
                if (listerino[listerino[idd - 1].ChildOneId - 1].Letter == true)
                {
                    x = cal.GetSign('~', listerino[listerino[idd - 1].ChildOneId - 1].Val, false);
                }
                else if (listerino[listerino[idd - 1].ChildOneId - 1].Letter == false)
                {
                    x = cal.GetSign('~', BM(listerino[idd - 1].ChildOneId), false);
                }
            }
            return x;
        }
    }
}
