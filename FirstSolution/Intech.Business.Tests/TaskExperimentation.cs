using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class TaskExperimentation
    {
        int SyncM1()
        {
            return 2;
        }

        string SyncM2( int i )
        {
            return new string( 'B', i );
        }

        Task<int> AsyncM1()
        {
            Thread.Sleep( 5 );
            return Task.FromResult( 2 );
        }

        Task<string> AsyncM2( int i )
        {
            Thread.Sleep( 5 );
            return Task.FromResult( "AsyncResult:" + new string( 'B', i ) );
        }

        [Test]
        public void SimpleOne()
        {
            //{
            //    int iR = SyncM1();
            //    string sR = SyncM2( iR );
            //    Console.WriteLine( sR );
            //}
            //{
            //    int iR = AsyncM1().Result;
            //    string sR = AsyncM2( iR ).Result;
            //    Console.WriteLine( sR );
            //}
            {
                Task result = CombinedActions();
                Console.WriteLine( "task Created." );
                result.Wait();
                Console.WriteLine( "task Done." );
            }
            {
                // With async/await
                DoSomething().Wait();
                Console.WriteLine( "async!!! task Done." );
            }
        }

        Task CombinedActions()
        {
            Task result;
            result = AsyncM1()
                .ContinueWith( t => AsyncM2( t.Result ).Result )
                .ContinueWith( t => Console.WriteLine( t.Result ) );
            return result;
        }


        async Task DoSomething()
        {
            int i = await AsyncM1();
            string s = await AsyncM2( i );
            Console.WriteLine( s );
        }
        
        async Task DoSomethingTwice()
        {
            Task<string> s1 = AsyncM2( await AsyncM1() );
            Task<string> s2 = AsyncM2( await AsyncM1() );
            Task.WaitAll( s1, s2 );
            //await Task.WhenAll( s1, s2 );
            Console.WriteLine( s1.Result + " - " + s2.Result );
        }
    }
}
