using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider();

// HttpContext Accessor
builder.Services.AddHttpContextAccessor();

// HttpClient for API Communication
builder.Services.AddHttpClient<ApiClient>(client =>
{
    //right now modified this..
    var apiBaseUrl =
        builder.Configuration["ApiSettings:BaseUrl"]
       ?? "http://localhost:7093/";
    //var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001/";
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
  //  client.Timeout = TimeSpan.FromMinutes(2);
});

// API SERVICES
builder.Services.AddScoped<ApartmentApiService>();     
builder.Services.AddScoped<AuthApiService>();
builder.Services.AddScoped<OnboardingApiService>();
builder.Services.AddScoped<DashboardApiService>(); 
builder.Services.AddScoped< AdminResidentApiService>();
builder.Services.AddScoped<CommunityMemberApiService>();
builder.Services.AddScoped<StaffMemberApiService>();
builder.Services.AddScoped<ResidentManagementApiService>();
builder.Services.AddScoped<EnhancedDashboardApiService>();
builder.Services.AddScoped<ManagerApiService>();
// Make sure CommunityMemberApiService is also registered

// COOKIE AUTHENTICATION 

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Index";
        options.LogoutPath = "/Login/Logout";

        // MUST NOT be Login
        options.AccessDeniedPath = "/Home/AccessDenied";

        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;

        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.Name = ".ApartmentManagement.Auth";
    });

// AUTHORIZATION
builder.Services.AddAuthorization();


// SESSION (TempData, OTP, flows)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// HTTP PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UserStatusMiddleware>();


app.UseSession();

// ROUTING
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


















