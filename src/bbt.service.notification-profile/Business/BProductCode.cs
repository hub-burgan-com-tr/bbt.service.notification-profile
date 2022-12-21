using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Notification.Profile.Enum;
using Notification.Profile.Helper;
using Notification.Profile.Model;
using Notification.Profile.Model.Database;
using System.Text;

namespace Notification.Profile.Business
{
    public class BProductCode : IProductCode
    {
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;

        public BProductCode(IConfiguration configuration, IDistributedCache cache)
        {
            _configuration = configuration;
            _cache = cache; 
        }

        public ProductCodeResponseModel DeleteProductCode(int id)
        {
            var returnValue = new ProductCodeResponseModel();
            using (var db = new DatabaseContext())
            {
                var productCode = db.ProductCodes.FirstOrDefault(s => s.Id == id);
                if (productCode == null)
                {
                    returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode471);
                    returnValue.MessageList.Add(StructStatusCode.StatusCode471.ToString());
                    returnValue.Result = ResultEnum.Error;
                    return returnValue;

                }


                db.Remove(productCode);
                db.SaveChanges();
                returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
                returnValue.Result = ResultEnum.Success;
            }
            return returnValue;
        }

        public GetProductCodeResponse GetProductCode()
        {
            var returnValue = new GetProductCodeResponse();
            using (var db = new DatabaseContext())
            {
                var productCodeList = db.ProductCodes.ToList();
                returnValue.ProductCodes = productCodeList;
                returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
                returnValue.Result = ResultEnum.Success;
            }
            return returnValue;
        }
        public ProductCodeResponseModel GetProductCodeWithId(int Id)
        {
            var returnValue = new ProductCodeResponseModel();
            using (var db = new DatabaseContext())
            {
                var productCode = db.ProductCodes.FirstOrDefault(x=>x.Id==Id);
                returnValue.productCode = productCode;
                returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
                returnValue.Result = ResultEnum.Success;
            }
            return returnValue;
        }
        public async Task<GetProductCodeResponse> ProductCodeListRedis()
        {
          
            GetProductCodeResponse productCodeResponse = new GetProductCodeResponse();
            List<ProductCode> productCodeList = new List<ProductCode>();
            var cachedList = await _cache.GetAsync("notificationProductCodeRedis");

            if (cachedList != null && !string.IsNullOrEmpty(System.Text.Encoding.UTF8.GetString(cachedList)))
            {
                productCodeList = JsonConvert.DeserializeObject<List<ProductCode>>(System.Text.Encoding.UTF8.GetString(cachedList));
            }
            else
            {
                productCodeList = GetProductCode().ProductCodes;

                await _cache.SetAsync("notificationProductCodeRedis", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(productCodeList)),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(Convert.ToDouble(_configuration.GetSection("RedisProductListTimeOut").Value))
                });
            }
            productCodeResponse.ProductCodes = productCodeList;
            productCodeResponse.Result = ResultEnum.Success;

            return productCodeResponse;
        }
        public ProductCodeResponseModel PatchProductCode(int id, PatchProductCode productCodeModel)
        {
            var returnValue = new ProductCodeResponseModel();
            ProductCode productCode = new ProductCode();
            using (var db = new DatabaseContext())
            {
                productCode = db.ProductCodes.FirstOrDefault(x => x.Id == id);
                if (productCode != null)
                {
                    productCode.ProductCodeName = productCodeModel.ProductCodeName;
                    db.ProductCodes.Update(productCode);
                    db.SaveChanges();

                }
                else
                {
                    returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode471);
                    returnValue.MessageList.Add(StructStatusCode.StatusCode471.ToString());
                    returnValue.Result = ResultEnum.Error;
                    return returnValue;
                }
            }
            returnValue.productCode = productCode;
            returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
            returnValue.Result = ResultEnum.Success;
            return returnValue;
        }

        public ProductCodeResponseModel PostProductCode(PostProductCodeRequest request)
        {
            var returnValue = new ProductCodeResponseModel();
            ProductCode productCode = new ProductCode();
            using (var db = new DatabaseContext())
            {
                if (request != null && request.ProductCodeName != null)
                {
                    productCode.ProductCodeName = request.ProductCodeName;
                    db.Add(productCode);
                    db.SaveChanges();
                    returnValue.productCode = productCode;
                    returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
                    returnValue.Result = ResultEnum.Success;
                }
                else
                {
                    returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode471);
                    returnValue.Result = ResultEnum.Error;
                }
            }
            return returnValue;
        }
        
    }
}

