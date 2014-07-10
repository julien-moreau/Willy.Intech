using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class ITIDictionaryTests
    {
        [Test]
        public void OurDictionaryIsNotPerfect()
        {
            ITIDictionary<string,int> db = new ITIDictionary<string,int>( StringComparer.OrdinalIgnoreCase );

            db.Add( "Paul", 45456 );
            Assert.That( db["Paul"] == 45456 );
            
            Assert.That( db["paul"] == 45456 );
        }


        [Test]
        public void TraverseTest()
        {
            var db = new ITIDictionary<string,int>();
            db.Add( "Paul", 45456 );
            db.Add( "Jonn", 123 );
            db.Add( "Michaël", 123 );

            db.Traverse( ( k, v ) => Console.WriteLine( "K={0}, V={1}", k, v ) );

            foreach( var p in db )
            {
                Console.WriteLine( "K={0}, V={1}", p.Key, p.Value );
            }

        }

    }
}
