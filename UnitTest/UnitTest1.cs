using System;
using Granda.IZ.CodeDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            MyEvaluator myEvaluator = new MyEvaluator(typeof(int), "1+1", "plus");
            var result = myEvaluator.EvaluateInt("plus");

            var intType = typeof(Int32);
        }
    }
}
