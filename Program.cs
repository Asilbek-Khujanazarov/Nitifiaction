using Microsoft.EntityFrameworkCore;
using PatientRecovery.NotificationService.Data;
using PatientRecovery.NotificationService.Services;
using PatientRecovery.NotificationService.Repository;
using PatientRecovery.NotificationService.Configuration;
using PatientRecoverySystem.NotificationService.BackgroundServices;
using PatientRecovery.NotificationService.Messaging;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Messaging
builder.Services.AddSingleton<IRabbitMQService>(sp => 
    new RabbitMQService(builder.Configuration.GetValue<string>("RabbitMQ:Host") ?? "localhost"));
// Background Services
builder.Services.AddHostedService<NotificationProcessorService>();
builder.Services.AddHostedService<EmergencyNotificationConsumer>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
// Globalization settings
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configuration
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<SmsSettings>(
    builder.Configuration.GetSection("SmsSettings"));

// Services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationTemplateService, NotificationTemplateService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();