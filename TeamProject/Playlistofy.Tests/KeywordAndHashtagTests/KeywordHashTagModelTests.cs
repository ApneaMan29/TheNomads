using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Playlistofy.Models;

namespace Playlistofy.Tests.KeywordAndHashtagTests
{
    class KeywordHashTagModelTests
    {
        [Test]
        public void HashTag_DefaultModelIs_NotValid()
        {
            Hashtag t = new Hashtag();

            ModelValidator mv = new ModelValidator(t);

            Assert.That(mv.Valid, Is.False);
        }

        [Test]
        public void Hashtag_StartsWithHashSymbolReturns_Valid()
        {
            Hashtag t = new Hashtag()
            {
                Id = 1,
                HashTag1 = "#Hash"
            };

            ModelValidator mv = new ModelValidator(t);

            Assert.That(mv.Valid, Is.True);
        }

        [Test]
        public void Hashtag_StartsWithAReturns_NotVaild()
        {
            Hashtag t = new Hashtag()
            {
                Id = 1,
                HashTag1 = "Assumptions"
            };

            ModelValidator mv = new ModelValidator(t);

            Assert.That(mv.Valid, Is.False);
        }

        [Test]
        public void Keyword_DefaultModelIs_NotValid()
        {
            Keyword t = new Keyword();

            ModelValidator mv = new ModelValidator(t);

            Assert.That(mv.Valid, Is.False);
        }

        [Test]
        public void Keyword_StartsWithAReturns_Vaild()
        {
            Keyword t = new Keyword()
            {
                Id = 1,
                Keyword1 = "Assumptions"
            };

            ModelValidator mv = new ModelValidator(t);

            Assert.That(mv.Valid, Is.True);
        }

        [Test]
        public void Keyword_StartsWithHashSymbolReturns_NotVaild()
        {
            Keyword t = new Keyword()
            {
                Id = 1,
                Keyword1 = "#Assumptions"
            };

            ModelValidator mv = new ModelValidator(t);

            Assert.That(mv.Valid, Is.False);
        }
    }
}
