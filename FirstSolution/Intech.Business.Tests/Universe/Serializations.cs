using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Intech.Space.Spi;
using NUnit.Framework;

namespace Intech.Business.Tests.UniverseTests
{
    [TestFixture]
    public class Serializations
    {
        [Test]
        public void SimpleOne()
        {
            Universe u = CreateTestUniverse();

            using( MemoryStream m = new MemoryStream() )
            { 
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize( m, u );

                m.Position = 0;
                var u2 = (Universe)f.Deserialize( m );

                Assert.That( u, Is.Not.SameAs( u2 ) );
            }

            string fName = Path.Combine( TestHelper.TestSupportFolder.FullName, "Universe.dat" );
            using( FileStream f = File.OpenWrite( fName ) )
            {
                new BinaryFormatter().Serialize( f, u );
            }

        }

        private static Universe CreateTestUniverse()
        {
            Universe u = new Universe();
            var g1 = u.FindOrCreateGalaxy( "Andromède" );
            var g2 = u.FindOrCreateGalaxy( "Cassiopée" );
            g1.CreateStar( "S1" );
            g1.CreateStar( "S2" );
            g1.CreateStar( "S3" );
            g2.CreateStar( "s4" );
            return u;
        }

        [Test]
        public void HomeMade()
        {
            Universe u = CreateTestUniverse();

            string fName = Path.Combine( TestHelper.TestSupportFolder.FullName, "Universe.HomeMade.dat" );
            using( FileStream f = File.OpenWrite( fName ) )
            {
                u.HomeMadeSerialize( f );
            }

        }


        [Test]
        public void XmlHomeMadeExternalization()
        {
            string fName = Path.Combine( TestHelper.TestSupportFolder.FullName, "Universe.xml" );
            string xmlUOriginal;
            string xmlUReloaded;
            {
                Universe u = CreateTestUniverse();
                var xml = u.ToXml();
                xmlUOriginal = xml.ToString();
                var docXml = new XDocument( xml );
                using( var writer = XmlWriter.Create( fName, new XmlWriterSettings()
                {
                    Indent = true,
                    NewLineOnAttributes = true
                } ) )
                {
                    docXml.WriteTo( writer );
                }
            }
            {
                var doc = XDocument.Load( XmlReader.Create( fName ) );
                
                var uLoaded = new Universe( doc.Root );
                var uExportAgain = new Universe( uLoaded.ToXml() );
                xmlUReloaded = uExportAgain.ToXml().ToString();
            }
            Assert.That( xmlUOriginal, Is.EqualTo( xmlUReloaded ) );
        }
    }
}
