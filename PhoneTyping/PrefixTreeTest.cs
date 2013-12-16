using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneTyping
{
    [TestClass]
    public class PrefixTreeTest
    {
        [TestMethod]
        public void Test001_CreatePrefixTree()
        {
            Assert.IsNotNull(new PrefixTree());
        }

        [TestMethod]
        public void Test002_InsertIntoPrefixTree()
        {
            var tree = new PrefixTree();
            
            tree.Add("tea");
            tree.Add("tee");

            foreach (var value in tree)
            {
                Console.WriteLine(value);
            }

            Assert.IsTrue(tree.Contains("tea"));
            Assert.IsTrue(tree.Contains("tee"));
        }
    }
}
