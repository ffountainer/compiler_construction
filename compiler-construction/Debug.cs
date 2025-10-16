namespace compiler_construction;

public static class Debug
{
    private const bool debug = true;
    
    public static void Log(object message)
    {
        if (debug)
        {
            Console.WriteLine("Debug: " + message);
        }
    }
}
