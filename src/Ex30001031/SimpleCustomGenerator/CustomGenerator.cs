using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Text;

namespace SimpleCustomGenerator
{
    [Generator]
    public class CustomGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("MyGeneratedFile.cs", SourceText.From(@"
namespace GeneratedNamespace
{
    public class GeneratedClass
    {
        public static void GeneratedMethod()
        {
            // Here we go....
            // generated code
            System.Console.WriteLine(""Sri Ram Jai Ram Jai Jai Ram..."");
        }
    }
}", Encoding.UTF8));
        }
    }
}
