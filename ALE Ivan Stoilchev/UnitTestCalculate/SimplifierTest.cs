using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASI;
using System.Collections.Generic;

namespace UnitTestCalculate
{
    [TestClass]
    public class SimplifierTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string zero1 = "100";
            string zero2 = "001";
            string one1 = "000";
            string one2 = "010";
            string one3 = "011";
            string one4 = "101";
            string one5 = "110";
            string one6 = "111";
            string answ = "0*0";
            string answ1 = "1*1";
            string answ2 = "*1*";
            List <string> one = new List<string>();
            List<string> zero = new List<string>();
            List<string> ans = new List<string>();
            one.Add(one1);
            one.Add(one2);
            one.Add(one3);
            one.Add(one4);
            one.Add(one5);
            one.Add(one6);
            zero.Add(zero1);
            zero.Add(zero2);
            ans.Add(answ);
            ans.Add(answ1);
            ans.Add(answ2);
            Simplifier simp = new Simplifier();
            List<string> lis = simp.Simlify(one, zero);
            CollectionAssert.AreEqual(ans, lis);
        }
    }
}
