using ASK.Core.Data;
using ASK.Core.Services;
using ASK.Shared;
using ASK.Shared.Interfaces;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wms.Domain.Net6;

var builder = WebApplication.CreateBuilder();

// Configs
var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

var configSection = builder.Configuration.GetSection(nameof(AppSettings));
builder.Services.Configure<AppSettings>(configSection);

//needed for localization, see IStringLocalizer<AccountService> _localizer
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

// Databases
var wmsDbConnection = config["ConnectionStrings:WmsDbConnection"];
if (string.IsNullOrEmpty(wmsDbConnection))
    throw new Exception("No ConnectionString available");

builder.Services.AddDbContext<WmsDbContext>(options => options.UseSqlServer(wmsDbConnection));

builder.Services.Configure<PasswordHasherOptions>(options =>
{
    options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2;
});

// Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISprocRepository, SprocRepository>();
builder.Services.AddScoped<IAppService, AppService>();

// FastEndPoint
builder.Services.AddFastEndpoints();
builder.Services.AddAuthenticationJWTBearer(builder.Configuration["AppSettings:JwtSecretKey"]);
builder.Services.AddSwaggerDoc(settings =>
{
    settings.Title = "WmsServer";
    settings.Version = "v1";
    settings.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header
    });

},
 shortSchemaNames: true);

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints(c =>
{
    //c.VersioningOptions = o =>
    //{
    //	o.Prefix = "v";
    //	o.SuffixedVersion = false;
    //};
    //c.ShortEndpointNames = true;
    //c.SerializerOptions = o =>
    //{
    //    o.Converters.Add(new JsonStringEnumConverter());
    //};
});
app.UseOpenApi();
app.UseSwaggerUi3(s => s.ConfigureDefaults());
app.UseDefaultExceptionHandler();
app.Run();
