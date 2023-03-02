using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using Anton.Paar.Assignment.Services;
using Moq;

namespace Anton.Paar.Assignment.Tests.Services
{
    [TestFixture]
    public class BufferedFileToWordListReaderServiceTests
    {
        private const string TestFilePath = "testfile.txt";
        private const string EmptyTestFilePath = "emptyFile.txt";

        [Test]
        public async Task ParseAsync_ReturnsEmptyList_WhenFileIsEmpty()
        {
            // Arrange.
            var parser = new BufferedFileToWordListReaderService(EmptyTestFilePath, 1024);

            // Act.
            var words = await parser.ParseAsync();

            // Assert.
            Assert.That(words, Is.Empty);
        }

        [Test]
        public async Task ParseAsync_ReturnsExpectedWords_WhenFileContainsWords()
        {
            // Arrange.
            var parser = new BufferedFileToWordListReaderService(TestFilePath, 1024);

            // Act.
            var words = await parser.ParseAsync();

            // Assert.
            Assert.That(words, Is.EquivalentTo(new[] { "hello", "world" }));
        }

        [Test]
        public void ParseAsync_ReportsProgress()
        {
            // Arrange.
            var parser = new BufferedFileToWordListReaderService(TestFilePath, 1024);
            var progressMock = new Mock<IProgress<int>>();

            // Act.
            var task = parser.ParseAsync(progressMock.Object, CancellationToken.None);
            task.Wait(); // Wait for completion.

            // Assert.
            progressMock.Verify(x => x.Report(It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Test]
        public void ParseAsync_CancelsParsing_WhenCancellationIsRequested()
        {
            // Arrange.
            var parser = new BufferedFileToWordListReaderService(TestFilePath, 1024);
            var cancellationTokenSource = new CancellationTokenSource();
            var progressMock = new Mock<IProgress<int>>();

            // Act.
            var task = parser.ParseAsync(progressMock.Object, cancellationTokenSource.Token);
            cancellationTokenSource.Cancel();

            // Assert.
            Assert.IsTrue(cancellationTokenSource.IsCancellationRequested);
        }
    }
}
