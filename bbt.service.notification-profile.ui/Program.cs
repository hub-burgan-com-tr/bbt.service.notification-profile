
using bbt.service.notification.ui.Component;
using bbt.service.notification.ui.Component.Pagination;
using bbt.service.notification.ui.Configuration;
using bbt.service.notification.ui.Data;
using bbt.service.notification.ui.Override;
using bbt.service.notification.ui.Override.Service;
using bbt.service.notification.ui.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Refit;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IBaseConfiguration,BaseConfiguration>();
builder.Services.AddRefitClient<IDengageService>().
               ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiUri"]));
builder.Services.AddRefitClient<ISourceService>().
               ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiUri"]));
builder.Services.AddRefitClient<IMessageNotificationLogService>().
               ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiUri"]));
builder.Services.AddRefitClient<IProductCodeService>().
               ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiUri"]));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
