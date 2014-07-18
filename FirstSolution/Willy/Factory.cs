using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Willy
{
    using Willy.Interfaces;

    public class FormFactory : Factory
    {
        /// <summary>
        /// ctor
        /// </summary>
        public FormFactory() { }

        public override Form CreateQuestionForm(string title)
        {
            return new Form(title);
        }

        public override FormAnswer CreateAnswerForm(string name)
        {
            return new FormAnswer(name);
        }
    }

    public abstract class Factory
    {
        public static Factory GetFactory()
        {
            if (_factory == null)
            {
                _factory = new FormFactory();
            }
            return _factory;
        }

        /// <summary>
        /// Creates a new question Form
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public abstract Form CreateQuestionForm(string title);

        /// <summary>
        /// Creates a new answer form (AnswerForm)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract FormAnswer CreateAnswerForm(string name);

        private static FormFactory _factory;
    }
}
