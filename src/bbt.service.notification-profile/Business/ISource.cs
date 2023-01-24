using Notification.Profile.Model;

namespace Notification.Profile.Business
{
    public interface ISource
    {
        GetSourcesResponse GetSources();
        GetSourceTopicByIdResponse GetSourceById(int id);

        SourceResponseModel Post(PostSourceRequest data);

        SourceResponseModel Patch(int id, PatchSourceRequest data);

        SourceResponseModel Delete(int id,string user);

        GetSourceConsumersResponse GetSourceConsumers(GetSourceConsumersRequestBody requestModel);

        GetSourcesResponse GetSourceWithSearchModel(SearchSourceModel model);
        SourceListResponse GetSourceByProductCodeId(string productCodeName);
        SourceResponseModel PostProd(PostSourceRequest data);



    }
}
