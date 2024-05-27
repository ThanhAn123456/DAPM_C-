using DAPM_C_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using X.PagedList;

namespace DAPM_C_.Controllers
{
    public class DuyetDeXuatController : Controller
    {
        private readonly QuanlyphanphoikhoYodyContext _context;
        private readonly IConfiguration _configuration;
        QuanlyphanphoikhoYodyContext data = new QuanlyphanphoikhoYodyContext();
        public DuyetDeXuatController(QuanlyphanphoikhoYodyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string searchdocs,string MaCuaHang, string TrangThai, int? pageNumber)
        {
            IQueryable<DeXuat> quanlyphanphoikhoYodyContext = _context.DeXuats.Include(c => c.ChiTietDeXuats).Include(c => c.MaCuaHangNavigation);
            if (!string.IsNullOrEmpty(searchdocs))
            {
                quanlyphanphoikhoYodyContext = quanlyphanphoikhoYodyContext.Where(q => q.Tieude.Contains(searchdocs) || q.MaCuaHangNavigation.TenCuahang.Contains(searchdocs) || q.TrangThai.Contains(searchdocs));
            }
            if (!string.IsNullOrEmpty(MaCuaHang))
            {
                quanlyphanphoikhoYodyContext = quanlyphanphoikhoYodyContext.Where(q => q.MaCuaHang == Convert.ToInt32(MaCuaHang));
            }
            if (!string.IsNullOrEmpty(TrangThai))
            {
                quanlyphanphoikhoYodyContext = quanlyphanphoikhoYodyContext.Where(q => q.TrangThai.Contains(TrangThai));
            }
            // Sắp xếp list theo thời gian mới nhất 
            quanlyphanphoikhoYodyContext = quanlyphanphoikhoYodyContext.OrderByDescending(q => q.NgayDeXuat);
            // view bag truyền list cửa hàng và trạng thái đề xuất
            ViewBag.ListCuaHang = new SelectList(_context.CuaHangs.ToList(),"MaCuaHang","TenCuahang");
            ViewBag.ListTrangThai = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "Chờ duyệt", Text = "Chờ duyệt" },
                new SelectListItem { Value = "Đã duyệt", Text = "Đã duyệt" },
                new SelectListItem { Value = "Xác nhận VC", Text = "Xác nhận vận chuyển" },
                new SelectListItem { Value = "Đã vận chuyển", Text = "Đã vận chuyển" },
            }, "Value", "Text");
            int pageSize = Convert.ToInt32(_configuration["PageList:PageSize"]);
            int currentPage = pageNumber ?? 1;
            ViewData["CurrentSearchDocs"] = searchdocs;
            ViewData["CurrentCuaHang"] = MaCuaHang;
            ViewData["CurrentTrangThai"] = TrangThai;
            //return View(deXuats);
            return View(await quanlyphanphoikhoYodyContext.ToPagedListAsync(currentPage, pageSize));
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
            .Include(d => d.MaCuaHangNavigation)
            .Include(d => d.ChiTietDeXuats)
              .ThenInclude(cd => cd.MaChiTietSanPhamNavigation)
                  .ThenInclude(cs => cs.MaSanPhamNavigation)
               .Include(d => d.ChiTietDeXuats)
                   .ThenInclude(cd => cd.MaChiTietSanPhamNavigation)
                      .ThenInclude(cs => cs.MaSizeNavigation)
              .Include(d => d.ChiTietDeXuats)
                   .ThenInclude(cd => cd.MaChiTietSanPhamNavigation)
                      .ThenInclude(cs => cs.MaLoaiSanPhamNavigation)
              .Include(d => d.ChiTietDeXuats)
                   .ThenInclude(cd => cd.MaChiTietSanPhamNavigation)
                      .ThenInclude(cs => cs.MaMauNavigation)
              .Include(d => d.ChiTietDeXuats)
                   .ThenInclude(cd => cd.MaChiTietSanPhamNavigation)
                      .ThenInclude(cs => cs.MaDoiTuongNavigation)
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
                dx.TrangThai = "Đã duyệt";
                data.SaveChanges();
            }         
            return RedirectToAction("Index");
        }
    }
}
