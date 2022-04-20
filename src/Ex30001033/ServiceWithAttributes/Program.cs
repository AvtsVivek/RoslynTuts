// See https://aka.ms/new-console-template for more information
using System.Reflection;

Console.WriteLine("Hello, World!");

var currentAssembly = Assembly.GetExecutingAssembly();

foreach (Type type in currentAssembly.GetTypes())
{
    Console.WriteLine(type.FullName);
}
