using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Willy.Interfaces;

namespace Willy
{
    public abstract class QuestionBase : IQuestionContainer
    {
        private IQuestionFolderContainer _parent;
        private string _questionText;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent"></param>
        public QuestionBase(IQuestionFolderContainer parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Return the QuestionBase's index or sets a new index to the QuestionBase
        /// </summary>
        public int Index
        {
            get { return _parent.Questions.IndexOf(this); }
            set
            {
                if (value < 0 || value >= _parent.Questions.Count)
                {
                    throw new IndexOutOfRangeException("The Index value is out of range");
                }
                else
                {
                    int lastIndex = this.Index;
                    _parent.Questions.Insert(value+1, this);
                    _parent.Questions.RemoveAt(lastIndex);
                }
            }
        }

        /// <summary>
        /// Return the QuestionBase's parent
        /// </summary>
        public IQuestionFolderContainer Parent
        {
            get { return _parent; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("A parent cannot be null");

                _parent.Questions.Remove(this);
                _parent = value;
                _parent.Questions.Add(this);
            }
        }

        /// <summary>
        /// You must create the appropriate answer
        /// </summary>
        /// <param name="answerForm"></param>
        /// <param name="questionForm"></param>
        /// <returns></returns>
        internal abstract AnswerBase CreateAnswer(FormAnswer answerForm, QuestionBase questionForm);

        /// <summary>
        /// Returns or sets the question's text
        /// </summary>
        public string QuestionText
        {
            get { return _questionText; }
            set { _questionText = value; }
        }

    }
}
