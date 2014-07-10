using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class RealWebClient : IWebClient
    {
        public void UploadFile( string localFile, string targetUrl )
        {
            using( var c = new WebClient() )
            {
                c.UploadFile( targetUrl, localFile );
            }
        }
    }
}
