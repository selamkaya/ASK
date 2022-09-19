using ASK.WebApp;
using ASK.WebApp.Helpers;
using ASK.Shared.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

builder.Services.AddAntDesign();
//builder.Services.Configure<ProSettings>( x =>
//{
//	x.Title = "Ant Design Pro";
//	x.NavTheme = "light";
//	x.Layout = "mix";
//	x.PrimaryColor = "daybreak";
//	x.ContentWidth = "Fluid";
//	x.HeaderHeight = 64;
//} );

var configSection = builder.Configuration.GetSection(nameof(AppSettings));
builder.Services.Configure<AppSettings>(configSection);

var config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.Build();

builder.Services.AddLocalization(options =>
						 {
							 options.ResourcesPath = "Resources";
						 });

//builder.AddInfrastructure( config );

builder.Services.AddAuthorizationCore(config =>
	{
		config.AddPolicy("IsAdmin", policy => policy.RequireClaim("role", "Admin"));
	});

builder.Services.AddHttpContextAccessor();

// Web
builder.Services.AddScoped<WmsAppState>();
builder.Services.AddScoped<IWmsStorageService, WmsStorageService>();

builder.Services.AddScoped<WmsAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, WmsAuthStateProvider>();
//builder.Services.AddSingleton<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddScoped<IWmsHttpService, WmsHttpService>();


var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

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
