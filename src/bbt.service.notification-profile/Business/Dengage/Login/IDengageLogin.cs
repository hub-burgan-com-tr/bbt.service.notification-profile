using RestEase;

namespace Notification.Profile.Business.Dengage.Login
{
    public interface IDengageLogin
    {
        [Post("/login")]        
        Task<DengageLoginResponseModel> Login([Body(BodySerializationMethod.Serialized)] DengageLoginRequestModel model);
    }
}
