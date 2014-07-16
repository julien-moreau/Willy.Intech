using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Willy.Interfaces
{
    public interface ICommand
    {
        /// <summary>
        /// Execute method, all objects (answers here) must implement the Execute method and
        /// then update the answer state
        /// </summary>
        void Execute();
    }
}
