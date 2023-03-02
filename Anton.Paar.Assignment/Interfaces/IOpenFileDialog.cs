using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anton.Paar.Assignment.Interfaces
{
    public interface IOpenFileDialog
    {
        bool? ShowDialog();
        string FileName { get; }
    }
}
