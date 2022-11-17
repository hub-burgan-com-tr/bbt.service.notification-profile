using System.Reflection;
using Elastic.Apm.Api;
using Microsoft.AspNetCore.Mvc;
using Notification.Profile.Business;
using Notification.Profile.Enum;
using Notification.Profile.Helper;
using Notification.Profile.Model;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("[controller]")]
public class ProductCodeController : ControllerBase
{

    private readonly ILogger<ProductCodeController> _logger;
    private readonly ITracer _tracer;
    private readonly ILogHelper _logHelper;
    private readonly IProductCode _IproductCode;

    public ProductCodeController(ILogger<ProductCodeController> logger, ITracer tracer, ILogHelper logHelper, IProductCode IproductCode)
    {
        _logger = logger;
        _tracer = tracer;
        _logHelper = logHelper;
        _IproductCode = IproductCode;
    }

    [SwaggerOperation(
           Summary = "Returns prodoctCode list",
           Tags = new[] { "ProductCode" }
       )]
    [HttpGet("/ProductCodes")]
    [SwaggerResponse(200, "Success, consumers is returned successfully", typeof(GetProductCodeResponse))]

    public IActionResult GetProductCode()
    {
        GetProductCodeResponse returnValue = new GetProductCodeResponse();
        var span = _tracer.CurrentTransaction?.StartSpan("GetProductCodeSpan", "GetProductCode");
        try
        {
            returnValue = _IproductCode.GetProductCode();

        }
        catch (Exception e)
        {
            span?.CaptureException(e);

            _logHelper.LogCreate(null, returnValue, MethodBase.GetCurrentMethod().Name, e.Message);
            return this.StatusCode(500, e.Message);
        }

        return Ok(returnValue);
    }
    [SwaggerOperation(
       Summary = "Insert productcode ",
       Tags = new[] { "ProductCode" }
   )]
    [HttpPost("/PostProductCode")]
    [SwaggerResponse(200, "Success, productCode is returned successfully", typeof(PostProductCodeRequest))]

    public IActionResult PostProductCode([FromBody] PostProductCodeRequest productCode)

    {
        var span = _tracer.CurrentTransaction?.StartSpan("PostProductCodeSpan", "PostProductCode");
        ProductCodeResponseModel postProductCodeResponse = new ProductCodeResponseModel();
        try
        {
            postProductCodeResponse = _IproductCode.PostProductCode(productCode);
            if (postProductCodeResponse != null && postProductCodeResponse.Result == ResultEnum.Error)
            {
                span.CaptureErrorLog(new ErrorLog("Error Message( StatusCode:" + postProductCodeResponse.StatusCode + " - Message:" + postProductCodeResponse.MessageList[0].ToString() + ")")
                {
                    Level = "error",
                    ParamMessage = postProductCodeResponse.StatusCode + " - " + postProductCodeResponse.MessageList[0].ToString()
                });
                _logHelper.LogCreate(productCode, postProductCodeResponse.StatusCode, MethodBase.GetCurrentMethod().Name, postProductCodeResponse.MessageList[0]);
                return this.StatusCode(Convert.ToInt32(postProductCodeResponse.StatusCode), postProductCodeResponse.MessageList);
            }
        }
        catch (Exception e)
        {
            span?.CaptureException(e);

            _logHelper.LogCreate(productCode, postProductCodeResponse, MethodBase.GetCurrentMethod().Name, e.Message);
            return this.StatusCode(500, e.Message);
        }
        return Ok(postProductCodeResponse);
    }
    [SwaggerOperation(
    Summary = "Update productcode ",
    Tags = new[] { "ProductCode" }
)]
    [HttpPost("/PatchProductCode/{id}")]
    [SwaggerResponse(200, "Success, productCode is updated successfully", typeof(PatchProductCode))]

    public IActionResult PatchProductCode([FromRoute] int id, [FromBody] PatchProductCode productCode)

    {
        var span = _tracer.CurrentTransaction?.StartSpan("PostProductCodeSpan", "PostProductCode");
        ProductCodeResponseModel postProductCodeResponse = new ProductCodeResponseModel();
        try
        {
            postProductCodeResponse = _IproductCode.PatchProductCode(id, productCode);
            if (postProductCodeResponse != null && postProductCodeResponse.Result == ResultEnum.Error)
            {
                span.CaptureErrorLog(new ErrorLog("Error Message( StatusCode:" + postProductCodeResponse.StatusCode + " - Message:" + postProductCodeResponse.MessageList[0].ToString() + ")")
                {
                    Level = "error",
                    ParamMessage = postProductCodeResponse.StatusCode + " - " + postProductCodeResponse.MessageList[0].ToString()
                });
                _logHelper.LogCreate(productCode, postProductCodeResponse.StatusCode, MethodBase.GetCurrentMethod().Name, postProductCodeResponse.MessageList[0]);
                return this.StatusCode(Convert.ToInt32(postProductCodeResponse.StatusCode), postProductCodeResponse.MessageList);
            }
        }
        catch (Exception e)
        {
            span?.CaptureException(e);

            _logHelper.LogCreate(productCode, postProductCodeResponse, MethodBase.GetCurrentMethod().Name, e.Message);
            return this.StatusCode(500, e.Message);
        }
        return Ok(postProductCodeResponse);
    }
    [SwaggerOperation(
           Summary = "Delete productCode",
           Tags = new[] { "ProductCode" }
       )]
    [HttpDelete("/DeleteProductCode/{id}")]
    [SwaggerResponse(200, "Success, productCode is deleted successfully", typeof(SourceResponseModel))]
    public IActionResult DeleteProductCode([FromRoute] int id)
    {
        var span = _tracer.CurrentTransaction?.StartSpan("DeleteSpan", "Delete");
        ProductCodeResponseModel respModel = new ProductCodeResponseModel();
        try
        {
            respModel = _IproductCode.DeleteProductCode(id);
            if (respModel != null && respModel.Result == Notification.Profile.Enum.ResultEnum.Error)
            {
                span.CaptureErrorLog(new ErrorLog("Error Message( StatusCode:" + respModel.StatusCode + " - Message:" + respModel.MessageList[0].ToString() + ")")
                {
                    Level = "error",
                    ParamMessage = respModel.StatusCode + " - " + respModel.MessageList[0].ToString()
                });
                _logHelper.LogCreate(id, respModel.StatusCode, MethodBase.GetCurrentMethod().Name, respModel.MessageList[0]);
                return this.StatusCode(Convert.ToInt32(respModel.StatusCode), respModel.MessageList);
            }

        }
        catch (Exception e)
        {
            span?.CaptureException(e);
            _logHelper.LogCreate(id, StatusCodeEnum.StatusCode500.ToString(), MethodBase.GetCurrentMethod().Name, e.Message);
            return this.StatusCode(500, e.Message);
        }
        return Ok(respModel);
    }

}
