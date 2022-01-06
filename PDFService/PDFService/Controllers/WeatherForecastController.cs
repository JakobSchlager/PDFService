using Aspose.BarCode.Generation;
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
        const string barcodeFile = "barcode.jpg";
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
            GenerateBarcode("12345567");
            DownloadMoviePicture();
            BuildDocument();

            return NoContent();
        }

        private void GenerateBarcode(string id)
        {
            // instantiate object and set different barcode properties
            BarcodeGenerator generator = new BarcodeGenerator(EncodeTypes.Code128, id);
            generator.Parameters.Barcode.XDimension.Millimeters = 1f;

            // save the image to your system and set its image format to Jpeg
            generator.Save(barcodeFile, BarCodeImageFormat.Jpeg);
        }

        private static void DownloadMoviePicture()
        {
            const string url = "https://image.tmdb.org/t/p/w500/rjkmN1dniUHVYAtwuV3Tji7FsDO.jpg";
            const string moviePicLocation = @"c:\tempImg\img.jpg";
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(url), moviePicLocation);
            }
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
            .AddColumnPercentToTable("", 10)
            .AddColumnPercentToTable("", 70)
            .AddColumnPercentToTable("", 20)
            .AddRow()
                .AddCell()
                    .SetRowSpan(3)
                    .AddImageToCell(@"c:\tempImg\img.jpg", XSize.FromHeight(200))
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
                .AddCell()
                    .AddImageToCell(barcodeFile, XSize.FromHeight(200))
                    .SetHorizontalAlignment(HorizontalAlignment.Center)
                    .SetVerticalAlignment(VerticalAlignment.Center)
                    .ToRow()
                .AddCellToRow("Vorname Nachname").SetHorizontalAlignment(HorizontalAlignment.Right);
        }
    }
}