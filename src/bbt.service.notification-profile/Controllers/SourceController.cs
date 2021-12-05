using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        List<Source> sources;

        using (var db = new DatabaseContext())
        {
            sources = db.Sources
                //.Where(s => s.ParentId == null)
                .Include(s => s.Parameters)
                .Include(s => s.Children)               
               // .Take(pageSize)
               // .Skip(pageSize * pageIndex)
                .ToList();
        }


        return Ok(new GetSourcesResponse
        {
            Sources = sources.Select(s => new GetSourcesResponse.Source
            {
                Id = s.Id,
                Title = new GetSourcesResponse.Source.TitleLabel { EN = s.Title_EN, TR = s.Title_TR },
                Parameters = s.Parameters.Select(p => new GetSourcesResponse.Source.SourceParameter
                {
                    JsonPath = p.JsonPath,
                    Type = p.Type,
                    Title = new GetSourcesResponse.Source.TitleLabel { EN = p.Title_EN, TR = p.Title_TR },
                }).ToList(),
                Topic = s.Topic,
                ParentSourceId = s.ParentId,
                ApiKey = s.ApiKey,
                Secret = s.Secret,
                PushServiceReference = s.PushServiceReference,
                SmsServiceReference = s.SmsServiceReference,
                EmailServiceReference = s.EmailServiceReference
            }).ToList()
        }
        );
    }

    [SwaggerOperation(
             Summary = "Adds new data sources",
             Tags = new[] { "Source" }
         )]
    [HttpPost("/sources")]
    [SwaggerResponse(200, "Success, sources is created successfully", typeof(void))]
    public IActionResult Post(
     [FromBody] PostSourceRequest data)
    {
        using (var db = new DatabaseContext())
        {
            db.Add(new Source
            {
                Id = data.Id,
                //Title = data.Title,
                Topic = data.Topic,
                ApiKey = data.ApiKey,
                Secret = data.Secret,
                PushServiceReference = data.PushServiceReference,
                SmsServiceReference = data.SmsServiceReference,
                EmailServiceReference = data.EmailServiceReference
            });

            db.SaveChanges();
        }


        return Ok();
    }


    [SwaggerOperation(
             Summary = "Updates existing data sources. Only not null values are going be to uptated",
             Tags = new[] { "Source" }
         )]
    [HttpPatch("/sources/{id}")]
    [SwaggerResponse(200, "Success, source is updated successfully", typeof(void))]
    [SwaggerResponse(460, "Source is not found.", typeof(Guid))]
    public IActionResult Patch(
        [FromRoute] int id,
        [FromBody] PatchSourceRequest data)
    {

        using (var db = new DatabaseContext())
        {
            var source = db.Sources.FirstOrDefault(s => s.Id == id);

            if (source == null)
                return new ObjectResult(id) { StatusCode = 460 };

            //if (data.Title != null) source.Title = data.Title;
            if (data.Topic != null) source.Topic = data.Topic;
            if (data.ApiKey != null) source.ApiKey = data.ApiKey;
            if (data.Secret != null) source.Secret = data.Secret;
            if (data.PushServiceReference != null) source.PushServiceReference = data.PushServiceReference;
            if (data.SmsServiceReference != null) source.SmsServiceReference = data.SmsServiceReference;
            if (data.EmailServiceReference != null) source.EmailServiceReference = data.EmailServiceReference;

            db.SaveChanges();
        }


        return Ok();
    }


    [SwaggerOperation(
            Summary = "Deletes existing data source if only there is no referenced consumer",
            Tags = new[] { "Source" }
        )]
    [HttpDelete("/sources/{id}")]
    [SwaggerResponse(200, "Success, source is deleted successfully", typeof(void))]
    [SwaggerResponse(460, "Source is not found", typeof(Guid))]
    [SwaggerResponse(461, "Source has consumer(s)", typeof(Guid))]
    public IActionResult Delete([FromRoute] int id)
    {

        using (var db = new DatabaseContext())
        {
            var source = db.Sources.FirstOrDefault(s => s.Id == id);

            if (source == null)
                return new ObjectResult(id) { StatusCode = 460 };

            var references = db.Consumers.FirstOrDefault(c => c.SourceId == id);

            if (references != null)
                return new ObjectResult(id) { StatusCode = 461 };

            db.Remove(source);
            db.SaveChanges();
        }

        return Ok();
    }
}
