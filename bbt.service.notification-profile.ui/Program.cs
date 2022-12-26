
using bbt.service.notification.ui.Base;
using bbt.service.notification.ui.Base.Token;
using bbt.service.notification.ui.Component;
using bbt.service.notification.ui.Component.Pagination;
using bbt.service.notification.ui.Configuration;
using bbt.service.notification.ui.Data;
using bbt.service.notification.ui.Override;
using bbt.service.notification.ui.Override.Service;
using bbt.service.notification.ui.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using Radzen;
using Refit;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<bbt.service.notification.ui.Data.HttpContextAccessor>();
IdentityModelEventSource.ShowPII = true;
builder.Services.AddAuthentication(options =>
{

    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
   .AddCookie()
    .AddOpenIdConnect("Auth0", options =>
    {


        options.NonceCookie.SameSite = SameSiteMode.Unspecified;
        options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;

        options.Authority = $"{builder.Configuration["Okta:OktaDomain"]}";
        // options.SaveTokens = true;
        // Configure the Auth0 Client ID and Client Secret
        options.ClientId = builder.Configuration["Okta:ClientId"];
        options.ClientSecret = builder.Configuration["Okta:ClientSecret"];
        options.AuthenticationMethod = OpenIdConnectRedirectBehavior.FormPost;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.RequireHttpsMetadata = false;
        options.GetClaimsFromUserInfoEndpoint = true;
        // Use the authorization code flow.
        options.ResponseType = OpenIdConnectResponseType.Code;

        options.CallbackPath = new PathString("/authorization-code/callback");
        options.SignedOutCallbackPath = new PathString("/authorization-code/signout/callback");
        // Configure the Claims Issuer to be Auth0
        options.ClaimsIssuer = "Auth0";
        //options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        //{
        //    ValidateIssuer = true
        //};
        options.SecurityTokenValidator = new JwtSecurityTokenHandler
        {
            // Disable the built-in JWT claims mapping feature.
            InboundClaimTypeMap = new Dictionary<string, string>()
        };
        options.Events = new OpenIdConnectEvents

        {

            OnRedirectToIdentityProvider = context =>
            {

                var builder = new UriBuilder(context.ProtocolMessage.RedirectUri);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    builder.Scheme = "https";
                    builder.Port = -1;
                }
                //else
                //{
                //    builder.Scheme = "http";

                //}

                context.ProtocolMessage.RedirectUri = builder.ToString();
                return Task.FromResult(0);
            },
            OnTokenValidated = context =>
            {

                try
                {
                    if (context is not null && context.Principal is not null && context.Principal.Identity is not null)
                    {
                        var identity = (ClaimsIdentity)context.Principal.Identity;
                        List<Claim> addToken = new();
                        if (context?.TokenEndpointResponse is not null && context?.TokenEndpointResponse?.AccessToken is not null)
                        {
                            addToken.Add(new Claim("access_token", context?.TokenEndpointResponse?.AccessToken));
                            using (var client = new HttpClient())
                            {
                                string clientid = builder.Configuration["Okta:TokenUrl"];
                                client.BaseAddress = new Uri(clientid);
                                var content = new FormUrlEncodedContent(new[]
                            {
                        new KeyValuePair<string, string>("access_token",  context?.TokenEndpointResponse?.AccessToken),
                        });
                                var result = client.PostAsync("/ib/Resource", content);
                                string responseContent = result.Result.Content.ReadAsStringAsync().Result;
                                AccessTokenResources? accessTokenResources =
                       JsonConvert.DeserializeObject<AccessTokenResources>(responseContent);
                                if (accessTokenResources != null && !string.IsNullOrEmpty(accessTokenResources.sicil) && accessTokenResources.sicil.Length < 12)
                                    addToken.Add(new Claim("sicil", accessTokenResources.sicil));

                            }
                        }
                        if (context?.TokenEndpointResponse is not null && context?.TokenEndpointResponse?.IdToken is not null)
                        {
                            addToken.Add(new Claim("id_token", context?.TokenEndpointResponse?.IdToken));
                        }
                        if (context?.TokenEndpointResponse is not null && context?.TokenEndpointResponse?.RefreshToken is not null)
                        {
                            addToken.Add(new Claim("refresh_token", context?.TokenEndpointResponse?.RefreshToken));
                        }

                        if (addToken.Count > 0)
                        {
                            identity.AddClaims(addToken);
                        }
                    // so that we don't issue a session cookie but one with a fixed expiration
                        context.Properties.IsPersistent = true;
                        context.Properties.AllowRefresh = true;

                    // align expiration of the cookie with expiration of the

                        var accessToken = new JwtSecurityToken(context.TokenEndpointResponse.AccessToken);

                    }
                    else
                    {
                    //hk todo 
                    //redirect
                    }
                }
                catch
                {

                }

                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
                {
                // If your authentication logic is based on users then add your logic here
                    return Task.CompletedTask;
                },
            OnTicketReceived = context =>
            {
                // If your authentication logic is based on users then add your logic here
                return Task.CompletedTask;
            },

            //HK save for later
            OnSignedOutCallbackRedirect = context =>
            {
                context.Response.Redirect("~/");
                context.HandleResponse();

                return Task.CompletedTask;
            },
            OnUserInformationReceived = context =>
            {
                //IHttpContextAccessor httpContextAccessor;
                //   RegisterUser(context);

                return Task.CompletedTask;
            },
        };
    });










// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ThemeState>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<BaseComponent>();
builder.Services.AddScoped<BasePaginationComponent>();
builder.Services.AddScoped<INotification, RadzenNotification>();
builder.Services.AddSingleton<IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
builder.Services.AddSingleton<IBaseConfiguration,BaseConfiguration>();
builder.Services.AddRefitClient<IDengageService>().
               ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiUri"]));
builder.Services.AddRefitClient<ISourceService>().
               ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiUri"]));
builder.Services.AddRefitClient<IMessageNotificationLogService>().
               ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiUri"]));
builder.Services.AddRefitClient<IProductCodeService>().
               ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiUri"]));
builder.Services.AddRefitClient<IUserRegistryService>().
               ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiUri"]));
builder.Services.Configure<OktaSettings>(builder.Configuration.GetSection("Okta"));
builder.Services.AddScoped<ITokenService, OktaTokenService>();

FrameworkDependencyHelper.Instance.LoadServiceCollection(builder.Services);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
