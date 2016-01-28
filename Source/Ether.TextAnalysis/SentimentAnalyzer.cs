using System;
using edu.stanford.nlp.pipeline;
using Ether.Outcomes;
using Ether.TextAnalysis.Internals;

namespace Ether.TextAnalysis
{
    public class SentimentAnalyzer
    {
        private StanfordCoreNLP Pipeline;
        private bool IsTrainedAndReady = false;

        /// <summary>
        /// Training and configuration takes several seconds. Training will happen automatically if you just call parse, but I've broken 
        /// this out to enable front-loading. this way, you can train once and re-use the instance. Or perhaps build a singleton.
        /// </summary>
        public void TrainAndConfigure()
        {
            var config = new PipelineConfigurator();

            Pipeline = config.GetPipeline();
            IsTrainedAndReady = true;  
        }

        /// <summary>
        /// Execute our analysis, producing an Outcome object with an AnalysisResult in it.
        /// </summary>
        public IOutcome<AnalysisResult> Parse(string text)
        {
            if (!IsTrainedAndReady) 
                TrainAndConfigure();

            var ta = new SentimentService();

            return ta.Analyze(Pipeline, text);
        }
    }
}
