using System.Text.Json.Serialization;

// The following is a use case with Serialization and De-Serialization
// https://youtu.be/J_Y1obNh_RA?t=869
public record class Entity
{
    public string? Name { get; set; }
    public int Number { get; set; }
}

public record class SomeOtherEntity
{
    public string? Name { get; set; }
    public int Number { get; set; }
}


[JsonSerializable(typeof(Entity))]
[JsonSerializable(typeof(SomeOtherEntity))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public partial class SerializerContext : JsonSerializerContext
{
     
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