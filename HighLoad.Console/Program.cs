using HighLoad;

internal class Program
{
    public static void Main(string[] args)
    {
        var helper = Helper.Instance;
        Console.WriteLine($"Helper instance: {helper.IdentityTag}");
        helper.Initialize(3, 2);
        // helper.Initialize(3,1);
        foreach (var entry in helper.Dict)
        {
            Console.WriteLine($"{entry.Key} = {entry.Value.MaxLen(80)}");
        }

        Console.WriteLine("=== Press ENTER to close the console ===");
        Console.ReadLine();
    }
}
