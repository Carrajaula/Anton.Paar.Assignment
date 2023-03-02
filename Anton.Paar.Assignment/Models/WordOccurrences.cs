using Anton.Paar.Assignment.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anton.Paar.Assignment.Models
{
    public class WordOccurrences : IWordOccurrences
    {
        public List<WordOccurrence> Table { get; } = new List<WordOccurrence>();

        public void AddWord(string word)
        {
            var existingWord = Table.FirstOrDefault(x => x.Word == word);
            if (existingWord != null)
            {
                existingWord.Occurrences++;
            }
            else
            {
                Table.Add(new WordOccurrence { Word = word, Occurrences = 1 });
            }
        }
    }
}
