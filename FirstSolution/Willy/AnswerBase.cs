using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Willy
{
    using Willy.Interfaces;

    [Serializable]
    public abstract class AnswerBase : ICommand, IMemento, IMementoObject
    {
        protected readonly FormAnswer _formAnswer;
        protected readonly QuestionBase _question;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="answerForm"></param>
        /// <param name="questionForm"></param>
        public AnswerBase(FormAnswer answerForm, QuestionBase questionForm)
        {
            _formAnswer = answerForm;
            _question = questionForm;
        }

        /// <summary>
        /// Command that executes the answer
        /// For example, it can auto-answer by taking informations from the GUI
        /// Then, the developer must respect the ICommande contract (command pattern)
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Internal : Returns the associated question
        /// </summary>
        internal IQuestionContainer Question
        {
            get { return _question; }
        }

        /// <summary>
        /// Must clone the current object for the memento
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();

        /// <summary>
        /// Saves the current object to the memento
        /// </summary>
        /// <returns></returns>
        public abstract object SaveToMemento();

        /// <summary>
        /// Restores the current object from the memento
        /// </summary>
        /// <param name="obj"></param>
        public abstract void RestoreFromMemento(object obj);

        /// <summary>
        /// Memento class to save the objects
        /// </summary>
        protected class Memento
        {
            private object obj;

            public Memento(IMementoObject objectToSave)
            {
                obj = objectToSave.Clone();
            }

            public object GetSavedObject()
            {
                return obj;
            }
        }
    }
}
