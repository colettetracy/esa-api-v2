using ESA.API.Extensions;
using ESA.Core;
using ESA.Core.Models.Session;
using ESA.Infrastructure;
using Mailjet.Client;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("JwtSetting"));

builder.Services.AddApiAuthentication(builder.Configuration);
builder.Services.AddInfrastructureContext(builder.Configuration.GetConnectionString("Academy"), false, builder.Configuration.GetConnectionString("EnvironmentVariable"));
builder.Services.AddInfrastructureServices();
builder.Services.AddHttpClient<IMailjetClient, MailjetClient>(client =>
{
    client.SetDefaultSettings();
    client.UseBasicAuthentication("c49385448c0ed61aeb55a356393b9082", "10a1fccdc0d9711aef04f41c45cf67ec");
    client.Timeout = TimeSpan.FromMinutes(3);
});
builder.Services.AddBusinessLogic();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options => { options.ReportApiVersions = true; });
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("esa-cors", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("esa-cors");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.MapControllers();
app.UseSerilogRequestLogging();

app.Run();
