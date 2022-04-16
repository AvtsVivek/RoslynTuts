
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;


//// namespace SourceGenTalks;

ILogger logger = Program.CreateLogger(LogLevel.Trace);

logger.LogInformation("Hello {name}!", "World" );

logger.LogInformation("Hello {name}!", 0x_F0); // This is Hello 240!

// The above has performance hits.

// So we need to use logger message extension.

logger.Hello(9);

// The following using source generator
logger.HelloWithSourceGenerator(9);

Entity entity = new()
{
    Name = "Generation",
    // Number = 0x_F0,
    Number = 1
};

SomeOtherEntity someOtherEntity = new()
{
    Name = "GenerationTwo",
    // Number = 0x_F0,
    Number = 2
};

var jsonEntity = JsonSerializer.Serialize(entity, SerializerContext.Default.Entity);
logger.LogInformation(jsonEntity);
Entity? roundtrip = JsonSerializer.Deserialize<Entity>(jsonEntity, SerializerContext.Default.Entity);
logger.LogError(roundtrip!.ToString());
//Note we are logging that as error(above). So it shows up as red colored 'fail' in the logged console.

var jsonSomeOtherEntity = JsonSerializer.Serialize(someOtherEntity, SerializerContext.Default.SomeOtherEntity);
logger.LogInformation(jsonSomeOtherEntity);
var roundtripSomeOtherEntity = JsonSerializer.Deserialize<SomeOtherEntity>(jsonSomeOtherEntity, SerializerContext.Default.SomeOtherEntity);
logger.LogError(roundtripSomeOtherEntity!.ToString());

await Task.Yield();



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