using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests
{
    [TestFixture]
    class FromDelegateToLambda
    {
        // This is a Type definition.
        delegate void TestPerformanceFunction();
        delegate TimeSpan TestPerformanceFunctionWithValue( int counter );
        delegate int TestPerformanceFunctionWithMultipleValues( int counter, string s );
        
        public void SimplifiedTestPerf()
        {
            // Step 0
            long ticks1 = SimplePerf( 1000, new TestPerformanceFunctionWithValue( DoSomething ) );
            // Step 1
            long ticks2 = SimplePerf( 1000, DoAnotherThing );
            // Step 2:  Anonymous function
            long ticks3 = SimplePerf( 1000, delegate( int c )
            {
                for( int i = 0; i < 10000; ++i )
                {

                }
                return TimeSpan.FromSeconds( 1 );
            }
            );
            // Step 3: Lambda function.
            long ticks4 = SimplePerf( 1000, theVar => TimeSpan.FromMinutes( 1 ) );
            long ticks5 = SimplePerf( 1000, x => { return TimeSpan.Zero; } );

            TestPerformanceFunction noParameter = () => { };
            TestPerformanceFunctionWithValue oneParam = x => TimeSpan.Zero;
            TestPerformanceFunctionWithMultipleValues moreParams = ( x, s ) => 3712;
            TestPerformanceFunctionWithMultipleValues moreParams2 = delegate( int x, string s ) { return 3712; };
            var r = moreParams( 3, "Toto" );
        }

        private long SimplePerf( int c, TestPerformanceFunctionWithValue a )
        {
            Stopwatch w = new Stopwatch();
            w.Start();
            a( c );
            w.Stop();
            return w.ElapsedTicks;
        }

        static TimeSpan DoSomething( int counter )
        {
            //...
            return TimeSpan.Zero;
        }

        int _toto = 4;

        TimeSpan DoAnotherThing( int c )
        {
            //...
            this._toto = 5;
            return TimeSpan.Zero;
        }

        #region 1.0 .Net framework

        [Test]
        public void WhatIsADelegate()
        {
            // Creating an instance of this type.
            TestPerformanceFunctionWithValue f = new TestPerformanceFunctionWithValue( DoSomething );
            // Calling the instance.
            f( 3 );
            // This instance is bound to DoAnotherThing that is an Instance method.
            TestPerformanceFunctionWithValue f2 = new TestPerformanceFunctionWithValue( DoAnotherThing );
            f2( 4 );
        }
        #endregion

        #region Next step...

        [Test]
        public void EasierToUse()
        {
            // Creating an instance of this type.
            TestPerformanceFunctionWithValue f = DoSomething;
            // Calling the instance.
            f( 2 );
            // This instance is bound to DoAnotherThing that is an Instance method.
            TestPerformanceFunctionWithValue f2 = DoAnotherThing;
            f2( 1 );
            Assert.That( _toto, Is.EqualTo( 5 ) );
        }
        #endregion




    }
}
