using DAPM_C_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DAPM_C_.Controllers
{
    public class DuyetDeXuatController : Controller
    {
        private readonly QuanlyphanphoikhoYodyContext _context;
        QuanlyphanphoikhoYodyContext data = new QuanlyphanphoikhoYodyContext();
        public DuyetDeXuatController(QuanlyphanphoikhoYodyContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var deXuats = await _context.DeXuats
             .Include(dx => dx.MaCuaHangNavigation)
             .OrderByDescending(dx => dx.NgayDeXuat) // Sắp xếp theo thời gian đề xuất giảm dần
             .ToListAsync();
            return View(deXuats);
        }
        // xem chi tiet de xuat de duyet
        public async Task<IActionResult> XemChiTietDX(int? id)
        {
            if (TempData["MessageError"] != null)
            {
                ViewBag.errorMSG = TempData["MessageError"];
            }
            if (id == null)
            {
                return NotFound();
            }

            var deXuat = await _context.DeXuats
            .Include(d => d.ChiTietDeXuats)
                .ThenInclude(cd => cd.MaChiTietSanPhamNavigation)
                    .ThenInclude(cs => cs.MaSanPhamNavigation)
            .FirstOrDefaultAsync(m => m.MaDeXuat == id);
            if (deXuat == null)
            {
                return NotFound();
            }
            TempData["id"] = id;
            return View(deXuat);
        }
        // duyet de xuat
        // Duyet chi tiet cua de xuat
        public async Task<IActionResult> DuyetDeXuat(int? id, int? maDX)
        {
            if (id == null || maDX == null)
            {
                return NotFound();
            }
            ViewBag.TrangThaiOptions = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "CN", Text = "Duyệt" },
                new SelectListItem { Value = "KD", Text = "Không duyệt" }
            }, "Value", "Text");
            var chiTietDeXuat = await _context.ChiTietDeXuats
             .FirstOrDefaultAsync(m => m.MaDeXuat == maDX && m.MaChiTietSanPham == id);
            if (chiTietDeXuat == null)
            {
                return NotFound();
            }
            return View(chiTietDeXuat);
        }
        // Action để xử lý yêu cầu chỉnh sửa

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DuyetDeXuat(ChiTietDeXuat chiTietDeXuat)
        {
            if (chiTietDeXuat != null)
            {
                ChiTietDeXuat ctdx = data.ChiTietDeXuats.Find(chiTietDeXuat.MaDeXuat, chiTietDeXuat.MaChiTietSanPham);
                ctdx.LyDoDeXuat = chiTietDeXuat.LyDoDeXuat;
                ctdx.SoLuongDeXuat = chiTietDeXuat.SoLuongDeXuat;
                ctdx.SoLuongDuyet = chiTietDeXuat.SoLuongDuyet;
                ctdx.TrangThaiDeXuat = chiTietDeXuat.TrangThaiDeXuat;
                data.SaveChanges();
                return RedirectToAction("XemChiTietDX", new { id = chiTietDeXuat.MaDeXuat });
            }
            return View(chiTietDeXuat);
        }
        // cap nhat trang thai cua de xuat
        [HttpPost]
        public ActionResult CapNhatTrangThaiDX(int MaDeXuat)
        {
            DeXuat dx = data.DeXuats.Find(MaDeXuat);
            if (dx != null)
            {
                dx.TrangThai = "DaDuyet";
                data.SaveChanges();
            }         
            return RedirectToAction("Index");
        }
    }
}
