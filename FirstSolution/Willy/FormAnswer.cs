using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Willy
{
    using Interfaces;

    [Serializable]
    public class FormAnswer
    {
        private string _name;
        private List<AnswerBase> _answers;

        /// <summary>
        /// ctor
        /// To completely use the Factory Pattern, just set the FormAnswer constructor to "internal"
        /// </summary>
        /// <param name="name"></param>
        public FormAnswer(string name)
        {
            _name = name;
            _answers = new List<AnswerBase>();
        }

        /// <summary>
        /// Try to find the answer of the given question.
        /// If it doesn't exsist, then null is returned
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public AnswerBase FindAnswer(IQuestionContainer question)
        {
            foreach (var a in _answers)
            {
                if (a.Question == question)
                    return a;
            }
            return null;
        }

        /// <summary>
        /// Creates a new Answer for the given question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public AnswerBase CreateAnswer(QuestionBase question)
        {
            AnswerBase a = question.CreateAnswer(this, question);
            _answers.Add(a);
            return a;
        }

        /// <summary>
        /// Return or set the name of the answer form
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Remove the answer related to the given question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public bool RemoveAnswer(IQuestionContainer question)
        {
            foreach (var a in _answers)
            {
                if (a.Question == question)
                {
                    _answers.Remove(a);
                    return true;
                }
            }
            return false;
        }

    }
}
