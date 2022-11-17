using Notification.Profile.Model;

namespace Notification.Profile.Business

{
    public interface IProductCode
    {
        GetProductCodeResponse GetProductCode();

        ProductCodeResponseModel PatchProductCode(int id, PatchProductCode request);
        ProductCodeResponseModel PostProductCode(PostProductCodeRequest request);
        ProductCodeResponseModel DeleteProductCode(int id);

        Task<GetProductCodeResponse> ProductCodeListRedis();
    }
}