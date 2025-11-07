using ClosedXML.Excel;
using CsvHelper;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.IServices;

namespace TatiPharma.Application.Services
{
    public class ExportService : IExportService
    {
        public byte[] GeneratePdf<T>(IEnumerable<T> data, string title = "Export Report")
        {
            // ADD THIS LINE (License)
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    // FIX #1: Switch to Landscape for more width (wider page)
                    page.Size(PageSizes.A4.Landscape());

                    // FIX #2: Smaller margins
                    page.Margin(0.5f, Unit.Inch);

                    page.Header().Text(title).FontSize(16).Bold().AlignCenter(); // Smaller header

                    page.Content().PaddingVertical(0.5f, Unit.Centimetre).Table(table =>
                    {
                        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                        // FIX #3: Auto columns, but allow flexible widths
                        table.ColumnsDefinition(columns =>
                        {
                            foreach (var property in properties)
                            {
                                columns.RelativeColumn(); // Equal share, but will wrap
                            }
                        });

                        // Header
                        table.Header(header =>
                        {
                            foreach (var property in properties)
                            {
                                header.Cell()
                                    .Padding(3)
                                    .Border(0.5f)
                                    .Text(property.Name)
                                    .Bold()
                                    .FontSize(8); // Smaller font
                            }
                        });

                        // Data Rows
                        foreach (var item in data)
                        {
                            foreach (var property in properties)
                            {
                                var value = property.GetValue(item)?.ToString() ?? string.Empty;
                                table.Cell()
                                    .Padding(3)
                                    .Border(0.5f)
                                    .Text(value)
                                    .FontSize(6)  // Even smaller for data
                                    .LineHeight(0.8f); // Tighter lines
                            }
                        }
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GenerateExcel<T>(IEnumerable<T> data, string sheetName = "Data")
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int col = 0; col < properties.Length; col++)
            {
                worksheet.Cell(1, col + 1).Value = properties[col].Name;
                worksheet.Cell(1, col + 1).Style.Font.Bold = true;
            }

            int row = 2;
            foreach (var item in data)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    var value = properties[col].GetValue(item)?.ToString() ?? string.Empty;
                    worksheet.Cell(row, col + 1).Value = value;
                }
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        public byte[] GenerateCsv<T>(IEnumerable<T> data)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(data);
            writer.Flush();
            return memoryStream.ToArray();
        }
    }
}
