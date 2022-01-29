using MassTransit;

namespace PDFService.Events;
public class PDFCreatedEvent
{
    public int TicketId { get; set; }
    public string Email { get; set; }
    public MessageData<byte[]> Document { get; set; }
}
