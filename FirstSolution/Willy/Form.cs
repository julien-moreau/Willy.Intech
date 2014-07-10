using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Willy.Interfaces;

namespace Willy
{
    public class Form : IQuestionContainer
    {
        private string _title;
        private List<IQuestionContainer> _questions;

        public Form(string title)
        {
            _title = title;
            _questions = new List<IQuestionContainer>();
        }

        public IQuestionContainer AddQuestion(string pluginTypeName)
        {
            Type pluginType = Type.GetType(pluginTypeName);
            IQuestionContainer thePlugin = (IQuestionContainer)Activator.CreateInstance(pluginType, this);
            _questions.Add(thePlugin);

            return thePlugin;
        }

        public int Index
        {
            get { return 0; }
            set { }
        }
        public IQuestionContainer Parent
        {
            get { return null; }
            set { }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

    }
}
