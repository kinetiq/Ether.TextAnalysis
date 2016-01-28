using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.TextAnalysis.Internals
{
    internal class ScoredSentence
    {
        public decimal Score { get; set; }
        public string Sentence { get; set; }

        /// <summary>
        /// Calculate WeightedScore, where the sentence's score is weighted according to its proportion of the
        /// total text.
        /// </summary>
        public decimal WeightedScore(int totalText)
        {
            decimal proportion = (Sentence.Length == 0 || totalText == 0) ? 0 : decimal.Divide(Sentence.Length, totalText);
            var weightedScore = proportion*Score;

            return weightedScore;
        }
    }
}
