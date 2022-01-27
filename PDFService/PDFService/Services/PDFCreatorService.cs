﻿using Aspose.BarCode.Generation;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using PDFService.Events;
using System.Net;
using TicketService.Events;

namespace PDFService.Services
{
    public class PDFCreatorService
    {
        private readonly IBus _bus; 
        public PDFCreatorService(IBus bus)
        {
            this._bus = bus;
        }

        const string barcodeFile = "barcode.jpg";
        const string moviePicLocation = @"c:\tempImg\img.jpg";
        public void GeneratePDF(int ticketId, string movieTitle, string moviepicUrl, int seat, int room, string date, string firstname, string lastname, string address)
        {
            Console.WriteLine("PDFCreatorService::GeneratePDF"); 
            GenerateBarcode(ticketId.ToString());
            Console.WriteLine("PDFCreatorService::GenerateBarcoode worked!"); 
            DownloadMoviePicture(moviepicUrl);
            Console.WriteLine("PDFCreatorService::DownloadMoviePicutre worked!"); 
            BuildDocument(firstname, lastname, movieTitle, room.ToString(), seat.ToString(), date, address);

            Console.WriteLine("PDFCreated, sending out event!"); 
            _bus.Publish(new PDFCreatedEvent
            {
                Email = "jakobschlager.biz@gmail.com", 
                TicketId = 1, 
            });
            Console.WriteLine("PDFCreated, sent out event!"); 
        }

        public void GeneratePDF(TicketCreatedEvent ticketCreatedEvent)
        {
            GeneratePDF(ticketCreatedEvent.TicketId,
                ticketCreatedEvent.MovieTitle,
                ticketCreatedEvent.MoviePicUrl,
                ticketCreatedEvent.Seat,
                ticketCreatedEvent.Room,
                ticketCreatedEvent.Date,
                ticketCreatedEvent.Firstname,
                ticketCreatedEvent.Lastname,
                ticketCreatedEvent.Address);
        } 

        private void GenerateBarcode(string id)
        {
            // instantiate object and set different barcode properties
            BarcodeGenerator generator = new BarcodeGenerator(EncodeTypes.Code128, id);
            generator.Parameters.Barcode.XDimension.Millimeters = 1f;

            // save the image to your system and set its image format to Jpeg
            generator.Save(barcodeFile, BarCodeImageFormat.Jpeg);
        }

        private void DownloadMoviePicture(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(url), moviePicLocation);
            }
        }
        private void BuildDocument(string firstname, string lastname, string title, string room, string seat, string date, string address)
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

            AddTablePartToCell(table.AddRow().AddCell(), firstname, lastname, title, room, seat, date, address);

            table.AddRow()
                    .AddCell(address)
            .ToDocument().Build("Testrun.pdf");
        }

        private void AddTablePartToCell(TableCellBuilder cell, string firstname, string lastname, string title, string room, string seat, string date, string address)
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
                .AddCell(title).SetFontSize(20).SetColSpan(2)
                .ToTable()
            .AddRow()
                .AddCellToRow()
                .AddCellToRow($"Raum {room}, Sitzplatz {seat}").SetFontSize(15)
                .AddCellToRow(date).SetHorizontalAlignment(HorizontalAlignment.Right).ToTable()
            .AddRow()
                .AddCellToRow()
                .AddCell()
                    .AddImageToCell(barcodeFile, XSize.FromHeight(200))
                    .SetHorizontalAlignment(HorizontalAlignment.Center)
                    .SetVerticalAlignment(VerticalAlignment.Center)
                    .ToRow()
                .AddCellToRow($"{firstname} {lastname}").SetHorizontalAlignment(HorizontalAlignment.Right);
        }
    }
}
