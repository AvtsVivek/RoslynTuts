using Xunit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions;

namespace SimpleCustomGenerator.xUnitTests
{
    public class CustomGeneratorTests
    {
        [Fact]
        public void SimpleGeneratorTest()
        {
            // Create the 'input' compilation that the generator will act on
            Compilation inputCompilation = CreateCompilation(@"
namespace MyCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
        }
    }
}
");

            // directly create an instance of the generator
            // (Note: in the compiler this is loaded from an assembly, and created via reflection at runtime)
            CustomGenerator generator = new CustomGenerator();

            // Create the driver that will control the generation, passing in our generator
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            // Run the generation pass
            // (Note: the generator driver itself is immutable, and all calls return an updated version of the driver that you should use for subsequent calls)
            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

            // We can now assert things about the resulting compilation:
            diagnostics.Count().Should().Be(0); // there were no diagnostics created by the generators  

            outputCompilation.SyntaxTrees.Count().Should().Be(2); // we have two syntax trees, the original 'user' provided one, and the one added by the generator

            var diag = outputCompilation.GetDiagnostics().FirstOrDefault();
            
            outputCompilation.GetDiagnostics().Count().Should().Be(0); // verify the compilation with the added source has no diagnostics

            // Or we can look at the results directly:
            GeneratorDriverRunResult runResult = driver.GetRunResult();

            // The runResult contains the combined results of all generators passed to the driver
            runResult.GeneratedTrees.Length.Should().Be(1);
            runResult.Diagnostics.Should().BeEmpty();

            // Or you can access the individual results on a by-generator basis
            GeneratorRunResult generatorResult = runResult.Results[0];
            generatorResult.Generator.Should().Be(generator);
            generatorResult.Diagnostics.Should().BeEmpty();
            generatorResult.GeneratedSources.Length.Should().Be(1);
            var generatedText = generatorResult.GeneratedSources[0].SourceText; // get the text of the generated source
            generatedText.ToString().Should().Be(SourceText); // verify the generator added the expected text
            generatorResult.Exception.Should().BeNull();
        }

        private static Compilation CreateCompilation(string source)
        {
            var location = MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location);
            var compilation = CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { location },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
            return compilation;
        }


        private static string SourceText = @"
namespace GeneratedNamespace
{
    public class GeneratedClass
    {
        public static void GeneratedMethod()
        {
            // generated code
            System.Console.WriteLine(""Hello..."");
        }
    }
}";

    }
}