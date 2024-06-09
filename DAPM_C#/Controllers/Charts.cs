using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DAPM_C_.Controllers
{
    public class Charts : Controller
    {
        private readonly string connectionString = "Data Source=DESKTOP-0DSCUFU\\SQLEXPRESS;Initial Catalog=quanlyphanphoikhoYody;User ID=sa;Password=Tan0369463503@;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetDataCharts(int year)
        {
            var query = @"SELECT lsp.TenLoaiSanPham, SUM(ctdx.SoLuongDuyet) AS SoLuongSanPham 
                            FROM LoaiSanPham lsp
                            LEFT JOIN ChiTietSanPham ctsp ON lsp.MaLoaiSanPham = ctsp.MaLoaiSanPham
                            LEFT JOIN ChiTietDeXuat ctdx ON ctsp.MaChiTietSanPham = ctdx.MaChiTietSanPham
                            LEFT JOIN DeXuat dx ON ctdx.MaDeXuat = dx.MaDeXuat
                            WHERE YEAR(dx.NgayDeXuat) = @Nam
                            AND dx.TrangThai = N'Đã duyệt'
                            GROUP BY lsp.TenLoaiSanPham; ";

            var data = new List<object>();

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
                                data.Add(new
                                {
                                    TenLoaiSanPham = reader["TenLoaiSanPham"].ToString(),
                                    SoLuongSanPham = Convert.ToInt32(reader["SoLuongSanPham"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log it, rethrow it, return an error response, etc.)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Json(data);
        }
    }
}
