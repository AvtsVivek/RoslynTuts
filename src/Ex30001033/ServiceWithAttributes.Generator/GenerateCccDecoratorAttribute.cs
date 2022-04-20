namespace ServiceWithAttributes.Generator
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GenerateCccDecoratorAttribute : Attribute
    {
        public GenerateCccDecoratorAttribute(string template = null)
        {

        }
    }

}
