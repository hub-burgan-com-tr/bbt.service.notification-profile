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
              Summary = "Returns users of client. For individual clients genarally has one user -himself. ",
              Tags = new[] { "Consumer" }
          )]
    [HttpGet("/consumers/clients/{client}/users")]
    [SwaggerResponse(200, "Success, users are returned successfully", typeof(GetClientUsersResponse))]

    public IActionResult GetUsers(
        [FromRoute] long client
    )
    {

        GetClientUsersResponse returnValue = new GetClientUsersResponse();

        using (var db = new DatabaseContext())
        {
            var users = db.Consumers.Where(s => s.Client == client)
                .Select(m => m.User)
                .Distinct()
                .ToList();

            returnValue.Users = users;

        }
        return Ok(returnValue);
    }

    [SwaggerOperation(
           Summary = "Returns returns consumer configurations of user",
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

    [SwaggerOperation(
                Summary = "Updates user email address in all consumers",
             Tags = new[] { "Consumer" }
         )]
    [HttpPatch("/consumers/users/{user}/update-email")]
    [SwaggerResponse(200, "Success, email addresses are updated successfully", typeof(PostUpdateResponse))]

    public IActionResult UpdateEmail(
        [FromRoute] long user,
        [FromBody] PostUpdateEmailRequest data

    )
    {
        using (var db = new DatabaseContext())
        {
            var result = db.Database.ExecuteSqlInterpolated($"UPDATE [Consumers] SET Email = '{data.NewEmail}' WHERE Email = '{data.OldEmail}' AND [User] = {user}");

            return Ok(new PostUpdateResponse { UpdatedRecordCount = result });
        }
    }

    [SwaggerOperation(
               Summary = "Updates user phone in all consumers",
             Tags = new[] { "Consumer" }
         )]
    [HttpPatch("/consumers/users/{user}/update-phone")]
    [SwaggerResponse(200, "Success, consumer is returned successfully", typeof(PostUpdateResponse))]

    public IActionResult UpdatePhone(
        [FromRoute] long user,
        [FromBody] PostUpdatePhoneRequest data

    )
    {
        using (var db = new DatabaseContext())
        {
            var result = db.Database.ExecuteSqlInterpolated($@"UPDATE [Consumers] 
                        SET Phone_CountryCode = {data.NewPhone.CountryCode},  
                            Phone_Prefix = {data.NewPhone.Prefix},
                            Phone_Number = {data.NewPhone.Number} 
                        WHERE Phone_CountryCode = {data.OldPhone.CountryCode} AND
                              Phone_Prefix = {data.OldPhone.Prefix} AND 
                              Phone_Number = {data.OldPhone.Number} AND 
                              [User] = {user}");

            return Ok(new PostUpdateResponse { UpdatedRecordCount = result });
        }

    }


    [SwaggerOperation(
              Summary = "Updates user device in all consumers",
           Tags = new[] { "Consumer" }
       )]
    [HttpPatch("/consumers/users/{user}/update-device")]
    [SwaggerResponse(200, "Success, consumer is returned successfully", typeof(PostUpdateResponse))]

    public IActionResult UpdateDevice(
      [FromRoute] long user,
        [FromBody] PostUpdateDeviceRequest data

    )
    {
        using (var db = new DatabaseContext())
        {

            var result = db.Database.ExecuteSqlInterpolated($"UPDATE [Consumers] SET DeviceKey = '{data.NewDeviceKey}' WHERE DeviceKey = '{data.OldDeviceKey}' AND [User] = {user}");
            return Ok(new PostUpdateResponse { UpdatedRecordCount = result });
        }
    }

    [SwaggerOperation(
         Summary = "Returns all consumers with filtering (if available)",
         Tags = new[] { "Consumer" }
     )]
    [HttpGet("/consumers/source/{source}/{client}")]
    [SwaggerResponse(200, "Success, consumers is returned successfully", typeof(GetSourceConsumersResponse))]

    public IActionResult GetSourceConsumers(
    [FromRoute] int source,
    [FromRoute] long client,
    [DefaultValue("{   \"data\": {     \"amount\": 600,     \"iban\":\"TR1234567\",     \"name\": {       \"first\": \"ugur\",       \"last\": \"karatas\"     }   } }")]
    [FromQuery] string jsonData
)
    {
        GetSourceConsumersResponse returnValue = new GetSourceConsumersResponse { Consumers = new List<GetSourceConsumersResponse.Consumer>() };

        dynamic message = null;

        using (var db = new DatabaseContext())
        {
            // 0 nolu musteri generic musteri olarak kabul ediliyor. Banka kullanicilarin ozel durumlarda subscription olusturmalari icin kullanilacak.
            var consumers = db.Consumers.Where(s => (s.Client == client || s.Client == 0) && s.SourceId == source).ToList();

            // Eger filtre yoksa bosu bosuna deserialize etme
            if (consumers.Any(c => c.Filter != null))
            {
                message = JsonConvert.DeserializeObject(jsonData);
            }

            consumers.ForEach(c =>
            {
                bool canSend = true; // eger filtre yoksa gonderim sekteye ugramasin.

                if (c.Filter != null)
                {
                    canSend = Extensions.Evaluate(c.Filter, message);
                }

                if (canSend)

                    returnValue.Consumers.Add(new GetSourceConsumersResponse.Consumer
                    {
                        Id = c.Id,
                        Client = c.Client,
                        User = c.User,
                        IsPushEnabled = c.IsPushEnabled,
                        DeviceKey = c.DeviceKey,
                        Filter = c.Filter,
                        IsSmsEnabled = c.IsSmsEnabled,
                        Phone = c.Phone,
                        IsEmailEnabled = c.IsEmailEnabled,
                        Email = c.Email
                    });
            });
        }

        return Ok(returnValue);
    }

}
