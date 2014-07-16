using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Willy.Interfaces;

namespace Willy
{
    [Serializable]
    public class QuestionFolder : QuestionBase, IQuestionFolderContainer
    {
        private List<IQuestionContainer> _questions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent"></param>
        public QuestionFolder(IQuestionFolderContainer parent)
            : base(parent)
        {
            _questions = new List<IQuestionContainer>();
        }

        /// <summary>
        /// QuestionBase doesn't have any questions in it
        /// </summary>
        public List<IQuestionContainer> Questions
        {
            get { return _questions; }
        }

        /// <summary>
        /// Returns null because a QuestionFolder cannnot have any answer
        /// </summary>
        /// <param name="answerForm"></param>
        /// <param name="questionForm"></param>
        /// <returns></returns>
        internal override AnswerBase CreateAnswer(FormAnswer answerForm, QuestionBase questionForm)
        {
            return null;
        }

    }
}
