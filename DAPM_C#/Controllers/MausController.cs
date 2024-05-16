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
    public class MausController : Controller
    {
        private readonly QuanlyphanphoikhoYodyContext _context;

        public MausController(QuanlyphanphoikhoYodyContext context)
        {
            _context = context;
        }

        // GET: Maus
        public async Task<IActionResult> Index()
        {
            return View(await _context.Maus.ToListAsync());
        }

        // GET: Maus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mau = await _context.Maus
                .FirstOrDefaultAsync(m => m.MaMau == id);
            if (mau == null)
            {
                return NotFound();
            }

            return View(mau);
        }

        // GET: Maus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Maus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaMau,TenMau")] Mau mau)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mau);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mau);
        }

        // GET: Maus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mau = await _context.Maus.FindAsync(id);
            if (mau == null)
            {
                return NotFound();
            }
            return View(mau);
        }

        // POST: Maus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaMau,TenMau")] Mau mau)
        {
            if (id != mau.MaMau)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mau);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MauExists(mau.MaMau))
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
            return View(mau);
        }

        // GET: Maus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mau = await _context.Maus
                .FirstOrDefaultAsync(m => m.MaMau == id);
            if (mau == null)
            {
                return NotFound();
            }

            return View(mau);
        }

        // POST: Maus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mau = await _context.Maus.FindAsync(id);
            if (mau != null)
            {
                _context.Maus.Remove(mau);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MauExists(int id)
        {
            return _context.Maus.Any(e => e.MaMau == id);
        }
    }
}
