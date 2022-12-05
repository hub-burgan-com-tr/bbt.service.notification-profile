using bbt.framework.dengage.Business;
using bbt.service.notification_profile.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Notification.Profile.Enum;
using Notification.Profile.Helper;
using Notification.Profile.Model;
using Notification.Profile.Model.Database;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace Notification.Profile.Business
{
    public class BSource : ISource
    {
        private readonly IConfiguration _configuration;
        private readonly IConsumer _Iconsumer;
        private readonly ILogHelper _logHelper;
        private readonly IProductCode _IproductCode;


        public BSource(IConfiguration configuration, IConsumer Iconsumer, ILogHelper logHelper, IProductCode productCode)
        {
            _configuration = configuration;
            _Iconsumer = Iconsumer;
            _logHelper = logHelper;
            _IproductCode = productCode;
        }

        public SourceResponseModel Delete(int id)
        {
            var returnValue = new SourceResponseModel();
            using (var db = new DatabaseContext())
            {
                var source = db.Sources.FirstOrDefault(s => s.Id == id);
                if (source == null)
                {
                    returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode460);
                    returnValue.MessageList.Add(StructStatusCode.StatusCode460.ToString());
                    returnValue.Result = ResultEnum.Error;
                    return returnValue;
                    //  return new ObjectResult(id) { StatusCode = 460 };
                }

                var references = db.Consumers.FirstOrDefault(c => c.SourceId == id);
                if (references != null)
                {
                    returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode461);
                    returnValue.MessageList.Add(StructStatusCode.StatusCode461.ToString());
                    returnValue.Result = ResultEnum.Error;
                    return returnValue;
                }

                db.Remove(source);
                db.SaveChanges();
                returnValue.Result = ResultEnum.Success;
            }
            return returnValue;
        }

        public GetSourceTopicByIdResponse GetSourceById(int id)
        {
            GetSourceTopicByIdResponse returnValue = new GetSourceTopicByIdResponse();
            Model.Database.Source source = null;
            List<SourceServicesUrl> servicesUrls = null;
            using (var db = new DatabaseContext())
            {
                source = db.Sources.Where(s => s.Id == id).FirstOrDefault();
                servicesUrls = db.SourceServices.Where(s => id == s.SourceId).Select(x => new SourceServicesUrl
                {
                    Id = x.Id,
                    ServiceUrl = x.ServiceUrl

                }).ToList();
            }

            if (source == null)
            {
                returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode460);
                returnValue.MessageList.Add(StructStatusCode.StatusCode460.ToString());
                returnValue.Result = ResultEnum.Error;
                return returnValue;
            }

            SourceServicesUrl sourceServicesUrl = new SourceServicesUrl();

            returnValue.Id = source.Id;
            returnValue.Topic = source.Topic;
            returnValue.SmsServiceReference = source.SmsServiceReference;
            returnValue.EmailServiceReference = source.EmailServiceReference;
            returnValue.PushServiceReference = source.PushServiceReference;
            returnValue.Title_TR = source.Title_TR;
            returnValue.Title_EN = source.Title_EN;
            returnValue.ParentId = source.ParentId;
            returnValue.DisplayType = source.DisplayType;
            returnValue.ApiKey = source.ApiKey;
            returnValue.Secret = source.Secret;
            returnValue.ClientIdJsonPath = source.ClientIdJsonPath;
            returnValue.KafkaUrl = source.KafkaUrl;
            returnValue.KafkaCertificate = source.KafkaCertificate;
            returnValue.RetentationTime = source.RetentationTime;
            returnValue.ServiceUrlList = servicesUrls;
            returnValue.ProductCodeId = source.ProductCodeId;

            return returnValue;
        }

        public GetSourceConsumersResponse GetSourceConsumers(GetSourceConsumersRequestBody requestModel)
        {
            GetSourceConsumersResponse returnValue = new GetSourceConsumersResponse { Consumers = new List<GetSourceConsumersResponse.Consumer>() };
            dynamic message = null;
            List<Consumer> consumers = null;
            using (var db = new DatabaseContext())
            {
                // 0 nolu musteri generic musteri olarak kabul ediliyor. Banka kullanicilarin ozel durumlarda subscription olusturmalari icin kullanilacak.
                consumers = db.Consumers.Where(s => (s.Client == requestModel.client || s.Client == 0) && s.SourceId == requestModel.sourceid).ToList();
            }
            if (consumers == null || consumers.Count() < 1)
            {
                BGetCustomerInfo bGetCustomerInfo = new BGetCustomerInfo(null);
                ConfigurationHelper configurationHelper = new ConfigurationHelper();
                Console.WriteLine("GetTelephoneNumber" + "SourceId :" + requestModel.sourceid + " ClientId: " + requestModel.client);
                CustomerInformationModel customerInformationModel = bGetCustomerInfo.GetTelephoneNumber(new GetTelephoneNumberRequestModel() { name = requestModel.client.ToString() }).Result;
                _logHelper.LogCreate(requestModel, JsonConvert.SerializeObject(customerInformationModel), "GetTelephoneNumber", "");
                Console.WriteLine("customerInformationModel" + JsonConvert.SerializeObject(customerInformationModel));
                if (customerInformationModel != null && customerInformationModel.customerList != null && customerInformationModel.customerList.Count > 0)
                {
                    GetSourceTopicByIdResponse source = GetSourceById(requestModel.sourceid);
                    PostConsumerRequest postConsumerRequest = new PostConsumerRequest();
                    postConsumerRequest.Phone = new Phone();
                    postConsumerRequest.Phone.Number = Convert.ToInt32(customerInformationModel.customerList[0].gsmPhone.number);
                    postConsumerRequest.Phone.CountryCode = Convert.ToInt32(customerInformationModel.customerList[0].gsmPhone.country);
                    postConsumerRequest.Phone.Prefix = Convert.ToInt32(customerInformationModel.customerList[0].gsmPhone.prefix);
                    postConsumerRequest.Email = customerInformationModel.customerList[0].email;
                    postConsumerRequest.DeviceKey = customerInformationModel.customerList[0].device == null ? null : customerInformationModel.customerList[0].device.ToString();
                    postConsumerRequest.IsSmsEnabled = source.SmsServiceReference == "string" ? false : true;
                    postConsumerRequest.IsEmailEnabled = source.EmailServiceReference == "string" ? false : true;
                    postConsumerRequest.IsPushEnabled = source.PushServiceReference == "string" ? false : true;
                    postConsumerRequest.IsStaff = customerInformationModel.customerList[0].isStaff;
                    PostConsumerResponse postConsumerResponse = _Iconsumer.PostConsumers(requestModel.client, requestModel.sourceid, postConsumerRequest);
                    returnValue.Consumers.Add(new GetSourceConsumersResponse.Consumer
                    {
                        Id = postConsumerResponse.Consumer.Id,
                        Client = postConsumerResponse.Consumer.Client,
                        User = postConsumerResponse.Consumer.User,
                        IsPushEnabled = postConsumerResponse.Consumer.IsPushEnabled,
                        DeviceKey = postConsumerResponse.Consumer.DeviceKey,
                        Filter = postConsumerResponse.Consumer.Filter,
                        IsSmsEnabled = postConsumerResponse.Consumer.IsSmsEnabled,
                        Phone = postConsumerResponse.Consumer.Phone,
                        IsEmailEnabled = postConsumerResponse.Consumer.IsEmailEnabled,
                        Email = postConsumerResponse.Consumer.Email,
                        IsStaff = postConsumerResponse.Consumer.IsStaff


                    });
                }
                else
                {
                    returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode470);
                    returnValue.MessageList.Add(customerInformationModel.returnDescription);
                    returnValue.Result = ResultEnum.Error;
                    return returnValue;
                }
            }


            // Eger filtre yoksa bosu bosuna deserialize etme             
            if (consumers.Count > 1 && consumers.FirstOrDefault(c => c.Client == 0) != null)
            {
                consumers.Remove(consumers.FirstOrDefault(c => c.Client == 0));
            }
            if (consumers.Any(c => c.Filter != null) && requestModel.jsonData is not null)
            {
                requestModel.jsonData = requestModel.jsonData.Replace(@"\", "");
                message = JsonConvert.DeserializeObject(requestModel.jsonData);
            }

            consumers.ForEach(async c =>
            {
                bool canSend = true; // eger filtre yoksa gonderim sekteye ugramasin.

                if (c.Filter != null && requestModel.jsonData is not null)
                {
                    canSend = Extensions.Evaluate(c.Filter, message);
                }

                if (canSend)
                {
                    if (c.Client == 0)
                    {
                        BGetCustomerInfo bGetCustomerInfo = new BGetCustomerInfo(null);
                        Console.WriteLine("GetTelephoneNumberClient0" + "SourceId :" + requestModel.sourceid + " ClientId: " + requestModel.client);
                        ConfigurationHelper configurationHelper = new ConfigurationHelper();
                        CustomerInformationModel customerInformationModel = await bGetCustomerInfo.GetTelephoneNumber(new GetTelephoneNumberRequestModel() { name = requestModel.client.ToString() });
                        _logHelper.LogCreate(requestModel, JsonConvert.SerializeObject(customerInformationModel), "GetTelephoneNumberCustomerInformationModel0", "");
                        if (customerInformationModel != null && customerInformationModel.customerList != null && customerInformationModel.customerList.Count > 0)
                        {
                            c.Phone = new Phone();
                            c.Phone.Number = Convert.ToInt32(customerInformationModel.customerList[0].gsmPhone.number);
                            c.Phone.CountryCode = Convert.ToInt32(customerInformationModel.customerList[0].gsmPhone.country);
                            c.Phone.Prefix = Convert.ToInt32(customerInformationModel.customerList[0].gsmPhone.prefix);
                        }

                    }
                    returnValue.Consumers.Add(new GetSourceConsumersResponse.Consumer
                    {
                        Id = c.Id,
                        Client = c.Client,
                        User = c.User,
                        IsPushEnabled = c.IsPushEnabled,
                        DeviceKey = c.DeviceKey,
                        Filter = c.Filter,
                        IsSmsEnabled = c.IsSmsEnabled,
                        Phone = c.Phone,
                        IsEmailEnabled = c.IsEmailEnabled,
                        Email = c.Email,
                        IsStaff = c.IsStaff
                    });
                }
            });
            return returnValue;
        }
        public GetSourcesResponse GetSourceWithSearchModel(SearchSourceModel model)
        {
            GetSourcesResponse getSourcesResponse = new GetSourcesResponse();
            IQueryable<Model.Source> sources;
            getSourcesResponse.Sources = new List<Model.Source>();
            using (var db = new DatabaseContext())
            {

                sources = (from source in db.Sources
                           join productCode in db.ProductCodes on source.ProductCodeId equals productCode.Id
                            into pc
                           from p in pc.DefaultIfEmpty()
                           where (String.IsNullOrEmpty(model.Topic) || source.Topic.Contains(model.Topic)) && (String.IsNullOrEmpty(model.Title) || source.Title_TR.Contains(model.Title)) &&
                           (String.IsNullOrEmpty(model.SmsServiceReference) || source.SmsServiceReference.Contains(model.SmsServiceReference)) &&
                           (String.IsNullOrEmpty(model.EmailServiceReference) || source.EmailServiceReference.Contains(model.EmailServiceReference))
                           select new Model.Source
                           {
                               Id = source.Id,
                               ApiKey = source.ApiKey,
                               ClientIdJsonPath = source.ClientIdJsonPath,
                               ParentId = source.ParentId,
                               KafkaUrl = source.KafkaUrl,
                               KafkaCertificate = source.KafkaCertificate,
                               ProductCodeId = source.ProductCodeId,
                               DisplayType = (int)source.DisplayType,
                               EmailServiceReference = source.EmailServiceReference,
                               PushServiceReference = source.PushServiceReference,
                               RetentationTime = source.RetentationTime,
                               SmsServiceReference = source.SmsServiceReference,
                               Title = new Model.Source.TitleLabel { EN = source.Title_EN, TR = source.Title_TR },
                               Topic = source.Topic,
                               Secret = source.Secret,
                               ProductCodeName = p == null ? null : p.ProductCodeName
                           }).Skip(((model.CurrentPage) - 1) * model.RequestItemSize)
                            .Take(model.RequestItemSize);

                getSourcesResponse.Result = ResultEnum.Success;
                getSourcesResponse.Sources = sources.ToList();
                getSourcesResponse.Count =db.Sources.Count();
            }
            return getSourcesResponse;

        }
        public GetSourcesResponse GetSources()
        {

            List<Model.Database.Source> sources = null;
            using (var db = new DatabaseContext())
            {
                sources = db.Sources
                    .Include(s => s.Parameters)
                    .Include(s => s.Children)
                    .ToList();
            }

            return new GetSourcesResponse
            {
                Sources = sources.Where(s => s.ParentId == null).Select(s => BuildSource(s)).ToList()
            };


            Model.Source BuildSource(Model.Database.Source s)
            {
                return new Model.Source
                {
                    Id = s.Id,
                    Title = new Model.Source.TitleLabel { EN = s.Title_EN, TR = s.Title_TR },
                    Children = s.Children.Select(c => BuildSource(c)).ToList(),
                    Parameters = s.Parameters.Select(p => new Model.Source.SourceParameter
                    {
                        JsonPath = p.JsonPath,
                        Type = p.Type,
                        Title = new Model.Source.TitleLabel { EN = p.Title_EN, TR = p.Title_TR },
                    }).ToList(),
                    Topic = s.Topic,
                    DisplayType = (int)s.DisplayType,
                    ClientIdJsonPath = s.ClientIdJsonPath,
                    ApiKey = s.ApiKey,
                    Secret = s.Secret,
                    PushServiceReference = s.PushServiceReference,
                    SmsServiceReference = s.SmsServiceReference,
                    EmailServiceReference = s.EmailServiceReference,
                    KafkaCertificate = s.KafkaCertificate,
                    KafkaUrl = s.KafkaUrl,
                    RetentationTime = s.RetentationTime,
                    ProductCodeId = s.ProductCodeId


                };

            }

        }

        public SourceResponseModel Patch(int id, PatchSourceRequest data)
        {
            SourceResponseModel sourceResp = new SourceResponseModel();
            using (var db = new DatabaseContext())
            {
                var source = db.Sources.FirstOrDefault(s => s.Id == id);
                if (source == null)
                {

                    sourceResp.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode470);
                    sourceResp.MessageList.Add(StructStatusCode.StatusCode470.ToString());
                    sourceResp.Result = ResultEnum.Error;
                    return sourceResp;

                }

                //if (data.Title != null) source.Title = data.Title;
                if (data.Title_TR != null) source.Title_TR = data.Title_TR;
                if (data.Title_EN != null) source.Title_EN = data.Title_EN;
                if (data.Topic != null) source.Topic = data.Topic;
                if (data.ApiKey != null) source.ApiKey = data.ApiKey;
                if (data.Secret != null) source.Secret = data.Secret;
                if (data.PushServiceReference != null) source.PushServiceReference = data.PushServiceReference;
                if (data.SmsServiceReference != null) source.SmsServiceReference = data.SmsServiceReference;
                if (data.EmailServiceReference != null) source.EmailServiceReference = data.EmailServiceReference;
                if (data.KafkaUrl != null) source.KafkaUrl = data.KafkaUrl;
                if (data.ClientIdJsonPath != null) source.ClientIdJsonPath = data.ClientIdJsonPath;
                if (data.KafkaCertificate != null) source.KafkaCertificate = data.KafkaCertificate;
                if (data.RetentationTime != null) source.RetentationTime = data.RetentationTime;
                if (data.ProductCodeId != null) source.ProductCodeId = data.ProductCodeId;
                db.Sources.Update(source);
                db.SaveChanges();
                sourceResp.Result = ResultEnum.Success;
            }
            if (data.CheckDeploy == true)
            {

                string path = _configuration.GetSection("NotificationProdSearchEndPoint").Value.ToString();
                var uri = new Uri(path);
                GetSourcesResponse result = new GetSourcesResponse();
                using (var httpClient = new HttpClient())
                {
                    SearchSourceModel searchSourceModel = new SearchSourceModel();
                    searchSourceModel.Topic = data.Topic;
                    data.CheckDeploy = false;
                    var json = JsonConvert.SerializeObject(searchSourceModel);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
                    result = response.Content.ReadAsAsync<GetSourcesResponse>().Result;
                }
                if (result != null && result.Sources.Count > 0)
                {
                    string pathEnd = _configuration.GetSection("NotificationProdPatchEndPoint").Value.ToString().Replace("{id}", result.Sources.FirstOrDefault().Id.ToString());
                    var uriEnd = new Uri(pathEnd);
                    using (var httpClient = new HttpClient())
                    {
                        data.CheckDeploy = false;
                        var json = JsonConvert.SerializeObject(data);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = httpClient.PatchAsync(uriEnd, content).Result;
                        if (response.IsSuccessStatusCode == true)
                        {
                            string resultEnd = response.Content.ReadAsStringAsync().Result;
                        }
                        else
                        {
                            sourceResp.Result = ResultEnum.Error;
                            sourceResp.MessageList.Add("Prod ortamına kaydederken hata oluştu. "+response.ReasonPhrase);
                        }
                    }
                }

                else
                {
                    string paths = _configuration.GetSection("NotificationProdEndPoint").Value.ToString();
                    var uris = new Uri(paths);
                    using (var httpClient = new HttpClient())
                    {
                        data.CheckDeploy = false;
                        var json = JsonConvert.SerializeObject(data);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = httpClient.PostAsync(uris, content).Result;
                        if (response.IsSuccessStatusCode == true)
                        {
                            string resultres = response.Content.ReadAsStringAsync().Result;
                        }
                        else
                        {
                            sourceResp.Result = ResultEnum.Error;
                            sourceResp.MessageList.Add("Prod ortamına kaydederken hata oluştu. " + response.ReasonPhrase);
                        }
                    }

                }
            }
            return sourceResp;
        }

        public SourceResponseModel Post(PostSourceRequest data)
        {

            SourceResponseModel sourceResp = new SourceResponseModel();


            using (var db = new DatabaseContext())
            {
                db.Add(new Model.Database.Source
                {
                    //Id = data.Id,
                    //Title = data.Title,
                    Topic = data.Topic,
                    ApiKey = data.ApiKey,
                    Secret = data.Secret,
                    PushServiceReference = data.PushServiceReference,
                    SmsServiceReference = data.SmsServiceReference,
                    EmailServiceReference = data.EmailServiceReference,
                    KafkaUrl = data.KafkaUrl,
                    KafkaCertificate = data.KafkaCertificate,
                    DisplayType = (SourceDisplayType)data.DisplayType,
                    Title_EN = data.Title_EN,
                    Title_TR = data.Title_TR,
                    ParentId = data.ParentId,
                    ClientIdJsonPath = data.ClientIdJsonPath,
                    RetentationTime = data.RetentationTime,
                    ProductCodeId = data.ProductCodeId,

                });

                db.SaveChanges();
                sourceResp.Result = ResultEnum.Success;

                if (data.CheckDeploy == true)
                {
                    string paths = _configuration.GetSection("NotificationProdEndPoint").Value.ToString();
                    var uris = new Uri(paths);
                    using (var httpClient = new HttpClient())
                    {
                        data.CheckDeploy = false;
                        var json = JsonConvert.SerializeObject(data);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = httpClient.PostAsync(uris, content).Result;
                     
                        if (response.IsSuccessStatusCode == true)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                        }
                        else
                        {
                            sourceResp.Result = ResultEnum.Error;
                            sourceResp.MessageList.Add("Prod ortamına kaydederken hata oluştu. " + response.ReasonPhrase);
                        }
                    }
                }

                return sourceResp;
            }
        }
        public SourceListResponse GetSourceByProductCodeId(string productCodeName)
        {
            SourceListResponse response = new SourceListResponse();
            GetProductCodeResponse productCodeResponse = new GetProductCodeResponse();
            productCodeResponse = _IproductCode.ProductCodeListRedis().Result;
            List<ProductCode> productCodeList = new List<ProductCode>();
            if (productCodeResponse != null && productCodeResponse.Result == ResultEnum.Success)
            {
                productCodeList = productCodeResponse.ProductCodes;
                if (productCodeList.Count > 0)
                {
                    ProductCode product = productCodeList.FirstOrDefault(x => x.ProductCodeName == productCodeName);
                    List<Notification.Profile.Model.Database.Source> sourceList = new List<Notification.Profile.Model.Database.Source>();
                    using (var db = new DatabaseContext())
                    {
                        if (product != null)
                        {
                            sourceList = db.Sources.Where(x => x.ProductCodeId == product.Id).ToList();
                            response.sources = sourceList;
                        }
                    }
                }
                else
                {
                    response.Result = ResultEnum.Error;
                    response.MessageList.Add("Product Code list is null!");
                    return response;
                }
            }
            else
            {
                response.Result = ResultEnum.Error;
                response.MessageList.Add("Product Code list is error!.");
                return response;
            }


            return response;
        }
    }
}
