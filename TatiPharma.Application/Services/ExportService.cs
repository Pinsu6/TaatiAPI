using ClosedXML.Excel;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Reflection;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.IServices;

namespace TatiPharma.Application.Services
{
    public class ExportService : IExportService
    {
        public byte[] GeneratePdf<T>(IEnumerable<T> data, string title = "Export Report")
        {
            // ADD THIS LINE (Fixes the license error)
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(1, Unit.Inch);
                    page.Header().Text(title).FontSize(20).Bold().AlignCenter();
                    page.Content().PaddingVertical(1, Unit.Centimetre).Table(table =>
                    {
                        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        table.ColumnsDefinition(columns =>
                        {
                            foreach (var _ in properties) columns.RelativeColumn();
                        });

                        // Header
                        table.Header(header =>
                        {
                            foreach (var property in properties)
                                header.Cell().Padding(5).Border(1).Text(property.Name).Bold().FontSize(12);
                        });

                        // Data Rows
                        foreach (var item in data)
                        {
                            foreach (var property in properties)
                            {
                                var value = property.GetValue(item)?.ToString() ?? string.Empty;
                                table.Cell().Padding(5).Border(1).Text(value).FontSize(10);
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
