using bbt.service.notification.ui.Component;
using bbt.service.notification.ui.Component.Modal;
using bbt.service.notification.ui.Service;
using Microsoft.AspNetCore.Components;
using Notification.Profile.Enum;
using Notification.Profile.Model;
using Notification.Profile.Model.Database;
using Radzen;
using Radzen.Blazor;


namespace bbt.service.notification.ui.Pages
{
    public partial class LogListPage : BaseComponent
    {
        [Inject]
        public IMessageNotificationLogService logService { get; set; }
        public IEnumerable<MessageNotificationLog> logList { get; set; }

        private RadzenDataGrid<MessageNotificationLog> grid { get; set; }
        public GetMessageNotificationLogResponse responseModel { get; set; }
        public GetMessageNotificationLogRequest searchModel { get; set; } = new GetMessageNotificationLogRequest();
        public BaseModal ModalSource { get; set; }
        public bool textChange { get; set; } = true;

        [Inject]
        public Radzen.DialogService dialogService { get; set; }

       


        public void Search()
        {
            ExecuteMethod(() =>
            {

                LoadingModal.Open();
                BeforeSearch();
                
                    responseModel = new GetMessageNotificationLogResponse();
                    responseModel = logService.GetMessageNotificationLogs(searchModel).Result;
                    if (responseModel.Result == ResultEnum.Success)
                    {

                        logList = responseModel.MessageNotificationLogs;
                      
                    }
             
                AfterSearch();
                LoadingModal.Close();

            });

        }

        public void Cancel()
        {
            searchModel = new GetMessageNotificationLogRequest();
            responseModel = new GetMessageNotificationLogResponse();
            if (responseModel.Result == ResultEnum.Success)
            {
                logList = logService.GetMessageNotificationLogs(searchModel).Result.MessageNotificationLogs;

            }

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
     
        public void OnChange()
        {
            ExecuteMethod(() =>
            {
                if (searchModel.CustomerNo == null && String.IsNullOrEmpty(searchModel.PhoneNumber))
                {
                    textChange = true;
                }
                else
                {

                    textChange = false;
                }
           });
            StateHasChanged();
        }
    }
}
