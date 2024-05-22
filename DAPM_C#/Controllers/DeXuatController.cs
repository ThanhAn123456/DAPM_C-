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
            var DeXuats = data.DeXuats.Include(x => x.MaCuaHangNavigation).ToList();
            return View(DeXuats);
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chitietdexuat = await _context.ChiTietDeXuats.FindAsync(id);
            if (chitietdexuat == null)
            {
                return NotFound();
            }
            return View(chitietdexuat);
        }

        // POST: CuaHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaCuaHang,TenCuahang,DiaChi")] CuaHang cuaHang)
        {
            if (id != cuaHang.MaCuaHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cuaHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuaHangExists(cuaHang.MaCuaHang))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cuaHang);
        }
        */
    }
}
