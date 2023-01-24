using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notification.Profile.Model.Database
{
    public class SourceLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SourceId { get; set; }
        public string Title_TR { get; set; }
        public string Title_EN { get; set; }
        public Source Parent { get; set; }
        public int? ParentId { get; set; }
        public ICollection<Source> Children { get; set; }
        public SourceDisplayType DisplayType { get; set; }
        public string Topic { get; set; }
        public string KafkaUrl { get; set; }
        public string ClientIdJsonPath { get; set; }
        public string ApiKey { get; set; }
        public string Secret { get; set; }
        public List<SourceParameter> Parameters { get; set; }
        public string PushServiceReference { get; set; }
        public string SmsServiceReference { get; set; }
        public string EmailServiceReference { get; set; }
        public string KafkaCertificate { get; set; }
        public int RetentationTime { get; set; }
        public int? ProductCodeId { get; set; }
        public bool SaveInbox { get; set; }

        public string MethodType { get; set; }
        public DateTime CreateDate { get; set; }

        public string CreateUser { get; set; }

        public string Environment { get; set; }
    }
}
