using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.TextAnalysis
{
    public class AnalysisResult
    {
        public Sentiments Sentiment = Sentiments.Neutral;
        public List<string> MentionedEntities = new List<string>();

        /// <summary>
        /// Plain-English sentiment representation.
        /// </summary>
        public string SentimentText()
        {

            //The SentimentScore enum is really plain english, except it has underscores instead of spaces.
            return Sentiment.ToString() 
                                 .Replace("_", " ");
        }
    }
}
