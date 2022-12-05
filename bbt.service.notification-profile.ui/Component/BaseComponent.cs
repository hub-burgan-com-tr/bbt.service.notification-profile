using bbt.service.notification.ui.Component.Modal;
using bbt.service.notification.ui.Component.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace bbt.service.notification.ui.Component
{
    public class BaseComponent : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected INotification Notification { get; set; }
        [Parameter]
        public bool IsFirstLoad { get; set; } = true;
        [Parameter]
        public int PageSize { get; set; } = 20;
        public BasePaginationComponent Pagination { get; set; }

        [Inject]
        protected IJSRuntime jsRuntime { get; set; }

        public BaseLoadingModal LoadingModal { get; set; }

        [Parameter]
        public Action OnAfterSearch { get; set; }

        [Parameter]
        public Action OnBeforeSearch { get; set; }

        public virtual async void ExecuteMethod(Action action)
        {
            try
            {
                OpenModal();
                await Task.Run(action);
            }
            catch (Exception ex)
            {
                Notification.ShowErrorMessage("Bilgiler kaydedilirken bir hata oluştu.", ex.ToString());
      
            }
            finally
            {
                CloseModal();

                InvokeAsync(() => StateHasChanged()).Wait();
            }
        }

        protected void OpenModal()
        {
            if (LoadingModal != null && !LoadingModal.IsModalOpen)
            {
                LoadingModal.Open();
            }
        }
        protected void CloseModal()
        {
            if (LoadingModal != null)
            {
                LoadingModal.Close();
            }
        }
        public bool IsAuthenticated { get; set; } = false;

   

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Notification.Clear();

                LoadCustomOnAfterRenderAsync(firstRender);

            }
            else
            {
                CustomOnAfterRenderAsync(firstRender);
            }
        }

        private async Task LoadCustomOnAfterRenderAsync(bool firstRender)
        {
            await Task.Run(() =>
            {
              
            }).ContinueWith(async prev =>
            {
                CustomOnAfterRenderAsync(firstRender);
            }).ContinueWith(async prev =>
            {
                InvokeAsync(() => StateHasChanged()).Wait();
            });
        }

        protected virtual void CustomOnAfterRenderAsync(bool firstRender)
        {

        }

    }
}
