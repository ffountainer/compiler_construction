namespace compiler_construction;

public static class Debug
{
    private const bool debug = true;
    private const bool info = true;
    
    public static void Log(object message)
    {
        if (debug)
        {
            Console.WriteLine("Debug: " + message);
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
