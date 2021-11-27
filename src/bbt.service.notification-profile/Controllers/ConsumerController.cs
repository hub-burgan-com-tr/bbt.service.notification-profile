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
           Summary = "Returns returns consumer configurations of user for specific customer",
           Tags = new[] { "Consumer" }
       )]
    [HttpGet("/consumers/clients/{client}/users/{user}/topics")]
    [SwaggerResponse(200, "Success, consumers is returned successfully", typeof(GetUserTopicsResponse))]

    public IActionResult GetUserTopics(
      [FromRoute] long client,
      [FromRoute] long user,
      [FromQuery] string source
 )
    {
        return Ok(new GetUserTopicsResponse
        {
            Topics = new List<GetUserTopicsResponse.TopicInfo> {
                new GetUserTopicsResponse.TopicInfo {
                    Source = "Incoming-EFT",
                    IsPushEnabled = true,
                    DeviceKey = "eadd523b0fdc40b5984c6326f1bc9232",
                    IsSmsEnabled = false,
                    IsMailEnabled = false,
                },
                 new GetUserTopicsResponse.TopicInfo {
                    Variants = new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string> ("IBAN", "TR330006100519786457841326")
                        },
                    Source = "Incoming-EFT",
                    Filter = "Amount >= 500",
                    IsPushEnabled = true,
                    DeviceKey = "eadd523b0fdc40b5984c6326f1bc9232",
                    IsSmsEnabled = true,
                    Phone = new Phone { CountryCode = 90, Prefix = 530, Number = 2896073   },
                    IsMailEnabled = false,
                },
                 new GetUserTopicsResponse.TopicInfo {
                    Source = "Incoming-QR",
                    IsPushEnabled = true,
                    DeviceKey = "eadd523b0fdc40b5984c6326f1bc9232",
                    IsSmsEnabled = false,
                    IsMailEnabled = false,
                }
            }
        });
    }

    [SwaggerOperation(
                Summary = "Add new consumer configuration to user",
                Tags = new[] { "Consumer" }
            )]
    [HttpPost("/consumers/clients/{client}/users/{user}/topics")]
    [SwaggerResponse(201, "Success, consumer is crated successfully", typeof(PostConsumerResponse))]

    public IActionResult Post(
          [FromRoute] long client,
          [FromRoute] long user,
          [FromBody] PostConsumerRequest data
      )
    {
        throw new NotImplementedException();
    }


    [SwaggerOperation(
                Summary = "Updates user email address in all topics",
             Tags = new[] { "Consumer" }
         )]
    [HttpPatch("/consumers/users/{user}/update-email")]
    [SwaggerResponse(200, "Success, email addresses are updated successfully", typeof(PostUpdateResponse))]

    public IActionResult UpdateEmail(
        [FromRoute] long user,
        [FromBody] PostUpdateEmailRequest data

    )
    {
        throw new NotImplementedException();
    }

    [SwaggerOperation(
               Summary = "Updates user phone in all topics",
             Tags = new[] { "Consumer" }
         )]
    [HttpPatch("/consumers/users/{user}/update-phone")]
    [SwaggerResponse(200, "Success, consumer is returned successfully", typeof(PostUpdateResponse))]

    public IActionResult UpdatePhone(
        [FromRoute] long user,
        [FromBody] PostUpdatePhoneRequest data

    )
    {
        throw new NotImplementedException();
    }


    [SwaggerOperation(
              Summary = "Updates user device in all topics",
           Tags = new[] { "Consumer" }
       )]
    [HttpPatch("/consumers/users/{user}/update-device")]
    [SwaggerResponse(200, "Success, consumer is returned successfully", typeof(PostUpdateResponse))]

    public IActionResult UpdateDevice(
      [FromRoute] long user,
        [FromBody] PostUpdateDeviceRequest data

    )
    {
        throw new NotImplementedException();
    }


    [SwaggerOperation(
         Summary = "Returns all consumers with checking fields values (After variant check and filtering ). ",
         Tags = new[] { "Consumer" }
     )]
    [HttpGet("/consumers/source/{source}")]
    [SwaggerResponse(200, "Success, consumers is returned successfully", typeof(GetSourceConsumersResponse))]

    public IActionResult GetSourceTopics(
    [FromRoute] string source,
    [FromRoute] long client,  
    [FromQuery] KeyValuePair<string, string>[] fields
)
    {
        return Ok(new GetSourceConsumersResponse
        {
            Consumers = new List<GetSourceConsumersResponse.Consumer> {
                new GetSourceConsumersResponse.Consumer
                {
                    IsPushEnabled = true,
                    DeviceKey = "eadd523b0fdc40b5984c6326f1bc9232",
                    IsSmsEnabled = false,
                    IsMailEnabled = false,
                },
            new GetSourceConsumersResponse.Consumer
            {
                IsPushEnabled = true,
                DeviceKey = "eadd523b0fdc40b5984c6326f1bc9232",
                IsSmsEnabled = true,
                Phone = new Phone { CountryCode = 90, Prefix = 530, Number = 2896073 },
                IsMailEnabled = false,
            },
            new GetSourceConsumersResponse.Consumer
            {
                IsPushEnabled = true,
                DeviceKey = "eadd523b0fdc40b5984c6326f1bc9232",
                IsSmsEnabled = false,
                IsMailEnabled = false,
            }
        }
        });
    }

}
