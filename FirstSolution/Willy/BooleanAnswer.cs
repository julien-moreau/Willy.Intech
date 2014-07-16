using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Willy
{
    [Serializable]
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
            _answer = false;
        }

        /// <summary>
        /// Executes the 
        /// </summary>
        public override void Execute()
        {
            _answer = true;
        }

        /// <summary>
        /// Saved the current instance to the memento
        /// </summary>
        /// <returns></returns>
        public override object SaveToMemento()
        {
            return new Memento(this);
        }

        /// <summary>
        /// Restores the current instance (_answer) from the Memento obj
        /// </summary>
        /// <param name="obj"></param>
        public override void RestoreFromMemento(object obj)
        {
            Memento a = obj as Memento;
            if (a != null)
            {
                _answer = (bool)a.GetSavedObject();
            }
        }

        /// <summary>
        /// Clones 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return _answer;
            /*
             * if the answer isn't a value-type, then we create a new instance of
             * the current type (AnswerBase) and set the current properties to
             * the new object
            */
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
