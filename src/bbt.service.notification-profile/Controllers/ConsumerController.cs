using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public IActionResult GetUserTopics(
      [FromRoute] long client,
      [FromRoute] long user,
      [FromQuery] string source
 )
    {
        GetUserConsumersResponse returnValue = new GetUserConsumersResponse();

        using (var db = new DatabaseContext())
        {
            var consumers = db.Consumers.Where(s =>
                s.Client == client &&
                s.User == user &&
                (source == null || s.SourceId == source))
            .Include(s => s.Variants)
            .AsNoTracking();

            returnValue.Consumers = consumers.Select(c =>
                new GetUserConsumersResponse.Consumer
                {
                    Source = c.SourceId,
                    Filter = c.Filter,
                    Variants = c.Variants.Select(v => new GetUserConsumersResponse.Consumer.Variant { Key = v.Key, Value = v.Value }).ToList(),
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
            var consumers = db.Consumers.Where(s =>
                s.Client == client &&
                s.User == user &&
                (data.Source == null || s.SourceId == data.Source) &&
                (data.Filter == null || s.Filter == data.Filter)
                )
            .Include(s => s.Variants)
            .ToList();

            // variant kontrolude yapmak lazim
            if (consumers != null)
            {
                if (data.Variants == null) data.Variants = new List<PostConsumerRequest.Variant>();
                foreach (var c in consumers)
                {

                    var firstNotSecond = data.Variants.Except(c.Variants.Select(v => new PostConsumerRequest.Variant { Key = v.Key, Value = v.Value }).ToList()).ToList();
                    var secondNotFirst = c.Variants.Select(v => new PostConsumerRequest.Variant { Key = v.Key, Value = v.Value }).ToList().Except(data.Variants).ToList();

                    if (!firstNotSecond.Any() && !secondNotFirst.Any())
                    {
                        // ee artik variant da esit olduguna gore, guncelle
                        c.IsPushEnabled = data.IsPushEnabled;
                        c.DeviceKey = data.DeviceKey;
                        c.IsSmsEnabled = data.IsSmsEnabled;
                        c.Phone = data.Phone;
                        c.IsEmailEnabled = data.IsEmailEnabled;
                        c.Email = data.Email;
                        db.SaveChanges();

                        return Ok(new PostConsumerResponse { Consumer = c });
                    }
                }
            }

            // Buraya kadar geldigine gore yeni kayit ekle gitsin :)

            var newConsumer = new Consumer
            {
                Id = Guid.NewGuid(),
                Client = client,
                User = user,
                SourceId = data.Source,
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
        using (var db = new DatabaseContext())
        {
            var result = db.Database.ExecuteSqlInterpolated($"UPDATE [Consumers] SET Email = '{data.NewEmail}' WHERE Email = '{data.OldEmail}' AND [User] = {user}");

            return Ok(new PostUpdateResponse { UpdatedRecordCount = result });
        }
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
        using (var db = new DatabaseContext())
        {

            var result = db.Database.ExecuteSqlInterpolated($"UPDATE [Consumers] SET DeviceKey = '{data.NewDeviceKey}' WHERE DeviceKey = '{data.OldDeviceKey}' AND [User] = {user}");
            return Ok(new PostUpdateResponse { UpdatedRecordCount = result });
        }
    }


    [SwaggerOperation(
         Summary = "Returns all consumers with checking fields values (After variant check and filtering ). ",
         Tags = new[] { "Consumer" }
     )]
    [HttpGet("/consumers/source/{source}/{client}")]
    [SwaggerResponse(200, "Success, consumers is returned successfully", typeof(GetSourceConsumersResponse))]

    public IActionResult GetSourceTopics(
    [FromRoute] string source,
    [FromRoute] long client,
    [FromQuery] string jsonData
)
    {

        JsonDocument data = JsonDocument.Parse(jsonData);

        //data.RootElement.


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
