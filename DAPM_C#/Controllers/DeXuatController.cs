using DAPM_C_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DAPM_C_.Controllers
{
    public class DeXuatController : Controller
    {
        int madx;
        private readonly QuanlyphanphoikhoYodyContext _context;
        QuanlyphanphoikhoYodyContext data = new QuanlyphanphoikhoYodyContext();      

        public DeXuatController(QuanlyphanphoikhoYodyContext context)
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeXuat deXuat)
        {
            if (ModelState.IsValid)
            {
                deXuat.NgayDeXuat = DateTime.Now;
                _context.Add(deXuat);
                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                return RedirectToAction("Details", new { id = deXuat.MaDeXuat });

            }
            // int id = deXuat.MaDeXuat;
            // DeXuat dx = _context.DeXuats.Find(id);
            return View(deXuat);
        }
        public ActionResult XoaDeXuat(int? MaDeXuat)
        {
            DeXuat dx = data.DeXuats.Find(MaDeXuat);
            return View(dx);
        }
        [HttpPost]
        public ActionResult XoaDeXuat(int MaDeXuat)
        {
            DeXuat dx = data.DeXuats.Find(MaDeXuat);
            if (dx != null)
            {
                data.DeXuats.Remove(dx);
                data.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // trang xem chi tiet de xuat
        public async Task<IActionResult> ChiTietDeXuat(int? id)
        {
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
        // details trang them chi tiet san pham
        public async Task<IActionResult> Details(int? id)
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
        // thêm chi tiết sản phẩm
        public IActionResult AddChiTiet(int id)
        {
            if (id != 0)
            {
                madx = id;
                HttpContext.Session.SetInt32("truyenmdx", madx);

            }
            // lay ma de xuat duoc truyen lai 
            var mdx = HttpContext.Session.GetInt32("truyenlaiMDX");
            int maDX = Convert.ToInt32(mdx);
            // lay machitietsanpham tu session
            var maChiTietSanPham = HttpContext.Session.GetInt32("SelectedProductId");
            ViewBag.SelectedProductId = maChiTietSanPham;
            if (maDX == 0)
            {
                ViewBag.MaDeXuat = id;

            }
            else
            {
                ViewBag.MaDeXuat = maDX;

            }
            return View();
        }           

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChiTiet(ChiTietDeXuat chiTietDeXuat)
        {          
            var dxct = _context.ChiTietDeXuats.Where(c => c.MaDeXuat == chiTietDeXuat.MaDeXuat && c.MaChiTietSanPham == chiTietDeXuat.MaChiTietSanPham).FirstOrDefault();
            if (dxct != null)
            {               
                TempData["MessageError"] = "Thêm thất bại do bạn đã chọn sản phẩm đó!";             
                return RedirectToAction("Details", new { id = chiTietDeXuat.MaDeXuat });
            }
            else
            {
                TempData["MessageError"] = null;             
                _context.ChiTietDeXuats.Add(chiTietDeXuat);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = chiTietDeXuat.MaDeXuat });
            }
          
        }
        // Edit chi tiet cua de xuat
        public async Task<IActionResult> Edit(int? id, int? maDX)
        {
            if (id == null || maDX == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Edit(ChiTietDeXuat chiTietDeXuat)
        {
            if (chiTietDeXuat != null)
            {
                ChiTietDeXuat ctdx = data.ChiTietDeXuats.Find(chiTietDeXuat.MaDeXuat, chiTietDeXuat.MaChiTietSanPham);
                ctdx.LyDoDeXuat = chiTietDeXuat.LyDoDeXuat;
                ctdx.SoLuongDeXuat = chiTietDeXuat.SoLuongDeXuat;
                data.SaveChanges();
                return RedirectToAction("Details", new { id = chiTietDeXuat.MaDeXuat });
            }
            return View(chiTietDeXuat);         
        }    
        // xoa chi tiet de xuat
        public ActionResult Delete(int? MaDeXuat, int? MaChiTietSanPham)
        {
            ChiTietDeXuat ctdx = data.ChiTietDeXuats.Find(MaDeXuat, MaChiTietSanPham);
            return View(ctdx);
        }
        [HttpPost]
        public ActionResult Delete(int MaDeXuat, int? MaChiTietSanPham)
        {
            ChiTietDeXuat sc = data.ChiTietDeXuats.Find(MaDeXuat, MaChiTietSanPham);
            if (sc != null)
            {
                data.ChiTietDeXuats.Remove(sc);
                data.SaveChanges();
                return RedirectToAction("Details", new { id = sc.MaDeXuat });
            }
            return View(sc);
           
        }
        // xem chi tiết đề xuất
        public ActionResult DetailsCTDX(int MaDeXuat, int? MaChiTietSanPham)
        {
            ChiTietDeXuat ctdx = data.ChiTietDeXuats.Find(MaDeXuat, MaChiTietSanPham);
            return View(ctdx);
        }
    }
          
}
