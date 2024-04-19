using System.Diagnostics;
using HighLoad;

internal class Program
{
    private static void Log(string msg = null, bool addLineBreak = true, bool addTimestamp = false)
    {
        if (!string.IsNullOrWhiteSpace(msg))
        {
            if (addTimestamp)
            {
                Console.Write($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} ");
            }

            Console.Write(msg);
        }

        if (addLineBreak)
        {
            Console.WriteLine();
        }
    }

    private static bool QueryParameters(out int eCount, out int tCount)
    {
        bool result = true;
        Console.Write("Please enter the number of ENTRIES to create: ");
        string input = Console.ReadLine();
        result = int.TryParse(input, out eCount);
        if (result)
        {
            Console.Write("Please enter the number of TASKS to execute: ");
            input = Console.ReadLine();
            result = int.TryParse(input, out tCount);
        }
        else
        {
            tCount = 0;
        }

        return result;
    }

    public static void Main(string[] args)
    {
        int entryCount = 0;
        int taskCount = 0;

        var helper = Helper.Instance;
        Log($"Helper instance: {helper.IdentityTag}", true, true);

        var sw = new Stopwatch();
        Action<int, int> runInit = (e, t) =>
        {
            helper.Reset();
            Log($"Initializing {entryCount} entries using {taskCount} tasks.", false);
            sw.Start();
            helper.Initialize(entryCount, taskCount);
            sw.Stop();
            Log($" This resulted in {helper.Dict.Count} entries. It took {sw.ElapsedMilliseconds:D2}ms");
            sw.Reset();
        };

        bool continueRun = false;
        if (args?.Length == 2)
        {
            continueRun = int.TryParse(args[0], out entryCount) && int.TryParse(args[1], out taskCount);
        }

        bool singleRun = continueRun;
        if (!continueRun)
        {
            continueRun = QueryParameters(out entryCount, out taskCount);
        }

        while (continueRun)
        {
            runInit(entryCount, taskCount);
            if (singleRun)
            {
                break;
            }

            continueRun = QueryParameters(out entryCount, out taskCount);
        }

        Log("Execution ended", true, true);
    }
}