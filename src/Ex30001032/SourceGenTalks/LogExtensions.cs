
using Microsoft.Extensions.Logging;

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


