using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASI;
using System.Collections.Generic;

namespace UnitTestCalculate
{
    [TestClass]
    public class NormalizationTest
    {
        [TestMethod]
        public void SimplifyedNormalization()
        {
            string an = "|( |( &(~(A),~(C)) , &(A,C)) , B)";
            Normalizer n = new Normalizer();
            List<string> lis = new List<string>();
            string letters = "ABC";
            string answ = "0*0";
            string answ1 = "1*1";
            string answ2 = "*1*";
            lis.Add(letters);
            lis.Add(answ);
            lis.Add(answ1);
            lis.Add(answ2);
            string ans = n.Normalize(lis);
            Assert.AreEqual(an, ans);
        }

        [TestMethod]
        public void Normalization()
        {
            string an = "|( |( |( |( |( &(&(~(A),~(B)),~(C)) , &(&(~(A),B),~(C))) , &(&(~(A),B),C)) , &(&(A,~(B)),C)) , &(&(A,B),~(C))) , &(&(A,B),C))";
            Normalizer n = new Normalizer();
            List<string> lis = new List<string>();
            string letters = "ABC";
            string one1 = "000";
            string one2 = "010";
            string one3 = "011";
            string one4 = "101";
            string one5 = "110";
            string one6 = "111";
            lis.Add(letters);
            lis.Add(one1);
            lis.Add(one2);
            lis.Add(one3);
            lis.Add(one4);
            lis.Add(one5);
            lis.Add(one6);
            string ans = n.Normalize(lis);
            Assert.AreEqual(an, ans);
        }

    }
}
