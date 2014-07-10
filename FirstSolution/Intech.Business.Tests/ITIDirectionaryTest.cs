using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests
{
    [TestFixture]
    class ITIDirectionaryTest
    {

        [Test]
        public void AddTest()
        {
            Business.ITIDictionary<string, int> myDict = new Business.ITIDictionary<string, int>();
            myDict.Add("string1", 1);
            Assert.That(myDict.Count == 1);
            myDict.Add("string2", 2);
            Assert.That(myDict.Count == 2);
        }

        [Test]
        public void TryGetValueTest()
        {
            Business.ITIDictionary<string, int> myDict = new Business.ITIDictionary<string, int>();
            myDict.Add("string1", 1);
            myDict.Add("string2", 2);

            Assert.That(myDict["string1"] == 1);
            Assert.That(myDict["string2"] == 2);

            int outValue;
            bool triedToGet = myDict.TryGetValue("string1", out outValue);
            Assert.That(triedToGet);
            Assert.That(outValue == 1);
        }

        [Test]
        public void AddWithReallocTest()
        {
            int addToDictCount = 10000;
            Business.ITIDictionary<int, string> myDict = new Business.ITIDictionary<int, string>();
            for (int i = 0; i < addToDictCount; i++)
            {
                StringBuilder b = new StringBuilder("string");
                b.Append(i);
                myDict.Add(i, b.ToString());
            }

            Assert.That(myDict.Count == addToDictCount);
            Assert.That(myDict[addToDictCount/2] == "string5000");
            Assert.That(myDict[6789] == "string6789");
            Assert.That(myDict.Count == addToDictCount);

            for (int i=0; i < addToDictCount; i++) {
                Assert.That(myDict[i] == "string" + i);
            }

            myDict.Remove(0);
        }

    }
}
