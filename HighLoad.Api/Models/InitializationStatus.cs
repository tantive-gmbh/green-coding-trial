namespace HighLoad.Api.Models;

public class InitializationStatus
{
    internal InitializationStatus(TimeSpan timeSpan, int initItem, int initTask, int total)
    {
        Duration = timeSpan;
        InitItemCount = initItem;
        InitTaskCount = initTask;
        TotalCount = total;
    }

    internal TimeSpan Duration { get; }
    public string InitDuration => $"{Duration.Seconds}.{Duration.Milliseconds}";
    public int TotalCount { get; }
    public int ExpectedCount => InitItemCount * InitTaskCount;
    public int InitTaskCount { get; }
    public int InitItemCount { get; }
}