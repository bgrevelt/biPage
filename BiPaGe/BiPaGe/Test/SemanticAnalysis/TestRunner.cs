using System;
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
    }
}
