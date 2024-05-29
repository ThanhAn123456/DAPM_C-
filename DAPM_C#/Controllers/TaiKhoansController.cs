using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAPM_C_.Models;

namespace DAPM_C_.Controllers
{
    public class TaiKhoansController : Controller
    {
        private readonly QuanlyphanphoikhoYodyContext _context;

        public TaiKhoansController(QuanlyphanphoikhoYodyContext context)
        {
            _context = context;
        }

        // GET: TaiKhoans
        public async Task<IActionResult> Index()
        {
            var quanlyphanphoikhoYodyContext = _context.TaiKhoans.Include(t => t.MaCuaHangNavigation).Include(t => t.MaQuyenNavigation);
            return View(await quanlyphanphoikhoYodyContext.ToListAsync());
        }

        // GET: TaiKhoans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans
                .Include(t => t.MaCuaHangNavigation)
                .Include(t => t.MaQuyenNavigation)
                .FirstOrDefaultAsync(m => m.MaTk == id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

        // GET: TaiKhoans/Create
        public IActionResult Create()
        {
            ViewData["MaCuaHang"] = new SelectList(_context.CuaHangs, "MaCuaHang", "MaCuaHang");
            ViewData["MaQuyen"] = new SelectList(_context.PhanQuyens, "MaQuyen", "MaQuyen");
            return View();
        }

        // POST: TaiKhoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTk,MaQuyen,MaCuaHang,HoTen,Cccd,HinhAnh,Sdt,NgaySinh,Gioitinh,DiaChi,Email,Matkhau")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taiKhoan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaCuaHang"] = new SelectList(_context.CuaHangs, "MaCuaHang", "MaCuaHang", taiKhoan.MaCuaHang);
            ViewData["MaQuyen"] = new SelectList(_context.PhanQuyens, "MaQuyen", "MaQuyen", taiKhoan.MaQuyen);
            return View(taiKhoan);
        }

        // GET: TaiKhoans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }
            ViewData["MaCuaHang"] = new SelectList(_context.CuaHangs, "MaCuaHang", "MaCuaHang", taiKhoan.MaCuaHang);
            ViewData["MaQuyen"] = new SelectList(_context.PhanQuyens, "MaQuyen", "MaQuyen", taiKhoan.MaQuyen);
            return View(taiKhoan);
        }

        // POST: TaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTk,MaQuyen,MaCuaHang,HoTen,Cccd,HinhAnh,Sdt,NgaySinh,Gioitinh,DiaChi,Email,Matkhau")] TaiKhoan taiKhoan)
        {
            if (id != taiKhoan.MaTk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taiKhoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoanExists(taiKhoan.MaTk))
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
            ViewData["MaCuaHang"] = new SelectList(_context.CuaHangs, "MaCuaHang", "MaCuaHang", taiKhoan.MaCuaHang);
            ViewData["MaQuyen"] = new SelectList(_context.PhanQuyens, "MaQuyen", "MaQuyen", taiKhoan.MaQuyen);
            return View(taiKhoan);
        }

        // GET: TaiKhoans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans
                .Include(t => t.MaCuaHangNavigation)
                .Include(t => t.MaQuyenNavigation)
                .FirstOrDefaultAsync(m => m.MaTk == id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

        // POST: TaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan != null)
            {
                _context.TaiKhoans.Remove(taiKhoan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaiKhoanExists(int id)
        {
            return _context.TaiKhoans.Any(e => e.MaTk == id);
        }
    }
}
