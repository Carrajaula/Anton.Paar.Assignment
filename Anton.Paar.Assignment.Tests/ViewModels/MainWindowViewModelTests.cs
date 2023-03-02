using Anton.Paar.Assignment.ViewModels;
using Microsoft.Win32;
using Moq;
using System.Windows;

namespace Anton.Paar.Assignment.Tests.ViewModels
{
    [TestFixture]
    public class MainWindowViewModelTests
    {
        private MainWindowViewModel _viewModel;
        private Application _app;

        [OneTimeSetUp]
        public void SetUp()
        {
            _app = new Application();
            _viewModel = new MainWindowViewModel();
        }

        [Test]
        public void CanProcessFile_WhenFileSelected_ReturnsTrue()
        {
            // Arrange.
            _viewModel.FilePath = "test.txt";

            // Act.
            var canProcessFile = _viewModel.CanProcessFile();

            // Assert.
            Assert.IsTrue(canProcessFile);
        }

        [Test]
        public void CanProcessFile_WhenProcessingInProgress_ReturnsFalse()
        {
            // Arrange.
            _viewModel.IsProcessing = true;

            // Act.
            var canProcessFile = _viewModel.CanProcessFile();

            // Assert.
            Assert.IsFalse(canProcessFile);
        }

        [Test]
        public void CanCancelProcessing_WhenProcessingInProgress_ReturnsTrue()
        {
            // Arrange.
            _viewModel.IsProcessing = true;

            // Act.
            var canCancelProcessing = _viewModel.CanCancelProcessing();

            // Assert.
            Assert.IsTrue(canCancelProcessing);
        }

        [Test]
        public void CanCancelProcessing_WhenProcessingNotInProgress_ReturnsFalse()
        {
            // Arrange.
            _viewModel.IsProcessing = false;

            // Act.
            var canCancelProcessing = _viewModel.CanCancelProcessing();

            // Assert.
            Assert.IsFalse(canCancelProcessing);
        }

        [Test]
        public async Task ProcessFile_WhenFileSelectedAndNotInProgress_UpdatesWordOccurrencesList()
        {
            // Arrange.
            _viewModel.FilePath = "testfile.txt";

            // Act.
            _viewModel.ProcessFile();
            await Task.Delay(500); // Wait for processing to complete.

            // Assert.
            Assert.IsNotNull(_viewModel.WordOccurrencesList);
        }

        [Test]
        public async Task ProcessFile_WhenFileSelectedAndNotInProgress_UpdatesIsProcessing()
        {
            // Arrange.
            _viewModel.FilePath = "testfile.txt";

            // Act.
            _viewModel.ProcessFile();
            await Task.Delay(500); // Wait for processing to complete.

            // Assert.
            Assert.IsFalse(_viewModel.IsProcessing);
        }

        [Test]
        public async Task ProcessFile_WhenFileSelectedAndNotInProgress_UpdatesIsCancelVisible()
        {
            // Arrange.
            _viewModel.FilePath = "testfile.txt";

            // Act.
            _viewModel.ProcessFile();
            await Task.Delay(500); // Wait for processing to complete.

            // Assert.
            Assert.IsFalse(_viewModel.IsCancelVisible);
        }

        [Test]
        public async Task CancelProcessing_WhenProcessingInProgress_UpdatesIsProcessing()
        {
            // Arrange.
            _viewModel.IsProcessing = true;

            // Act.
            _viewModel.CancelProcessing();
            await Task.Delay(500); // Wait for processing to complete.

            // Assert.
            Assert.IsFalse(_viewModel.IsProcessing);
        }
    }
}