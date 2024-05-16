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
    public class DoiTuongsController : Controller
    {
        private readonly QuanlyphanphoikhoYodyContext _context;

        public DoiTuongsController(QuanlyphanphoikhoYodyContext context)
        {
            _context = context;
        }

        // GET: DoiTuongs
        public async Task<IActionResult> Index()
        {
            return View(await _context.DoiTuongs.ToListAsync());
        }

        // GET: DoiTuongs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doiTuong = await _context.DoiTuongs
                .FirstOrDefaultAsync(m => m.MaDoiTuong == id);
            if (doiTuong == null)
            {
                return NotFound();
            }

            return View(doiTuong);
        }

        // GET: DoiTuongs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DoiTuongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDoiTuong,TenDoiTuong")] DoiTuong doiTuong)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doiTuong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doiTuong);
        }

        // GET: DoiTuongs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doiTuong = await _context.DoiTuongs.FindAsync(id);
            if (doiTuong == null)
            {
                return NotFound();
            }
            return View(doiTuong);
        }

        // POST: DoiTuongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDoiTuong,TenDoiTuong")] DoiTuong doiTuong)
        {
            if (id != doiTuong.MaDoiTuong)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doiTuong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoiTuongExists(doiTuong.MaDoiTuong))
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
            return View(doiTuong);
        }

        // GET: DoiTuongs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doiTuong = await _context.DoiTuongs
                .FirstOrDefaultAsync(m => m.MaDoiTuong == id);
            if (doiTuong == null)
            {
                return NotFound();
            }

            return View(doiTuong);
        }

        // POST: DoiTuongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doiTuong = await _context.DoiTuongs.FindAsync(id);
            if (doiTuong != null)
            {
                _context.DoiTuongs.Remove(doiTuong);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoiTuongExists(int id)
        {
            return _context.DoiTuongs.Any(e => e.MaDoiTuong == id);
        }
    }
}
