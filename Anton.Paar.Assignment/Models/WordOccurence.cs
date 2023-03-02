using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anton.Paar.Assignment.Models
{
    public class WordOccurrence
    {
        private string _word;
        private int _occurrences;

        public string Word
        {
            get { return _word; }
            set
            {
                if (_word != value)
                {
                    _word = value;
                }
            }
        }

        public int Occurrences
        {
            get { return _occurrences; }
            set
            {
                if (_occurrences != value)
                {
                    _occurrences = value;
                }
            }
        }

        public void Increment()
        {
            Occurrences++;
        }
    }
}
