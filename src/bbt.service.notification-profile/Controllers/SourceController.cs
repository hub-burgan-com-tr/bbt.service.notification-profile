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
        return Ok(new GetSourcesResponse
        {
            Sources = new List<GetSourcesResponse.SourceItem> {
             new GetSourcesResponse.SourceItem {
                 Source = "Incoming-EFT",
                 Title = "Gelen EFT",
                 Topic = "http://localhost:8082/topics/cdc_eft/incoming_eft",
                 ApiKey = "a1b2c33d4e5f6g7h8i9jakblc",
                 Secret = "11561681-8ba5-4b46-bed0-905ae1769bc6",
                 PushServiceReference = "notify_push_incoming_eft",
                 SmsServiceReference = "notify_sms_incoming_eft",
                 EmailServiceReference = "notify_email_incoming_eft"
             },
              new GetSourcesResponse.SourceItem {
                Source = "Incoming-FAST",
                 Title = "Gelen EFT",
                 Topic = "http://localhost:8082/topics/cdc_eft/incoming_fast",
                 ApiKey = "a1b2c33d4e5f6g7h8i9jakblc",
                 Secret = "11561681-8ba5-4b46-bed0-905ae1769bc6",
                 PushServiceReference = "notify_push_incoming_fast",
                 SmsServiceReference = "notify_sms_incoming_fast",
                 EmailServiceReference = "notify_email_incoming_fast"
             },
             new GetSourcesResponse.SourceItem {
                 Source = "Incoming-QR",
                 Title = "Gelen EFT",
                 Topic = "http://localhost:8082/topics/cdc_eft/incoming_qr",
                 ApiKey = "a1b2c33d4e5f6g7h8i9jakblc",
                 Secret = "11561681-8ba5-4b46-bed0-905ae1769bc6",
                 PushServiceReference = "notify_push_incoming_qr",
                 SmsServiceReference = "notify_sms_incoming_qr",
                 EmailServiceReference = "notify_email_incoming_qr"
             }





              }
        });
    }
}
