
using Notification.Profile.Model;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[SwaggerSchemaFilter(typeof(SourceSchemaFilter))]
public class PostSourceRequest
{
    public int Id { get; set; }
    // public string Title { get; set; }
    public string Title_TR { get; set; }
    public string Title_EN { get; set; }

    [Required(ErrorMessage = "Topic boþ olamaz!")]
    public string Topic { get; set; }
    public string ApiKey { get; set; }
    public string Secret { get; set; }
    public int DisplayType { get; set; }
    //public SourceDisplayType DisplayType { get; set; }
    public string PushServiceReference { get; set; }
    public string SmsServiceReference { get; set; }
    public string EmailServiceReference { get; set; }

    [Required(ErrorMessage = "KafkaUrl boþ olamaz!")]
    public string KafkaUrl { get; set; }

    [Required(ErrorMessage = "Kafka Sertifikasý boþ olamaz!")]
    public string KafkaCertificate { get; set; }

    public string ClientIdJsonPath { get; set; }

    public int? ParentId { get; set; }

    public int RetentationTime { get; set; }

    public int ProductCodeId { get; set; }

    [NotMapped]
    public bool CheckDeploy { get; set; }

}
