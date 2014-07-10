using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests.DI
{
    [TestFixture]
    public class SimpleUploaderController
    {
        public class FakeFileSystem : IFileSystemProvider
        {
            public string MapPath( string url )
            {
                if( url.StartsWith("Private/"))
                    return Path.Combine( TestHelper.SolutionFolder.FullName, url );
                return Path.Combine( TestHelper.TestSupportFolder.FullName, "WebRoot", url );
            }
        }

        class FakeWebClient : IWebClient
        {
            public List<Tuple<string,string>> UploadCalls = new List<Tuple<string,string>>();

            public void UploadFile( string localFile, string targetUrl )
            {
                UploadCalls.Add( Tuple.Create( localFile, targetUrl ) );
            }
        }

        public void BuildUploader()
        {
            // Arrange
            // - Buildup SUT dependencies.
            var fs = new FakeFileSystem();
            var wc = new FakeWebClient();
            var c = new StringConsole();
            // - Create the SUT. 
            var p = new UploadController( fs, wc, c );

            // Act.
            p.UpLoad( "", "http://uploadify.org" );

            // Assert.
            wc.UploadCalls.Single( call => File.ReadAllText( call.Item1 ) == "ExportMe" );
            wc.UploadCalls.Single( call => File.ReadAllText( call.Item1 ) == "ExportMeSub" );
            Assert.That( wc.UploadCalls.Any( call => call.Item1.Contains( "DONOTExport" ) ), Is.False );
        }


    }
}
