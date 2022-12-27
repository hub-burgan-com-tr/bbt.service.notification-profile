
using Auth0.AspNetCore.Authentication;
using bbt.service.notification.ui.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace bbt.service.notification.ui.Pages.Authorize
{
    public class LoginModel : PageModel
    {
        [Inject]
        bbt.service.notification.ui.Data.HttpContextAccessor contextAccessor { get; set; }

        public async  Task OnGet(string redirectUri)
        {
           IConfiguration config= FrameworkDependencyHelper.Instance.Get<IConfiguration>();
            string path = config.GetValue<string>("BaseUiUri");
             //   + "/Pages/SourceListPage";
            //string redirect ="http://"+ HttpContext.Request.Host.Value+"/callback";
            //await JS.InvokeAsync<string>("console.log", redirect);
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(path)
                .Build();
           await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }
        
    }
}
