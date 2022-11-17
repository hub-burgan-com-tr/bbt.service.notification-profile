using Elastic.Apm.Api;
using Microsoft.AspNetCore.Mvc;
using Notification.Profile.Business;
using Notification.Profile.Enum;
using Notification.Profile.Helper;
using Notification.Profile.Model;
using System.Reflection;

[ApiController]
[Route("[controller]")]
public class DengageController : ControllerBase
{

    private readonly ILogger<DengageController> _logger;
    private readonly ITracer _tracer;
    private readonly ILogHelper _logHelper;
    private readonly IDengage _dengage;
    private readonly IGetTemplate _template;

    public DengageController(ILogger<DengageController> logger, ITracer tracer, ILogHelper logHelper, IDengage dengage, IGetTemplate template)
    {
        _logger = logger;
        _tracer = tracer;
        _logHelper = logHelper;
        _dengage = dengage;
        _template = template;   

    }

    [HttpGet("/dengage/email")]
    public IActionResult GetDengageEmailContent()
    {
        GetDengageContentResponse resp = new GetDengageContentResponse();
        var span = _tracer.CurrentTransaction?.StartSpan("GetDengageEmailContentSpan", "GetDengageEmailContent");

        try
        {
            resp = _dengage.GetDengageEmailContent();
            if (resp != null && resp.message != StructResult.Successful)
            {
                span.CaptureErrorLog(new ErrorLog("Error Message( StatusCode:" + resp.message + " - Message:" + resp.message + ")")
                {
                    Level = "error",
                    ParamMessage = resp.message,
                }
                );

                return this.StatusCode(500, resp.message);
            }
            else
            {
                return Ok(resp);
            }
        }
        catch (Exception e)
        {
            span?.CaptureException(e);
            _logHelper.LogCreate("", resp, MethodBase.GetCurrentMethod().Name, e.Message);
            return this.StatusCode(500, e.Message);
        }
        return Ok(resp);
    }
    [HttpGet("/dengage/sms")]
    public IActionResult GetDengageSmsContent()
    {
        GetDengageContentResponse resp = new GetDengageContentResponse();
        var span = _tracer.CurrentTransaction?.StartSpan("GetDengageSmsContentSpan", "GetDengageSmsContent");

        try
        {
            resp = _dengage.GetDengageSmsContent();
            if (resp != null && resp.message != StructResult.Successful)
            {
                span.CaptureErrorLog(new ErrorLog("Error Message( StatusCode:" + resp.message + " - Message:" + resp.message + ")")
                {
                    Level = "error",
                    ParamMessage = resp.message,
                }
                );

                return this.StatusCode(500, resp.message);
            }
            else
            {
                return Ok(resp);
            }
        }
        catch (Exception e)
        {
            span?.CaptureException(e);
            _logHelper.LogCreate("", resp, MethodBase.GetCurrentMethod().Name, e.Message);
            return this.StatusCode(500, e.Message);
        }
        return Ok(resp);
    }
    [HttpGet("/dengage/push")]
    public IActionResult GetDengagePushContent()
    {
        GetDengageContentResponse resp = new GetDengageContentResponse();
        var span = _tracer.CurrentTransaction?.StartSpan("GetDengagePushContentSpan", "GetDengagePushContent");

        try
        {
            resp = _dengage.GetDengagePushContent();
            if (resp != null && resp.message != StructResult.Successful)
            {
                span.CaptureErrorLog(new ErrorLog("Error Message( StatusCode:" + resp.message + " - Message:" + resp.message + ")")
                {
                    Level = "error",
                    ParamMessage = resp.message,
                }
                );

                return this.StatusCode(500, resp.message);
            }
            else
            {
                return Ok(resp);
            }
        }
        catch (Exception e)
        {
            span?.CaptureException(e);
            _logHelper.LogCreate("", resp, MethodBase.GetCurrentMethod().Name, e.Message);
            return this.StatusCode(500, e.Message);
        }
        return Ok(resp);
    }


