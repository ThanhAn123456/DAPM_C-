using DAPM_C_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DAPM_C_.Controllers
{
    public class DeXuatController : Controller
    {
        int madx;
        private readonly IConfiguration _configuration;
        private readonly QuanlyphanphoikhoYodyContext _context;
        QuanlyphanphoikhoYodyContext data = new QuanlyphanphoikhoYodyContext();      

        public DeXuatController(QuanlyphanphoikhoYodyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
       
        public async Task<IActionResult> Index(string searchdocs, string MaCuaHang, string TrangThai, int? pageNumber)
        {
			var MaQuyen = HttpContext.Session.GetString("MaQuyen");
			if (MaQuyen == null)
			{
				return RedirectToAction("Login", "TaiKhoans");
			}
			if (MaQuyen != "1" && MaQuyen != "2")
			{
				return Forbid();
			}

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
            ViewBag.ListCuaHang = new SelectList(_context.CuaHangs.ToList(), "MaCuaHang", "TenCuahang");
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
        public IActionResult Create()
        {
			var MaQuyen = HttpContext.Session.GetString("MaQuyen");         
            if (MaQuyen == null)
			{
				return RedirectToAction("Login", "TaiKhoans");
			}
			if (MaQuyen != "1" && MaQuyen != "2")
			{
				return Forbid();
			}

			ViewBag.CuaHangList = new SelectList(_context.CuaHangs.ToList(), "MaCuaHang", "TenCuahang");                              
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
            ViewBag.CuaHangList = new SelectList(_context.CuaHangs.ToList(), "MaCuaHang", "TenCuahang");
            return View(deXuat);
        }
        public ActionResult XoaDeXuat(int? MaDeXuat)
        {
			var MaQuyen = HttpContext.Session.GetString("MaQuyen"); ;
			if (MaQuyen == null)
			{
				return RedirectToAction("Login", "TaiKhoans");
			}
			if (MaQuyen != "1" && MaQuyen != "2")
			{
				return Forbid();
			}

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
			var MaQuyen = HttpContext.Session.GetString("MaQuyen");
			if (MaQuyen == null)
			{
				return RedirectToAction("Login", "TaiKhoans");
			}
			if (MaQuyen != "1" && MaQuyen != "2")
			{
				return Forbid();
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
        // details trang them chi tiet san pham
        public async Task<IActionResult> Details(int? id)
        {
			var MaQuyen = HttpContext.Session.GetString("MaQuyen"); 
			if (MaQuyen == null)
			{
				return RedirectToAction("Login", "TaiKhoans");
			}
			if (MaQuyen != "1" && MaQuyen != "2")
			{
				return Forbid();
			}

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
        // thêm chi tiết sản phẩm
        public IActionResult AddChiTiet(int id)
        {
			var MaQuyen = HttpContext.Session.GetString("MaQuyen"); 
			if (MaQuyen == null)
			{
				return RedirectToAction("Login", "TaiKhoans");
			}
			if (MaQuyen != "1" && MaQuyen != "2")
			{
				return Forbid();
			}

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
            var tenSanPham = (from ctsp in _context.ChiTietSanPhams
                              join sp in _context.Sanphams on ctsp.MaSanPham equals sp.MaSanPham
                              where ctsp.MaChiTietSanPham == maChiTietSanPham
                              select sp.TenSanPham).FirstOrDefault();
            if (tenSanPham != null)
            {
                ViewBag.nameSelectpr = tenSanPham;
            }
            else
            {
                ViewBag.nameSelectpr = null;
            }
        
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
                if (chiTietDeXuat.SoLuongDeXuat > 0 && chiTietDeXuat.MaChiTietSanPham != 0)
                {
                    TempData["MessageError"] = null;
                    _context.ChiTietDeXuats.Add(chiTietDeXuat);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = chiTietDeXuat.MaDeXuat });
                }
                else
                {
                    return View(chiTietDeXuat);

                }
            }
          
        }
        // Edit chi tiet cua de xuat
        public async Task<IActionResult> Edit(int? id, int? maDX)
        {
			var MaQuyen = HttpContext.Session.GetString("MaQuyen"); 
			if (MaQuyen == null)
			{
				return RedirectToAction("Login", "TaiKhoans");
			}
			if (MaQuyen != "1")
			{
				return Forbid();
			}

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
			var MaQuyen = HttpContext.Session.GetString("MaQuyen"); 
			if (MaQuyen == null)
			{
				return RedirectToAction("Login", "TaiKhoans");
			}
			if (MaQuyen != "1" && MaQuyen != "2")
			{
				return Forbid();
			}

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
        public async Task<IActionResult> DetailsCTDX(int MaDeXuat, int? MaChiTietSanPham)
        {
			var MaQuyen = HttpContext.Session.GetString("MaQuyen"); 
			if (MaQuyen == null)
			{
				return RedirectToAction("Login", "TaiKhoans");
			}
			if (MaQuyen != "1" && MaQuyen != "2")
			{
				return Forbid();
			}

			var ctdx = await _context.ChiTietDeXuats
                .Include(ct => ct.MaChiTietSanPhamNavigation)
                    .ThenInclude(cd => cd.MaSanPhamNavigation)
                .Include(ct => ct.MaChiTietSanPhamNavigation)
                    .ThenInclude(cd => cd.MaLoaiSanPhamNavigation)
                .Include(ct => ct.MaChiTietSanPhamNavigation)
                    .ThenInclude(cd => cd.MaSizeNavigation)
                .Include(ct => ct.MaChiTietSanPhamNavigation)
                    .ThenInclude(cd => cd.MaDoiTuongNavigation)
                .Include(ct => ct.MaChiTietSanPhamNavigation)
                    .ThenInclude(cd => cd.MaMauNavigation)
                        .FirstOrDefaultAsync(m => m.MaDeXuat == MaDeXuat && m.MaChiTietSanPham == MaChiTietSanPham);
            return View(ctdx);
        }
    }
          
}
