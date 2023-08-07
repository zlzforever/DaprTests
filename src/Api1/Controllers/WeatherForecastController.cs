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
        await _daprClient.PublishEventAsync("pubshub", "inventory", new Widget { });
        await _daprClient.PublishEventAsync("pubshub", "sub", new { });
    }

    [Topic("pubsub", "sub")]
    [HttpPost("sub")]
    public void Sub()
    {
        _logger.LogInformation("sub");
    }

    [Topic("pubsub", "inventory", "event.type ==\"widget\"", 1)]
    [HttpPost("widgets")]
    public ActionResult<Stock> HandleWidget(Widget widget, [FromServices] DaprClient daprClient)
    {
        _logger.LogInformation("widgets");
        // Logic
        return new ActionResult<Stock>(new Stock());
    }

    [Topic("pubsub", "inventory", "event.type ==\"gadget\"", 2)]
    [HttpPost("gadgets")]
    public ActionResult<Stock> HandleGadget(Gadget gadget, [FromServices] DaprClient daprClient)
    {
        _logger.LogInformation("gadgets");
        // Logic
        return new ActionResult<Stock>(new Stock());
    }

    [Topic("pubsub", "inventory")]
    [HttpPost("products")]
    public ActionResult<Stock> HandleProduct(Product product, [FromServices] DaprClient daprClient)
    {
        _logger.LogInformation("products");
        // Logic
        return new ActionResult<Stock>(new Stock());
    }
}

public class Product
{
}

public class Gadget
{
}

public class Widget
{
}

public class Stock
{
}