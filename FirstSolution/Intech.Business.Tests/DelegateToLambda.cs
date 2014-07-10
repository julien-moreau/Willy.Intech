using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    [TestFixture]
    class DelegateToLambda
    {

        static void DoSomething()
        {

        }
        static void DoAnotherThing()
        {

        }
        delegate void TestPerformanceFunction();

        public void simplifiedTestPerf()
        {
            long ticks = SimplePerf(1000, DoSomething);
            long ticks2 = SimplePerf(1000, DoAnotherThing);
        }

        private long SimplePerf(int p, Action a)
        {
            Stopwatch w = new Stopwatch();

            w.Start();

            while (--p > 0)
                a();

            w.Stop();

            return w.ElapsedTicks;
        }

        [Test]
        public void WhatIsADelegate()
        {
            TestPerformanceFunction f = new TestPerformanceFunction(DoSomething);
        }

    }

}
