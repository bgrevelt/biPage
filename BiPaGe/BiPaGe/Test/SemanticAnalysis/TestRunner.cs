using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using Helpers;
using NUnit.Framework;

namespace BiPaGe.Test.SemanticAnalysis
{
    public class TestRunner
    {
        public TestRunner()
        {
        }

        public static void Run(string input, int expectedNumberOfWarnings, int expectedNumberOfErrors)
        {
            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            int numberOfWarnings = 0;
            int numberOfErrors = 0;
            foreach(var e in events)
            {
                if (e is Warning)
                    numberOfWarnings++;
                else if (e is Error)
                    numberOfErrors++;
            }

            Assert.AreEqual(expectedNumberOfWarnings, numberOfWarnings);
            Assert.AreEqual(expectedNumberOfErrors, numberOfErrors);         
        }

        public static void Run(string input, List<(string, int, int, string)> testset)
        {
            foreach(var test in testset)
            {
                var AST = SimpleBuilder.Build(input.Replace("VALUE", test.Item1));
                BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
                var events = analyzer.CheckSemantics(AST);

                int numberOfWarnings = 0;
                int numberOfErrors = 0;
                foreach (var e in events)
                {
                    if (e is Warning)
                        numberOfWarnings++;
                    else if (e is Error)
                        numberOfErrors++;
                }

                Assert.AreEqual(test.Item2, numberOfWarnings, test.Item4);
                Assert.AreEqual(test.Item3, numberOfErrors, test.Item4);
            }
        }
    }
}
