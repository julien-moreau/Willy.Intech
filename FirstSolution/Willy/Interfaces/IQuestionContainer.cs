using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Willy.Interfaces
{
    public interface IQuestionContainer
    {
        int Index
        {
            get;
            set;
        }

        IQuestionContainer Parent
        {
            get;
            set;
        }
    }
}
