using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public interface IWebClient
    {
        void UploadFile( string localFile, string targetUrl );
    }
}
