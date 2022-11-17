namespace bbt.service.notification_profile.Helper
{
    public class ConfigurationHelper
    {
        private readonly IConfiguration _config;
        public ConfigurationHelper()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{GetEnviroment()}.json", false, true)
            .AddEnvironmentVariables();
           _config = builder.Build();
        }
        public string GetReminderConnectionString()
        {
            return _config.GetSection("ConnectionStrings:ReminderConnectionString").Value;
        }
        public string GetCustomerProfileEndpoint()
        {
           
            return _config.GetSection("CustomerProfileEndpoint").Value;
        }
        public string GetSmsOnTemplateEndpoint()
        {
           
            return _config.GetSection("MessagingGateway:EndPoints:TemplateSmsOn").Value;
        }
        public string GetSmsBurganTemplateEndpoint()
        {

            return _config.GetSection("MessagingGateway:EndPoints:TemplateSmsBurgan").Value;
        }
        public string GetMailOnTemplateEndpoint()
        {
          
            return _config.GetSection("MessagingGateway:EndPoints:TemplateEmailOn").Value;
        }
        public string GetMailBurganTemplateEndpoint()
        {

            return _config.GetSection("MessagingGateway:EndPoints:TemplateEmailBurgan").Value;
        }
        public string GetPushOnTemplateEndpoint()
        {
           
            return _config.GetSection("MessagingGateway:EndPoints:TemplatePushOn").Value;
        }
        public string GetPushBurganTemplateEndpoint()
        {

            return _config.GetSection("MessagingGateway:EndPoints:TemplatePushBurgan").Value;
        }
        string? GetEnviroment()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
    }

}