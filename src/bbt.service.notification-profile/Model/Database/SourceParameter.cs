

using System.ComponentModel.DataAnnotations;

public class SourceParameter
{
    public Source Source { get; set; }
    public string SourceId { get; set; }
    public string JsonPath { get; set; }
    public SourceParameterType Type { get; set; }
    public bool AutoGenerate { get; set; }
    public string Title_TR { get; set; }
    public string Title_EN { get; set; }
}
