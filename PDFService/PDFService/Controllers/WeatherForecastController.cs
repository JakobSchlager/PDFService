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
                            .AddCell("City information")
                                .SetHorizontalAlignment(HorizontalAlignment.Center)
                    .ToTable();
            AddTablePartToCell(table.AddRow().AddCell());
            table.AddRow()
                    .AddCell("Complex table is completed.")
            .ToDocument().Build("Testrun.pdf");
        }

        private static void AddTablePartToCell(TableCellBuilder cell)
        {
            cell.AddTable()
            .SetWidth(XUnit.FromPercent(100))
            .AddColumnPercentToTable("", 30)
            .AddColumnPercentToTable("", 35)
            .AddColumnPercentToTable("", 35)
            .AddRow()
                .AddCell()
                    .SetRowSpan(4)
                    .AddImageToCell(@"c:\tempImg\img.jpg").ToRow()
                .AddCellToRow("New York")
                .AddCellToRow("New York").ToTable()
            .AddRow()
                .AddCellToRow()
                .AddCellToRow("Los Angeles")
                .AddCellToRow("California").ToTable()
            .AddRow()
                .AddCellToRow()
                .AddCellToRow("Chicago")
                .AddCellToRow("Illinois").ToTable()
            .AddRow()
                .AddCellToRow()
                .AddCell("\n").SetColSpan(2)
                          .SetPadding(0, 32);
        }
    }
}