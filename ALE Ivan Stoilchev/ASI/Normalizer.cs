using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI
{
    public class Normalizer
    {
        public string Normalize(List<string> lis)
        {
            string smalFormula = "";
            string bigFormula = "";
            string letters = lis[0];
            for (int i = 1; i < lis.Count; i++)
            {
                string s = lis[i];
                for (int j = 0; j < s.Length; j++)
                {                   
                    if (smalFormula == "")
                    {
                        if (s[j] == '1')
                        {
                            smalFormula = letters[j].ToString();
                        }
                        else if(s[j] == '0')
                        {
                            smalFormula = "~(" + letters[j].ToString() + ")";
                        }
                    }
                    else
                    {
                        if (s[j] == '1')
                        {
                            smalFormula = "&(" + smalFormula + "," + letters[j].ToString() + ")";
                        }
                        else if (s[j] == '0')
                        {
                            smalFormula = "&(" + smalFormula + "," + "~(" + letters[j].ToString() + ")" + ")";
                        }
                    }
                }
                if (bigFormula == "")
                {
                    bigFormula = smalFormula;
                    smalFormula = "";
                }
                else
                {
                    bigFormula = "|( " + bigFormula + " , " + smalFormula + ")";
                    smalFormula = "";
                }
            }
            return bigFormula;
        }
    }
}
