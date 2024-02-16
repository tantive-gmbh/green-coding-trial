using System.Collections.Concurrent;
using System.Globalization;
using System.Text;
using HighLoad;

namespace HighLoad;


public sealed class Helper
{
    #region --- singleton ---
    private static readonly Lazy<Helper> lazy = new Lazy<Helper>(() => new Helper());

    public static Helper Instance { get { return lazy.Value; } }

    public readonly string IdentityTag = $"Created at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff} in process {System.Diagnostics.Process.GetCurrentProcess().Id}";
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
    }

    private void DoInitialize(int entryCount)
    {
        for (int index = 0; index < entryCount; index++)
        {
            var key = Guid.NewGuid();
            var now = DateTime.UtcNow;
            string value1 = $"{key} at {now:yyyy-MM-dd HH:mm:ss} ({now.Ticks})";
            string longValue = value1.Multiply(10000);
            var data = Encoding.UTF8.GetBytes(longValue);
            var bcd = Convert.ToBase64String(data);
            string value2 = bcd.Reverse().Checksum().Reverse().Checksum();
            m_Dict.GetOrAdd(key, $"{value1} = {value2} {bcd}");
        }
    }

    public int Initialize(int entryCount = 100, int taskCount = 1)
    {
        var tasks = new Task[taskCount];

        for (int i = 0; i < taskCount; i++)
        {
            tasks[i] = Task.Run(() => DoInitialize(entryCount));
        }

        Task.WaitAll(tasks);
        return m_Dict.Count;
    }
}