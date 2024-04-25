using bbt.framework.dengage.Business;
using bbt.service.notification_profile.Helper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Notification.Profile.Enum;
using Notification.Profile.Helper;
using Notification.Profile.Model;
using Notification.Profile.Model.Database;
using System.Data;
using System.Net.Http.Headers;
using System.Text;

namespace Notification.Profile.Business
{
    public class BSource : ISource
    {
        private readonly IConfiguration _configuration;
        private readonly IConsumer _Iconsumer;
        private readonly ILogHelper _logHelper;
        private readonly IProductCode _IproductCode;
        private readonly IinstandReminder _IinstandReminder;
        private readonly ISourceLog _ISourceLog;

        public BSource(IConfiguration configuration, IConsumer Iconsumer, ILogHelper logHelper, IProductCode productCode, IinstandReminder ıinstandReminder, ISourceLog ISourceLog)
        {
            _configuration = configuration;
            _Iconsumer = Iconsumer;
            _logHelper = logHelper;
            _IproductCode = productCode;
            _IinstandReminder = ıinstandReminder;
            _ISourceLog = ISourceLog;
        }

        public SourceResponseModel Delete(int id, string user)
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

                SourceLogRequest logRequest = new SourceLogRequest();
                logRequest.sourceLog = source;
                logRequest.User = user;
                logRequest.MethodType = "Delete";
                logRequest.Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                SourceLogResponse logResponse = _ISourceLog.PostSourceLog(logRequest);

                if (logResponse.Result == ResultEnum.Error)
                {
                    returnValue.MessageList.Add("SourceLog kaydederken hata oluştu. ");
                }
            }

