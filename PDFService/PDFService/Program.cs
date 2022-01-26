using PDFServiceService;
using MassTransit;
using TicketService;
using TicketService.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Masstransit RabbitMQ
var queueSettings = builder.Configuration.GetSection("RabbitMQ:QueueSettings").Get<QueueSettings>();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(queueSettings.HostName, queueSettings.VirtualHost, h =>
        {
            h.Username(queueSettings.UserName);
            h.Password(queueSettings.Password);
        });
        cfg.ConfigureEndpoints(context);
        cfg.ReceiveEndpoint(x => x.Consumer<TicketCreatedEventConsumer>()); 
    });
});
builder.Services.AddMassTransitHostedService();

// basic configure services

builder.Services.AddScoped<PDFCreatorService>(); 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
