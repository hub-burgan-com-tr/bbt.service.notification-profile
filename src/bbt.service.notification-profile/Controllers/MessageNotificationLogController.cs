using System.Reflection;
using Elastic.Apm.Api;
using Microsoft.AspNetCore.Mvc;
using Notification.Profile.Business;
using Notification.Profile.Helper;
using Notification.Profile.Model;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("[controller]")]
public class MessageNotificationLogController : ControllerBase
{

    private readonly ILogger<MessageNotificationLogController> _logger;
    private readonly ITracer _tracer;
    private readonly ILogHelper _logHelper;
    private readonly IMessageNotificationLog _Ilog;

    public MessageNotificationLogController(ILogger<MessageNotificationLogController> logger, ITracer tracer, ILogHelper logHelper, IMessageNotificationLog Ilog)
    {
        _logger = logger;
        _tracer = tracer;
        _logHelper = logHelper;
        _Ilog = Ilog;
    }

    [SwaggerOperation(
           Summary = "Returns message send log",
           Tags = new[] { "MessageNotificationLog" }
       )]
    [HttpPost("/MessageNotificationLogs")]
    [SwaggerResponse(200, "Success, log is returned successfully", typeof(GetMessageNotificationLogResponse))]

    public IActionResult GetMessageNotificationLogs([FromBody]GetMessageNotificationLogRequest logRequestModel)
    {
        GetMessageNotificationLogResponse returnValue = new GetMessageNotificationLogResponse();
        var span = _tracer.CurrentTransaction?.StartSpan("GetMessageNotificationLogSpan", "GetMessageNotificationLog");
        try
        {
            returnValue = _Ilog.GetMessageNotificationLogs(logRequestModel);
        }
        catch(Exception e)
        {
            span?.CaptureException(e);
         
            _logHelper.LogCreate(logRequestModel, returnValue, MethodBase.GetCurrentMethod().Name, e.Message);
            return this.StatusCode(500, e.Message);
        }

        return Ok(returnValue);
    }
   

}
