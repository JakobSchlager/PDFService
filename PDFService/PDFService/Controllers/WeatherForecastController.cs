using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PDFService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public ActionResult Get() 
        {
            const string url = "https://image.tmdb.org/t/p/w500/rjkmN1dniUHVYAtwuV3Tji7FsDO.jpg"; 
            const string moviePicLocation = @"c:\tempImg\img.jpg"; 
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(url), moviePicLocation);
            }

            DocumentBuilder builder = DocumentBuilder.New();
            // Create a section builder and customize the section:

            /*var sectionBuilder =
                builder
                    .AddSection()
                        // Customize settings:
                        //.SetMargins(horizontal: 30, vertical: 10)
                        .SetSize(PaperSize.A4)
                        .SetOrientation(PageOrientation.Portrait)
                        .SetNumerationStyle(NumerationStyle.Arabic);
/*
            
            /*  string imageSmile = @"c:\tempImg\img.jpg";
            sectionBuilder
               .AddParagraph("")
               .SetAlignment(HorizontalAlignment.Left)
               .AddInlineImageToParagraph(imageSmile)
               .AddText(" NAME OF THE THING, DATE AND SO ON HAHAH"); 
            */

            BuildDocument(); 

            return NoContent(); 
        }

        public static void BuildDocument()
        {
            var builder = DocumentBuilder.New();

            var table = DocumentBuilder.New()
                .AddSection()
                    .AddTable().SetWidth(750)
                        .AddColumnToTable()
                        .AddRow()
                            .AddCell("Kino")
                                .SetHorizontalAlignment(HorizontalAlignment.Center)
                    .ToTable();

            AddTablePartToCell(table.AddRow().AddCell());
            
            table.AddRow()
                    .AddCell("Kino in Ortschaft, PLZ 0000")
            .ToDocument().Build("Testrun.pdf");
        }

        private static void AddTablePartToCell(TableCellBuilder cell)
        {
            cell.AddTable()
            .SetWidth(XUnit.FromPercent(100))
            .AddColumnPercentToTable("", 20)
            .AddColumnPercentToTable("", 60)
            .AddColumnPercentToTable("", 20)
            .AddRow()
                .AddCell()
                    .SetRowSpan(4)
                    .AddImageToCell(@"c:\tempImg\img.jpg", XSize.FromHeight(250))
                    .SetHorizontalAlignment(HorizontalAlignment.Center)
                    .SetVerticalAlignment(VerticalAlignment.Center)
                    .ToRow()
                .AddCell("Title").SetFontSize(20).SetColSpan(2)
                .ToTable()
            .AddRow()
                .AddCellToRow()
                .AddCellToRow("Raum 2, Sitzplatz 12").SetFontSize(15)
                .AddCellToRow("20.01.2022").SetHorizontalAlignment(HorizontalAlignment.Right).ToTable()
            .AddRow()
                .AddCellToRow()
                .AddCell("\n").SetColSpan(2)
                .SetPadding(0, 32); 
        }
    }
}