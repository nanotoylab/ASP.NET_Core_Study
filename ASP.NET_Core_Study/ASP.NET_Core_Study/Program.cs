using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

// Add services to the container.
builder.Services.AddRazorPages();

// ▼▼ ここから追加：セッション機能の有効化 ▼▼
builder.Services.AddDistributedMemoryCache(); // ロッカーをメモリ上に作る準備
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 30分でロッカーの中身を消す
    options.Cookie.HttpOnly = true;                 // JavaScriptから鍵(Cookie)を盗まれないための防具
    options.Cookie.IsEssential = true;              // 必須のクッキーとして扱う
});
// ▲▲ ここまで ▲▲

builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication(); // 認証（あなたは誰か？）
app.UseAuthorization();  // 認可（入っていいか？）
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapControllers();
app.Run();
