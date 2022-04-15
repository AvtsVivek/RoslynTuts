
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;


//// namespace SourceGenTalks;

ILogger logger = Program.CreateLogger(LogLevel.Trace);

logger.LogInformation("Hello {name}!", "World" );

await Task.Yield();

logger.LogInformation("Hello {name}!", 0x_F0); // This is Hello 240!

logger.Hello(9);

logger.HelloWithSourceGenerator(9);
// The above has performance hits.

// So we need to use logger message extension.

public static partial class LogExtensions
{
    private static Action<ILogger, int, Exception> hello = LoggerMessage.Define<int>(LogLevel.Information, 0x_F0, "Hello, {name}");
    public static void Hello(this ILogger logger, int number)
    {
        hello.Invoke(logger, number, null!);
    }

    // The following is using source generators.
    // To see where it has got the gnerated code,
    // Right click HelloWithSourceGenerator and then go to definition.

    [LoggerMessage(240, LogLevel.Information, "Hello, {name}!")]
    public static partial void HelloWithSourceGenerator(this ILogger logger, int name);

}


//logger.Hello("World");

//logger.LogWarning(Helper.Text);
//logger.LogWarning(PostInitialization.Roslyn3_9.Get());

//Entity entity = new()
//{
//    Name = "Generation",
//    Number = 0x_F0,
//};
//string json = JsonSerializer.Serialize(entity, SerializerContext.Default.Entity);
//logger.LogInformation(json);
//Entity? roundtrip = JsonSerializer.Deserialize<Entity>(json, SerializerContext.Default.Entity);
//logger.LogError(roundtrip!.ToString());

////logger.LogInformation(Helper.Get());

//Console.ReadKey();

//public static partial class Log
//{
//    [LoggerMessage(240, LogLevel.Information, "Hello, {name}!")]
//    public static partial void Hello(this ILogger logger, string name);
//}

//public record class Entity
//{
//    public string? Name { get; set; }
//    public int Number { get; set; }
//}

//[JsonSerializable(typeof(Entity))]
//[JsonSourceGenerationOptions(WriteIndented = true)]
//public partial class SerializerContext : JsonSerializerContext
//{
//}

////public static partial class Helper
////{
////    public static string? Text { get; set; }

////    [ModuleInitializer]
////    internal static void Init()
////    {
////        Text = "initialized";
////    }

////    internal static partial string Get();
////}