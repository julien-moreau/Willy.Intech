using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Willy.Interfaces
{
    public interface IQuestionFolderContainer : IQuestionContainer
    {
        List<IQuestionContainer> Questions
        {
            get;
        }

    }
}
