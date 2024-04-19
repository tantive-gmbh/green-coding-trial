using System.Diagnostics;
using HighLoad.Models;
using Microsoft.AspNetCore.Mvc;

namespace HighLoad.Api.Controllers;

[ApiController]
[Route("")]
public class DefaultController : ControllerBase
{
    private readonly ILogger<DefaultController> m_Logger;
    private readonly Helper m_Helper = Helper.Instance;
    private readonly string m_Tag;

    public DefaultController(ILogger<DefaultController> logger)
    {
        m_Logger = logger;
        m_Tag = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}";
    }

    [HttpPost("reset")]
    public void ResetDict()
    {
        m_Helper.Reset();
    }

    [HttpPost("init")]
    public InitializationStatus RunInit(int itemCount, int taskCount)
    {
        m_Helper.Initialize(itemCount, taskCount);
        return m_Helper.LastInitializationStatus;
    }

    [HttpGet("count")]
    public int GetCurrentCount() => m_Helper.Dict.Count;

    [HttpGet("status")]
    public InitializationStatus GetLastStatus() => m_Helper.LastInitializationStatus;

    [HttpGet("")]
    public string GetCurrentIdentity()
    {
        string result = $"Current process: {Process.GetCurrentProcess().Id}\nCurrent tag: {m_Tag}\nHelper: {m_Helper.IdentityTag}";
        m_Logger.LogDebug("[{0:yyyy-MM-dd HH:mm:ss.fff}] {1}", DateTime.UtcNow, result);
        return result;
    }
}