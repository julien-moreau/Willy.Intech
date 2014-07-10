﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Willy.Interfaces;

namespace Willy
{
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

        public override void answer() { }

    }
}