            return returnValue;
        }

        public GetSourceTopicByIdResponse GetSourceById(int id)
        {
            using (var db = new DatabaseContext())
            {
                var res = db.Sources.AsNoTracking()
                    .Where(t => t.Id == id)
                    .Include(t => t.SourceServices)
                    .Include(t => t.ProductCode)
                    .FirstOrDefault();

                var returnValue = new GetSourceTopicByIdResponse();

                if (res == null)
                {
                    returnValue.StatusCode = EnumHelper.GetDescription(StatusCodeEnum.StatusCode460);
                    returnValue.MessageList.Add(StructStatusCode.StatusCode460.ToString());
                    returnValue.Result = ResultEnum.Error;
                    return returnValue;
                }

                var servicesUrls = res.SourceServices.Select(x => new SourceServicesUrl
                {
                    Id = x.Id,
                    ServiceUrl = x.ServiceUrl

                }).ToList();

                returnValue.StatusCode = EnumHelper.GetDescription(StatusCodeEnum.StatusCode200);
                returnValue.Id = id;
                returnValue.Topic = res.Topic;
                returnValue.SmsServiceReference = res.SmsServiceReference;
                returnValue.EmailServiceReference = res.EmailServiceReference;
                returnValue.PushServiceReference = res.PushServiceReference;
                returnValue.Title_TR = res.Title_TR;
                returnValue.Title_EN = res.Title_EN;
                returnValue.ParentId = res.ParentId;
                returnValue.DisplayType = res.DisplayType;
                returnValue.ApiKey = res.ApiKey;
                returnValue.Secret = res.Secret;
                returnValue.ClientIdJsonPath = res.ClientIdJsonPath;
                returnValue.KafkaUrl = res.KafkaUrl;
                returnValue.KafkaCertificate = res.KafkaCertificate;
                returnValue.RetentationTime = res.RetentationTime;
                returnValue.ServiceUrlList = servicesUrls;
                returnValue.ProductCodeId = res.ProductCodeId;
                returnValue.ProductCodeName = res.ProductCode == null ? null : res.ProductCode.ProductCodeName;
                returnValue.SaveInbox = res.SaveInbox;
                returnValue.ProcessName = res.ProcessName;
                returnValue.ProcessItemId = res.ProcessItemId;
                returnValue.InheritanceType = res.InheritanceType;
                returnValue.AlwaysSendType = res.AlwaysSendType;

                return returnValue;
            }
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
                if (customerInformationModel != null && customerInformationModel.customerList != null && customerInformationModel.customerList.Count > 0 && customerInformationModel.customerList[0].gsmPhone != null)
                {
                    string productCodeName;
                    GetSourceTopicByIdResponse source = GetSourceById(requestModel.sourceid);


                    if (source != null && source.ProductCodeId != null)
                    {
                        ProductCodeResponseModel productCodeResponseModel = _IproductCode.GetProductCodeWithId(source.ProductCodeId.Value);
                        if (productCodeResponseModel.productCode != null)
                        {
                            productCodeName = productCodeResponseModel.productCode.ProductCodeName;
                        }
                        else
                        {
                            returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode473);
                            returnValue.MessageList.Add(customerInformationModel.returnDescription);
                            returnValue.Result = ResultEnum.Error;
                            return returnValue;
                        }
                    }
                    else
                    {
                        returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode470);
                        returnValue.MessageList.Add(customerInformationModel.returnDescription);
                        returnValue.Result = ResultEnum.Error;
                        return returnValue;
                    }
                    PostConsumerRequest postConsumerRequest = new PostConsumerRequest();
                    postConsumerRequest.Phone = new Phone();
                    postConsumerRequest.Phone.Number = Convert.ToInt32(customerInformationModel.customerList[0].gsmPhone.number);
                    postConsumerRequest.Phone.CountryCode = Convert.ToInt32(customerInformationModel.customerList[0].gsmPhone.country);
                    postConsumerRequest.Phone.Prefix = Convert.ToInt32(customerInformationModel.customerList[0].gsmPhone.prefix);
                    postConsumerRequest.Email = customerInformationModel.customerList[0].email;
                    postConsumerRequest.DeviceKey = customerInformationModel.customerList[0].device == null ? null : customerInformationModel.customerList[0].device.ToString();
                    GetInstantDGReminderResponse getInstantDGReminderResp = _IinstandReminder.GetCustomerPermissionWithProductCode(requestModel.client, productCodeName).Result;
                    if (getInstantDGReminderResp != null && getInstantDGReminderResp.Result == ResultEnum.Success && getInstantDGReminderResp.Count == 1)
                    {
                        postConsumerRequest.IsSmsEnabled = getInstantDGReminderResp.SEND_SMS;
                        postConsumerRequest.IsEmailEnabled = getInstantDGReminderResp.SEND_EMAIL;
                        postConsumerRequest.IsPushEnabled = getInstantDGReminderResp.SEND_PUSHNOTIFICATION;
                    }
                    if (getInstantDGReminderResp != null && getInstantDGReminderResp.Result == ResultEnum.Success && getInstantDGReminderResp.Count == -1)
                    {
                        postConsumerRequest.IsSmsEnabled = true;
                        postConsumerRequest.IsEmailEnabled = true;
                        postConsumerRequest.IsPushEnabled = true;
                    }
                    if (getInstantDGReminderResp != null && getInstantDGReminderResp.Result == ResultEnum.Error)
                    {
                        returnValue.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode475);
                        returnValue.MessageList.Add(getInstantDGReminderResp.MessageList[0]);
                        returnValue.Result = ResultEnum.Error;

                        postConsumerRequest.IsSmsEnabled = true;
                        postConsumerRequest.IsEmailEnabled = true;
                        postConsumerRequest.IsPushEnabled = true;
                        //  return returnValue;
                    }
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
                               SaveInbox = source.SaveInbox,
                               ProductCodeName = p == null ? null : p.ProductCodeName,
                               ProcessName = source.ProcessName,
                               ProcessItemId = source.ProcessItemId,
                               InheritanceType = (int)source.InheritanceType,
                               AlwaysSendType = (int)source.AlwaysSendType
                           }).Skip(((model.CurrentPage) - 1) * model.RequestItemSize)
                            .Take(model.RequestItemSize);

                getSourcesResponse.Result = ResultEnum.Success;
                getSourcesResponse.Sources = sources.ToList();
                getSourcesResponse.Count = db.Sources.Count();
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
                    ProductCodeId = s.ProductCodeId,
                    SaveInbox = s.SaveInbox,
                    ProcessName = s.ProcessName,
                    ProcessItemId = s.ProcessItemId,
                    InheritanceType = (int)s.InheritanceType,
                    AlwaysSendType = s.AlwaysSendType
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
                if (data.ProcessName != null) source.ProcessName = data.ProcessName;
                if (data.ProcessItemId != null) source.ProcessItemId = data.ProcessItemId;
                if (data.KafkaCertificate != null) source.KafkaCertificate = data.KafkaCertificate;
                if (data.RetentationTime != null) source.RetentationTime = data.RetentationTime;
                if (data.ProductCodeId != null) source.ProductCodeId = data.ProductCodeId;
                if (data.SaveInbox != null) source.SaveInbox = data.SaveInbox;
                if (data.InheritanceType != null) source.InheritanceType = data.InheritanceType;
                if (data.AlwaysSendType != null) source.AlwaysSendType = data.AlwaysSendType;

                if (data.ParentId != null)
                {
                    var sourcePatch = db.Sources.FirstOrDefault(s => s.Id == data.ParentId);

                    if (sourcePatch == null || data.ParentId == 1)
                    {
                        source.ParentId = null;
                    }
                    else
                    {
                        source.ParentId = data.ParentId;
                    }
                };

                db.Sources.Update(source);
                db.SaveChanges();
                sourceResp.Result = ResultEnum.Success;

                SourceLogRequest logRequest = new SourceLogRequest();
                logRequest.sourceLog = source;
                logRequest.User = data.User;
                logRequest.MethodType = "Update";
                logRequest.Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                SourceLogResponse logResponse = _ISourceLog.PostSourceLog(logRequest);
                if (logResponse.Result == ResultEnum.Error)
                {
                    sourceResp.MessageList.Add("SourceLog update ederken hata oluştu. ");
                }
            }

            return sourceResp;
        }

        public SourceResponseModel Post(PostSourceRequest data)
        {
            SourceResponseModel sourceResp = new SourceResponseModel();
            Model.Database.Source sourceModel = new Model.Database.Source();
            sourceModel.Topic = data.Topic;
            sourceModel.ApiKey = data.ApiKey;
            sourceModel.Secret = data.Secret;
            sourceModel.PushServiceReference = data.PushServiceReference;
            sourceModel.SmsServiceReference = data.SmsServiceReference;
            sourceModel.EmailServiceReference = data.EmailServiceReference;
            sourceModel.KafkaUrl = data.KafkaUrl;
            sourceModel.KafkaCertificate = data.KafkaCertificate;
            sourceModel.DisplayType = (SourceDisplayType)data.DisplayType;
            sourceModel.Title_EN = data.Title_EN;
            sourceModel.Title_TR = data.Title_TR;

            if (data.ParentId != null)
            {
                using (var db = new DatabaseContext())
                {
                    var source = db.Sources.FirstOrDefault(s => s.Id == data.ParentId);
                    if (source == null || data.ParentId == 1)
                    {
                        sourceModel.ParentId = null;

                    }
                    else
                    {
                        sourceModel.ParentId = data.ParentId;
                    }
                };
            }

            sourceModel.ClientIdJsonPath = data.ClientIdJsonPath;
            sourceModel.ProcessName = data.ProcessName;
            sourceModel.ProcessItemId = data.ProcessItemId;
            sourceModel.RetentationTime = data.RetentationTime;
            sourceModel.ProductCodeId = data.ProductCodeId;
            sourceModel.SaveInbox = data.SaveInbox;
            sourceModel.InheritanceType = data.InheritanceType;
            sourceModel.AlwaysSendType = EnumHelper.IntListToInt(data.AlwaysSendTypes);

            using (var db = new DatabaseContext())
            {
                db.Add(sourceModel);

                db.SaveChanges();
                sourceResp.Result = ResultEnum.Success;
                SourceLogRequest logRequest = new SourceLogRequest();
                logRequest.sourceLog = sourceModel;
                logRequest.User = data.User;
                logRequest.MethodType = "Insert";
                logRequest.Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                SourceLogResponse logResponse = _ISourceLog.PostSourceLog(logRequest);
                if (logResponse.Result == ResultEnum.Error)
                {
                    sourceResp.MessageList.Add("SourceLog kaydederken hata oluştu. ");
                }

                return sourceResp;
            }
        }
        public SourceResponseModel PostProd(PostSourceRequest data)
        {
            if (data.AlwaysSendTypes == null)
            {
                data.AlwaysSendTypes = EnumHelper.ToIntArray((AlwaysSendType)data.AlwaysSendType);
            }

            SourceResponseModel sourceResp = new SourceResponseModel();

            string path = _configuration.GetSection("NotificationProdSearchEndPoint").Value.ToString();
            var uri = new Uri(path);
            GetSourcesResponse result = new GetSourcesResponse();
            using (var httpClient = new HttpClient())
            {
                SearchSourceModel searchSourceModel = new SearchSourceModel();
                searchSourceModel.Topic = data.Topic;
                searchSourceModel.Title = data.Title_TR;
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
                        sourceResp.MessageList.Add("Prod ortamına kaydederken hata oluştu. " + response.ReasonPhrase);
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

            SourceLogRequest logRequest = new SourceLogRequest();
            Model.Database.Source sourceModel = new Model.Database.Source();
            sourceModel.Id = sourceResp.Id;
            sourceModel.Topic = data.Topic;
            sourceModel.ApiKey = data.ApiKey;
            sourceModel.Secret = data.Secret;
            sourceModel.PushServiceReference = data.PushServiceReference;
            sourceModel.SmsServiceReference = data.SmsServiceReference;
            sourceModel.EmailServiceReference = data.EmailServiceReference;
            sourceModel.KafkaUrl = data.KafkaUrl;
            sourceModel.KafkaCertificate = data.KafkaCertificate;
            sourceModel.DisplayType = (SourceDisplayType)data.DisplayType;
            sourceModel.Title_EN = data.Title_EN;
            sourceModel.Title_TR = data.Title_TR;
            sourceModel.ParentId = data.ParentId;
            sourceModel.ClientIdJsonPath = data.ClientIdJsonPath;
            sourceModel.ProcessName = data.ProcessName;
            sourceModel.ProcessItemId = data.ProcessItemId;
            sourceModel.RetentationTime = data.RetentationTime;
            sourceModel.ProductCodeId = data.ProductCodeId;
            sourceModel.SaveInbox = data.SaveInbox;
            sourceModel.InheritanceType = data.InheritanceType;
            sourceModel.AlwaysSendType = EnumHelper.IntListToInt(data.AlwaysSendTypes);

            logRequest.sourceLog = sourceModel;
            logRequest.Environment = "Prod";
            logRequest.User = data.User;
            logRequest.MethodType = "Release";

            SourceLogResponse logResponse = _ISourceLog.PostSourceLog(logRequest);

            if (logResponse.Result == ResultEnum.Error)
            {
                sourceResp.MessageList.Add("SourceLog kaydederken hata oluştu. ");
            }

            return sourceResp;
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
        public SourceResponseModel TfsReleaseCreate(PostSourceRequest data)
        {
            var model = new SourceReleaseVariables
            {
                id = 258,
                type = "Vsts",
                name = "RestApi",
                variables = new Variable
                {
                    Id = new Deger
                    {
                        isSecret = false,
                        value = data.Id.ToString()
                    },
                    Title_TR = new Deger
                    {
                        isSecret = false,
                        value = data.Title_TR
                    },
                    Title_EN = new Deger
                    {
                        isSecret = false,
                        value = data.Title_EN
                    },
                    Topic = new Deger
                    {
                        isSecret = false,
                        value = data.Topic
                    },
                    ApiKey = new Deger
                    {
                        isSecret = false,
                        value = data.ApiKey
                    },
                    Secret = new Deger
                    {
                        isSecret = false,
                        value = data.Secret
                    },
                    DisplayType = new Deger
                    {
                        isSecret = false,
                        value = data.DisplayType.ToString()
                    },
                    PushServiceReference = new Deger
                    {
                        isSecret = false,
                        value = data.PushServiceReference
                    },
                    SmsServiceReference = new Deger
                    {
                        isSecret = false,
                        value = data.SmsServiceReference
                    },
                    EmailServiceReference = new Deger
                    {
                        isSecret = false,
                        value = data.EmailServiceReference
                    },
                    KafkaUrl = new Deger
                    {
                        isSecret = false,
                        value = data.KafkaUrl
                    },
                    KafkaCertificate = new Deger
                    {
                        isSecret = false,
                        value = data.KafkaCertificate
                    },
                    ParentId = new Deger
                    {
                        isSecret = false,
                        value = data.ParentId.ToString()
                    },
                    RetentationTime = new Deger
                    {
                        isSecret = false,
                        value = data.RetentationTime.ToString()
                    },
                    ProductCodeId = new Deger
                    {
                        isSecret = false,
                        value = data.ProductCodeId.ToString()
                    },
                    SaveInbox = new Deger
                    {
                        isSecret = false,
                        value = data.SaveInbox.ToString()
                    },
                    CheckDeploy = new Deger
                    {
                        isSecret = false,
                        value = data.CheckDeploy.ToString()
                    },
                    User = new Deger
                    {
                        isSecret = false,
                        value = data.User
                    },
                    ClientIdJsonPath = new Deger
                    {
                        isSecret = false,
                        value = data.ClientIdJsonPath
                    },
                    ProcessName = new Deger
                    {
                        isSecret = false,
                        value = data.ProcessName
                    },
                    ProcessItemId = new Deger
                    {
                        isSecret = false,
                        value = data.ProcessItemId
                    },
                    InheritanceType = new Deger
                    {
                        isSecret = false,
                        value = data.InheritanceType.ToString()
                    },
                    AlwaysSendType = new Deger
                    {
                        isSecret = false,
                        value = EnumHelper.IntListToInt(data.AlwaysSendTypes).ToString()
                    }
                }
            };

            SourceResponseModel responseModel = new SourceResponseModel();
            string apiUrl = _configuration.GetValue<string>("TfsEndpoints:PutVariables");
            string apiRelease = _configuration.GetValue<string>("TfsEndpoints:CreateRelease");
            string personalAccessToken = _configuration.GetValue<string>("TfsToken");

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", "", personalAccessToken))));

                var httpContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = client.PutAsync(
                        apiUrl, httpContent).Result)
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)

                    {
                        var createReleaseModel = new CreateReleaseModel
                        {
                            definitionId = 7,
                            isDraft = false,
                            description = "",
                            manualEnvironments = new object[100],
                            artifacts = new object[100],
                            variables = new Variables { },
                            environmentsMetadata = new EnvironmentsMetadata[]
                            {
                             new EnvironmentsMetadata
                             {
                                definitionEnvironmentId=7,
                                variables = new Variables { },
                             }
                            },
                            properties = new Properties
                            {
                                ReleaseCreationSource = "ReleaseHub"
                            }
                        };
                        var httpContentRelease = new StringContent(System.Text.Json.JsonSerializer.Serialize(createReleaseModel), Encoding.UTF8, "application/json");
                        using (HttpResponseMessage responseRelease = client.PostAsync(
                      apiRelease, httpContentRelease).Result)
                        {
                            if (responseRelease.IsSuccessStatusCode)
                            {
                                responseModel.Result = ResultEnum.Success;
                            }
                            else
                            {
                                responseModel.Result = ResultEnum.Error;
                                responseModel.MessageList.Add(responseRelease.ReasonPhrase);
                            }
                        }
                    }
                    else
                    {
                        responseModel.Result = ResultEnum.Error;
                        responseModel.MessageList.Add(response.ReasonPhrase);
                    }
                }
            };

            return responseModel;
        }
    }
}