using Elastic.Apm.Api;
using Microsoft.AspNetCore.Mvc;
using Notification.Profile.Business;
using Notification.Profile.Enum;
using Notification.Profile.Helper;
using Notification.Profile.Model;
using Swashbuckle.AspNetCore.Annotations;
using System.Reflection;

namespace Notification.Profile.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserRegistryController : ControllerBase
    {
        private readonly ITracer _tracer;
        private readonly ILogger<SourceController> _logger;
        private readonly ILogHelper _logHelper;
        private readonly IUserRegistry _userRegistry;

        public UserRegistryController(ILogger<SourceController> logger, ITracer tracer, ILogHelper logHelper, IUserRegistry userRegistry)
        {
            _logger = logger;
            _tracer = tracer;
            _logHelper = logHelper;
            _userRegistry = userRegistry;
        }

        [SwaggerOperation(
               Summary = "Returns UserRegistry list",
               Tags = new[] { "UserRegistry" }
           )]
        [HttpGet("/UserRegistries")]
        [SwaggerResponse(200, "Success, UserRegistry is returned successfully", typeof(GetUserRegistryModel))]

        public IActionResult GetUserRegistry()
        {
            GetUserRegistryModel returnValue = new GetUserRegistryModel();
            var span = _tracer.CurrentTransaction?.StartSpan("GetUserRegistrySpan", "GetUserRegistry");
            try
            {
                returnValue = _userRegistry.GetUserRegistry();

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
           Summary = "Insert UserRegistry ",
           Tags = new[] { "UserRegistry" }
       )]
        [HttpPost("/PostUserRegistry")]
        [SwaggerResponse(200, "Success, userRegistry is returned successfully", typeof(UserRegistryResponseModel))]

        public IActionResult PostUserRegistry([FromBody] UserRegistryRequestModel userRegistry)

        {
            var span = _tracer.CurrentTransaction?.StartSpan("PostUserRegistrySpan", "PostUserRegistry");
            UserRegistryResponseModel postUserRegistryResponse = new UserRegistryResponseModel();
            try
            {
                postUserRegistryResponse = _userRegistry.PostUserRegistry(userRegistry);
                if (postUserRegistryResponse != null && postUserRegistryResponse.Result == ResultEnum.Error)
                {
                    span.CaptureErrorLog(new ErrorLog("Error Message( StatusCode:" + postUserRegistryResponse.StatusCode + " - Message:" + postUserRegistryResponse.MessageList[0].ToString() + ")")
                    {
                        Level = "error",
                        ParamMessage = postUserRegistryResponse.StatusCode + " - " + postUserRegistryResponse.MessageList[0].ToString()
                    });
                    _logHelper.LogCreate(userRegistry, postUserRegistryResponse.StatusCode, MethodBase.GetCurrentMethod().Name, postUserRegistryResponse.MessageList[0]);
                    return this.StatusCode(Convert.ToInt32(postUserRegistryResponse.StatusCode), postUserRegistryResponse.MessageList);
                }
            }
            catch (Exception e)
            {
                span?.CaptureException(e);

                _logHelper.LogCreate(userRegistry, postUserRegistryResponse, MethodBase.GetCurrentMethod().Name, e.Message);
                return this.StatusCode(500, e.Message);
            }
            return Ok(postUserRegistryResponse);
        }
        [SwaggerOperation(
        Summary = "Update UserRegistry ",
        Tags = new[] { "UserRegistry" }
    )]
        [HttpPatch("/PatchUserRegistry/{id}")]
        [SwaggerResponse(200, "Success, userRegistry is updated successfully", typeof(UserRegistryResponseModel))]

        public IActionResult PatchUserRegistry([FromRoute] int id, [FromBody] UserRegistryRequestModel model)

        {
            var span = _tracer.CurrentTransaction?.StartSpan("PatchUserRegistrySpan", "PatchUserRegistryCode");
            UserRegistryResponseModel patchuserRegistryResponse = new UserRegistryResponseModel();
            try
            {
                patchuserRegistryResponse = _userRegistry.PatchUserRegistry(id, model.RegistryNo);
                if (patchuserRegistryResponse != null && patchuserRegistryResponse.Result == ResultEnum.Error)
                {
                    span.CaptureErrorLog(new ErrorLog("Error Message( StatusCode:" + patchuserRegistryResponse.StatusCode + " - Message:" + patchuserRegistryResponse.MessageList[0].ToString() + ")")
                    {
                        Level = "error",
                        ParamMessage = patchuserRegistryResponse.StatusCode + " - " + patchuserRegistryResponse.MessageList[0].ToString()
                    });
                    _logHelper.LogCreate(model, patchuserRegistryResponse.StatusCode, MethodBase.GetCurrentMethod().Name, patchuserRegistryResponse.MessageList[0]);
                    return this.StatusCode(Convert.ToInt32(patchuserRegistryResponse.StatusCode), patchuserRegistryResponse.MessageList);
                }
            }
            catch (Exception e)
            {
                span?.CaptureException(e);

                _logHelper.LogCreate(model, patchuserRegistryResponse, MethodBase.GetCurrentMethod().Name, e.Message);
                return this.StatusCode(500, e.Message);
            }
            return Ok(patchuserRegistryResponse);
        }
        [HttpGet("/GetUserRegistryNo/{registryNo}")]
        [SwaggerResponse(200, "Success, userRegistry is updated successfully", typeof(UserRegistryResponseModel))]

        public IActionResult GetUserRegistryWithRegistryNo([FromRoute]string registryNo)

        {
            var span = _tracer.CurrentTransaction?.StartSpan("GetUserRegistryWithRegistryNoSpan", "GetUserRegistryWithRegistryNo");
            UserRegistryResponseModel getuserRegistryResponse = new UserRegistryResponseModel();
            try
            {
                getuserRegistryResponse = _userRegistry.GetUserRegistryWithRegistryNo(registryNo);
                if (getuserRegistryResponse != null && getuserRegistryResponse.Result == ResultEnum.Error)
                {
                    span.CaptureErrorLog(new ErrorLog("Error Message( StatusCode:" + getuserRegistryResponse.StatusCode + " - Message:" + getuserRegistryResponse.MessageList[0].ToString() + ")")
                    {
                        Level = "error",
                        ParamMessage = getuserRegistryResponse.StatusCode + " - " + getuserRegistryResponse.MessageList[0].ToString()
                    });
                    _logHelper.LogCreate(registryNo, getuserRegistryResponse.StatusCode, MethodBase.GetCurrentMethod().Name, getuserRegistryResponse.MessageList[0]);
                    return this.StatusCode(Convert.ToInt32(getuserRegistryResponse.StatusCode), getuserRegistryResponse.MessageList);
                }
            }
            catch (Exception e)
            {
                span?.CaptureException(e);

                _logHelper.LogCreate(registryNo, getuserRegistryResponse, MethodBase.GetCurrentMethod().Name, e.Message);
                return this.StatusCode(500, e.Message);
            }
            return Ok(getuserRegistryResponse);
        }
        [HttpGet("/GetUserRegistryId/{id}")]
        [SwaggerResponse(200, "Success, userRegistry is updated successfully", typeof(UserRegistryResponseModel))]

        public IActionResult GetUserRegistryWithRegistryId([FromRoute] int id)

        {
            var span = _tracer.CurrentTransaction?.StartSpan("GetUserRegistryWithRegistryNoSpan", "GetUserRegistryWithRegistryNo");
            UserRegistryResponseModel getuserRegistryResponse = new UserRegistryResponseModel();
            try
            {
                getuserRegistryResponse = _userRegistry.GetUserRegistryWithId(id);
                if (getuserRegistryResponse != null && getuserRegistryResponse.Result == ResultEnum.Error)
                {
                    span.CaptureErrorLog(new ErrorLog("Error Message( StatusCode:" + getuserRegistryResponse.StatusCode + " - Message:" + getuserRegistryResponse.MessageList[0].ToString() + ")")
                    {
                        Level = "error",
                        ParamMessage = getuserRegistryResponse.StatusCode + " - " + getuserRegistryResponse.MessageList[0].ToString()
                    });
                    _logHelper.LogCreate(id, getuserRegistryResponse.StatusCode, MethodBase.GetCurrentMethod().Name, getuserRegistryResponse.MessageList[0]);
                    return this.StatusCode(Convert.ToInt32(getuserRegistryResponse.StatusCode), getuserRegistryResponse.MessageList);
                }
            }
            catch (Exception e)
            {
                span?.CaptureException(e);

                _logHelper.LogCreate(id, getuserRegistryResponse, MethodBase.GetCurrentMethod().Name, e.Message);
                return this.StatusCode(500, e.Message);
            }
            return Ok(getuserRegistryResponse);
        }
        [SwaggerOperation(
               Summary = "Delete UserRegistry",
               Tags = new[] { "UserRegistry" }
           )]
        [HttpDelete("/DeleteUserRegistry/{id}")]
        [SwaggerResponse(200, "Success, userRegistry is deleted successfully", typeof(UserRegistryResponseModel))]
        public IActionResult DeleteUserRegistry([FromRoute] int id)
        {
            var span = _tracer.CurrentTransaction?.StartSpan("DeleteSpan", "Delete");
            UserRegistryResponseModel respModel = new UserRegistryResponseModel();
            try
            {
                respModel = _userRegistry.DeleteUserRegistry(id);
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
}
