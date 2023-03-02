using Anton.Paar.Assignment.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anton.Paar.Assignment.Services
{
    public class BufferedFileToWordListReaderService : IBufferedFileToWordListReaderService
    {
        private readonly string _filePath;
        private readonly int _chunkSize;

        public BufferedFileToWordListReaderService(string filePath, int chunkSize)
        {
            _filePath = filePath;
            _chunkSize = chunkSize;
        }

        public async Task<IEnumerable<string>> ParseAsync(IProgress<int> progress = null, CancellationToken cancellationToken = default)
        {
            var words = new List<string>();

            if (_filePath == string.Empty || _filePath == null)
                return null;

            using (var fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read, _chunkSize, true))
            using (var streamReader = new StreamReader(fileStream, Encoding.Default, true, _chunkSize))
            {
                var buffer = new char[_chunkSize];

                while (!streamReader.EndOfStream && !cancellationToken.IsCancellationRequested)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    var bytesRead = await streamReader.ReadAsync(buffer, 0, _chunkSize);

                    for (var i = 0; i < bytesRead; i++)
                    {
                        if (char.IsWhiteSpace(buffer[i]))
                        {
                            var word = new string(buffer.Take(i).ToArray());

                            if (!string.IsNullOrWhiteSpace(word))
                            {
                                words.Add(word.ToLower());
                            }

                            // Advance buffer to next word.
                            buffer = buffer.Skip(i + 1).Concat(new char[_chunkSize - i - 1]).ToArray();
                            i = -1; // Reset index to -1 because of loop increment.
                        }
                    }

                    progress?.Report((int)(fileStream.Position * 100 / fileStream.Length));
                }
            }

            return words;
        }
    }
}
