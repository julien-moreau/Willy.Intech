﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Willy
{
    public class AnswerBase
    {
        private FormAnswer _formAnswer;
        private QuestionBase _question;

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
        /// Internal : Returns the associated question
        /// </summary>
        internal QuestionBase Question
        {
            get { return _question; }
        }

    }
}
