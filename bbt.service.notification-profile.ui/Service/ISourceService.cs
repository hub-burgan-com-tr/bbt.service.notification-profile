
using Notification.Profile.Model;
using Refit;

namespace bbt.service.notification.ui.Service
{
    public interface ISourceService
    {

        [Get("/sources")]
        Task<GetSourcesResponse> GetSources();

        [Post("/sources/searchSourceModel")]
       
        Task<GetSourcesResponse> GetSourceWithSearchModel(SearchSourceModel model);
        [Post("/sources")]
        Task<SourceResponseModel> Post(PostSourceRequest model);


        [Patch("/sources/{id}")]
        Task<SourceResponseModel> Patch(int id, PatchSourceRequest model);

        [Delete("/sources/{id}")]
        Task<SourceResponseModel> Delete(int id,string user);

        [Post("/sources/prod")]
        Task<SourceResponseModel> PostProd(PostSourceRequest model);


    }
}

