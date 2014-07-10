using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public interface IFileSystemProvider
    {
        /// <summary>
        /// If the url can not be mapped to a physical path null is returned.
        /// </summary>
        /// <param name="url">Url path relative to the application root.</param>
        /// <returns>Physical path or null if unable to map the url.>/returns>
        string MapPath( string url );
    }
}
