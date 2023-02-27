

using Notification.Profile.Model;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
[SwaggerSchemaFilter(typeof(SourceSchemaFilter))]
public class PatchSourceRequest
{
    //public string Title { get; set; }
    public string Title_TR { get; set; }

    public string Title_EN { get; set; }
    public string Topic { get; set; }
    public string ApiKey { get; set; }
    public string Secret { get; set; }
    public string PushServiceReference { get; set; }
    public string SmsServiceReference { get; set; }
    public string EmailServiceReference { get; set; }
    public string KafkaUrl { get; set; }
    public string KafkaCertificate { get; set; }
    public int  DisplayType { get; set; }
    public string ClientIdJsonPath { get; set; }
    public int RetentationTime { get; set; }
    public int? ProductCodeId { get; set; }
    public int? ParentId { get; set; }
    public bool SaveInbox { get; set; }
    [NotMapped]
    public bool CheckDeploy { get; set; } = false;

    [NotMapped]
    public string User { get; set; } 
}
