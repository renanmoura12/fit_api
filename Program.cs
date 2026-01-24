using api_fit.Data;
using api_fit.Interfaces;
using api_fit.Services;
using api_fit.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Db(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IRepository, Repository>();

builder.Services.SwaggerInfra(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddTransient<IMailService, MailService>();

builder.Services.Cors();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(a =>
{
    a.SwaggerEndpoint("/swagger/v1/swagger.json", "FIT API");
    a.RoutePrefix = string.Empty;
});

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
