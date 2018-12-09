using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI
{
    public class Nandify
    {
        List<Node> listerino;
        public void GetList(List<Node> liste)
        {
            listerino = liste;
        }

        public string BM(int idd)
        {
            string x = "";
            if (listerino[idd - 1].Param != '~')
            {
                if (listerino[listerino[idd - 1].ChildOneId - 1].Letter == false && listerino[listerino[idd - 1].ChildTwoId - 1].Letter == false)
                {
                    x = Find(listerino[idd - 1].Param, BM(listerino[idd - 1].ChildOneId), BM(listerino[idd - 1].ChildTwoId));
                }
                else if (listerino[listerino[idd - 1].ChildOneId - 1].Letter == true && listerino[listerino[idd - 1].ChildTwoId - 1].Letter == false)
                {
                    x = Find(listerino[idd - 1].Param, listerino[listerino[idd - 1].ChildOneId - 1].Param.ToString(), BM(listerino[idd - 1].ChildTwoId));
                }
                else if (listerino[listerino[idd - 1].ChildOneId - 1].Letter == false && listerino[listerino[idd - 1].ChildTwoId - 1].Letter == true)
                {
                    x = Find(listerino[idd - 1].Param, BM(listerino[idd - 1].ChildOneId), listerino[listerino[idd - 1].ChildTwoId - 1].Param.ToString());
                }
                else if (listerino[listerino[idd - 1].ChildOneId - 1].Letter == true && listerino[listerino[idd - 1].ChildTwoId - 1].Letter == true)
                {
                    x = Find(listerino[idd - 1].Param, listerino[listerino[idd - 1].ChildOneId - 1].Param.ToString(), listerino[listerino[idd - 1].ChildTwoId - 1].Param.ToString());
                }
            }
            else
            {
                if (listerino[listerino[idd - 1].ChildOneId - 1].Letter == true)
                {
                    x = Find('~', listerino[listerino[idd - 1].ChildOneId - 1].Param.ToString(), "");
                }
                else if (listerino[listerino[idd - 1].ChildOneId - 1].Letter == false)
                {
                    x = Find('~', BM(listerino[idd - 1].ChildOneId), "");
                }
            }
            return x;
        }

        public string Find(char sign, string ch1, string ch2)
        {
            switch (sign)
            {
                case '|':
                    return "%(%(" + ch1 + "," + ch1 + "),%(" + ch2 + "," + ch2 + "))";
                    break;
                case '&':
                    return "%(%(" + ch1 + "," + ch2 + "),%(" + ch1 + "," + ch2 + "))";
                    break;
                case '~':
                    return "%(" + ch1 + "," + ch1 + ")";
                    break;
                case '>':
                    return "%(" + ch1 + ",%(" + ch2 + "," + ch2 + "))";
                    break;
                case '=':
                    return "%(%(%(" + ch1 + "," + ch1 + "),%(" + ch2 + "," + ch2 + ")),%(" + ch1 + "," + ch2 + "))";
                    break;
                case '%':
                    return "%(" + ch1 + "," + ch2 + ")";
                    break;
                default:
                    return "";
            }
        }
    }
}
