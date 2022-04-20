using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ServiceWithAttributes.Generator
{
    public class AttributeSyntaxReceiver<TAttribute> : ISyntaxReceiver where TAttribute : Attribute
    {
        public IList<ClassDeclarationSyntax> Classes { get; } = new List<ClassDeclarationSyntax>();

        public IList<ClassDeclarationSyntax> ClassesImplimentingInterfaces { get; } = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax &&
                classDeclarationSyntax.AttributeLists.Count > 0 &&
                classDeclarationSyntax.AttributeLists
                    .Any(al => al.Attributes
                        .Any(a => a.Name.ToString().EnsureEndsWith("Attribute").Equals(typeof(TAttribute).Name))))
            {
                Classes.Add(classDeclarationSyntax);

                // Can we get or atleast know if this class impliments any interfaces?
                // We can know this in the generators execute method. 
                // But how can we have that info here in the syntax reciever?
                // Need to find out.
                var syntaxTreeForClass = classDeclarationSyntax.SyntaxTree;


            }
        }
    }

}
