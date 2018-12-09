using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASI;
using System.Collections.Generic;

namespace UnitTestCalculate
{
    [TestClass]
    public class NandifyTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string str = "=( >(A,B), |( ~(C) ,B) )";
            string ans = "%(%(%(%(A,%(B,B)),%(A,%(B,B))),%(%(%(%(C,C),%(C,C)),%(B,B)),%(%(%(C,C),%(C,C)),%(B,B)))),%(%(A,%(B,B)),%(%(%(C,C),%(C,C)),%(B,B))))";
            str = str.Replace(" ", "");
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            Tree tre = new Tree();
            Nandify nan = new Nandify();
            List<Node> n = tre.ReadFormula(str);
            nan.GetList(n);
            string ans1 = nan.BM(1);
            Assert.AreEqual(ans, ans1);
        }
    }
}
