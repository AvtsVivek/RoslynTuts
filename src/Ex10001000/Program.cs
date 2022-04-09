using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World");

            var tree = CSharpSyntaxTree.ParseText(@"
class C
{
    void MyMethod()
    {
        try
        {
            // This is try block
        }
        catch(exception ex)
        {
        }
        finally
        {
        }
    }
}");

            Console.WriteLine(tree.ToString());
            
            var errorDiagnostics = tree.GetDiagnostics()
                .Where(n => n.Severity == DiagnosticSeverity.Error);

            if(errorDiagnostics.Count() == 0)
            {
                Console.WriteLine("No errors");
            }
            else
            {
                foreach (var error in errorDiagnostics)
                {
                    Console.WriteLine(error.GetMessage());
                }
            }

            var root = tree.GetRoot();
            Console.WriteLine($"Root name is {root.GetType().Name}"); // CompilationUnitSyntax
            Console.WriteLine($"Root has {root.ChildNodes().Count()} number of child nodes.");
            Console.WriteLine($"Its Raw kind is {root.ChildNodes().Single().RawKind} ");
            Console.WriteLine($"Its kind is {root.ChildNodes().Single().Kind()} ");

            var cds = root.ChildNodes().Single() as ClassDeclarationSyntax;

            if (cds != null)
            {
                Console.WriteLine("The members are as follows....");
                foreach (MemberDeclarationSyntax member in cds.Members)
                {
                    Console.WriteLine(member.ToString());
                }
            }

            var childTokens = root.ChildTokens();
            
            Console.WriteLine($"Child tokens count is {childTokens.Count()}");
            
            foreach (var childToken in childTokens)
            {
                Console.WriteLine($"The Raw Kind of the child token is {childToken.RawKind}");
                Console.WriteLine($"The Kind of the child token is {childToken.Kind()}");
            }

            var allDecendentTokens = root.DescendantTokens();

            Console.WriteLine($"Child tokens count is {allDecendentTokens.Count()}");

            foreach (var desendentToken in allDecendentTokens)
            {
                Console.WriteLine($"The Raw Kind of the desendent token is {desendentToken.RawKind}");
                Console.WriteLine($"The Kind of the desendent token is {desendentToken.Kind(),-30} {desendentToken.Text}");
            }

            Console.WriteLine("Now the method inside of the class is as follows.");
            var method = root.DescendantNodes().OfType<MethodDeclarationSyntax>().FirstOrDefault();
            Console.WriteLine(method!.Identifier);
            Console.WriteLine("Method body is " + method.Body!.ToFullString());

            Console.WriteLine("Now we have a try catch block inside of our method .");
            var tryStatement = root.DescendantNodes().OfType<TryStatementSyntax>().FirstOrDefault();
            var tryBlock = tryStatement!.Block;
            Console.WriteLine(tryBlock.ToFullString());

            Console.WriteLine("No. Of Catches here are " + tryStatement.Catches.Count);
            Console.WriteLine("Finally block is" + tryStatement.Finally!.ToFullString());

            var catchStatements = root.DescendantNodes().OfType<CatchClauseSyntax>();
            var catchStatement = catchStatements.FirstOrDefault();
            var catchBlock = catchStatement!.Block;
            Console.WriteLine(catchBlock.ToFullString());


            var finallyStatement = root.DescendantNodes().OfType<FinallyClauseSyntax>().FirstOrDefault();
            var finallyBlock = finallyStatement!.Block;
            Console.WriteLine(finallyBlock.ToFullString());

            // https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/Microsoft.CodeAnalysis.Analyzers.md#rs1034-prefer-iskind-for-checking-syntax-kinds
            
            var ourMethod = root.DescendantNodes()
                .First(m => m!.Kind() == SyntaxKind.MethodDeclaration);                      
            
            var ourMethodUsingIsKind = root.DescendantNodes()
                .First(m => m!.IsKind(SyntaxKind.MethodDeclaration));

            var ourMethodUsingOfType = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>().Single();

            var myMethod = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Single(m => m.Identifier.Text == "MyMethod");

            var myMethodNode = root.DescendantNodes().First(m =>
                m.IsKind(SyntaxKind.MethodDeclaration) &&
                m.ChildTokens().Any(t => t.IsKind(SyntaxKind.IdentifierToken) && t.Text == "MyMethod"));
        }
    }
}