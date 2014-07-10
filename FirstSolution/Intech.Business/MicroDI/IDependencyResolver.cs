using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.MicroDI
{
    public interface IDependencyResolver
    {
        object Resolve( Type t );
    }
}
