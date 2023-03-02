using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anton.Paar.Assignment.Models;

namespace Anton.Paar.Assignment.Interfaces
{
    public interface IWordOccurrences
    {
        List<WordOccurrence> Table { get; }
        void AddWord(string word);
    }
}
