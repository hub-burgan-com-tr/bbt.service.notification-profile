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
public class ConfigurationController : ControllerBase
{

    private readonly ILogger<ConfigurationController> _logger;

    public ConfigurationController(ILogger<ConfigurationController> logger)
    {
        _logger = logger;
    }

    [SwaggerOperation(
              Summary = "Returns users of client. For individual clients genarally has one user -himself. ",
              Tags = new[] { "Configuration" }
          )]
    [HttpGet("/configuration/clients/{client}/users")]
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
           Summary = "Returns consumer configurations tree of user",
           Tags = new[] { "Configuration" }
       )]
    [HttpGet("/configuration/clients/{client}/users/{user}/consumer-tree")]
    [SwaggerResponse(200, "Success, consumers is returned successfully", typeof(GetUserConsumersResponse))]

    public IActionResult GetUserConsumers(
      [FromRoute] long client,
      [FromRoute] long user)
     
    {
        throw new NotImplementedException();
    }

    [SwaggerOperation(
           Summary = "Updates consumer configurations tree branch of user",
           Tags = new[] { "Configuration" }
       )]
    [HttpPost("/configuration/clients/{client}/users/{user}/consumer-tree")]
    [SwaggerResponse(200, "Success, consumers is returned successfully", typeof(GetUserConsumersResponse))]

    public IActionResult PostUserConsumers(
      [FromRoute] long client,
      [FromRoute] long user)
     
    {
        throw new NotImplementedException();
    }

    


    [SwaggerOperation(
                Summary = "Updates user email address in all consumers",
             Tags = new[] { "Configuration" }
         )]
    [HttpPatch("/configuration/users/{user}/update-email")]
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
             Tags = new[] { "Configuration" }
         )]
    [HttpPatch("/configuration/users/{user}/update-phone")]
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
           Tags = new[] { "Configuration" }
       )]
    [HttpPatch("/configuration/users/{user}/update-device")]
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


}
