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
            pdfCreatorService.GeneratePDF(1, 
                "Title", 
                "https://image.tmdb.org/t/p/w500/rjkmN1dniUHVYAtwuV3Tji7FsDO.jpg", 
                23, 
                2, 
                "20.01.2022", 
                "Jakob", 
                "Schlager", 
                "Kino in Ortschaft, PLZ 0000"); 
            return Ok(); 
        }
    }
}
