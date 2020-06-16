using Microsoft.Reporting.WinForms;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FaithFeed.Common.Business
{
    public static  class ReportService
    {
        public static string ReportDirectory = Path.Combine(AppContext.BaseDirectory, @"Reports\");
        public static PdfDocument CreateAccountInvoice(ReportDataSource[] dataSources, string fileName)
        {
            var invoice = new PdfDocument();
            string reportPath = Path.Combine(ReportDirectory, fileName);
            var report = RenderReport(reportPath, dataSources);

            using (MemoryStream ms = new MemoryStream(report)) {
                PdfDocument tempPDFDoc = PdfReader.Open(ms, PdfDocumentOpenMode.Import);
                for (int i = 0; i < tempPDFDoc.PageCount; i++) {
                    PdfPage page = tempPDFDoc.Pages[i];
                    invoice.AddPage(page);
                }
            }
            return invoice;
        }

        public static byte[] RenderReport(string reportPath, ReportDataSource[] dataSources) {

            var pdfDoc = new PdfDocument();
            string format = "PDF";
            string deviceInfo = "<DeviceInfo><HumanReadablePDF>True</HumanReadablePDF></DeviceInfo>";
            string encoding = String.Empty;
            string mimeType = String.Empty;
            string extension = String.Empty;
            Warning[] warnings = null;
            string[] streamIDs = null;

            var report = new LocalReport();
            report.ReportPath = reportPath;

            if (dataSources != null) {
                foreach (ReportDataSource rds in dataSources) {
                    report.DataSources.Add(rds);
                }
            }

            return report.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamIDs, out warnings);

        }

        public static string SaveReport(PdfDocument report, string accountName) {
            string path = Directory.CreateDirectory($@"{Environment.CurrentDirectory}\Reports\Accounts\{accountName}").FullName;
            string file = $"{DateTime.Now.Month}-{DateTime.Now.Year}.pdf";
            string fullPath = Path.Combine(path, file);
            report.Save(fullPath);
            return fullPath;
        }
        public static string SaveReport(PdfDocument report) {
            string path = Directory.CreateDirectory($@"{Environment.CurrentDirectory}\Reports\Monthly").FullName;
            string file = $"{DateTime.Now.Month}-{DateTime.Now.Year}.pdf";
            string fullPath = Path.Combine(path, file);
            report.Save(fullPath);
            return fullPath;
        }
    }
}
