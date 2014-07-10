using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class UploadController
    {
        readonly IFileSystemProvider _fileProvider;
        readonly IConsoleOutput _console;
        readonly IWebClient _webClient;

        public UploadController( IFileSystemProvider fileProvider, IWebClient webClient, IConsoleOutput output )
        {
            if( fileProvider == null ) throw new ArgumentNullException( "fileProvider" );
            if( webClient == null ) throw new ArgumentNullException( "webClient" );
            if( output == null ) throw new ArgumentNullException( "output" );
            _fileProvider = fileProvider;
            _console = output;
            _webClient = webClient;
        }

        public void UpLoad( string localUrlPath, string targetUrl )
        {
            string p = _fileProvider.MapPath( localUrlPath );
            if( p == null ) return;

            var processor = new FileProcessorWithFieldCtorInjection( _console );
            processor.StopOnFirstError = false;
            processor.Process( p, ( isHidden, file ) =>
                {
                    if( !isHidden )
                    {
                        _webClient.UploadFile( p, targetUrl );
                    }
                }
            );
        }
    }
}
