using Microsoft.EntityFrameworkCore;

namespace Notification.Profile.Business
{
    public class BConsumer : IConsumer
    {
        private readonly IConfiguration _configuration;

        public BConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public GetUserConsumersResponse GetUserConsumers(long client, long user, int? source)
        {
            GetUserConsumersResponse returnValue = new GetUserConsumersResponse();
            using (var db = new DatabaseContext())
            {
                var consumers = (from _consumer in db.Consumers
                                 join _source in db.Sources on _consumer.SourceId equals _source.Id
                                 where _consumer.Client == client && _consumer.User == user
                                 && _source.DisplayType != SourceDisplayType.NotDisplay
                                 && (source == null || source.HasValue || _consumer.SourceId == source.Value)
                                 select new GetUserConsumersResponse.Consumer
                                 {
                                     Source = _consumer.SourceId,
                                     Filter = _consumer.Filter,
                                     IsPushEnabled = _consumer.IsPushEnabled,
                                     DeviceKey = _consumer.DeviceKey,
                                     IsSmsEnabled = _consumer.IsSmsEnabled,
                                     Phone = _consumer.Phone,
                                     IsEmailEnabled = _consumer.IsEmailEnabled,
                                     Email = _consumer.Email,
                                     DefinitionCode = _consumer.DefinitionCode,
                                     IsStaff=_consumer.IsStaff
                                 }).AsNoTracking();
                returnValue.Consumers = consumers.ToList();

            }
            return returnValue;
        }
        public PostConsumerResponse PostConsumers(long client, long sourceId, PostConsumerRequest consumer)
        {
            PostConsumerResponse returnValue = new PostConsumerResponse();
            Consumer consumerInsert = new Consumer();
            using (var db = new DatabaseContext())
            {
                var consumers = db.Consumers.FirstOrDefault(x => x.Client == client && x.SourceId == sourceId);
                if (consumers != null)
                {

                    var result = db.Database.ExecuteSqlInterpolated($@"UPDATE [Consumers] 
                        SET DefinitionCode = {consumer.DefinitionCode},  
                            IsPushEnabled = {consumer.IsPushEnabled} ,
                            IsSmsEnabled = {consumer.IsSmsEnabled} ,
                            IsEmailEnabled={consumer.IsEmailEnabled}
                        WHERE SourceId = {sourceId} AND
                              Client = {client}  
                             ");
                }
                else
                {
                    consumerInsert.DefinitionCode = consumer.DefinitionCode;
                    consumerInsert.Client = client;
                    consumerInsert.User = consumer.User;
                    consumerInsert.Email = consumer.Email;
                    consumerInsert.DeviceKey = consumer.DeviceKey;
                    consumerInsert.IsEmailEnabled = consumer.IsEmailEnabled;
                    consumerInsert.IsPushEnabled = consumer.IsPushEnabled;
                    consumerInsert.IsSmsEnabled = consumer.IsSmsEnabled;
                    consumerInsert.Phone = consumer.Phone;
                    consumerInsert.SourceId = (int)sourceId;
                    consumerInsert.Filter = consumer.Filter;
                    consumerInsert.IsStaff = consumer.IsStaff;
                    db.Add(consumerInsert);
                }

                db.SaveChanges();
                returnValue.Consumer = consumerInsert;

                db.NotificationLogs.Add(new NotificationLog
                {
                    IsEmailEnabled = consumer.IsEmailEnabled,
                    Client = client,
                    IsPushEnabled = consumer.IsPushEnabled,
                    IsSmsEnabled = consumer.IsSmsEnabled,
                    SourceId = (int)sourceId,
                    Filter = consumer.Filter,
                    User = consumer.User
                });
                db.SaveChanges();
            }
            returnValue.Result = Enum.ResultEnum.Success;
           
            return returnValue;
        }
    }
}
