using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

//namespace Notification.Profile.Controllers;

[ApiController]
[Route("[controller]")]
public class ConsumerController : ControllerBase
{

    private readonly ILogger<ConsumerController> _logger;

    public ConsumerController(ILogger<ConsumerController> logger)
    {
        _logger = logger;
    }

    [SwaggerOperation(
              Summary = "Returns consumers",
              Tags = new[] { "Consumer" }
          )]
    [HttpGet("/consumers/{handle}")]
    [SwaggerResponse(200, "Success, consumers are returned successfully", typeof(GetConsumersResponse))]

    public IActionResult Get(
        [FromRoute] long handle,
        [FromQuery] long user,
        [FromQuery(Name = "page-index")][Range(0, 100)] int pageIndex = 0,
        [FromQuery(Name = "page-size")][Range(1, 100)] int pageSize = 20
    )
    {
        throw new NotImplementedException();
    }

    [SwaggerOperation(
              Summary = "Add new consumer configuration",
              Tags = new[] { "Consumer" }
          )]
    [HttpPost("/consumers/{handle-id}")]
    [SwaggerResponse(201, "Success, consumer is crated successfully", typeof(PostConsumerResponse))]

    public IActionResult Post(
        [FromRoute] long handle,
        [FromBody] PostConsumerRequest data
    )
    {
        throw new NotImplementedException();
    }


    [SwaggerOperation(
             Summary = "Returns specific consumer",
             Tags = new[] { "Consumer" }
         )]
    [HttpGet("/consumers/{handle}/{user}/{path}")]
    [SwaggerResponse(200, "Success, consumer is returned successfully", typeof(GetConsumerByPathResponse))]

    public IActionResult GetByPath(
        [FromRoute] long handle,
        [FromRoute] long user,
        [FromRoute] long path
   )
    {
        throw new NotImplementedException();
    }

}
