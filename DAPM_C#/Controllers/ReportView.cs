using DAPM_C_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAPM_C_.Controllers
{
    public class ReportView : Controller
    {
        private readonly QuanlyphanphoikhoYodyContext _context;
        private readonly IWebHostEnvironment _oHostEnvironment;

        public ReportView(QuanlyphanphoikhoYodyContext context, IWebHostEnvironment oHostEnvironment)
        {
            _context = context;
            _oHostEnvironment = oHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PrintPDF(int param)
        {
            try
            {
                //List<Sanpham> isanphams = GenerateSanphams();
                var isanphams = await _context.Sanphams.ToListAsync();
                ReportSP rpt = new ReportSP(_oHostEnvironment);
                byte[] pdfData = rpt.Report(isanphams);

                return File(pdfData, "application/pdf");
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here
                return StatusCode(500, "Internal server error");
            }
        }

        private List<Sanpham> GenerateSanphams()
        {
            List<Sanpham> isanphams = new List<Sanpham>();
            for (int i = 1; i <= 10; i++)
            {
                Sanpham isanpham = new Sanpham();
                isanpham.MaSanPham = i;
                isanpham.TenSanPham = "Products" + i;
                isanphams.Add(isanpham);
            }
            return isanphams;
        }
    }
}
