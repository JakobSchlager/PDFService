using MassTransit;
using PDFService.Services;

namespace TicketService.Events;
public class TicketCreatedEvent
{
    public int TicketId { get; set; }
    public string MovieTitle { get; set; }
    public string MoviePicUrl { get; set; }
    public int Seat { get; set; }
    public int Room { get; set; }
    public string Date { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Address { get; set; }
}

public class TicketCreatedEventConsumer : IConsumer<TicketCreatedEvent>
{
    private readonly PDFCreatorService _pdfCreatorService; 
    public TicketCreatedEventConsumer(PDFCreatorService pdfCreatorService)
    {
        this._pdfCreatorService = pdfCreatorService; 
    }

    public async Task Consume(ConsumeContext<TicketCreatedEvent> context)
    {
        Console.WriteLine("TicketCreated: " + context.Message.MovieTitle);
        Console.WriteLine("logging _pdfCreatorService: " + _pdfCreatorService);
        _pdfCreatorService.GeneratePDF(context.Message);
    }
}
