using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class WhatIsEquality
    {
        class SimpleReferenceType
        {
        }

        [Test]
        public void SimpleClass()
        {
            var c1 = new SimpleReferenceType();
            var c2 = new SimpleReferenceType();
            var c3 = c1;

            // We write that:
            Assert.That( c3 == c1 );
            Assert.That( c1 != c2 );
            Assert.That( c3 != c2 );
            // What is called:
            Assert.That( ReferenceEquals( c3, c1 ), "This is a static method of Object." );
            Assert.That( Object.ReferenceEquals( c1, c2 ) == false );
            Assert.That( ReferenceEquals( c3, c2 ) == false );
            // The Equals method:
            Assert.That( c3.Equals( c1 ), Is.True );
            Assert.That( c1.Equals( c2 ), Is.False, "Default Object.Equals() calls ReferenceEquals." );
            Assert.That( c3.Equals( c2 ), Is.False );

        }

        [Test]
        public void SimpleString()
        {
            // The compiler merges the 2 strings:
            // The exact same object is used in memory.
            var s1 = "Albert";
            var s2 = "Albert";
            var s3 = s1;
            // We write that:
            Assert.That( s3 == s1 );
            Assert.That( s1 == s2 );
            Assert.That( s3 == s2 );
            // What is called (in java, NOT in c#!):
            Assert.That( ReferenceEquals( s3, s1 ), "This is a static method of Object." );
            Assert.That( Object.ReferenceEquals( s1, s2 ), Is.True );
            Assert.That( ReferenceEquals( s3, s2 ), Is.True );

            {
                // Compiler is clever enough to resolve the concatenation
                // at compile time!
                string x1 = "Alb" + "ert";
                Assert.That( ReferenceEquals( x1, s1 ), "Compiler is clever." );
            }
            {
                // Well... not that much...
                string x1 = "Alb" + 'e' + "rt";
                Assert.That( ReferenceEquals( x1, s1 ), Is.False, "2 different objects." );
                Assert.That( x1.Equals( s1 ), Is.True, "Value equality!" );
                Assert.That( x1 == s1, Is.True, "C# String defines == to call the virtual .Equals method." );
            }
        }

        class RefTypeWithEqualityValue
        {
            int _age;
            string _name;

            public RefTypeWithEqualityValue( int age, string name )
            {
                _age = age;
                _name = name;
            }

            public override bool Equals( object obj )
            {
                RefTypeWithEqualityValue other = obj as RefTypeWithEqualityValue;
                if( other == null ) return false;
                return _age == other._age && _name == other._name;
            }

            public override int GetHashCode()
            {
                return _age ^ _name.GetHashCode();
            }
        }

        // Reference Type With Equality Value With Equals
        class Person
        {
            int _age;
            string _name;

            public Person( int age, string name )
            {
                _age = age;
                _name = name;
            }

            public override bool Equals( object obj )
            {
                var other = obj as Person;
                if( ReferenceEquals( other, null ) ) return false;
                return _age == other._age && _name == other._name;
            }

            public override int GetHashCode()
            {
                // This is invalid in our case because
                // this does not capture the way our Equals does its job:
                // "Equals" objects (in our sense) will have different Hash code!
                //
                // Object.GetHashCode() uses the "memory address" of the object
                // to compute the hash .
                // This is perfectly valid with the Object.Equals implementation
                // that simply calls ReferenceEquals...
                //
                // But for us, this is NOT good. We have overridden Equals so we 
                // MUST override GetHashCode() to return an int based on the same
                // things we use to test for equality.
                //
                // return base.GetHashCode();

                // Valid... But stupid!
                // return 1; 

                // Not ideal... We CAN rely on a subset of the data used by the Equals
                // method. But we SHOULD avoid this.
                //
                // return _age;
                // return _age * _name.GetHashCode();
                
                // In practice use the xor:
                return _age ^ _name.GetHashCode();
            }

            public static bool operator ==( Person o1, Person o2 )
            {
                return ReferenceEquals( o1, null ) ? ReferenceEquals( o2, null ) : o1.Equals( o2 );
            }

            public static bool operator!=( Person o1, Person o2 )
            {
                // Recommended standard implementation.
                return !(o1 == o2);
            }
        }

        [Test]
        public void EqualityValueOrValueSemantics()
        {
            {
                RefTypeWithEqualityValue p1 = new RefTypeWithEqualityValue( 12, "Toto" );
                RefTypeWithEqualityValue p2 = new RefTypeWithEqualityValue( 12, "Toto" );

                Assert.That( ReferenceEquals( p1, p2 ), Is.False );
                Assert.That( p1.Equals( p2 ), Is.True );
                Assert.That( p1 == p2, Is.False, "== is NOT redefined for our type." );
            }
            {
                var p1 = new Person( 12, "Toto" );
                var p2 = new Person( 12, "Toto" );
                
                Assert.That( ReferenceEquals( p1, p2 ), Is.False );
                Assert.That( p1.Equals( p2 ), Is.True );
                Assert.That( p1 == p2, Is.True, "== IS redefined for our type." );
            }
        }

        class IdentityCard
        {
            public readonly Person Person;

            public IdentityCard( Person p )
            {
                Person = p;
            }
        }

        [Test]
        public void FunWithDictionary()
        {
            {
                Dictionary<string,IdentityCard> db = new Dictionary<string, IdentityCard>();

                var john = "John";
                var paul = "Paul";

                var cardJohn = new IdentityCard( null );
                var cardPaul = new IdentityCard( null );
                db.Add( john, cardJohn );
                db.Add( paul, cardPaul );

                Assert.That( db[john] == cardJohn );
                // Another key...
                var searched = "J" + 'o' + "hn";
                Assert.That( searched == john );
                Assert.That( db[searched] == cardJohn );
            }
            {
                Dictionary<Person,IdentityCard> db = new Dictionary<Person, IdentityCard>();

                var john = new Person( 12, "John" );
                var paul = new Person( 81, "Paul" );

                var cardJohn = new IdentityCard( john );
                var cardPaul = new IdentityCard( paul );

                db.Add( john, cardJohn );
                db.Add( paul, cardPaul );

                Assert.That( db[john] == cardJohn );
                Assert.That( db[paul] == cardPaul );

                // Another key...
                var searched = new Person( 12, "John" );

                Assert.That( searched == john );
                Assert.That( db[searched] == cardJohn );
            }
        }

    }
}
