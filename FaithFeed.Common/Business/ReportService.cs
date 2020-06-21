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
    public static class ReportService
    {
        private static string ReportDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Reports\");
        public static string CreateAccountInvoice(ReportDataSource[] dataSources, string accountName)
        {
            string reportPath = Path.Combine(ReportDirectory, "SelectedAccountInvoice.rdlc");
            var reportByteArray = RenderReport(reportPath, dataSources);
            var invoice = AddPages(reportByteArray, new PdfDocument());
            return SaveReport(invoice, accountName);
        }

        public static string CreateMonthlyInvoices(List<ReportDataSource[]> dataSources) {
            
            var monthlyInvoices = new PdfDocument();
            string reportPath = Path.Combine(ReportDirectory, "SelectedAccountInvoice.rdlc");
            foreach(var dataSource in dataSources) {
                var reportByteArray = RenderReport(reportPath, dataSource);
                monthlyInvoices = AddPages(reportByteArray, monthlyInvoices);
            }
            return SaveReport(monthlyInvoices);
        }

        private static byte[] RenderReport(string reportPath, ReportDataSource[] dataSources) {

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

        private static PdfDocument CreatePdf(byte[] reportByte) {
            var pdf = AddPages(reportByte, new PdfDocument());
            return pdf;
        }

        private static PdfDocument AddPages(byte[] reportByte, PdfDocument pdf) {
            using (MemoryStream ms = new MemoryStream(reportByte)) {
                var tempDoc = PdfReader.Open(ms, PdfDocumentOpenMode.Import);
                for (int i = 0; i < tempDoc.PageCount; i++) {
                    PdfPage page = tempDoc.Pages[i];
                    pdf.AddPage(page);
                }
            }
            return pdf;
        }

        private static string SaveReport(PdfDocument report, string accountName) {
            string path = Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\Reports\Accounts\{accountName}").FullName;
            string file = $"{DateTime.Now.Month}-{DateTime.Now.Year}.pdf";
            string fullPath = Path.Combine(path, file);
            report.Save(fullPath);
            return fullPath;
        }
        private static string SaveReport(PdfDocument report) {
            string path = Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\Reports\Monthly").FullName;
            string file = $"{DateTime.Now.Month}-{DateTime.Now.Year}.pdf";
            string fullPath = Path.Combine(path, file);
            report.Save(fullPath);
            return fullPath;
        }
    }
}