    [HttpGet("/dengage/MessagingGatewayEmail")]
    public IActionResult GetMessagingGatewayEmailContent()
    {
        GetTemplateResponseModel resp = new GetTemplateResponseModel();
        var span = _tracer.CurrentTransaction?.StartSpan("GetMessagingGatewayEmailContent", "GetMessagingGatewayEmailContent");

        try
        {
            resp = _template.GetTemplateMailBurganOn().Result;
            if (resp.Result==ResultEnum.Success )
            {
                return Ok(resp);
            }
            else
            {
                span.CaptureErrorLog(new ErrorLog("Error Message( StatusCode:" + resp.StatusCode + " - Message:" + resp.MessageList[0].ToString() + ")")
                {
                    Level = "error",
                    ParamMessage = resp.StatusCode + " - " + resp.MessageList[0].ToString()
                });
                _logHelper.LogCreate(null, resp.StatusCode, MethodBase.GetCurrentMethod().Name, resp.MessageList[0]);
                return this.StatusCode(Convert.ToInt32(resp.StatusCode), resp.MessageList);
            }
        }
        catch (Exception e)
        {
            span?.CaptureException(e);
            _logHelper.LogCreate("", resp, MethodBase.GetCurrentMethod().Name, e.Message);
            return this.StatusCode(500, e.Message);
        }
        return Ok(resp);
    }
    [HttpGet("/dengage/MessagingGatewayPush")]
    public IActionResult GetMessagingGatewayPushContent()
    {
        GetTemplateResponseModel resp = new GetTemplateResponseModel();
        var span = _tracer.CurrentTransaction?.StartSpan("GetMessagingGatewayEmailContent", "GetMessagingGatewayEmailContent");

        try
        {
            resp = _template.GetTemplatePushBurganOn().Result;
            if (resp.Result == ResultEnum.Success)
            {
                return Ok(resp);
            }
            else
            {
                span.CaptureErrorLog(new ErrorLog("Error Message( StatusCode:" + resp.StatusCode + " - Message:" + resp.MessageList[0].ToString() + ")")
                {
                    Level = "error",
                    ParamMessage = resp.StatusCode + " - " + resp.MessageList[0].ToString()
                });
                _logHelper.LogCreate(null, resp.StatusCode, MethodBase.GetCurrentMethod().Name, resp.MessageList[0]);
                return this.StatusCode(Convert.ToInt32(resp.StatusCode), resp.MessageList);
            }
        }
        catch (Exception e)
        {
            span?.CaptureException(e);
            _logHelper.LogCreate("", resp, MethodBase.GetCurrentMethod().Name, e.Message);
            return this.StatusCode(500, e.Message);
        }
        return Ok(resp);
    }
        [HttpGet("/dengage/MessagingGatewaySms")]
        public IActionResult GetMessagingGatewaySmsContent()
        {
            GetTemplateResponseModel resp = new GetTemplateResponseModel();
            var span = _tracer.CurrentTransaction?.StartSpan("GetMessagingGatewayEmailContent", "GetMessagingGatewayEmailContent");

            try
            {
                resp = _template.GetTemplateSmsBurganOn().Result;
                if (resp.Result == ResultEnum.Success)
                {
                    return Ok(resp);
                }
                else
                {
                    span.CaptureErrorLog(new ErrorLog("Error Message( StatusCode:" + resp.StatusCode + " - Message:" + resp.MessageList[0].ToString() + ")")
                    {
                        Level = "error",
                        ParamMessage = resp.StatusCode + " - " + resp.MessageList[0].ToString()
                    });
                    _logHelper.LogCreate(null, resp.StatusCode, MethodBase.GetCurrentMethod().Name, resp.MessageList[0]);
                    return this.StatusCode(Convert.ToInt32(resp.StatusCode), resp.MessageList);
                }
            }
            catch (Exception e)
            {
                span?.CaptureException(e);
                _logHelper.LogCreate("", resp, MethodBase.GetCurrentMethod().Name, e.Message);
                return this.StatusCode(500, e.Message);
            }
            return Ok(resp);
        }
}


