using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Willy.Interfaces;

namespace Willy
{
    public partial class QuestionBase : IQuestionContainer
    {
        IQuestionContainer _parent;

        public QuestionBase(IQuestionContainer parent)
        {
            _parent = parent;
        }

        public int Index
        {
            get { return _parent.Questions.IndexOf(this); }
            set { }
        }

        public IQuestionContainer Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        //public partial void answer();

        public AnswerBase CreateAnswer()
        {
            throw new NotImplementedException();
        }

    }
}
