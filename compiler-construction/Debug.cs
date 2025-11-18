namespace compiler_construction;

public static class Debug
{
    public const bool debug = false;
    public const bool info = false;
    
    public static void Log(object message)
    {
        if (debug)
        {
            Console.WriteLine("Debug: " + message);
        }
    }

    public static void Log(string tag, object message)
    {
        if (debug)
        {
            Console.WriteLine($"Debug [{tag}]: {message}");
        }
    }

    public static void Info(object message)
    {
        if (info)
        {
            Console.WriteLine("Info: " + message);
        }
    }
}
