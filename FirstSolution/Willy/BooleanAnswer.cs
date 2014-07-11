using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Willy
{
    public class BooleanAnswer : AnswerBase
    {
        private bool _answer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="answerForm"></param>
        /// <param name="questionForm"></param>
        public BooleanAnswer(FormAnswer answerForm, QuestionBase questionForm)
            : base(answerForm, questionForm)
        {

        }

        /// <summary>
        /// Return the answer's result
        /// </summary>
        public bool Answer
        {
            get { return _answer; }
            set { _answer = value; }
        }

    }
}
