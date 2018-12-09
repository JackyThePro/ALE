using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI
{
    public class Simplifier
    {
        public List<string> Simlify(List<string> one, List<string> zero)
        {
            List<string> listo = one;
            List<string> list0 = zero;
            List<string> final = new List<string>();
            int f = -1;
            //int count = 0;
            while (f != 0)
            {
                List<int> rem = new List<int>();
                f = 0;
                for (int i = 0; i < listo.Count; i++)
                {
                    string first = listo[i];
                    for (int j = i; j < listo.Count; j++)
                    {
                        string second = listo[j];
                        int k = -1;
                        int k1 = -1;
                        char[] c1 = first.ToCharArray();
                        char[] c2 = second.ToCharArray();
                        for (int w = 0; w < first.Length; w++)
                        {
                            if (c1[w] != c2[w] && c1[w] != '*' && c2[w] != '*')
                            {
                                c1[w] = '*';
                                k++;
                            }
                            else if (c1[w] != c2[w] && (c1[w] == '*' || c2[w] == '*'))
                            {
                                c1[w] = '*';
                                k1 = 0;
                            }
                        }
                        string simpli = new string(c1);
                        if ((k == 0 && k1 == -1) || (k == -1 && k1 == 0))
                        {
                            bool push = false;     
                            if(zero.Count==0)
                            {
                                push = true;
                            }
                            else
                            {
                                for (int q = 0; q < zero.Count; q++)
                                {
                                    push = false;
                                    string test = zero[q];
                                    for (int e = 0; e < test.Length; e++)
                                    {
                                        char zz = simpli[e];
                                        string zs = simpli;
                                        if (test[e] != simpli[e] && simpli[e] != '*')
                                        {
                                            push = true;    
                                        }
                                    }
                                    if (push == false)
                                    {
                                        break;
                                    }
                                }
                            }             
                            bool push1 = true;
                            
                            
                            foreach (string s in final)
                            {
                                if (s == simpli)
                                {
                                    push1 = false;
                                }
                            }
                            if (push1 && push)
                            {
                                final.Add(simpli);
                                rem.Add(i);
                                rem.Add(j);
                                f++;
                            }
                        }
                    }
                }
                foreach (int re in rem)
                {
                    listo[re] = "";
                }
                listo.RemoveAll(p => string.IsNullOrEmpty(p));
                //if (count == 0)
                //{
                //    listo.Clear();
                //    count++;
                //}
                listo.AddRange(final);
                final.Clear();
            }
            listo = listo.Distinct().ToList();
            return listo;
        }
    }
}
