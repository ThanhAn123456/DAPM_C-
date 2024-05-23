using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAPM_C_.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DAPM_C_.Controllers
{
    public class ChiTietSanPhamsController : Controller
    {
        private readonly QuanlyphanphoikhoYodyContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ChiTietSanPhamsController(QuanlyphanphoikhoYodyContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: ChiTietSanPhams
        public async Task<IActionResult> Index()
        {
            var quanlyphanphoikhoYodyContext = _context.ChiTietSanPhams.Include(c => c.MaDoiTuongNavigation).Include(c => c.MaLoaiSanPhamNavigation).Include(c => c.MaMauNavigation).Include(c => c.MaSanPhamNavigation).Include(c => c.MaSizeNavigation);
            return View(await quanlyphanphoikhoYodyContext.ToListAsync());
        }

        public IActionResult SelectProduct()
        {
            var ListSanPham = _context.ChiTietSanPhams.Include(s => s.MaSanPhamNavigation).Include(s => s.MaSizeNavigation).Include(m => m.MaMauNavigation)
                                                        .Include(l => l.MaLoaiSanPhamNavigation).Include(d => d.MaDoiTuongNavigation).ToList();
            return View(ListSanPham);
        }
        [HttpPost]
        public IActionResult SelectProduct(int id)
        {
            var mdx = HttpContext.Session.GetInt32("truyenmdx");
            int ma = Convert.ToInt32(mdx);
            // Lưu mã sản phẩm được chọn vào Session
            HttpContext.Session.SetInt32("SelectedProductId", id);        
            HttpContext.Session.SetInt32("truyenlaiMDX", ma);
            return RedirectToAction("AddChiTiet", "DeXuat"); // Chuyển hướng về trang tạo đề xuất
        }
        // GET: ChiTietSanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietSanPham = await _context.ChiTietSanPhams
                .Include(c => c.MaDoiTuongNavigation)
                .Include(c => c.MaLoaiSanPhamNavigation)
                .Include(c => c.MaMauNavigation)
                .Include(c => c.MaSanPhamNavigation)
                .Include(c => c.MaSizeNavigation)
                .FirstOrDefaultAsync(m => m.MaChiTietSanPham == id);
            if (chiTietSanPham == null)
            {
                return NotFound();
            }

            return View(chiTietSanPham);
        }

        // GET: ChiTietSanPhams/Create
        public IActionResult Create()
        {
            ViewData["MaDoiTuong"] = new SelectList(_context.DoiTuongs, "MaDoiTuong", "TenDoiTuong");
            ViewData["MaLoaiSanPham"] = new SelectList(_context.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham");
            ViewData["MaMau"] = new SelectList(_context.Maus, "MaMau", "TenMau");
            ViewData["MaSanPham"] = new SelectList(_context.Sanphams, "MaSanPham", "TenSanPham");
            ViewData["MaSize"] = new SelectList(_context.Sizes, "MaSize", "TenSize");
            return View();
        }

        // POST: ChiTietSanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaChiTietSanPham,SoLuong,HinhAnh,Nsx,ChatLieu,GiaTien,MaSanPham,MaLoaiSanPham,MaMau,MaSize,MaDoiTuong")] ChiTietSanPham chiTietSanPham, IFormFile HinhAnhFile)
        {
            if (ModelState.IsValid)
            {
                if (HinhAnhFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "hinhanhs");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + HinhAnhFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await HinhAnhFile.CopyToAsync(fileStream);
                    }
                    chiTietSanPham.HinhAnh = uniqueFileName;
                }

                _context.Add(chiTietSanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDoiTuong"] = new SelectList(_context.DoiTuongs, "MaDoiTuong", "TenDoiTuong", chiTietSanPham.MaDoiTuong);
            ViewData["MaLoaiSanPham"] = new SelectList(_context.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham", chiTietSanPham.MaLoaiSanPham);
            ViewData["MaMau"] = new SelectList(_context.Maus, "MaMau", "TenMau", chiTietSanPham.MaMau);
            ViewData["MaSanPham"] = new SelectList(_context.Sanphams, "TenSanPham", "MaSanPham", chiTietSanPham.MaSanPham);
            ViewData["MaSize"] = new SelectList(_context.Sizes, "MaSize", "TenSize", chiTietSanPham.MaSize);
            return View(chiTietSanPham);
        }

        // GET: ChiTietSanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietSanPham = await _context.ChiTietSanPhams.FindAsync(id);
            if (chiTietSanPham == null)
            {
                return NotFound();
            }
            ViewData["MaDoiTuong"] = new SelectList(_context.DoiTuongs, "MaDoiTuong", "TenDoiTuong", chiTietSanPham.MaDoiTuong);
            ViewData["MaLoaiSanPham"] = new SelectList(_context.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham", chiTietSanPham.MaLoaiSanPham);
            ViewData["MaMau"] = new SelectList(_context.Maus, "MaMau", "TenMau", chiTietSanPham.MaMau);
            ViewData["MaSanPham"] = new SelectList(_context.Sanphams, "MaSanPham", "TenSanPham", chiTietSanPham.MaSanPham);
            ViewData["MaSize"] = new SelectList(_context.Sizes, "MaSize", "TenSize", chiTietSanPham.MaSize);
            return View(chiTietSanPham);
        }

        // POST: ChiTietSanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaChiTietSanPham,SoLuong,HinhAnh,Nsx,ChatLieu,GiaTien,MaSanPham,MaLoaiSanPham,MaMau,MaSize,MaDoiTuong")] ChiTietSanPham chiTietSanPham, IFormFile HinhAnhFile)
        {
            if (id != chiTietSanPham.MaChiTietSanPham)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (HinhAnhFile != null)
                    {
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "hinhanhs");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + HinhAnhFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhAnhFile.CopyToAsync(fileStream);
                        }

                        // Delete the old avatar file if it exists
                        if (!string.IsNullOrEmpty(chiTietSanPham.HinhAnh))
                        {
                            string oldFilePath = Path.Combine(uploadsFolder, chiTietSanPham.HinhAnh);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        chiTietSanPham.HinhAnh = uniqueFileName;
                    }

                    _context.Update(chiTietSanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietSanPhamExists(chiTietSanPham.MaChiTietSanPham))
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
            ViewData["MaDoiTuong"] = new SelectList(_context.DoiTuongs, "MaDoiTuong", "TenDoiTuong", chiTietSanPham.MaDoiTuong);
            ViewData["MaLoaiSanPham"] = new SelectList(_context.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham", chiTietSanPham.MaLoaiSanPham);
            ViewData["MaMau"] = new SelectList(_context.Maus, "MaMau", "TenMau", chiTietSanPham.MaMau);
            ViewData["MaSanPham"] = new SelectList(_context.Sanphams, "MaSanPham", "TenSanPham", chiTietSanPham.MaSanPham);
            ViewData["MaSize"] = new SelectList(_context.Sizes, "MaSize", "TenSize", chiTietSanPham.MaSize);
            return View(chiTietSanPham);
        }

        // GET: ChiTietSanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietSanPham = await _context.ChiTietSanPhams
                .Include(c => c.MaDoiTuongNavigation)
                .Include(c => c.MaLoaiSanPhamNavigation)
                .Include(c => c.MaMauNavigation)
                .Include(c => c.MaSanPhamNavigation)
                .Include(c => c.MaSizeNavigation)
                .FirstOrDefaultAsync(m => m.MaChiTietSanPham == id);
            if (chiTietSanPham == null)
            {
                return NotFound();
            }

            return View(chiTietSanPham);
        }

        // POST: ChiTietSanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chiTietSanPham = await _context.ChiTietSanPhams.FindAsync(id);
            if (chiTietSanPham != null)
            {
                // Xóa ảnh trong thư mục avatars
                if (!string.IsNullOrEmpty(chiTietSanPham.HinhAnh))
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "hinhanhs");
                    string filePath = Path.Combine(uploadsFolder, chiTietSanPham.HinhAnh);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.ChiTietSanPhams.Remove(chiTietSanPham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChiTietSanPhamExists(int id)
        {
            return _context.ChiTietSanPhams.Any(e => e.MaChiTietSanPham == id);
        }
    }
}
