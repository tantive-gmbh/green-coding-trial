using HighLoad;

internal class Program
{
    public static void Main(string[] args)
    {
        var helper = Helper.Instance;
        helper.Initialize(1);
        // helper.Initialize(3,1);
        foreach (var entry in helper.Dict)
        {
            Console.WriteLine($"{entry.Key} = {entry.Value}");
        }
    }
}
