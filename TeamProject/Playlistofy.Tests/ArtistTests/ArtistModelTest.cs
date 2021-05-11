using NUnit.Framework;
using Playlistofy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playlistofy.Tests.ArtistTests
{
    class ArtistModelTest
    {
        [Test]
        public void Artist_Model_Default_Is_Valid()
        {
            Artist a = new Artist();

            ModelValidator mv = new ModelValidator(a);

            Assert.That(mv.Valid, Is.False);
        }

        [Test]
        public void ArtistModelAttributes_FulfilledRequirmentsShould_BeVaild()
        {
            Artist a = new Artist()
            {
                Id = "987456321",
                Name = "Hillbilly Tom",
                Popularity = 17
            };

            ModelValidator mv = new ModelValidator(a);

            Assert.That(mv.Valid, Is.True);
        }
    }
}