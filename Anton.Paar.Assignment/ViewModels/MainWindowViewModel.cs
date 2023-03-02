using Anton.Paar.Assignment.Commands;
using Anton.Paar.Assignment.Interfaces;
using Anton.Paar.Assignment.Models;
using Anton.Paar.Assignment.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Anton.Paar.Assignment.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IBufferedFileToWordListReaderService _BufferedFileToWordListReaderService;
        private readonly IWordOccurrences _wordOccurrences;
        private IFileWordCounterService _FileWordCounterService;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public ICommand ProcessFileCommand { get; }
        public ICommand CancelProcessCommand { get; }


        public MainWindowViewModel()
        {
            _BufferedFileToWordListReaderService = new BufferedFileToWordListReaderService(FilePath, 1024);
            _wordOccurrences = new WordOccurrences();
            _FileWordCounterService = new FileWordCounterService(_BufferedFileToWordListReaderService, _wordOccurrences, new Progress<int>(UpdateProgressBar), _cancellationTokenSource);
            ProcessFileCommand = new ProcessFileCommand(this);
            CancelProcessCommand = new CancelProcessCommand(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string FilePath { get; set; }

        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }

        private bool _isProcessing;
        public bool IsProcessing
        {
            get { return _isProcessing; }
            set
            {
                _isProcessing = value;
                OnPropertyChanged(nameof(IsProcessing));
                OnPropertyChanged(nameof(CanCancelProcessing));
            }
        }

        private List<WordOccurrence> _wordOccurrencesList;
        public List<WordOccurrence> WordOccurrencesList
        {
            get { return _wordOccurrencesList; }
            set
            {
                _wordOccurrencesList = value;
                OnPropertyChanged(nameof(WordOccurrencesList));
            }
        }

        private bool _isCancelVisible;
        public bool IsCancelVisible
        {
            get => _isCancelVisible;
            set
            {
                _isCancelVisible = value;
                OnPropertyChanged(nameof(IsCancelVisible));
            }
        }

        public bool CanProcessFile()
        {
            // Return true if file is selected and processing is not currently in progress.
            return !IsProcessing;
        }

        public async void ProcessFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Select a text file",
                Multiselect = false
            };

            if (FilePath == string.Empty || FilePath == null)
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    UpdateProgressBar(0); //Reset the progress.
                    _BufferedFileToWordListReaderService = new BufferedFileToWordListReaderService(openFileDialog.FileName, 1024);
                    _FileWordCounterService = new FileWordCounterService(_BufferedFileToWordListReaderService, _wordOccurrences, new Progress<int>(UpdateProgressBar), _cancellationTokenSource);
                }
            }
            else
            {
                UpdateProgressBar(0); //Reset the progress.
                _BufferedFileToWordListReaderService = new BufferedFileToWordListReaderService(FilePath, 1024);
                _FileWordCounterService = new FileWordCounterService(_BufferedFileToWordListReaderService, _wordOccurrences, new Progress<int>(UpdateProgressBar), _cancellationTokenSource);
            }
            IsProcessing = IsCancelVisible = true;
            CommandManager.InvalidateRequerySuggested(); // Force update of command.
            WordOccurrencesList = null;
            _wordOccurrences.Table.Clear();

            try
            {
                await _FileWordCounterService.ProcessFileAsync();
            }
            catch (OperationCanceledException)
            {
                // User canceled processing
            }
            finally
            {
                IsProcessing = IsCancelVisible = false;
            }

            WordOccurrencesList = new List<WordOccurrence>(_wordOccurrences.Table.OrderByDescending(w => w.Occurrences));
        }

        public bool CanCancelProcessing()
        {
            // Return true if processing is currently in progress.
            return IsProcessing;
        }

        public void CancelProcessing()
        {
            _FileWordCounterService.CancelProcessing();
            IsProcessing = IsCancelVisible = false;
        }

        private void OnProcessingCompleted(object sender, EventArgs e)
        {
            WordOccurrencesList = new List<WordOccurrence>(_wordOccurrences.Table.OrderByDescending(w => w.Occurrences));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateProgressBar(int value)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Progress = value;
            });
        }
    }
}
