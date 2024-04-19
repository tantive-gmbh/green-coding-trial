using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using HighLoad.Models;

namespace HighLoad;

public sealed class Helper
{
    #region --- singleton ---
    private static readonly Lazy<Helper> lazy = new Lazy<Helper>(() => new Helper());

    public static Helper Instance
    {
        get { return lazy.Value; }
    }

    public readonly string IdentityTag = $"Created at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff} in process {Process.GetCurrentProcess().Id}";
    private InitializationStatus m_LastInitializationStatus;
    public InitializationStatus LastInitializationStatus => m_LastInitializationStatus;

    private Helper()
    {
        Reset();
    }
    #endregion

    private ConcurrentDictionary<Guid, string> m_Dict;
    public ConcurrentDictionary<Guid, string> Dict => m_Dict;

    public void Reset()
    {
        m_Dict = new ConcurrentDictionary<Guid, string>();
        m_LastInitializationStatus = null;
    }

    private (Guid, string) CreateString()
    {
        var key = Guid.NewGuid();
        var now = DateTime.UtcNow;
        string value1 = $"{key} at {now:yyyy-MM-dd HH:mm:ss} ({now.Ticks})";
        string longValue = value1.Multiply(10000);
        byte[] data = Encoding.UTF8.GetBytes(longValue);
        string bcd = Convert.ToBase64String(data);
        string value2 = bcd.Reverse().Checksum().Reverse().Checksum();
        return (key, $"{value1} = {value2} {bcd}");
    }

    private void DoInitialize(int entryCount)
    {
        for (int index = 0; index < entryCount; index++)
        {
            var newEntry = CreateString();
            m_Dict.GetOrAdd(newEntry.Item1, newEntry.Item2);
        }
    }

    public InitializationStatus Initialize(int entryCount = 100, int taskCount = 1)
    {
        var sw = new Stopwatch();
        sw.Start();
        var tasks = new Task[taskCount];

        for (int i = 0; i < taskCount; i++)
        {
            tasks[i] = Task.Run(() => DoInitialize(entryCount));
        }

        Task.WaitAll(tasks);
        sw.Stop();

        m_LastInitializationStatus = new InitializationStatus(sw.Elapsed, entryCount, taskCount, m_Dict.Count);
        return m_LastInitializationStatus;
    }
}