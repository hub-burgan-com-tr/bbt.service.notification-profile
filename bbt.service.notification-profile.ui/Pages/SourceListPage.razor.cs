using bbt.service.notification.ui.Component;
using bbt.service.notification.ui.Component.Modal;
using bbt.service.notification.ui.Service;
using Microsoft.AspNetCore.Components;
using Notification.Profile.Enum;
using Notification.Profile.Model;
using Radzen;
using Radzen.Blazor;


namespace bbt.service.notification.ui.Pages
{
    public partial class SourceListPage : BaseComponent
    {
        [Inject]
        public ISourceService sourceService { get; set; }
        public IEnumerable<Source> sourceList { get; set; }

        private RadzenDataGrid<Source> grid { get; set; }

        public SearchSourceModel searchModel { get; set; } = new SearchSourceModel();

        public int SourceId { get; set; }

        public Source sourceDetayModel { get; set; }
        public BaseModal ModalSource { get; set; }

        public GetSourcesResponse responseModel { get; set; }
        [Inject]
        public Radzen.DialogService dialogService { get; set; }

        private int pageCount = 10;
        private int rowsCount = 0;


        public void Search()
        {
            ExecuteMethod(() =>
            {
                LoadingModal.Open();
                BeforeSearch();
                responseModel = sourceService.GetSourceWithSearchModel(searchModel).Result;
                if (responseModel.Result == ResultEnum.Success)
                {

                    sourceList = responseModel.Sources;

                }
                rowsCount = sourceList.Count();
                AfterSearch();
                LoadingModal.Close();

            });
        }
        protected override void CustomOnAfterRenderAsync(bool firstRender)
        {
            base.CustomOnAfterRenderAsync(firstRender);


            if (firstRender)
            {
                Pagination.OnPageChange += () =>
                {
                    Search();
                };


                if (IsFirstLoad)
                {
                    Search();
                }
            }

        }
        protected virtual void AfterSearch()
        {
            Pagination.CurrentPage = searchModel.CurrentPage;
            Pagination.Count = responseModel.Count;

            Pagination.CalculateTotalPage();

            OnAfterSearch?.Invoke();
        }
        public virtual void BeforeSearch()
        {
            searchModel.CurrentPage = Pagination.CurrentPage;
            searchModel.RequestItemSize = Pagination.PageSize;

            OnBeforeSearch?.Invoke();
        }
        public void Cancel()
        {
            searchModel = new SearchSourceModel();
            sourceList = sourceService.GetSourceWithSearchModel(searchModel).Result.Sources;
            Pagination.Count = sourceList.Count();
            // rowsCount = sourceList.Count();
        }
        public void OpenSourceDetailModal()
        {
            sourceDetayModel = new Source();
            ModalSource.Open();
        }
        public void SourceEdit(Source item)
        {
            sourceDetayModel = item;
            SourceId = item.Id;
            ModalSource.Open();
        }
        public void SourceDelete(Source item)
        {
            sourceDetayModel = item;
            DeleteConfirm();
        }
        public async void DeleteConfirm()
        {
            SourceResponseModel sourceResp = new SourceResponseModel();
            var result = await dialogService.Confirm("Silmek istediğinize emin misiniz?", "Onay", new ConfirmOptions() { OkButtonText = "Evet", CancelButtonText = "Hayır" });
            if (result.HasValue && result.Value)
            {

                ExecuteMethod(() =>
                {
                    sourceResp = sourceService.Delete(sourceDetayModel.Id).Result;
                    if (sourceResp.Result == ResultEnum.Success)
                    {

                        Notification.ShowSuccessMessage("Silindi.", string.Empty);

                        CustomOnAfterRenderAsync(true);

                    }
                    else
                    {
                        Notification.ShowErrorMessage("Hata", "Silinirken hata oluştu.");

                    }
                });
            }
        }

        protected async Task ListUpdate()
        {
            ExecuteMethod(() =>
            {
                Search();
            });

        }

    }
}
