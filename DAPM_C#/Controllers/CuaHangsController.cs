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
    public class CuaHangsController : Controller
    {
        private readonly QuanlyphanphoikhoYodyContext _context;

        public CuaHangsController(QuanlyphanphoikhoYodyContext context)
        {
            _context = context;
        }

        // GET: CuaHangs
        public async Task<IActionResult> Index()
        {
            return View(await _context.CuaHangs.ToListAsync());
        }

        // GET: CuaHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuaHang = await _context.CuaHangs
                .FirstOrDefaultAsync(m => m.MaCuaHang == id);
            if (cuaHang == null)
            {
                return NotFound();
            }

            return View(cuaHang);
        }

        // GET: CuaHangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CuaHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaCuaHang,TenCuahang,DiaChi")] CuaHang cuaHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cuaHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cuaHang);
        }

        // GET: CuaHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuaHang = await _context.CuaHangs.FindAsync(id);
            if (cuaHang == null)
            {
                return NotFound();
            }
            return View(cuaHang);
        }

        // POST: CuaHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: CuaHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuaHang = await _context.CuaHangs
                .FirstOrDefaultAsync(m => m.MaCuaHang == id);
            if (cuaHang == null)
            {
                return NotFound();
            }

            return View(cuaHang);
        }

        // POST: CuaHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cuaHang = await _context.CuaHangs.FindAsync(id);
            if (cuaHang != null)
            {
                _context.CuaHangs.Remove(cuaHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CuaHangExists(int id)
        {
            return _context.CuaHangs.Any(e => e.MaCuaHang == id);
        }
    }
}
