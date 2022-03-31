using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace EqualsAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class EqualsAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "EqualsAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.EqualsAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.EqualsAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.EqualsAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, 
            Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        // Register the list of rules this DiagnosticAnalizer supports
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            // context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);

            // The AnalyzeNode method will be called for each InvocationExpression of the Syntax tree
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var invocationExpr = context.Node as InvocationExpressionSyntax;

            if (invocationExpr == null)
                return;

            // invocationExpr.Expression is the expression before "(", here "string.Equals".
            // In this case it should be a MemberAccessExpressionSyntax, with a member name "Equals"
            var memberAccessExpr = invocationExpr.Expression as MemberAccessExpressionSyntax;
            if (memberAccessExpr == null)
                return;

            if (memberAccessExpr.Name.ToString() != nameof(string.Equals))
                return;

            // Now we need to get the semantic model of this node to get the type of the node
            // So, we can check it is of type string whatever the way you define it (string or System.String)
            var memberSymbol = context.SemanticModel.GetSymbolInfo(memberAccessExpr).Symbol as IMethodSymbol;
            if (memberSymbol == null)
                return;

            // Check the method is a member of the class string
            if (memberSymbol.ContainingType.SpecialType != SpecialType.System_String)
                return;

            // If there are not 3 arguments, the comparison type is missing => report it
            // We could improve this validation by checking the types of the arguments, but it would be a little longer for this post.
            var argumentList = invocationExpr.ArgumentList;
            if ((argumentList?.Arguments.Count ?? 0) == 2)
            {
                var diagnostic = Diagnostic.Create(Rule, invocationExpr.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}

