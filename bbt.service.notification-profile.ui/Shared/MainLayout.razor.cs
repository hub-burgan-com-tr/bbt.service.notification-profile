using bbt.service.notification.ui.Configuration;
using bbt.service.notification.ui.Override;
using bbt.service.notification.ui.Override.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Radzen.Blazor;

namespace bbt.service.notification.ui.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        ThemeState ThemeState { get; set; }
        [Inject]
        MenuService MenuService { get; set; }
        [Inject]
        NavigationManager UriHelper { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        [Inject]
        IHttpContextAccessor httpContextAccessor { get; set; }


        [Inject]
        protected NavigationManager NavigationManager { get; set; }

   

        [Inject]
        protected IBaseConfiguration baseConfiguration { get; set; }


        [CascadingParameter]
        protected Task<AuthenticationState> AuthenticationState { get; set; }
        [Inject]
        public bbt.service.notification.ui.Data.HttpContextAccessor httpContext { get; set; }

        RadzenSidebar sidebar0 { get; set; }
        RadzenBody body0 { get; set; }

        bool sidebarExpanded = true;
        bool bodyExpanded = false;

        dynamic themes = new[]
        {
    new { Text = "Default", Value = "default"},
    new { Text = "Dark", Value="dark" },
    new { Text = "Software", Value = "software"},
    new { Text = "Humanistic", Value = "humanistic" }
    };

        IEnumerable<MenuItem> examples;

        string Theme
        {
            get
            {
                return $"{ThemeState.CurrentTheme}.css";
            }
        }

        protected override async Task OnInitializedAsync()
        {
            httpContext.Context.Features.Get<HttpContext>();
            if (httpContextAccessor != null && httpContextAccessor.HttpContext != null &&
                 httpContextAccessor.HttpContext.Request != null && httpContextAccessor.HttpContext.Request.Headers.ContainsKey("User-Agent"))
            {
                var userAgent = httpContextAccessor.HttpContext.Request.Headers["User-Agent"].FirstOrDefault();
                if (!string.IsNullOrEmpty(userAgent))
                {
                    if (userAgent.Contains("iPhone") || userAgent.Contains("Android") || userAgent.Contains("Googlebot"))
                    {
                        sidebarExpanded = false;
                        bodyExpanded = true;
                    }
                }
            }

            examples = MenuService.MenuItems;

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            //  NavigationManager.NavigateTo("/Pages/SourceListPage");
            await base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {

                var example = MenuService.FindCurrent(UriHelper.ToAbsoluteUri(UriHelper.Uri));

                await JSRuntime.InvokeVoidAsync("setTitle", MenuService.TitleFor(example));

            }
            else
            {
                NavigationManager.NavigateTo("/Pages/SourceListPage");
            }
        }

        void FilterPanelMenu(ChangeEventArgs args)
        {
            var term = args.Value.ToString();

            examples = MenuService.Filter(term);
        }

        void ChangeTheme(object value)
        {
            ThemeState.CurrentTheme = value.ToString();
            UriHelper.NavigateTo(UriHelper.ToAbsoluteUri(UriHelper.Uri).ToString());
        }


        //protected override void OnInitialized()
        //{
        //    base.OnInitialized();
        //    httpContext.Context.Features.Get<HttpContext>();
        //}
        public void LoginSite()
        {
            NavigationManager.NavigateTo($"login?redirectUri=/", forceLoad: true);
        }
        public void LogoutSite()
        {
            NavigationManager.NavigateTo($"logoutPage?redirectUri=/", forceLoad: true);
        }
    }
}
