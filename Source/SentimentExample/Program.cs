using System;
using Ether.TextAnalysis;
using Console = System.Console;

namespace SentimentExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var analyzer = new SentimentAnalyzer();
            analyzer.TrainAndConfigure(); //pre-train, for speed.

            Console.WriteLine();

            //Analyze hand-typed input unti "exit" is recognized.
            var input = "";

            while (input != "exit")
            {
                input = AskInput();
                HandleInput(input, analyzer);
            }
        }


        /// <summary>
        /// Prompts the user for input.
        /// </summary>
        static string AskInput()
        {
            Console.WriteLine();
            Console.WriteLine("Enter text, 'exit' quits.");
            return Console.ReadLine();
        }

        /// <summary>
        /// This is where the analysis happens.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sentimentAnalyzer"></param>
        static void HandleInput(string text, SentimentAnalyzer sentimentAnalyzer)
        {
            var parseOutcome = sentimentAnalyzer.Parse(text);

            //Handle failure
            if (parseOutcome.Failure)
            {
                var messages = parseOutcome.ToMultiLine(Environment.NewLine);
                Console.WriteLine(messages);
                return;
            }

            //Handle success
            var result = parseOutcome.Value;

            Console.WriteLine("Sentiment: " + result.SentimentText());
            Console.WriteLine("Mentions: " + string.Join(", ", result.MentionedEntities));
        }

    }
}
