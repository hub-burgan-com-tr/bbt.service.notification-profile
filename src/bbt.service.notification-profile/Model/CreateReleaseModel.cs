namespace Notification.Profile.Model
{
    public class CreateReleaseModel
    {
        public long definitionId { get; set; }
        public bool isDraft { get; set; }
        public string description { get; set; }
        public object[] manualEnvironments { get; set; }
        public object[] artifacts { get; set; }
        public Variables variables { get; set; }
        public EnvironmentsMetadata[] environmentsMetadata { get; set; }
        public Properties properties { get; set; }
    }
    public class EnvironmentsMetadata
    {
        public long definitionEnvironmentId { get; set; }
        public Variables variables { get; set; }
    }
    public class Properties
    {
        public string ReleaseCreationSource { get; set; }
    }
    public partial class Variables
    {
    }
}
