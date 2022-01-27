namespace PDFService.Events;
public class PDFCreatedEvent
{
    public int TicketId { get; set; }
    public string Email { get; set; }
}
