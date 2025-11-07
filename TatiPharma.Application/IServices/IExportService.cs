using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.IServices
{
    public interface IExportService
    {
        byte[] GeneratePdf<T>(IEnumerable<T> data, string title = "Export Report");
        byte[] GenerateExcel<T>(IEnumerable<T> data, string sheetName = "Data");
        byte[] GenerateCsv<T>(IEnumerable<T> data);
    }
}
