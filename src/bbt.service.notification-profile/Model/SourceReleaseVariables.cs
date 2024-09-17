using Notification.Profile.Enum;

namespace Notification.Profile.Model
{

    public class SourceReleaseVariables
    {
        public int id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public Variable variables { get; set; }
    }
    public class Deger
    {
        public bool isSecret { get; set; }
        public string value { get; set; }
    }

    public class Variable
    {
        public Deger Id { get; set; }
        public Deger Title_TR { get; set; }
        public Deger Title_EN { get; set; }
        public Deger Topic { get; set; }
        public Deger ApiKey { get; set; }
        public Deger Secret { get; set; }
        public Deger DisplayType { get; set; }
        public Deger PushServiceReference { get; set; }
        public Deger SmsServiceReference { get; set; }
        public Deger EmailServiceReference { get; set; }
        public Deger KafkaUrl { get; set; }
        public Deger KafkaCertificate { get; set; }
        public Deger ParentId { get; set; }
        public Deger RetentationTime { get; set; }
        public Deger ProductCodeId { get; set; }
        public Deger SaveInbox { get; set; }
        public Deger CheckDeploy { get; set; }
        public Deger User { get; set; }
        public Deger ClientIdJsonPath { get; set; }
        public Deger ProcessName { get; set; }
        public Deger ProcessItemId { get; set; }
        public Deger InheritanceType { get; set; }
        public Deger AlwaysSendType { get; set; }
        public Deger MessageDataJsonPath { get; set; }
        public Deger MessageDataFieldType { get; set; }
    }
}
