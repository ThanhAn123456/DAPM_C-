using Microsoft.AspNetCore.Mvc;
using DAPM_C_.Models;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;

namespace DAPM_C_.Controllers
{
    public class ReportSP
    {
        private readonly IWebHostEnvironment _oHostEnvironment;

        private readonly int _maxColum = 2;
        private readonly Font _fontStyle;
        private readonly Font _fontStyleBold;
        private readonly PdfPTable _pdfTable;
        private readonly MemoryStream _memoryStream;

        public ReportSP(IWebHostEnvironment oHostEnvironment)
        {
            _oHostEnvironment = oHostEnvironment;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _fontStyleBold = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfTable = new PdfPTable(_maxColum) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            _memoryStream = new MemoryStream();
        }

        public byte[] Report(List<Sanpham> isanpham)
        {
            using (var document = new Document(PageSize.A4, 5f, 20f, 5f, 5f))
            {
                PdfWriter.GetInstance(document, _memoryStream);
                document.Open();

                float[] widths = { 20f, 100f };
                _pdfTable.SetWidths(widths);

                AddReportHeader();
                AddEmptyRows(2);
                AddReportBody(isanpham);

                _pdfTable.HeaderRows = 2;
                document.Add(_pdfTable);
                document.Close();
            }

            return _memoryStream.ToArray();
        }

        private void AddReportHeader()
        {
            var cell = new PdfPCell(new Phrase("Báo cáo Sản phẩm", _fontStyleBold))
            {
                Colspan = _maxColum,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0,
                ExtraParagraphSpace = 0
            };
            _pdfTable.AddCell(cell);
            _pdfTable.CompleteRow();
        }

        private void AddEmptyRows(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var cell = new PdfPCell(new Phrase("", _fontStyle))
                {
                    Colspan = _maxColum,
                    Border = 0,
                    ExtraParagraphSpace = 10
                };
                _pdfTable.AddCell(cell);
                _pdfTable.CompleteRow();
            }
        }

        private void AddReportBody(List<Sanpham> sanphams)
        {
            AddTableHeaders();
            AddTableBody(sanphams);
        }

        private void AddTableHeaders()
        {
            _pdfTable.AddCell(new PdfPCell(new Phrase("Mã Sản phẩm", _fontStyleBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.Gray,
                ExtraParagraphSpace = 0
            });

            _pdfTable.AddCell(new PdfPCell(new Phrase("Tên Sản phẩm", _fontStyleBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.Gray,
                ExtraParagraphSpace = 0
            });

            _pdfTable.CompleteRow();
        }

        private void AddTableBody(List<Sanpham> sanphams)
        {
            foreach (var sp in sanphams)
            {
                _pdfTable.AddCell(new PdfPCell(new Phrase(sp.MaSanPham.ToString(), _fontStyle))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.White,
                    ExtraParagraphSpace = 0
                });

                _pdfTable.AddCell(new PdfPCell(new Phrase(sp.TenSanPham, _fontStyle))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.White,
                    ExtraParagraphSpace = 0
                });

                _pdfTable.CompleteRow();
            }
        }
    }
}
