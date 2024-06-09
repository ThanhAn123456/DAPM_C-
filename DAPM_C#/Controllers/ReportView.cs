using DAPM_C_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace DAPM_C_.Controllers
{
    public class ReportView : Controller
    {
        private readonly QuanlyphanphoikhoYodyContext _context;
        private readonly IWebHostEnvironment _oHostEnvironment;
        private readonly string connectionString = "Data Source=DESKTOP-0DSCUFU\\SQLEXPRESS;Initial Catalog=quanlyphanphoikhoYody;User ID=sa;Password=Tan0369463503@;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public ReportView(QuanlyphanphoikhoYodyContext context, IWebHostEnvironment oHostEnvironment)
        {
            _context = context;
            _oHostEnvironment = oHostEnvironment;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PrintPDF(int year = 2024)
        {
            try
            {
                //List<Sanpham> isanphams = GenerateSanphams();
                //var isanphams = await _context.Sanphams.ToListAsync();
                var isanphams = await GetDataCharts(year);
                PDF rpt = new PDF(_oHostEnvironment);
                byte[] pdfData = rpt.Report<ProductWithQuantity>(
                    isanphams, new List<string>() { "Loại sản phẩm", "Số Lượng" }, "Thống kê theo loại sản phẩm năm 2024");
                Console.OutputEncoding = Encoding.UTF8;
                Console.InputEncoding = Encoding.UTF8;
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


        public async Task<List<ProductWithQuantity>> GetDataCharts(int year)
        {
            var query = @"SELECT lsp.TenLoaiSanPham, SUM(ctdx.SoLuongDuyet) AS SoLuongSanPham 
                            FROM LoaiSanPham lsp
                            LEFT JOIN ChiTietSanPham ctsp ON lsp.MaLoaiSanPham = ctsp.MaLoaiSanPham
                            LEFT JOIN ChiTietDeXuat ctdx ON ctsp.MaChiTietSanPham = ctdx.MaChiTietSanPham
                            LEFT JOIN DeXuat dx ON ctdx.MaDeXuat = dx.MaDeXuat
                            WHERE YEAR(dx.NgayDeXuat) = @Nam
                            AND dx.TrangThai = N'Đã duyệt'
                            GROUP BY lsp.TenLoaiSanPham; ";

            var data = new List<ProductWithQuantity>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nam", year);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                data.Add(new ProductWithQuantity()
                                {
                                    TenSanPham = reader["TenLoaiSanPham"].ToString(),
                                    SoLuong = Convert.ToInt32(reader["SoLuongSanPham"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return data;
            }
            return data;
        }
    }
}
