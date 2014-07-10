using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class StreamManipulation
    {
        [Test]
        public void ReadAndWriteStreams()
        {
            string path = Path.Combine( TestHelper.TestSupportPath, "TestFile.bin" );
            string pathTarget = path + ".copy";

            using( var fStream = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.None ) )
            using( var fOut = new FileStream( pathTarget, FileMode.Create, FileAccess.Write, FileShare.None ) )
            {
                Assert.That( fStream.CanWrite, Is.False );
                Assert.That( fOut.CanRead, Is.False );

                Transfer( fStream, fOut );
            }
            FileAssert.AreEqual( path, pathTarget );
        }

        [Test]
        public void CompressionDecompression()
        {
            string path = Path.Combine( TestHelper.TestSupportPath, "TestFile.bin" );
            string pathCompressed = path + ".gzip";
            string pathTarget = path + ".copy";
            // path ==> pathCompressed
            // path <- fIn -->X--> fZip -> fOut -> pathCompressed
            using( var fIn = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.None ) )
            using( var fOut = new FileStream( pathCompressed, FileMode.Create, FileAccess.Write, FileShare.None ) )
            using( var fZip = new GZipStream( fOut, CompressionMode.Compress ) )
            {
                Transfer( fIn, fZip );
            }
            Assert.That( new FileInfo( path ).Length > new FileInfo( pathCompressed ).Length );
            // pathCompressed ==> pathTarget
            // pathCompressed -> fIn -> fZip -->X--> fOut -> pathTarget
            using( var fIn = new FileStream( pathCompressed, FileMode.Open, FileAccess.Read, FileShare.None ) )
            using( var fZip = new GZipStream( fIn, CompressionMode.Decompress ) )
            using( var fOut = new FileStream( pathTarget, FileMode.Create, FileAccess.Write, FileShare.None ) )
            {
                Transfer( fZip, fOut );
            }            
            FileAssert.AreEqual( path, pathTarget );
        }

        private void Transfer( Stream fStream, Stream fOut )
        {
            byte[] buffer = new byte[4*1024];
            int lenRead;
            while( (lenRead = fStream.Read( buffer, 0, buffer.Length )) > 0 )
            {
                fOut.Write( buffer, 0, lenRead );
            }
        }

    }
}
