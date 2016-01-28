using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using edu.stanford.nlp.pipeline;
using java.util;

namespace Ether.TextAnalysis
{
    internal class PipelineConfigurator
    {
        /// <summary>
        /// Creates the StanfordCoreNLP instance, which annotates chunks of text. We then use the
        /// annotations to perform our analysis. The pipeline can be configured with various annotators;
        /// we use only the ones we need, for perf reasons.
        /// </summary>
        public StanfordCoreNLP GetPipeline()
        {
            var modelFolder = ConfigurationManager.AppSettings["CoreNLP.ModelDirectory"];

            ValidateModelFolder(modelFolder);

            //Switch the current directory momentarily so Pipeline can find the models. 
            //Wish there was a better way.
            var currentDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(modelFolder);

            var props = GetPipelineProperties();
            var pipeline = new StanfordCoreNLP(props);

            Directory.SetCurrentDirectory(currentDir); //Switch it back.
          
            return pipeline;
        }

        #region "Helpers"
        /// <summary>
        /// StanfordCoreNLP requires a model jar file. This AppSetting should point to
        /// containing folder. 
        /// </summary>
        private void ValidateModelFolder(string modelFolder)
        {
            if (string.IsNullOrWhiteSpace(modelFolder))
                throw new InvalidOperationException("The AppSetting CoreNLP.ModelDirectory was not found. " +
                                                    "This must be set to a folder containing the CoreNLP models jar, " +
                                                    "which can be downloaded here: http://stanfordnlp.github.io/CoreNLP/");

            if (!Directory.Exists(modelFolder))
                throw new InvalidOperationException("CoreNLP.ModelDirectory: directory not found.");
        }

        /// <summary>
        /// Annotation pipeline configuration. You can easily add more annotators here.
        /// </summary>
        /// <returns></returns>
        private Properties GetPipelineProperties()
        {            
            var props = new Properties();
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, parse, ner, entitymentions, sentiment");
            props.setProperty("ner.useSUTime", "0"); //Turns off NER's SUTime component.

            return props; 
        }
        #endregion
    }
}
