using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;

namespace SimpleCustomGenerator.xUnitTests
{
    public class HelloGeneratorTests
    {
        [Fact]
        public async Task HelloGeneratorTest()
        {
            var code = string.Empty;
            var generatedCode = @"namespace HelloGenerated
{
  public class HelloGenerator
  {
    public static void Test() => System.Console.WriteLine(""Hello Generator"");
  }
}";
            var tester = new CSharpSourceGeneratorTest<HelloGenerator, XUnitVerifier>()
            {
                TestState =
                {
                    Sources = { code },
                    GeneratedSources =
                    {
                        (typeof(HelloGenerator), $"{nameof(HelloGenerator)}.cs", SourceText.From(generatedCode, Encoding.UTF8)),
                    }
                },
            };

            await tester.RunAsync();
        }
    }
}
