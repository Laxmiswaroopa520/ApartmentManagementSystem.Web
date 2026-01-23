// ApartmentManagementSystem.Web/Program.cs
using ApartmentManagementSystem.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider();

// HttpContext Accessor
builder.Services.AddHttpContextAccessor();

// HttpClient for API Communication
builder.Services.AddHttpClient<ApiClient>(client =>
{
    var apiBaseUrl =
        builder.Configuration["ApiSettings:BaseUrl"]
        ?? "http://localhost:7093/";

    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
  //  client.Timeout = TimeSpan.FromMinutes(2);
});

// ===============================
// API SERVICES
// ===============================
builder.Services.AddScoped<AuthApiService>();
builder.Services.AddScoped<OnboardingApiService>();
builder.Services.AddScoped<DashboardApiService>(); 
builder.Services.AddScoped< AdminResidentApiService>();
builder.Services.AddScoped<CommunityMemberApiService>();
builder.Services.AddScoped<StaffMemberApiService>();
builder.Services.AddScoped<ResidentManagementApiService>();
builder.Services.AddScoped<EnhancedDashboardApiService>();


//builder.Services.AddScoped<ApiClient>();
// COOKIE AUTHENTICATION (CRITICAL)

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

app.UseSession();

// ROUTING
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();




/*

// ApartmentManagementSystem.Web/Program.cs
using ApartmentManagementSystem.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
//builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider();


// Add HttpContextAccessor (needed for ApiClient)
builder.Services.AddHttpContextAccessor();

// Configure HttpClient for API communication
builder.Services.AddHttpClient<ApiClient>(client =>
{
    var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:7093/";
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Register API Services
builder.Services.AddScoped<AuthApiService>();
builder.Services.AddScoped<OnboardingApiService>();

// ⭐ CRITICAL: Add Cookie Authentication for MVC
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Login/Index";
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.Name = ".ApartmentManagement.Auth";
    });

builder.Services.AddAuthorization();

// Add session (optional but recommended for TempData)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ⭐ CRITICAL: Authentication MUST come before Authorization
app.UseAuthentication();
app.UseAuthorization();

// Use session
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



*/













