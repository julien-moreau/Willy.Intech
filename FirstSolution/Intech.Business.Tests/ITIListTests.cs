using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class ITIListTests
    {
        [SetUp]
        public void Initialize()
        {
            // Done BEFORE each test of this fixture.
        }

        [TearDown]
        public void Unitialize()
        {
            // Done AFTER each test of this fixture.
        }

        [Test]
        public void Basics()
        {
            ITIList<int> myList = new ITIList<int>();
            ITIList<string> myList2 = new ITIList<string>();
            var myList3 = new ITIList<ITIList<string>>();
        }

        [Test]
        public void AddingItems()
        {
            // Arrange
            ITIList<int> myList = new ITIList<int>();
            Assert.That( myList.Count, Is.EqualTo( 0 ) );
            // Act
            myList.Add( 25 );
            // Assert
            Assert.That( myList.Count, Is.EqualTo( 1 ) );
            Assert.That( myList[0], Is.InstanceOf( typeof( int ) ) );
            Assert.That( myList[0], Is.EqualTo( 25 ) );
            myList.RemoveAt( 0 );
            Assert.That( myList.Count, Is.EqualTo( 0 ) );
        }

        [Test]
        public void TraversingItems()
        {
            var l = new ITIList<int>();
            l.Add( 5 );
            l.Add( 10 );
            int sum = 0;
            foreach( var i in l )
            {
                sum += i;
            }
            Assert.That( sum, Is.EqualTo( 15 ) );
        }

        [Test]
        public void NoItemsInList()
        {
            var l = new ITIList<int>();
            Assert.That( l.GetEnumerator().MoveNext(), Is.False );
            l.Add( 1 );
            var enu = l.GetEnumerator();
            Assert.That( enu.MoveNext(), Is.True );
            Assert.That( enu.MoveNext(), Is.False );
            l.RemoveAt( 0 );
            Assert.That( l.GetEnumerator().MoveNext(), Is.False );
        }

        [Test]
        public void ExceptionsMustBeThrown()
        {
            var l = new ITIList<int>();
            l.Add( 1 );

            AssertThrow( typeof( InvalidOperationException ), () => Console.WriteLine( l.GetEnumerator().Current ) );
            
            AssertThrow<InvalidOperationException>( () => Console.WriteLine( l.GetEnumerator().Current ) );

        }

        static void AssertThrow<T>( Action a )
        {
            bool exThrown = false;
            try
            {
                a();
            }
            catch( Exception ex )
            {
                if( ex is T == false )
                {
                    Assert.Fail( String.Format( "Expected exception {0} but got a {1}.",
                        typeof(T).FullName, ex.GetType().FullName ) );
                }
                exThrown = true;
            }
            Assert.That( exThrown, String.Format( "A {0} is expected.", typeof(T).FullName ) );
        }

        static void AssertThrow( Type expectedExceptionType, Action a )
        {
            bool exThrown = false;
            try
            {
                a();
            }
            catch( Exception ex )
            {
                if( expectedExceptionType.IsAssignableFrom( ex.GetType() ) == false )
                {
                    Assert.Fail( String.Format( "Expected exception {0} but got a {1}.",
                        expectedExceptionType.FullName, ex.GetType().FullName ) );
                }
                exThrown = true;
            }
            Assert.That( exThrown, String.Format( "A {0} is expected.", expectedExceptionType.FullName ) );
        }
    }
}
