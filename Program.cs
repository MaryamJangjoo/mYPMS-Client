using Microsoft.EntityFrameworkCore;
using mYPMS.Data;
using System.Globalization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using mYPMS.Models;
using mYPMS.Services;

var waOptions = new WebApplicationOptions
{
    ContentRootPath = AppContext.BaseDirectory,
    Args            = args,
    ApplicationName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
};

WebApplicationBuilder builder = WebApplication.CreateBuilder(waOptions);

builder.Host.UseWindowsService();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrgins", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddDbContext<mYPMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CNS")!));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount   = false;
    options.Password.RequiredLength         = 4;
    options.Password.RequireDigit           = false;
    options.Password.RequireLowercase       = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase       = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<mYPMSContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout        = TimeSpan.FromHours(2);
    option.Cookie.HttpOnly    = true;
    option.Cookie.IsEssential = true;
    option.Cookie.Name        = ".mYPMS.Session";
});

builder.Services.Configure<AlprOptions>(
    builder.Configuration.GetSection("Alpr"));

builder.Services.AddHttpClient<SatpaClient>();

builder.Services.AddHttpClient("camera")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
        UseCookies = false,
    });

builder.Services.AddScoped<IAlprService, AlprService>();
builder.Services.AddHttpClient<IAlprService, AlprService>();

builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.MinifyJsFiles("js/**/*.js");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<mYPMSContext>();
    context.Database.EnsureCreated();
}

CultureInfo.DefaultThreadCurrentCulture   = PersianDateExtensionMethods.GetPersianCulture();
CultureInfo.DefaultThreadCurrentUICulture = PersianDateExtensionMethods.GetPersianCulture();

app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
    DefaultContentType    = "text/html",
});
app.UseRouting();
app.UseCors("AllowAllOrgins");
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name:    "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

var startupLogger = app.Services
                       .GetRequiredService<ILoggerFactory>()
                       .CreateLogger("Startup");

startupLogger.LogInformation("mYPMS started — ALPR lazy mode (SatpaClient)");
startupLogger.LogInformation("ALPR endpoint: {Url}",
    builder.Configuration["Alpr:BaseUrl"] ?? "not configured");

app.Run();