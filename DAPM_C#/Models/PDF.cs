using Microsoft.AspNetCore.Mvc;
using DAPM_C_.Models;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System;
using System.Text;

namespace DAPM_C_.Models
{
    public class PDF
    {
        private readonly IWebHostEnvironment _oHostEnvironment;

        private readonly int _maxColum = 2;
        private readonly Font _fontStyle;
        private readonly Font _fontStyleBold;
        private readonly PdfPTable _pdfTable;
        private readonly MemoryStream _memoryStream;
        public PDF(IWebHostEnvironment oHostEnvironment)
        {
            _oHostEnvironment = oHostEnvironment;
            string fontPath = Path.Combine(_oHostEnvironment.WebRootPath, "fonts", "arial.ttf");
            BaseFont bfArial = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            _fontStyle = new Font(bfArial, 8f, Font.NORMAL);
            _fontStyleBold = new Font(bfArial, 9f, Font.BOLD);

            _pdfTable = new PdfPTable(_maxColum) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            _memoryStream = new MemoryStream();

        }

        public byte[] Report<T>(List<T> isanpham, 
            List<string> colHeadings, string heading = "")
        {
            using (var document = new Document(PageSize.A4, 5f, 20f, 5f, 5f))
            {
                PdfWriter.GetInstance(document, _memoryStream);
                document.Open();

                float[] widths = { 20f, 100f };
                _pdfTable.SetWidths(widths);

                AddReportHeader(heading);
                AddEmptyRows(2);
                AddReportBody<T>(isanpham, colHeadings);

                _pdfTable.HeaderRows = 2;
                document.Add(_pdfTable);
                document.Close();
            }

            return _memoryStream.ToArray();
        }

        private void AddReportHeader(string heading)
        {
            var cell = new PdfPCell(new Phrase(heading, _fontStyleBold))
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

        private void AddReportBody<T>(List<T> sanphams, List<string> colHeadings)
        {
            AddTableHeaders(colHeadings);
            AddTableBody<T>(sanphams);
        }

        private void AddTableHeaders(List<string> colHeadings)
        {
            _pdfTable.AddCell(new PdfPCell(new Phrase(colHeadings[0], _fontStyleBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.Gray,
                ExtraParagraphSpace = 0
            });

            _pdfTable.AddCell(new PdfPCell(new Phrase(colHeadings[1], _fontStyleBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.Gray,
                ExtraParagraphSpace = 0
            });

            _pdfTable.CompleteRow();
        }

        private void AddTableBody<T>(List<T> sanphams)
        {
            if (sanphams != null && sanphams is List<ProductWithQuantity>)
            {
                foreach (var sp in sanphams as List<ProductWithQuantity>)
                {
                    _pdfTable.AddCell(new PdfPCell(new Phrase(sp.TenSanPham, _fontStyle))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        BackgroundColor = BaseColor.White,
                        ExtraParagraphSpace = 0
                    });

                    _pdfTable.AddCell(new PdfPCell(new Phrase(Convert.ToString(sp.SoLuong), _fontStyle))
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
}
