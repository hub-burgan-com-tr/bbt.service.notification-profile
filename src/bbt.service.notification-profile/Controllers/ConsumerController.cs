using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

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
           Summary = "Returns consumer configurations of user",
           Tags = new[] { "Consumer" }
       )]
    [HttpGet("/consumers/clients/{client}/users/{user}")]
    [SwaggerResponse(200, "Success, consumers is returned successfully", typeof(GetUserConsumersResponse))]

    public IActionResult GetUserConsumers(
      [FromRoute] long client,
      [FromRoute] long user,
      [FromQuery] int? source
 )
    {
        GetUserConsumersResponse returnValue = new GetUserConsumersResponse();

        using (var db = new DatabaseContext())
        {
            var consumers = db.Consumers.Where(s =>
                s.Client == client &&
                s.User == user &&
                (source.HasValue || s.SourceId == source.Value))
            .AsNoTracking();

            returnValue.Consumers = consumers.Select(c =>
                new GetUserConsumersResponse.Consumer
                {
                    Source = c.SourceId,
                    Filter = c.Filter,
                    IsPushEnabled = c.IsPushEnabled,
                    DeviceKey = c.DeviceKey,
                    IsSmsEnabled = c.IsSmsEnabled,
                    Phone = c.Phone,
                    IsEmailEnabled = c.IsEmailEnabled,
                    Email = c.Email
                }

            ).ToList();
        }

        return Ok(returnValue);
    }

    [SwaggerOperation(
                Summary = "Add or updates consumer configuration of user",
                Tags = new[] { "Consumer" }
            )]
    [HttpPost("/consumers/clients/{client}/users/{user}")]
    [SwaggerResponse(200, "Success, consumer is updates successfully", typeof(PostConsumerResponse))]
    [SwaggerResponse(201, "Success, consumer is created successfully", typeof(PostConsumerResponse))]

    public IActionResult Post(
          [FromRoute] long client,
          [FromRoute] long user,
          [FromBody] PostConsumerRequest data
      )
    {
        // ilgili configurasyon var mi, kontrol edelim.
        using (var db = new DatabaseContext())
        {
            var consumer = db.Consumers.Where(s =>
                s.Client == client &&
                s.User == user &&
                (data.Source.HasValue || s.SourceId == data.Source.Value) &&
                (data.Filter == null || s.Filter == data.Filter)
                )
            .FirstOrDefault();

            if (consumer != null)
            {
                consumer.IsPushEnabled = data.IsPushEnabled;
                consumer.DeviceKey = data.DeviceKey;
                consumer.IsSmsEnabled = data.IsSmsEnabled;
                consumer.Phone = data.Phone;
                consumer.IsEmailEnabled = data.IsEmailEnabled;
                consumer.Email = data.Email;
                db.SaveChanges();

                return Ok(new PostConsumerResponse { Consumer = consumer });
            }
            else
            {
                var newConsumer = new Consumer
                {
                    Id = Guid.NewGuid(),
                    Client = client,
                    User = user,
                    SourceId = data.Source.Value,
                    Filter = data.Filter,
                    IsPushEnabled = data.IsPushEnabled,
                    DeviceKey = data.DeviceKey,
                    IsSmsEnabled = data.IsSmsEnabled,
                    Phone = data.Phone,
                    IsEmailEnabled = data.IsEmailEnabled,
                    Email = data.Email
                };

                db.Add(newConsumer);
                db.SaveChanges();

                return Created("", new PostConsumerResponse { Consumer = newConsumer });
            }
        }

    }
}
