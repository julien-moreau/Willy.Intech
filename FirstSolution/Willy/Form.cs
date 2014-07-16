using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Willy
{
    using Interfaces;

    public class Form : QuestionFolder
    {
        private string _title;
        private List<FormAnswer> _answerForms;

        public Form(string title)
            : base(null)
        {
            _title = title;
            _answerForms = new List<FormAnswer>();
        }

        /// <summary>
        /// Adds a question from the plugintype string
        /// </summary>
        /// <param name="pluginTypeName"></param>
        /// <returns></returns>
        public IQuestionContainer AddQuestion(string pluginTypeName, IQuestionFolderContainer folder = null)
        {
            Type pluginType = Type.GetType(pluginTypeName);
            IQuestionContainer thePlugin = (IQuestionContainer)Activator.CreateInstance(pluginType, folder == null ? this : folder);

            if (folder == null)
            {
                base.Questions.Add(thePlugin);
            }
            else
            {
                folder.Questions.Add(thePlugin);
            }

            return thePlugin;
        }

        /// <summary>
        /// Create a new answer form
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FormAnswer CreateAnswer(string name)
        {
            FormAnswer newFormAnswer = new FormAnswer(name);
            _answerForms.Add(newFormAnswer);
            return newFormAnswer;
        }

        /// <summary>
        /// Return the form's title
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public bool RemoveQuestion(IQuestionContainer question, IQuestionFolderContainer root = null)
        {
            if (root == null)
                root = question.Parent;

            foreach (IQuestionContainer q in root.Questions)
            {
                IQuestionFolderContainer folder = q as IQuestionFolderContainer;
                if (folder != null)
                {
                    if (folder == question)
                    {
                        /// Create a temp list
                        List<IQuestionContainer> temp = new List<IQuestionContainer>();
                        /// Fill the temp list (IEnumarator)
                        IEnumerator<IQuestionContainer> e = folder.Questions.GetEnumerator();
                        while (e.MoveNext())
                        {
                            temp.Add(e.Current);
                        }
                        /// Modifie parent of all children of folder
                        foreach (var tempElement in temp)
                        {
                            tempElement.Parent = root;
                        }
                        /// Clear the folder's questions
                        folder.Questions.Clear();
                        /// Delete the temp list
                        temp = null;

                        return root.Questions.Remove(folder);
                    }
                    return this.RemoveQuestion(question, folder);
                }
                else
                {
                    if (q == question)
                    {
                        RemoveAnswer(question);
                        return root.Questions.Remove(question);
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if the answer where removed or not
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        private bool RemoveAnswer(IQuestionContainer question)
        {
            foreach (var a in _answerForms)
            {
                AnswerBase answer = a.FindAnswer(question);
                if (answer != null)
                {
                    a.RemoveAnswer(question);
                    return true;
                }
            }

            return false;
        }

    }
}
