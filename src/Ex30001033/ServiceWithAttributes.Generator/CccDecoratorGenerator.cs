using Microsoft.CodeAnalysis;
using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Text;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ServiceWithAttributes.Generator
{
    [Generator]
    public class CccDecoratorGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new AttributeSyntaxReceiver<GenerateCccDecoratorAttribute>());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not AttributeSyntaxReceiver<GenerateCccDecoratorAttribute> syntaxReceiver)
            {
                return;
            }

            foreach (var classSyntax in syntaxReceiver.Classes)
            {
                var syntaxTreeForClass = classSyntax.SyntaxTree;
                // Converting the class to semantic model to access much more meaningful data.
                var model = context.Compilation.GetSemanticModel(syntaxTreeForClass);
                // Parse to declared symbol, so you can access each part of code separately, such as interfaces, methods, members, contructor parameters etc.
                var symbol = model.GetDeclaredSymbol(classSyntax);

                if (symbol.Interfaces.Count() == 0)
                {
                    continue;
                }

                var fileNameAndSourceString = GetSourceAndFileName(symbol);

                var loggerDecoratorFileName = fileNameAndSourceString.Item1;
                var finalSourceString = fileNameAndSourceString.Item2;

                context.AddSource(loggerDecoratorFileName, SourceText.From(finalSourceString, Encoding.UTF8));
            }
        }

        private static Tuple<string, string> GetSourceAndFileName(INamedTypeSymbol symbol)
        {
            var implementedInterfaces = symbol.Interfaces;

            var containingNamespaceName = symbol.ContainingNamespace.Name;

            var loggerDecoratorFileName = symbol.Name + "LoggerDecorator";
            var namespaceForDecorators = containingNamespaceName + ".Decorators";

            var finalSourceString = string.Empty;
            finalSourceString = finalSourceString + "// Auto-generated code";
            finalSourceString = finalSourceString + Environment.NewLine + "// " + symbol.Name;
            finalSourceString = finalSourceString + Environment.NewLine + "// Generated Time: " + DateTime.Now;
            finalSourceString = finalSourceString + Environment.NewLine + "// namespace of Service is " + containingNamespaceName;
            finalSourceString = finalSourceString + Environment.NewLine + "// namespace for decorators is " + namespaceForDecorators;
            finalSourceString = finalSourceString + Environment.NewLine + "// Interface count is " + implementedInterfaces.Count();

            string source = $@"
using System;

namespace {namespaceForDecorators}
{{
    public class {loggerDecoratorFileName}
    {{
        //static partial void HelloFrom(string name) =>
        //    Console.WriteLine($""Generator says: Hi from '{{name}}'"");
    }}
}}
";

            finalSourceString = finalSourceString + Environment.NewLine;
            finalSourceString = finalSourceString + source;

            return new Tuple<string, string>(loggerDecoratorFileName, finalSourceString);
        }
    }
}
