using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Space.BrokerService.Entities;
using Space.Common;
using Space.Common.Contracts;

namespace Space.BrokerService.Controllers;

[Route("api/broker")]
[ApiController]
public class BrokerController : ControllerBase
{
    private readonly IPublishEndpoint _publisher;
    private readonly IRepository<SensorDataItem> _repository;
    private readonly ILogger<BrokerController> _logger;

    private const int DefaultAmountOfItems = 50;

    public BrokerController(
        IPublishEndpoint publisher,
        IRepository<SensorDataItem> repository,
        ILogger<BrokerController> logger)
    {
        _publisher = publisher;
        _repository = repository;
        _logger = logger;
    }

    // GET: api/broker/getData/{amount}
    /// <summary>
    /// Get the result data
    /// </summary>
    /// <returns></returns>
    [HttpGet("getData/{amount}")]
    public async Task<ActionResult<IEnumerable<SensorDataItem>>> GetData(int amount = DefaultAmountOfItems)
    {
        var data = (await _repository
            .GetManyAsync(amount))
            .OrderByDescending(e => e.Timestamp)
            .ToList();
        return Ok(data);
    }

    // POST api/broker/resume
    /// <summary>
    /// Start the main app flow
    /// </summary>
    /// <returns></returns>
    [HttpPost("resume")]
    public IActionResult StartReceivingData()
    {
        var startCommand = _publisher.Publish(new SensorWorkIsStarted());
        _logger.LogInformation("Application started");
        return Ok();
    }

    // POST api/broker/stop
    /// <summary>
    /// Stop receiving data. Imitation of losing connection to/from the data collector.
    /// </summary>
    /// <returns></returns>
    [HttpPost("stop")]
    public IActionResult StopReceivingData()
    {
        var stopCommand = _publisher.Publish(new SensorWorkIsStopped());
        return Ok();
    }
}