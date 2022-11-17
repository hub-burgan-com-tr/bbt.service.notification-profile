using Elastic.Apm.Api;
using Newtonsoft.Json;
using Notification.Profile.Model.Database;

namespace Notification.Profile.Helper
{
    public class LogHelper : ILogHelper
    {
        private readonly ITracer _tracer;
        public LogHelper(ITracer tracer)
        {
            _tracer = tracer;
        }
        public bool LogCreate(object requestModel, object responseModel, string methodName, string errorMessage)
        {
            var span = _tracer.CurrentTransaction?.StartSpan("LogCreateSpan", "LogCreate");
            using (var db = new DatabaseContext())
            {
                try
                {
                    db.Add(new Log
                    {
                        ServiceName = methodName,
                        ProjectName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name,
                        ErrorDate = DateTime.Now,
                        ErrorMessage = errorMessage,
                        RequestData = JsonConvert.SerializeObject(requestModel),
                        ResponseData = JsonConvert.SerializeObject(responseModel)
                    });

                    db.SaveChanges();

                    return true;
                }

                catch (Exception e)
                {
                    Console.WriteLine("DB ERROR=> "+e.Message);
                    span?.CaptureException(e);
                    return false;
                }
            }
        }

    }
}

