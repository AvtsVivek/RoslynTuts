using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleClassGenerator
{
    [Generator]
    public class SimpleClassGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var compilation = context.Compilation;

            var mainMethod = compilation.GetEntryPoint(context.CancellationToken);
            var namespaceName = mainMethod.ContainingNamespace.ToDisplayString();

            var namespaceNameStatement = "";
            
            if (namespaceName == "<global namespace>" || string.IsNullOrWhiteSpace(namespaceName))
            {
                namespaceNameStatement = String.Empty;
            }
            else
            {
                namespaceNameStatement = "namespace " + namespaceName;
            }
            
            var logSrc = $@"
using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

"
+ namespaceNameStatement + 
$@"

[GeneratedCode(""LogAttribute"", ""x.x.x"")] // Check the namespace and version
[CompilerGenerated]
public class LogAttribute : Attribute {{ }}";

            var sourceText = SourceText.From(logSrc, Encoding.UTF8);

            context.AddSource("Loggg.cs", sourceText);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
