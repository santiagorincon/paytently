using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AcquirerSimulator;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{
    services.AddHostedService<KafkaRequestConsumer>();
});

var app = builder.Build();
await app.RunAsync();