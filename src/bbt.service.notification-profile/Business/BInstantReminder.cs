using System.Data.SqlClient;
using System.Data;
using notification_profile.Model;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using Notification.Profile.Model;
using Notification.Profile.Enum;

namespace Notification.Profile.Business
{
    public class BInstantReminder : IinstandReminder
    {
        private readonly IConfiguration _configuration;
        private readonly IConsumer _Iconsumer;
        private readonly IReminderDefinition _IreminderDefinition;
        private readonly IDistributedCache _cache;

        public BInstantReminder(IConfiguration configuration, IConsumer IConsumer, IReminderDefinition IreminderDefinition, IDistributedCache cache)
        {
            _configuration = configuration;
            _Iconsumer = IConsumer;
            _IreminderDefinition = IreminderDefinition;
            _cache = cache;
        }

        public async Task<GetInstantCustomerPermissionResponse> GetCustomerPermission(string customerId, string lang)
        {
            var connectionString = _configuration.GetConnectionString("ReminderConnectionString");

            // TO DO REFACTOR 
            //Dictionary<string, string> reminderDescription = new Dictionary<string, string>()
            //{
            //        { "accountMoneyEntry", "Hesabýma para geldiðinde" },
            //        { "accountMoneyExit", "Hesabýmdan para çýkýþý olduðunda" },
            //        { "checkingAccountUpperLimitExceeded", "Vadesiz hesap bakiyesi üst limite çýktýðýnda" },
            //        { "checkingAccountLowerLimitExceeded", "Vadesiz hesap bakiyesi alt limite düþtüðünde" },
            //        { "riskyFutureTransfersAndPayments", "Bakiye yetersizliði nedeniyle ödenmeme riski taþýyan ileri tarihli para transferleri ve ödemelerde" },
            //        { "savingAccountDueDateReturn", "Vadeli mevduat hesap dönüþü" },
            //        { "cardPosLimit", "Banka Kartý Alýþveriþ Limiti" },
            //        { "cardRefund", "Banka Kartý Ýade Bildirimi" },
            //        { "cardReccurring", "Banka Kartý Talimatlý Ödeme" },
            //};
            GetInstantCustomerPermissionResponse instantReminder = new GetInstantCustomerPermissionResponse();
            List<ReminderDefinition> reminderDefinitionList = new List<ReminderDefinition>();
            GetReminderDefinitionResponse reminderDefinitionResponse = new GetReminderDefinitionResponse();

            var cachedList = await _cache.GetAsync("notificationRedis");
            if (cachedList != null && !string.IsNullOrEmpty(System.Text.Encoding.UTF8.GetString(cachedList)))
            {
                reminderDefinitionList = JsonConvert.DeserializeObject<List<ReminderDefinition>>(System.Text.Encoding.UTF8.GetString(cachedList));
            }
            else
            {
                reminderDefinitionResponse = _IreminderDefinition.GetReminderDefinitionListWithLang(lang);
                reminderDefinitionList = reminderDefinitionResponse.ReminderDefinitionList;

                await _cache.SetAsync("notificationRedis", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reminderDefinitionList)),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(Convert.ToDouble(_configuration.GetSection("RedisTimeout").Value))
                });
            }



            List<DbDataEntity> dbParams = new List<DbDataEntity>();
            DbDataEntity dbData = new DbDataEntity();
            dbData.parameterName = "@CUSTOMER_ID";
            dbData.value = customerId;
            dbParams.Add(dbData);
            DataTableResponseModel responseModel = new DataTableResponseModel();
            responseModel = DbCalls.ExecuteDataTable(connectionString, "REM.DG_REMINDER_SELECT", dbParams);
            //Müþteri izinlerini çekiyor eski sistemden
            if (responseModel.Result != Enum.ResultEnum.Error)
            {
                DataTable dt = responseModel.DataTable;

                ReminderGet reminder;

                List<ReminderGet> reminders = new List<ReminderGet>();

                foreach (DataRow dr in dt.Rows)
                {
                    reminder = new ReminderGet();

                    instantReminder.showWithoutLogin = Convert.ToBoolean(dr["SHOW_WITHOUT_LOGIN"].ToString());
                    reminder.amount = Convert.ToDecimal(dr["AMOUNT"].ToString());
                    reminder.email = Convert.ToBoolean(dr["SEND_EMAIL"].ToString());
                    reminder.sms = Convert.ToBoolean(dr["SEND_SMS"].ToString());
                    reminder.mobileNotification = Convert.ToBoolean(dr["SEND_PUSHNOTIFICATION"].ToString());
                    reminder.reminderType = dr["PRODUCT_CODE"].ToString();
                    GetReminderDefinitionResponse reminderDefinitionResp = _IreminderDefinition.GetReminderDefinitionWithDefinitionCode(reminderDefinitionList, dr["PRODUCT_CODE"].ToString());
                    if (reminderDefinitionResp != null && reminderDefinitionResp.ReminderDefinitions != null)
                    {
                        reminder.reminderDescription = reminderDefinitionResp.ReminderDefinitions.Title;
                    }
                    reminder.hasAmountRestriction = Convert.ToBoolean(dr["HAS_AMOUNT_RESTRICTION"].ToString());
                    reminders.Add(reminder);
                }
                GetUserConsumersResponse consumers = _Iconsumer.GetUserConsumers(long.Parse(customerId), long.Parse(customerId), null);//buraya reminderDescription gönderecek miyiz

                foreach (var consumer in consumers.Consumers)
                {
                    reminder = new ReminderGet();
                    reminder.email = consumer.IsEmailEnabled;
                    reminder.sms = consumer.IsSmsEnabled;
                    reminder.mobileNotification = consumer.IsPushEnabled;
                    reminder.reminderType = consumer.DefinitionCode;
                    reminder.amount = 0;//Karar verilecek
                    reminder.hasAmountRestriction = true;//Karar verilecek

                    GetReminderDefinitionResponse reminderDefinitionRespNew = _IreminderDefinition.GetReminderDefinitionWithDefinitionCode(reminderDefinitionList, consumer.DefinitionCode);
                    if (reminderDefinitionRespNew != null && reminderDefinitionRespNew.ReminderDefinitions != null)
                    {
                        reminder.reminderDescription = reminderDefinitionRespNew.ReminderDefinitions.Title;
                    }
                    reminders.Add(reminder);
                }
                instantReminder.reminders = reminders;
                instantReminder.Result = Enum.ResultEnum.Success;
            }
            else
            {
                instantReminder.Result = Enum.ResultEnum.Error;
                instantReminder.MessageList = responseModel.MessageList;
            }

            return instantReminder;
        }

        public async Task<PostInstantCustomerPermissionResponse> PostCustomerPermission(string customerId, PostInstantCustomerPermissionRequest request)
        {
            var connectionString = _configuration.GetConnectionString("ReminderConnectionString");

            PostInstantCustomerPermissionResponse response = new PostInstantCustomerPermissionResponse();
            try
            {
                for (int i = 0; i < request.reminders.Count; i++)
                {
                    ReminderPost r = new ReminderPost();
                    r = request.reminders[i];


                    List<DbDataEntity> dbParams = new List<DbDataEntity>();
                    DbDataEntity dbData = new DbDataEntity();
                    dbData.dbType = DbType.String;
                    dbData.direction = ParameterDirection.Input;
                    dbData.parameterName = "@CUSTOMER_ID";
                    dbData.value = customerId;
                    dbParams.Add(dbData);

                    dbData = new DbDataEntity();
                    dbData.dbType = DbType.String;
                    dbData.direction = ParameterDirection.Input;
                    dbData.parameterName = "@PRODUCT_CODE";
                    dbData.value = r.reminderType;
                    dbParams.Add(dbData);

                    dbData = new DbDataEntity();
                    dbData.dbType = DbType.Int16;
                    dbData.direction = ParameterDirection.Input;
                    dbData.parameterName = "@SEND_SMS";
                    dbData.value = r.sms ? 1 : 0;
                    dbParams.Add(dbData);

                    dbData = new DbDataEntity();
                    dbData.dbType = DbType.Int16;
                    dbData.direction = ParameterDirection.Input;
                    dbData.parameterName = "@SEND_EMAIL";
                    dbData.value = r.email ? 1 : 0;
                    dbParams.Add(dbData);

                    dbData = new DbDataEntity();
                    dbData.dbType = DbType.Int16;
                    dbData.direction = ParameterDirection.Input;
                    dbData.parameterName = "@SEND_PUSHNOTIFICATION";
                    dbData.value = r.mobileNotification ? 1 : 0;
                    dbParams.Add(dbData);

                    dbData = new DbDataEntity();
                    dbData.dbType = DbType.Decimal;
                    dbData.direction = ParameterDirection.Input;
                    dbData.parameterName = "@AMOUNT";
                    dbData.value = r.amount;
                    dbParams.Add(dbData);

                    dbData = new DbDataEntity();
                    dbData.dbType = DbType.Int16;
                    dbData.direction = ParameterDirection.Input;
                    dbData.parameterName = "@SHOW_WITHOUT_LOGIN";
                    dbData.value = request.showWithoutLogin ? 1 : 0;
                    dbParams.Add(dbData);

                    DbCalls.ExecuteNonQuery("REM.DG_REMINDER_INSERT", dbParams, connectionString);
                }
                response.Result = Enum.ResultEnum.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.Result = Enum.ResultEnum.Error;
                response.MessageList.Add(ex.Message);

                return response;
            }
        }

        public async Task<GetInstantDGReminderResponse> GetCustomerPermissionWithProductCode(long customerNumber, string productCode)
        {
            GetInstantDGReminderResponse response = new GetInstantDGReminderResponse();
            var connectionString = _configuration.GetConnectionString("ReminderConnectionString");
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "Select * from [REM].[DG_REMINDER] where CUSTOMER_NUMBER=" + customerNumber + " And PRODUCT_CODE='" + productCode + "'";

                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader reader = command.ExecuteReader();
               
                while (reader.Read())
                {
                    response.SEND_EMAIL = (bool)reader["SEND_EMAIL"];
                    response.SEND_SMS = (bool)reader["SEND_SMS"];
                    response.SEND_NOTIFICATION = (bool)reader["SEND_NOTIFICATION"];
                    response.SEND_PUSHNOTIFICATION = (bool)reader["SEND_PUSHNOTIFICATION"];
                    response.CUSTOMER_NUMBER = (long)reader["CUSTOMER_NUMBER"];
                    response.PRODUCT_CODE = reader["PRODUCT_CODE"].ToString();
                }
                if (response.CUSTOMER_NUMBER != null && response.CUSTOMER_NUMBER > 0)
                {
                    response.Result = ResultEnum.Success;
                }
                else
                {
                    response.Result = ResultEnum.Error;
                }
                conn.Close();
            }
            return response;
        }
    }
}
