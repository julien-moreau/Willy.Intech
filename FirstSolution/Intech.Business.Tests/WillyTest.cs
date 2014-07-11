using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Willy.Interfaces;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class WillyTest
    {
        [Test]
        public void CreateForm()
        {
            Willy.Form f = new Willy.Form("sinthu");
            Assert.That(f.Title == "sinthu");
        }

        [Test]
        public void CreateAndEditQuestion()
        {
            Willy.Form f = new Willy.Form("sinthu");

            IQuestionContainer qb = f.AddQuestion("Willy.BooleanQuestion, Willy");
            IQuestionContainer qb2 = f.AddQuestion("Willy.BooleanQuestion, Willy");
            IQuestionFolderContainer qf = (IQuestionFolderContainer)f.AddQuestion("Willy.QuestionFolder, Willy");
            Assert.IsNotNull(qb);
            Assert.IsNotNull(qb2);
            Assert.IsNotNull(qf);

            int id = qb.Index;

            Assert.That(qb.Index == 0);
            Assert.That(qb2.Index == 1);
            Assert.That(qf.Index == 2);

            qb.Index = 2;
            Assert.That(qb.Index == 2);
            Assert.That(qb2.Index == 0);
            Assert.That(qf.Index == 1);
        }

        [Test]
        public void CreateAndEditAnswers()
        {
            Willy.Form f = new Willy.Form("sinthu");

            IQuestionContainer qb = f.AddQuestion("Willy.BooleanQuestion, Willy");
            Assert.IsInstanceOfType(typeof(Willy.BooleanQuestion), qb);
            ((Willy.BooleanQuestion)qb).QuestionText = "Coucou je suis le texte";
            Assert.That(((Willy.BooleanQuestion)qb).QuestionText == "Coucou je suis le texte");

            Willy.FormAnswer a = f.CreateAnswer("coucou");
            Assert.That(a.Name == "coucou");

            a.CreateAnswer((Willy.QuestionBase)qb);
            Willy.AnswerBase AnswerOfQuestion = a.FindAnswer((Willy.QuestionBase)qb);
            if (AnswerOfQuestion == null)
            {
                AnswerOfQuestion = a.CreateAnswer((Willy.QuestionBase)qb);
            }

            Assert.That(AnswerOfQuestion != null);
            Assert.IsInstanceOfType(typeof(Willy.BooleanAnswer), AnswerOfQuestion);

            ((Willy.BooleanAnswer)AnswerOfQuestion).Answer = true;
            Assert.That(((Willy.BooleanAnswer)AnswerOfQuestion).Answer == true);

            Assert.That(a.RemoveAnswer(qb) == true);
            Assert.That(a.RemoveAnswer(qb) == false);
        }

        [Test]
        public void TestRemoveQuestion()
        {
            Willy.Form f = new Willy.Form("sinthu");
            IQuestionContainer qb = f.AddQuestion("Willy.BooleanQuestion, Willy");

            Willy.FormAnswer a = f.CreateAnswer("coucou");
            a.CreateAnswer((Willy.QuestionBase)qb);
            Willy.AnswerBase AnswerOfQuestion = a.CreateAnswer((Willy.QuestionBase)qb);

            Assert.That(f.RemoveQuestion(qb) == true);
            Assert.That(f.Questions.Count == 0);
            Assert.That(f.RemoveQuestion(qb) == false);
        }

    }
}
