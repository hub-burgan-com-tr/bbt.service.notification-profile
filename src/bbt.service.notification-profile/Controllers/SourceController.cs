using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

//namespace Notification.Profile.Controllers;

[ApiController]
[Route("[controller]")]
public class SourceController : ControllerBase
{

    private readonly ILogger<SourceController> _logger;

    public SourceController(ILogger<SourceController> logger)
    {
        _logger = logger;
    }

    [SwaggerOperation(
              Summary = "Returns registered data sources",
              Tags = new[] { "Source" }
          )]
    [HttpGet("/sources")]
    [SwaggerResponse(200, "Success, sources are returned successfully", typeof(GetSourcesResponse))]

    public IActionResult Get(
      [FromQuery(Name = "page-index")][Range(0, 100)] int pageIndex = 0,
      [FromQuery(Name = "page-size")][Range(1, 100)] int pageSize = 20
    )
    {
        throw new NotImplementedException();
    }
}
