using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anton.Paar.Assignment.Interfaces
{
    public interface IBufferedFileToWordListReaderService
    {
        Task<IEnumerable<string>> ParseAsync(IProgress<int> progress = null, CancellationToken cancellationToken = default);
    }
}
