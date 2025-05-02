using Microsoft.Extensions.Options;
using Paytently.Kafka;
using Paytently.Repositories;
using Paytently.Services;

var builder = WebApplication.CreateBuilder(args);

// Add kafka config to the dependency injection
builder.Services.Configure<KafkaSettings>(
    builder.Configuration.GetSection("Kafka"));

// Add services to the dependency injection
builder.Services.AddSingleton<IPaymentRepository, InMemoryPaymentRepository>();
builder.Services.AddSingleton<IPaymentService, PaymentService>();
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
builder.Services.AddSingleton<IKafkaAwaiterService, KafkaAwaiterService>();
builder.Services.AddSingleton(r => r.GetRequiredService<IOptions<KafkaSettings>>().Value);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
