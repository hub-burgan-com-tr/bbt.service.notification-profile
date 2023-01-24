using bbt.service.notification.ui.Component;
using bbt.service.notification.ui.Component.Modal;
using bbt.service.notification.ui.Enum;
using bbt.service.notification.ui.Service;
using Microsoft.AspNetCore.Components;
using Notification.Profile.Enum;
using Notification.Profile.Model;
using Notification.Profile.Model.Database;
using Radzen;
using Radzen.Blazor;


namespace bbt.service.notification.ui.Pages
{
    public partial class SourceLogListPage : BaseComponent
    {
        [Inject]
        public ISourceLogService logService { get; set; }
        public IEnumerable<SourceLog> logList { get; set; }

        private RadzenDataGrid<SourceLog> grid { get; set; }
        public GetSourceLogResponse responseModel { get; set; }
        public GetSourceLogRequest searchModel { get; set; } = new GetSourceLogRequest();
        public BaseModal ModalSource { get; set; }
      
        public List<string> methodTypeList { get; set; }

        [Inject]
        public Radzen.DialogService dialogService { get; set; }

       


        public void Search()
        {
            if (searchModel != null && searchModel.StartDate != null && searchModel.EndDate != null)
            {
                ExecuteMethod(() =>
                {

                    LoadingModal.Open();
                    BeforeSearch();

                    responseModel = new GetSourceLogResponse();
                    responseModel = logService.GetSourceLogs(searchModel).Result;
                    if (responseModel.Result == ResultEnum.Success)
                    {

                        logList = responseModel.SourceLogs;

                    }

                    AfterSearch();
                    LoadingModal.Close();

                });
            }
            else
            {
                Notification.ShowWarningMessage("Uyarı", "Başlangıç ve bitiş tarihi giriniz!");
            }

        }

        public void Cancel()
        {
            searchModel = new GetSourceLogRequest();
            responseModel = new GetSourceLogResponse();
            if (responseModel.Result == ResultEnum.Success)
            {
                logList = logService.GetSourceLogs(searchModel).Result.SourceLogs;

            }

        }
        protected override void CustomOnAfterRenderAsync(bool firstRender)
        {
            base.CustomOnAfterRenderAsync(firstRender);
            //methodTypeList = System.Enum.GetValues(typeof(EnumMethodType))
            //                .Cast<EnumMethodType>()
            //                .ToList();
          methodTypeList = ((EnumMethodType[])System.Enum.GetValues(typeof(EnumMethodType))).Select(c => c.ToString()).ToList();


            if (firstRender)
            {
                Pagination.OnPageChange += () =>
                {
                    Search();
                };


                //if (IsFirstLoad)
                //{
                //    Search();
                //}
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
     
     
    }
}
