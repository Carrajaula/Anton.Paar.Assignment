using Anton.Paar.Assignment.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Anton.Paar.Assignment.Services
{
    public class FileWordCounterService : IFileWordCounterService
    {
        private readonly IBufferedFileToWordListReaderService _BufferedFileToWordListReaderService;
        private readonly IProgress<int> _progress;
        private readonly IWordOccurrences _wordOccurrences;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public FileWordCounterService(IBufferedFileToWordListReaderService BufferedFileToWordListReaderService, IWordOccurrences wordOccurrences, IProgress<int> progress, CancellationTokenSource cancellationTokenSource)
        {
            _BufferedFileToWordListReaderService = BufferedFileToWordListReaderService;
            _wordOccurrences = wordOccurrences;
            _progress = progress;
            _cancellationTokenSource = cancellationTokenSource;
        }

        public async Task ProcessFileAsync()
        {
            try
            {
                await Task.Run(async () =>
                {
                    var totalWords = 0;
                    var processedWords = 0;

                    List<string> words = (List<string>)await _BufferedFileToWordListReaderService.ParseAsync(_progress, _cancellationTokenSource.Token);

                    if (words != null)
                    {
                        foreach (var word in words)
                        {
                            totalWords++;

                            if (_cancellationTokenSource.IsCancellationRequested)
                            {
                                throw new TaskCanceledException();
                            }

                            _wordOccurrences.AddWord(word);

                            processedWords++;

                            _progress.Report((int)((double)processedWords / words.Count * 100));
                        }
                    }

                    _progress.Report(100);
                });
            }
            catch (OperationCanceledException)
            {
                throw new TaskCanceledException();
            }
        }

        public void CancelProcessing()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
