namespace Notification.Profile.Business.Dengage.Login
{
    public record DengageLoginRequestModel
    {
        public string userkey { get; set; }
        public string password { get; set; }
    }
    public record DengageLoginResponseModel
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }
    }
}
