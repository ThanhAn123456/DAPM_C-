using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAPM_C_.Models;
using X.PagedList;

namespace DAPM_C_.Controllers
{
    public class PhanQuyensController : Controller
    {
        private readonly QuanlyphanphoikhoYodyContext _context;
        private readonly IConfiguration _configuration;

        public PhanQuyensController(QuanlyphanphoikhoYodyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: PhanQuyens
        public async Task<IActionResult> Index(string searchdocs, int? pageNumber)
        {
			var MaQuyen = HttpContext.Session.GetString("MaQuyen"); ;
			if (MaQuyen == null)
			{
				return RedirectToAction("Login", "TaiKhoans");
			}
			if (MaQuyen != "1")
			{
				return Forbid();
			}

			IQueryable<PhanQuyen> quanlyphanphoikhoYodyContext = _context.PhanQuyens;

            if (!string.IsNullOrEmpty(searchdocs))
            {
                quanlyphanphoikhoYodyContext = quanlyphanphoikhoYodyContext.Where(p => p.TenQuyen.Contains(searchdocs));
            }

            quanlyphanphoikhoYodyContext = quanlyphanphoikhoYodyContext.OrderBy(p => p.MaQuyen);

            int pageSize = Convert.ToInt32(_configuration["PageList:PageSize"]);
            int currentPage = pageNumber ?? 1;

            ViewData["CurrentSearchDocs"] = searchdocs;

            return View(await quanlyphanphoikhoYodyContext.ToPagedListAsync(currentPage, pageSize));
        }

        // GET: PhanQuyens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
			var MaQuyen = HttpContext.Session.GetString("MaQuyen"); ;
			if (MaQuyen == null)
			{
				return RedirectToAction("Login", "TaiKhoans");
			}
			if (MaQuyen != "1")
			{
				return Forbid();
			}

			if (id == null)
            {
                return NotFound();
            }

            var phanQuyen = await _context.PhanQuyens
                .FirstOrDefaultAsync(m => m.MaQuyen == id);
            if (phanQuyen == null)
            {
                return NotFound();
            }

            return View(phanQuyen);
        }

        // GET: PhanQuyens/Create
        public IActionResult Create()
        {
			var MaQuyen = HttpContext.Session.GetString("MaQuyen"); ;
			if (MaQuyen == null)
			{
				return RedirectToAction("Login", "TaiKhoans");
			}
			if (MaQuyen != "1")
			{
				return Forbid();
			}

			return View();
        }

        // POST: PhanQuyens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaQuyen,TenQuyen")] PhanQuyen phanQuyen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phanQuyen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phanQuyen);
        }

        // GET: PhanQuyens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
			var MaQuyen = HttpContext.Session.GetString("MaQuyen"); ;
			if (MaQuyen == null)
			{
				return RedirectToAction("Login", "TaiKhoans");
			}
			if (MaQuyen != "1")
			{
				return Forbid();
			}

			if (id == null)
            {
                return NotFound();
            }

            var phanQuyen = await _context.PhanQuyens.FindAsync(id);
            if (phanQuyen == null)
            {
                return NotFound();
            }
            return View(phanQuyen);
        }

        // POST: PhanQuyens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaQuyen,TenQuyen")] PhanQuyen phanQuyen)
        {
            if (id != phanQuyen.MaQuyen)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phanQuyen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhanQuyenExists(phanQuyen.MaQuyen))
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
            return View(phanQuyen);
        }

        // GET: PhanQuyens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
			var MaQuyen = HttpContext.Session.GetString("MaQuyen"); ;
			if (MaQuyen == null)
			{
				return RedirectToAction("Login", "TaiKhoans");
			}
			if (MaQuyen != "1")
			{
				return Forbid();
			}

			if (id == null)
            {
                return NotFound();
            }

            var phanQuyen = await _context.PhanQuyens
                .FirstOrDefaultAsync(m => m.MaQuyen == id);
            if (phanQuyen == null)
            {
                return NotFound();
            }

            return View(phanQuyen);
        }

        // POST: PhanQuyens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phanQuyen = await _context.PhanQuyens.FindAsync(id);
            if (phanQuyen != null)
            {
                _context.PhanQuyens.Remove(phanQuyen);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhanQuyenExists(int id)
        {
            return _context.PhanQuyens.Any(e => e.MaQuyen == id);
        }
    }
}
