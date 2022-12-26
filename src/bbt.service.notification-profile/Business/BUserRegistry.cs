using Notification.Profile.Enum;
using Notification.Profile.Helper;
using Notification.Profile.Model;
using Notification.Profile.Model.Database;

namespace Notification.Profile.Business
{
    public class BUserRegistry:IUserRegistry
    {
        private readonly IConfiguration _configuration;

        public BUserRegistry(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public UserRegistryResponseModel DeleteUserRegistry(int id)
        {
            var returnValue = new UserRegistryResponseModel();
            using (var db = new DatabaseContext())
            {
                var userRegistry = db.UserRegistry.FirstOrDefault(s => s.Id == id);
                if (userRegistry == null)
                {
                    returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode474);
                    returnValue.MessageList.Add(StructStatusCode.StatusCode474.ToString());
                    returnValue.Result = ResultEnum.Error;
                    return returnValue;

                }


                db.Remove(userRegistry);
                db.SaveChanges();
                returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
                returnValue.Result = ResultEnum.Success;
            }
            return returnValue;
        }

        public GetUserRegistryModel GetUserRegistry()
        {
            var returnValue = new GetUserRegistryModel();
            using (var db = new DatabaseContext())
            {
                var userRegistryList = db.UserRegistry.ToList();
                returnValue.userRegistryList = userRegistryList;
                returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
                returnValue.Result = ResultEnum.Success;
            }
            return returnValue;
        }
        public UserRegistryResponseModel GetUserRegistryWithId(int Id)
        {
            var returnValue = new UserRegistryResponseModel();
            using (var db = new DatabaseContext())
            {
                var userRegistry = db.UserRegistry.FirstOrDefault(x => x.Id == Id);
                returnValue.userRegistry = userRegistry;
                returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
                returnValue.Result = ResultEnum.Success;
            }
            return returnValue;
        }
        public UserRegistryResponseModel GetUserRegistryWithRegistryNo(string registryNo)
        {
            var returnValue = new UserRegistryResponseModel();
            using (var db = new DatabaseContext())
            {
                var userRegistry = db.UserRegistry.FirstOrDefault(x => x.RegistryNo == registryNo);
                returnValue.userRegistry = userRegistry;
                returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
                returnValue.Result = ResultEnum.Success;
            }
            return returnValue;
        }
        public UserRegistryResponseModel PatchUserRegistry(int id, string registryNo)
        {
            var returnValue = new UserRegistryResponseModel();
            UserRegistry userRegistry = new UserRegistry();
            using (var db = new DatabaseContext())
            {
                userRegistry = db.UserRegistry.FirstOrDefault(x => x.Id == id);
                if (userRegistry != null)
                {
                    userRegistry.RegistryNo = registryNo;
                    db.UserRegistry.Update(userRegistry);
                    db.SaveChanges();

                }
                else
                {
                    returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode474);
                    returnValue.MessageList.Add(StructStatusCode.StatusCode474.ToString());
                    returnValue.Result = ResultEnum.Error;
                    return returnValue;
                }
            }
            returnValue.userRegistry = userRegistry;
            returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
            returnValue.Result = ResultEnum.Success;
            return returnValue;
        }

        public UserRegistryResponseModel PostUserRegistry(UserRegistryRequestModel request)
        {
            var returnValue = new UserRegistryResponseModel();
            UserRegistry userRegistry = new UserRegistry();
            using (var db = new DatabaseContext())
            {
                if (request != null && request.RegistryNo != null)
                {
                    userRegistry.RegistryNo = request.RegistryNo;
                    db.Add(userRegistry);
                    db.SaveChanges();
                    returnValue.userRegistry = userRegistry;
                    returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
                    returnValue.Result = ResultEnum.Success;
                }
                else
                {
                    returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode474);
                    returnValue.Result = ResultEnum.Error;
                }
            }
            return returnValue;
        }
    }
}
