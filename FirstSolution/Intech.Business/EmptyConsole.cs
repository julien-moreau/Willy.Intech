using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class EmptyConsole : IConsoleOutput
    {
        public static readonly EmptyConsole Instance = new EmptyConsole();

        private EmptyConsole()
        {
        }

        public void WriteLine( string message, params object[] parameters )
        {
        }
    }
}
