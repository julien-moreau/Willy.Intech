using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class RealConsole : IConsoleOutput
    {
        public void WriteLine( string message, params object[] parameters )
        {
            Console.WriteLine( message, parameters );
        }
    }
}
