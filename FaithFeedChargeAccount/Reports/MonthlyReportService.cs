using Microsoft.Reporting.WinForms;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaithFeedChargeAccountUI.Reports
{
    public class MonthlyReportService
    {
        LocalReport report = null;
        public PdfDocument pdfDoc = new PdfDocument();

        public void ProcessReport(ReportDataSource[] reportDS, string fileName)
        {
            string format = "PDF";
            string deviceInfo = "<DeviceInfo><HumanReadablePDF>True</HumanReadablePDF></DeviceInfo>";
            string encoding = String.Empty;
            string mimeType = String.Empty;
            string extension = String.Empty;
            Warning[] warnings = null;
            string[] streamIDs = null;

            report = new LocalReport();
            string reportPath = Path.Combine(Environment.CurrentDirectory, @"Reports\", fileName);
            report.ReportPath = reportPath;

            if(reportDS != null)
            {
                foreach(ReportDataSource rds in reportDS)
                {
                    report.DataSources.Add(rds);
                }
            }


            Byte[] pdfArray = report.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamIDs, out warnings);

            MemoryStream ms = new MemoryStream(pdfArray);

            PdfDocument tempPDFDoc = PdfReader.Open(ms, PdfDocumentOpenMode.Import);

            for(int i = 0; i < tempPDFDoc.PageCount; i++)
            {
                PdfPage page = tempPDFDoc.Pages[i];
                pdfDoc.AddPage(page);
            }
        }

    }
}
