using System.Reflection;
using Elastic.Apm.Api;
using Microsoft.AspNetCore.Mvc;
using Notification.Profile.Business;
using Notification.Profile.Helper;
using Notification.Profile.Model;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("[controller]")]
public class SourceLogController : ControllerBase
{

    private readonly ILogger<SourceLogController> _logger;
    private readonly ITracer _tracer;
    private readonly ILogHelper _logHelper;
    private readonly ISourceLog _Ilog;

    public SourceLogController(ILogger<SourceLogController> logger, ITracer tracer, ILogHelper logHelper, ISourceLog Ilog)
    {
        _logger = logger;
        _tracer = tracer;
        _logHelper = logHelper;
        _Ilog = Ilog;
    }

    //[SwaggerOperation(
    //       Summary = "Returns message send log",
    //       Tags = new[] { "SourceLogs" }
    //   )]
    //[HttpGet("/SourceLogs")]
    //[SwaggerResponse(200, "Success, log is returned successfully", typeof(GetSourceLogResponse))]

    //public IActionResult GetSourceLogs([FromBody]GetSourceLogRequest logRequestModel)
    //{
    //    GetSourceLogResponse returnValue = new GetSourceLogResponse();
    //    var span = _tracer.CurrentTransaction?.StartSpan("GetSourceLogsSpan", "GetSourceLogs");
    //    try
    //    {
    //        returnValue = _Ilog.GetSourceLogs(logRequestModel);
    //    }
    //    catch(Exception e)
    //    {
    //        span?.CaptureException(e);
         
    //        _logHelper.LogCreate(logRequestModel, returnValue, MethodBase.GetCurrentMethod().Name, e.Message);
    //        return this.StatusCode(500, e.Message);
    //    }

    //    return Ok(returnValue);
    //}

    [SwaggerOperation(
           Summary = "Returns source log",
           Tags = new[] { "SourceLogs" }
       )]
    [HttpPost("/SourceLogs")]
    [SwaggerResponse(200, "Success, log is returned successfully", typeof(GetSourceLogResponse))]

    public IActionResult GetSourceLogs([FromBody] GetSourceLogRequest logRequestModel)
    {
        GetSourceLogResponse returnValue = new GetSourceLogResponse();
        var span = _tracer.CurrentTransaction?.StartSpan("GetSourceLogsSpan", "GetSourceLogs");
        try
        {
            returnValue = _Ilog.GetSourceLogs(logRequestModel);
        }
        catch (Exception e)
        {
            span?.CaptureException(e);

            _logHelper.LogCreate(logRequestModel, returnValue, MethodBase.GetCurrentMethod().Name, e.Message);
            return this.StatusCode(500, e.Message);
        }

        return Ok(returnValue);
    }


}
