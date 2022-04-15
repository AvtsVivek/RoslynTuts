using Microsoft.CodeAnalysis;
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
            

            var logSrc = string.Empty;

            //            if (namespaceName == "<global namespace>" || string.IsNullOrWhiteSpace(namespaceName))
            //            {
            //                logSrc = $@"
            //using System;
            //using System.CodeDom.Compiler;
            //using System.Runtime.CompilerServices;

            //[GeneratedCode(""LogAttribute"", ""x.x.x"")] // Check the namespace and version
            //[CompilerGenerated]
            //public class LogAttribute : Attribute {{ }}";
            //            }
            //            else
            //            {
            //                logSrc = $@"
            //using System;
            //using System.CodeDom.Compiler;
            //using System.Runtime.CompilerServices;

            //namespace " + namespaceName + $@"

            //[GeneratedCode(""LogAttribute"", ""x.x.x"")] // Check the namespace and version
            //[CompilerGenerated]
            //public class LogAttribute : Attribute {{ }}";
            //            }

            
            logSrc = $@"
using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

"
+ namespaceNameStatement + 
$@"

[GeneratedCode(""LogAttribute"", ""x.x.x"")] // Check the namespace and version
[CompilerGenerated]
public class LogAttribute : Attribute {{ }}";


            context.AddSource("Log.cs", logSrc);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
