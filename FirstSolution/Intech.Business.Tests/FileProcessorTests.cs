using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class FileProcessorTests
    {
        [Test]
        public void EdgeCaseNoRootPath()
        {
            // Arrange
            FileProcessorWithFieldCtorInjection p = new FileProcessorWithFieldCtorInjection( new RealConsole() );
            // Act
            var r = p.Process( "Nimp" );
            // Assert
            Assert.That( r.RootPathExists == false );
            Assert.That( r.TotalDirectoryCount, Is.EqualTo( 0 ) );
            Assert.That( r.TotalFileCount, Is.EqualTo( 0 ) );
        }

        [Test]
        public void EdgeCaseEmptyRoot()
        {
            // Arrange
            FileProcessorWithFieldCtorInjection p = new FileProcessorWithFieldCtorInjection( new RealConsole() );
            var emptyFolder = Path.Combine( TestHelper.TestSupportPath, "EmptyFolder" );
            Directory.CreateDirectory( emptyFolder );
            // Act
            var r = p.Process( emptyFolder );
            // Assert
            Assert.That( r.RootPathExists, Is.True );
            Assert.That( r.TotalFileCount, Is.EqualTo( 0 ) );
            Assert.That( r.TotalDirectoryCount, Is.EqualTo( 1 ) );
        }
    }
}
