using Microsoft.AspNetCore.Mvc;
using HighLoad.Api.Models;
using System.Diagnostics;

namespace HighLoad.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HelperController : ControllerBase
{
    private readonly ILogger<HelperController> _logger;
    private readonly Helper _Helper = Helper.Instance;
    private readonly long _Tag;

    public HelperController(ILogger<HelperController> logger)
    {
        _logger = logger;
        _Tag = DateTime.UtcNow.Ticks;
    }

    [HttpPost("reset")]
    public void ResetDict() => _Helper.Reset();

    [HttpPost("init")]
    public InitializationStatus RunInit(int itemCount, int taskCount)
    {
        var sw = new Stopwatch();
        sw.Start();
        int totalCount = _Helper.Initialize(itemCount, taskCount);
        sw.Stop();
        return new InitializationStatus(sw.Elapsed, itemCount, taskCount, totalCount);
    }

    [HttpGet("count")]
    public int GetCurrentCount() => _Helper.Dict.Count;

    [HttpGet("ident")]
    public string GetCurrentIdentity()
    {
        var process = Process.GetCurrentProcess();
        return $"controller: {process.Id}: {_Tag}\nhelper: {_Helper.IdentityTag}";
    }
}
