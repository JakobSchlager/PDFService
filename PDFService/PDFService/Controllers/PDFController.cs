using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDFService.Services;

namespace PDFService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFController : ControllerBase
    {
        private readonly PDFCreatorService pdfCreatorService;
        public PDFController(PDFCreatorService pdfCreatorService)
        {
            this.pdfCreatorService = pdfCreatorService;
        }
        
        [HttpGet]
        public ActionResult<string> GetPdfUrl(int ticketId, string title, string picUrl, int seat, int room, string date, string firstname, string lastname, string address)
        {
            pdfCreatorService.GeneratePDF(ticketId, 
                title, 
                picUrl, 
                seat, 
                room, 
                date, 
                firstname, 
                lastname, 
                "Kino in Ortschaft, PLZ 0000"); 
            return Ok(); 
        }
    }
}
