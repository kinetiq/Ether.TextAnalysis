using System;
using System.Collections.Generic;
using System.Linq;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.neural.rnn;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.sentiment;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.util;
using Ether.Outcomes;
using ikvm.extensions;
using java.util;

namespace Ether.TextAnalysis.Internals
{
    internal class SentimentService
    {
        /// <summary>
        /// Executes Sentiment and EntitiesMentioned analysis.
        /// </summary>
        public IOutcome<AnalysisResult> Analyze(StanfordCoreNLP pipeline, string text)
        {
            //Create annotated document
            Annotation doc = new Annotation(text);
            pipeline.annotate(doc);

            //Validate
            var sentences = doc.get(typeof(CoreAnnotations.SentencesAnnotation));

            if (sentences == null)           
                return Outcomes.Outcomes
                               .Failure<AnalysisResult>()
                               .WithMessage("No sentences detected.");

            //Analyze
            var result = new AnalysisResult()
            {
                Sentiment = GetSentiment((ArrayList)sentences),
                MentionedEntities = GetMentions(doc)
            };

            return Outcomes.Outcomes
                           .Success<AnalysisResult>()
                           .WithValue(result);
        }

        /// <summary>
        /// Sentiment is scored on a sentence-by-sentence basis. In order to provide a sentiment
        /// score for the entire block text, I rolled my own algorithm that weights each sentence's
        /// score according to how much of the total text it encompasses.
        /// </summary>
        private Sentiments GetSentiment(ArrayList sentences) 
        {
            var weightedSentences = new List<ScoredSentence>();

            foreach (CoreMap sentence in sentences)
            {
                var weightedSentence = new ScoredSentence()
                {
                    Score = GetSentiment(sentence),
                    Sentence = sentence.toString()
                };

                weightedSentences.Add(weightedSentence);
            }
                               
            return CalculatedWeightedSentiment(weightedSentences);
        }

        /// <summary>
        /// Take each sentences, dillute the score according to this sentence's proportion of the total
        /// length, and then round down to the nearest whole number. That's our score. 0 is most negative,
        /// 4 is most positive.
        /// </summary>
        private Sentiments CalculatedWeightedSentiment(List<ScoredSentence> sentences)
        {
            var totalLength = sentences.Sum(s => s.Sentence.length());
            decimal averageScore = sentences.Sum(s => s.WeightedScore(totalLength));
            int finalScore =  (int) Math.Floor(averageScore);

            return (Sentiments) finalScore;
        }
          
        /// <summary>
        /// Pull the Sentiment annotation for this sentence.
        /// </summary>
        private int GetSentiment(CoreMap sentence)
        {
            var tree = (Tree)sentence.get(typeof(SentimentCoreAnnotations.SentimentAnnotatedTree)); //pull the annotated tree
            var sentiment = RNNCoreAnnotations.getPredictedClass(tree); //a score between 0-4, higher being more positive.  

            return sentiment;
        }

        /// <summary>
        /// Convert our mentions into a list of strings.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private List<string> GetMentions(Annotation doc)
        {
            var mentions = (ArrayList) doc.get(typeof(CoreAnnotations.MentionsAnnotation));
            var results = new List<string>();

            foreach (CoreMap mention in mentions)
            {
                var entityType = mention.get(typeof(CoreAnnotations.EntityTypeAnnotation));

                //Only get People!
                if (entityType.ToString() == "PERSON" && !results.Contains(mention.ToString()))
                    results.Add(mention.toString());
            }

            results.Sort();

            return results;
        }
    }
}
