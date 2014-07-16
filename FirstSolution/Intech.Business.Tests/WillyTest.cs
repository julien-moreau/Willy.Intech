using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization.Formatters.Binary;
using Willy.Interfaces;
using System.IO;

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

        [Test]
        public void TestWithQuestionFolderResetParent()
        {
            Willy.Form f = new Willy.Form("sinthu");
            IQuestionContainer qb = f.AddQuestion("Willy.BooleanQuestion, Willy");
            IQuestionContainer qf = f.AddQuestion("Willy.QuestionFolder, Willy");
            if (typeof(Willy.QuestionFolder) == qf)
            {
                Assert.Fail("it is not a folder. Final Point.");
            }

            IQuestionFolderContainer folder = (IQuestionFolderContainer)qf;

            Assert.That(qb.Index == 0);
            Assert.That(folder.Index == 1);

            IQuestionContainer qif = f.AddQuestion("Willy.BooleanQuestion, Willy", folder);
            Assert.That(qif.Index == 0);

            Assert.That(f.RemoveQuestion(qif, false, f) == true);
            qif = f.AddQuestion("Willy.BooleanQuestion, Willy", folder);
            IQuestionContainer qif2 = f.AddQuestion("Willy.BooleanQuestion, Willy", folder);

            Assert.That(f.RemoveQuestion(folder, false) == true);
            Assert.That(f.Questions.Count == 3);
            Assert.That(qif.Index == 1);
        }

        [Test]
        public void TestWithQuestionFolderRemoveChildren()
        {
            Willy.Form f = new Willy.Form("sinthu");
            IQuestionContainer qb = f.AddQuestion("Willy.BooleanQuestion, Willy");
            IQuestionContainer qf = f.AddQuestion("Willy.QuestionFolder, Willy");
            if (typeof(Willy.QuestionFolder) == qf)
            {
                Assert.Fail("it is not a folder. Final Point.");
            }

            IQuestionFolderContainer folder = (IQuestionFolderContainer)qf;

            Assert.That(qb.Index == 0);
            Assert.That(folder.Index == 1);

            IQuestionContainer qif = f.AddQuestion("Willy.BooleanQuestion, Willy", folder);
            Assert.That(qif.Index == 0);

            Assert.That(f.RemoveQuestion(qif, false, f) == true);
            qif = f.AddQuestion("Willy.BooleanQuestion, Willy", folder);
            IQuestionContainer qif2 = f.AddQuestion("Willy.BooleanQuestion, Willy", folder);

            Assert.That(f.RemoveQuestion(folder, true) == true);
            Assert.That(f.Questions.Count == 1);
        }

        [Test]
        public void TestCommandPattern()
        {
            Willy.Form f = new Willy.Form("sinthu");
            IQuestionContainer qb = f.AddQuestion("Willy.BooleanQuestion, Willy");

            Willy.FormAnswer a = f.CreateAnswer("coucou");
            a.CreateAnswer((Willy.BooleanQuestion)qb);

            Willy.AnswerBase answer = a.FindAnswer(qb);
            Assert.That(((Willy.BooleanAnswer)answer).Answer == false);
            answer.Execute();
            Assert.That(((Willy.BooleanAnswer)answer).Answer == true);
        }

        [Test]
        public void TestMemento()
        {
            Willy.Form f = new Willy.Form("sinthu");
            IQuestionContainer qb = f.AddQuestion("Willy.BooleanQuestion, Willy");

            Willy.FormAnswer a = f.CreateAnswer("coucou");
            a.CreateAnswer((Willy.BooleanQuestion)qb);

            Willy.AnswerBase answer = a.FindAnswer(qb);

            Willy.CareTaker ct = new Willy.CareTaker();
            Assert.That(ct.ObjectsCount == 0);
            ct.AddMemento(answer.SaveToMemento());
            Assert.That(ct.ObjectsCount == 1);
            answer.Execute();
            Assert.That(((Willy.BooleanAnswer)answer).Answer == true);
            ct.AddMemento(answer.SaveToMemento());
            Assert.That(ct.ObjectsCount == 2);
            answer.RestoreFromMemento(ct.GetMemento(0));
            Assert.That(((Willy.BooleanAnswer)answer).Answer == false);
        }

        [Test]
        public void SerializationTest()
        {
            Willy.Form f = new Willy.Form("sinthu");
            IQuestionContainer qb = f.AddQuestion("Willy.BooleanQuestion, Willy");
            Willy.FormAnswer a = f.CreateAnswer("coucou");
            a.CreateAnswer((Willy.BooleanQuestion)qb);
            Willy.AnswerBase answer = a.FindAnswer(qb);
            answer.Execute();

            string fName = Path.Combine(TestHelper.TestSupportFolder.FullName, "TP_SPI.dat");
            using (FileStream fs = File.OpenWrite(fName))
            {
                new BinaryFormatter().Serialize(fs, f);
            }

            Willy.Form f2;
            using (FileStream fs = File.OpenRead(fName))
            {
                 f2 = (Willy.Form)new BinaryFormatter().Deserialize(fs);
            }

            Assert.That(f2.Questions.Count == f.Questions.Count);
            Assert.That(f2.Title == "sinthu");
        }

        [Test]
        public void FactoryTest()
        {
            Willy.Factory factory = Willy.Factory.GetFactory();
            Assert.That(factory != null && factory is Willy.FormFactory);

            Willy.Form f = factory.CreateQuestionForm("sinthu");
            Assert.That(f != null && f.Title == "sinthu");

            Willy.FormAnswer a = factory.CreateAnswerForm("coucou");
            Assert.That(a != null && a.Name == "coucou");
        }

    }
}
