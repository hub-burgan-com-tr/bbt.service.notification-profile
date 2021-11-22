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
              Summary = "Returns users of client. For individual clients genarally has one user -himself. ",
              Tags = new[] { "Consumer" }
          )]
    [HttpGet("/consumers/clients/{client}/users")]
    [SwaggerResponse(200, "Success, users are returned successfully", typeof(GetClientUsersResponse))]

    public IActionResult GetUsers(
        [FromRoute] long client
    )
    {
        return Ok(new GetClientUsersResponse
        {
            Users = new List<long>
            {
                38552069001,
                38552069002,
                38552069003
            }
        });
    }

    [SwaggerOperation(
              Summary = "Add new consumer configuration",
              Tags = new[] { "Consumer" }
          )]
    [HttpPost("/consumers/clients/{client}")]
    [SwaggerResponse(201, "Success, consumer is crated successfully", typeof(PostConsumerResponse))]

    public IActionResult Post(
        [FromRoute] long client,
        [FromBody] PostConsumerRequest data
    )
    {
        throw new NotImplementedException();
    }

    [SwaggerOperation(
           Summary = "Returns specific consumer",
           Tags = new[] { "Consumer" }
       )]
    [HttpGet("/consumers/clients/{client}/users/{user}/paths")]
    [SwaggerResponse(200, "Success, consumer is returned successfully", typeof(GetUserPathsResponse))]

    public IActionResult GetUserPaths(
      [FromRoute] long client,
      [FromRoute] long user
 )
    {
        return Ok(new GetUserPathsResponse
        {
            Paths = new List<GetUserPathsResponse.PathInfo> {
                new GetUserPathsResponse.PathInfo {
                    Path = "Transfers/",
                    Source = "",
                    Filter = "",
                    IsPushEnabled = true,
                    DeviceKey = "",
                    IsSmsEnabled = true,
                    Phone = new Phone { },
                    IsMailEnabled = true,
                    Email =""
                }
            }
        });
    }




    [SwaggerOperation(
             Summary = "Returns specific consumer",
             Tags = new[] { "Consumer" }
         )]
    [HttpGet("/consumers/clients/{client}/users/{user}/paths/{path}")]
    [SwaggerResponse(200, "Success, consumer is returned successfully", typeof(GetConsumerByPathResponse))]

    public IActionResult GetByPath(
        [FromRoute] long client,
        [FromRoute] long user,
        [FromRoute] long path
   )
    {
        throw new NotImplementedException();
    }

    [SwaggerOperation(
             Summary = "Returns specific consumer",
             Tags = new[] { "Consumer" }
         )]
    [HttpPatch("/consumers/users/{user}/update-email")]
    [SwaggerResponse(200, "Success, consumer is returned successfully", typeof(GetConsumerByPathResponse))]

    public IActionResult UpdateEmail(
        [FromRoute] long client,
        [FromRoute] long user,
        [FromRoute] long path
    )
    {
        throw new NotImplementedException();
    }

    [SwaggerOperation(
             Summary = "Returns specific consumer",
             Tags = new[] { "Consumer" }
         )]
    [HttpPatch("/consumers/users/{user}/update-phone")]
    [SwaggerResponse(200, "Success, consumer is returned successfully", typeof(GetConsumerByPathResponse))]

    public IActionResult UpdatePhone(
        [FromRoute] long client,
        [FromRoute] long user,
        [FromRoute] long path
    )
    {
        throw new NotImplementedException();
    }


    [SwaggerOperation(
           Summary = "Returns specific consumer",
           Tags = new[] { "Consumer" }
       )]
    [HttpPatch("/consumers/users/{user}/update-device")]
    [SwaggerResponse(200, "Success, consumer is returned successfully", typeof(GetConsumerByPathResponse))]

    public IActionResult UpdateDevice(
      [FromRoute] long client,
      [FromRoute] long user,
      [FromRoute] long path
    )
    {
        throw new NotImplementedException();
    }

}
