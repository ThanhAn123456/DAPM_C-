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
    public class LoaiSanPhamsController : Controller
    {
        private readonly QuanlyphanphoikhoYodyContext _context;

        public LoaiSanPhamsController(QuanlyphanphoikhoYodyContext context)
        {
            _context = context;
        }

        // GET: LoaiSanPhams
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoaiSanPhams.ToListAsync());
        }

        // GET: LoaiSanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiSanPham = await _context.LoaiSanPhams
                .FirstOrDefaultAsync(m => m.MaLoaiSanPham == id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }

            return View(loaiSanPham);
        }

        // GET: LoaiSanPhams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoaiSanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLoaiSanPham,TenLoaiSanPham")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiSanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiSanPham);
        }

        // GET: LoaiSanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }
            return View(loaiSanPham);
        }

        // POST: LoaiSanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLoaiSanPham,TenLoaiSanPham")] LoaiSanPham loaiSanPham)
        {
            if (id != loaiSanPham.MaLoaiSanPham)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiSanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiSanPhamExists(loaiSanPham.MaLoaiSanPham))
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
            return View(loaiSanPham);
        }

        // GET: LoaiSanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiSanPham = await _context.LoaiSanPhams
                .FirstOrDefaultAsync(m => m.MaLoaiSanPham == id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }

            return View(loaiSanPham);
        }

        // POST: LoaiSanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(id);
            if (loaiSanPham != null)
            {
                _context.LoaiSanPhams.Remove(loaiSanPham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiSanPhamExists(int id)
        {
            return _context.LoaiSanPhams.Any(e => e.MaLoaiSanPham == id);
        }
    }
}
