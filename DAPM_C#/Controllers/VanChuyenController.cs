using DAPM_C_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DAPM_C_.Controllers
{
    public class VanChuyenController : Controller
    {
        private readonly QuanlyphanphoikhoYodyContext _context;
        private readonly IConfiguration _configuration;
        QuanlyphanphoikhoYodyContext data = new QuanlyphanphoikhoYodyContext();
        public VanChuyenController(QuanlyphanphoikhoYodyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string searchdocs, string MaCuaHang, string TrangThai, int? pageNumber)
        {
            IQueryable<DeXuat> quanlyphanphoikhoYodyContext = _context.DeXuats.Include(c => c.ChiTietDeXuats).Include(c => c.MaCuaHangNavigation).Where(c => c.TrangThai !="Chờ duyệt");
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
            // tao viewbag truyèn đói số 
            ViewBag.ListCuaHang = new SelectList(_context.CuaHangs.ToList(), "MaCuaHang", "TenCuahang");
            ViewBag.ListTrangThai = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "Đã duyệt", Text = "Đã duyệt" },
                new SelectListItem { Value = "Xác nhận VC", Text = "Xác nhận vận chuyển" },
                new SelectListItem { Value = "Đã vận chuyển", Text = "Đã vận chuyển" },
            }, "Value", "Text");

            int pageSize = Convert.ToInt32(_configuration["PageList:PageSize"]);
            int currentPage = pageNumber ?? 1;

            ViewData["CurrentSearchDocs"] = searchdocs;
            ViewData["CurrentCuaHang"] = MaCuaHang;
            ViewData["CurrentTrangThai"] = TrangThai;
            // var listDX = _context.DeXuats.Include(k => k.MaCuaHangNavigation).Where(k => k.TrangThai != "Chờ duyệt").ToList();
            return View(await quanlyphanphoikhoYodyContext.ToPagedListAsync(currentPage, pageSize));
        }
        public async Task<IActionResult> XemChiTietDX(int? id)
        {          
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
            DeXuat D =  data.DeXuats.Find(id);
            if (D != null)
            {
                if (D.TrangThai.Equals("Đã duyệt", StringComparison.OrdinalIgnoreCase))
                {
                    ViewBag.ttvc = "CD";
                }
                else
                {
                    ViewBag.ttvc = null;

                }
            }
           
            return View(deXuat);
        }
        // cap nhat trang thai cua de xuat
        [HttpPost]
        public ActionResult CapNhatTrangThaiDX(int MaDeXuat, string TrangThaiVanChuyen)
        {
            DeXuat dx = data.DeXuats.Find(MaDeXuat);
            if (dx != null)
            {
                dx.TrangThai = TrangThaiVanChuyen;
                data.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
