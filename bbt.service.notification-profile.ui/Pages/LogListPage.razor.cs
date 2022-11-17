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

        public GetMessageNotificationLogRequest searchModel { get; set; } = new GetMessageNotificationLogRequest();
        public BaseModal ModalSource { get; set; }
      
        [Inject]
        public Radzen.DialogService dialogService { get; set; }

        private int pageCount = 10;
        private int rowsCount = 0;
      
    
        public void Search()
        {
            ExecuteMethod(() =>
            {
                LoadingModal.Open();
            logList = logService.GetMessageNotificationLogs(searchModel).Result.MessageNotificationLogs;
            rowsCount = logList.Count();
            LoadingModal.Close();
            });

        }
  
        public void Cancel()
        {
            searchModel = new GetMessageNotificationLogRequest();
            logList = logService.GetMessageNotificationLogs(searchModel).Result.MessageNotificationLogs;
            rowsCount = logList.Count();
        }
     
    }
}
