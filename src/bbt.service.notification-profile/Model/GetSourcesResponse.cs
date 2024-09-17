
using Notification.Profile.Enum;
using Notification.Profile.Model.BaseResponse;

namespace Notification.Profile.Model
{
    public class GetSourcesResponse : BaseResponseModel
    {
        public List<Source> Sources { get; set; }

    }
    public class Source
    {
        public int Id { get; set; }
        public List<Source> Children { get; set; }
        public TitleLabel Title { get; set; }
        public List<SourceParameter> Parameters { get; set; }
        public int DisplayType { get; set; }
        public string Topic { get; set; }
        public string ClientIdJsonPath { get; set; }
        public string ApiKey { get; set; }
        public string Secret { get; set; }
        public string PushServiceReference { get; set; }
        public string SmsServiceReference { get; set; }
        public string EmailServiceReference { get; set; }
        public string KafkaUrl { get; set; }
        public string KafkaCertificate { get; set; }
        public int? ParentId { get; set; }
        public int RetentationTime { get; set; }
        public int? ProductCodeId { get; set; }       
        public string ProductCodeName { get; set; }
        public bool SaveInbox { get; set; }
        public string ProcessName { get; set; }
        public string ProcessItemId { get; set; }
        public int InheritanceType { get; set; }
        public int AlwaysSendType { get; set; }
        public string MessageDataJsonPath { get; set; }
        public int MessageDataFieldType { get; set; }

        public class SourceParameter
        {
            public string JsonPath { get; set; }
            public SourceParameterType Type { get; set; }
            public bool AutoGenerate { get; set; }
            public TitleLabel Title { get; set; }
        }

        public class TitleLabel
        {
            public string EN { get; set; }
            public string TR { get; set; }

        }
    }
}