using System.Text.Json;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace Api1.Controllers;

public record MessageEvent(string MessageType, string Message);

[ApiController]
[Route("test")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly DaprClient _daprClient;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, DaprClient daprClient)
    {
        _logger = logger;
        _daprClient = daprClient;
    }


    [HttpPost("sendEvent")]
    public async Task SendEvent()
    {
        await _daprClient.PublishEventAsync("pubsub", "inventory", new Widget(), new Dictionary<string, string>
        {
            { "cloudevent.type", "widget" }
        });
        await _daprClient.PublishEventAsync("pubsub", "inventory", new Gadget(), new Dictionary<string, string>
        {
            { "cloudevent.type", "gadget" }
        });
        await _daprClient.PublishEventAsync("pubsub", "inventory", new Product(), new Dictionary<string, string>
        {
            { "cloudevent.type", "product" }
        });
        await _daprClient.PublishEventAsync("pubsub", "sub", new { });
    }

    [Topic("pubsub", "sub")]
    [HttpPost("sub")]
    public void Sub()
    {
        _logger.LogWarning("receive sub event");
    }

    [Topic("pubsub", "inventory", "event.type ==\"widget\"", 1)]
    [HttpPost("widgets")]
    public void HandleWidget([FromBody] Widget widget)
    {
        _logger.LogWarning("receive widgets event: " + JsonSerializer.Serialize(widget));
    }

    [Topic("pubsub", "inventory", "event.type ==\"gadget\"", 2)]
    [HttpPost("gadgets")]
    public void HandleGadget([FromBody] Gadget gadget)
    {
        _logger.LogWarning("receive gadgets event: " + JsonSerializer.Serialize(gadget));
    }

    [Topic("pubsub", "inventory")]
    [HttpPost("products")]
    public void HandleProduct([FromBody] Product product)
    {
        _logger.LogWarning("receive products event: " + JsonSerializer.Serialize(product));
    }
}

public class Product
{
    public string P { get; set; } = "Product";
}

public class Gadget
{
    public string G { get; set; } = "Gadget";
}

public class Widget
{
    public string W { get; set; } = "Widget";
}

public class Stock
{
    public string S { get; set; } = "Stock";
}