using Notification.Profile.Model.Database;

namespace Notification.Profile.Model
{
    public class SourceLogRequest
    {
        public Model.Database.Source sourceLog { get; set; }

        public string Environment { get; set; }

        public string User { get; set; }

        public string MethodType { get; set; }
    }
}
