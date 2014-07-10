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
        public void CreateQuestion()
        {
            Willy.Form f = new Willy.Form("sinthu");
            Assert.That(f.Title == "sinthu");

            IQuestionContainer qb = f.AddQuestion("Willy.BooleanQuestion, Willy");
            Assert.IsNotNull(qb);
        }

    }
}
