using Anton.Paar.Assignment.Interfaces;
using Anton.Paar.Assignment.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Anton.Paar.Assignment.Tests.Services
{
    [TestFixture]
    public class FileWordCounterServiceTests
    {
        [Test]
        public async Task ProcessFileAsync_AddsWordsToWordOccurrences()
        {
            // Arrange
            var BufferedFileToWordListReaderService = new Mock<IBufferedFileToWordListReaderService>();
            var wordOccurrences = new Mock<IWordOccurrences>();
            var progress = new Progress<int>();
            var cancellationTokenSource = new CancellationTokenSource();
            var wordCounter = new FileWordCounterService(BufferedFileToWordListReaderService.Object, wordOccurrences.Object, progress, cancellationTokenSource);

            var words = new List<string> { "hello", "world", "hello", "world", "world" };
            BufferedFileToWordListReaderService.Setup(fp => fp.ParseAsync(progress, cancellationTokenSource.Token)).ReturnsAsync(words.AsEnumerable());

            // Act
            await wordCounter.ProcessFileAsync();

            // Assert
            wordOccurrences.Verify(wo => wo.AddWord("hello"), Times.Exactly(2));
            wordOccurrences.Verify(wo => wo.AddWord("world"), Times.Exactly(3));
        }

        [Test]
        public void CancelProcessing_CancelsProcessing()
        {
            // Arrange
            var BufferedFileToWordListReaderService = new Mock<IBufferedFileToWordListReaderService>();
            var wordOccurrences = new Mock<IWordOccurrences>();
            var progress = new Progress<int>();
            var cancellationTokenSource = new CancellationTokenSource();
            var wordCounter = new FileWordCounterService(BufferedFileToWordListReaderService.Object, wordOccurrences.Object, progress, cancellationTokenSource);

            // Act
            wordCounter.CancelProcessing();

            // Assert
            Assert.IsTrue(cancellationTokenSource.IsCancellationRequested);
        }
    }
}
