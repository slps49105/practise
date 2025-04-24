using practise.Models;
using Microsoft.EntityFrameworkCore;
using practise.data;
using Microsoft.AspNetCore.Authentication.Cookies;
using practise.SignalR.Hubs;
using practise.BackgroundTasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddSingleton<DailyStatusChecker>();
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DB_link")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // 登入頁面
        options.LogoutPath = "/Account/Logout"; // 登出頁面
        options.AccessDeniedPath = "/Account/AccessDenied"; // 權限拒絕頁面
    });
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None; // 確保跨站請求能攜帶 Cookie
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var app = builder.Build();
var checker = app.Services.GetRequiredService<DailyStatusChecker>();
app.MapHub<CheckboxHub>("/checkboxHub");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // 顯示詳細錯誤頁面
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}