using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Willy.Interfaces;

namespace Willy
{
    public class Form : QuestionFolder
    {
        private string _title;

        public Form(string title)
            : base(null)
        {
            _title = title;
        }

        /// <summary>
        /// Adds a question from the plugintype string
        /// </summary>
        /// <param name="pluginTypeName"></param>
        /// <returns></returns>
        public IQuestionContainer AddQuestion(string pluginTypeName)
        {
            Type pluginType = Type.GetType(pluginTypeName);
            IQuestionContainer thePlugin = (IQuestionContainer)Activator.CreateInstance(pluginType, this);
            base.Questions.Add(thePlugin);

            return thePlugin;
        }

        /// <summary>
        /// Return the form's title
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

    }
}
