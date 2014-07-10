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
            Assert.That(f.Title == "sinthu");

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

    }
}
