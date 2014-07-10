using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class StringConsole : IConsoleOutput
    {
        readonly StringBuilder _b;

        public StringConsole()
        {
            _b = new StringBuilder();
        }

        public void WriteLine( string message, params object[] parameters )
        {
            _b.AppendFormat( message, parameters ).AppendLine();
        }

        public void Clear()
        {
            _b.Length = 0;
        }

        public override string ToString()
        {
            return _b.ToString();
        }
    }
}
