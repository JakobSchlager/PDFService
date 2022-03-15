using PDFService.Services;
using MassTransit;
using TicketService.Events;
using PDFService;
using MassTransit.MessageData;
using MassTransit.MongoDbIntegration.MessageData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// basic configure services
builder.Services.AddScoped<PDFCreatorService>(); 

// Masstransit RabbitMQ
var queueSettings = builder.Configuration.GetSection("RabbitMQ:QueueSettings").Get<QueueSettings>();
var rabbitmqHostname = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME"); 
if(rabbitmqHostname != null)
{
    queueSettings.HostName = rabbitmqHostname; 
}
Console.WriteLine($"queueSettings.HostName = {queueSettings.HostName}");

IMessageDataRepository messageDataRepository = new MongoDbMessageDataRepository("mongodb://admin:password@localhost:27017", "pdfdata");
builder.Services.AddSingleton(messageDataRepository);
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TicketCreatedEventConsumer>().Endpoint(x => x.Name = "TicketCreated_queue");

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.UseMessageData(messageDataRepository);
        cfg.Host(queueSettings.HostName, queueSettings.VirtualHost, h =>
        {
            h.Username(queueSettings.UserName);
            h.Password(queueSettings.Password);
        });
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddMassTransitHostedService();

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
