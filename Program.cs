using Microsoft.EntityFrameworkCore;
using PatientRecovery.NotificationService.Data;
using PatientRecovery.NotificationService.Services;
using PatientRecovery.NotificationService.Repository;
using PatientRecovery.NotificationService.Configuration;
using PatientRecoverySystem.NotificationService.BackgroundServices;
using PatientRecovery.NotificationService.Messaging;
using System.Globalization;
using PatientRecovery.NotificationService.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SignalR
builder.Services.AddSignalR();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ChatPolicy", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

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
builder.Services.AddScoped<IChatService, ChatService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS
app.UseCors("ChatPolicy");

// Map SignalR hub
app.MapHub<ChatHub>("/chathub");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();