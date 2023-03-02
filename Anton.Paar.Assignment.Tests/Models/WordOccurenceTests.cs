using Anton.Paar.Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anton.Paar.Assignment.Tests.Models
{
    [TestFixture]
    public class WordOccurrenceTests
    {

        [Test]
        public void Occurrences_InitialValueIsZero()
        {
            // Arrange.
            var wordOccurrence = new WordOccurrence();

            // Act.
            var occurrences = wordOccurrence.Occurrences;

            // Assert.
            Assert.AreEqual(0, occurrences);
        }
    }
}
