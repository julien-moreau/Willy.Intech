using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class MultiThreading
    {
        [Test]
        public void ThreadCreation()
        {
            ThisIsPerThread = "A";
            Assert.That( ThisIsPerThread == "A" );
            Console.WriteLine( "In Thread: {0}", Thread.CurrentThread.Name );
            var t = new Thread( ThreadFunc );
            t.Name = "My first Thread";
            t.Start();
            Console.WriteLine( "Finished." );
            DoWork();

            t.Join();
            Console.WriteLine( "REALLY Finished." );
        }

        void ThreadFunc()
        {
            Console.WriteLine( "In Thread {0}.", Thread.CurrentThread.Name );
            ThisIsPerThread = "B";
            DoWork();
        }

        [ThreadStatic]
        static string ThisIsPerThread;

        void DoWork()
        {
            // Direct access to the Thread Local Storage
            //LocalDataStoreSlot s = new LocalDataStoreSlot();
            //Thread.SetData( s, "Toto" );
            //object o = Thread.GetData( s );

            for( int i = 0; i < 1000; ++i )
            {
                Thread.Sleep( 1 );

                bool inFirst = Thread.CurrentThread.Name == "My first Thread";
                Assert.That( (inFirst && ThisIsPerThread == "B") || (!inFirst && ThisIsPerThread == "A") );
                Console.WriteLine( "{0} says {1}.", Thread.CurrentThread.Name, i );
            }
        }


        int _iSuffer;

        [Test]
        public void RaceConditionDemo()
        {
            StressTest( DoASimpleThing, shouldFail: true );
            StressTest( DoASimpleThingWithLock, shouldFail: false );
            StressTest( DoASimpleThingWithLocalLock, shouldFail: false );
            StressTest( BestPerfThatWorks, shouldFail: false );
        }

        void StressTest( ParameterizedThreadStart func, bool shouldFail )
        {
            Console.Write( "{0}, ", func.Method.Name );
            Stopwatch w = new Stopwatch();
            _iSuffer = 0;
            Thread[] threads = new Thread[50];
            // Version A
            for( int i = 0; i < threads.Length; ++i ) threads[i] = new Thread( func );
            w.Start();
            for( int i = 0; i < threads.Length; ++i ) threads[i].Start( 100000 );

            // Version B
            //for( int i = 0; i < threads.Length; ++i )
            //{
            //    threads[i] = new Thread( DoASimpleThing );
            //    threads[i].Start( 100000 );
            //}

            // No choice
            for( int i = 0; i < threads.Length; ++i ) threads[i].Join();
            w.Stop();
            Console.WriteLine( "Elpsed: {0}", w.ElapsedTicks );
            if( shouldFail )
                Assert.That( _iSuffer, Is.LessThan( 50 * 100000 ) );
            else Assert.That( _iSuffer, Is.EqualTo( 50 * 100000 ) );
        }

        void DoASimpleThing( object param )
        {
            int loop = (int)param;
            while( --loop >= 0 )
            {
                ++_iSuffer;
            }
        }

        void DoASimpleThingWithLock( object param )
        {
            lock( this )
            {
                int loop = (int)param;
                while( --loop >= 0 )
                {
                    ++_iSuffer;
                }
            }
        }

        readonly object _lock = new object();

        void DoASimpleThingWithLocalLock( object param )
        {
            int loop = (int)param;
            while( --loop >= 0 )
            {
                lock( _lock )
                {
                    ++_iSuffer;
                }
            }
        }

        /// <summary>
        /// lock keyword is syntactic sugar (just like using()...).
        /// </summary>
        void SameWithMonitor( object param )
        {
            int loop = (int)param;
            while( --loop >= 0 )
            {
                Monitor.Enter( _lock );
                try
                {
                    ++_iSuffer;
                }
                finally
                {
                    Monitor.Exit( _lock );
                }
            }
        }

        void BestPerfThatWorks( object param )
        {
            int loop = (int)param;
            while( --loop >= 0 )
            {
                Interlocked.Increment( ref _iSuffer );
            }
        }

        int _version;

        public void CAS()
        {
            int myVersion = 3;
             
            if( Interlocked.CompareExchange( ref _version, _version+1, myVersion ) == myVersion )
            {

            }
        }

        readonly object _lockA = new object();
        readonly object _lockB = new object();

        /// <summary>
        /// This test blocks threads from the pool and prevents
        /// NUnit runner to close.
        /// This is why it is marked as Explicit: it runs only on demand. 
        /// </summary>
        [Test]
        [Explicit]
        public void DeadLockWithTreadPool()
        {
            ThreadPool.QueueUserWorkItem( unused => LockAThenB() );
            ThreadPool.QueueUserWorkItem( _ => LockBThenA() );
        }

        /// <summary>
        /// Same as above: this deadlocks the test thread.
        /// </summary>
        [Test]
        [Explicit]
        public void DeadLockDemo()
        {
            Thread t1 = new Thread( LockAThenB );
            Thread t2 = new Thread( LockBThenA );

            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            Console.WriteLine( "No deadlock :-(" );
        }

        void LockAThenB()
        {
            for( int i = 0; i < 1000; ++i )
            {
                lock( _lockA )
                {
                    lock( _lockB )
                    {
                        Console.WriteLine( "LockAThenB Works, i={0}", i );
                    }
                }
            }
        }

        void LockBThenA()
        {
            for( int i = 0; i < 1000; ++i )
            {
                lock( _lockB )
                {
                    lock( _lockA )
                    {
                        Console.WriteLine( "LockBThenA Works, i={0}", i );
                    }
                }
            }
        }
    }
}
