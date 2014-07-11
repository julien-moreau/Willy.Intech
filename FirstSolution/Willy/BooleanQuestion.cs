using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Willy.Interfaces;

namespace Willy
{
    public class BooleanQuestion : QuestionBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent"></param>
        public BooleanQuestion(IQuestionFolderContainer parent)
            : base(parent)
        {

        }

        /// <summary>
        /// Override CreateAnswer to create the proper answer
        /// </summary>
        /// <param name="answerForm"></param>
        /// <param name="questionForm"></param>
        /// <returns></returns>
        internal override AnswerBase CreateAnswer(FormAnswer answerForm, QuestionBase questionForm)
        {
            return new BooleanAnswer(answerForm, questionForm);
        }

    }
}
