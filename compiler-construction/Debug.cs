namespace compiler_construction;

public static class Debug
{
    private const bool debug = false;
    private const bool info = false;
    
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
