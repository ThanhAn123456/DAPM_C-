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
    public class SizesController : Controller
    {
        private readonly QuanlyphanphoikhoYodyContext _context;
        private readonly IConfiguration _configuration;

        public SizesController(QuanlyphanphoikhoYodyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Sizes
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

			IQueryable<Size> quanlyphanphoikhoYodyContext = _context.Sizes;

            if (!string.IsNullOrEmpty(searchdocs))
            {
                quanlyphanphoikhoYodyContext = quanlyphanphoikhoYodyContext.Where(m => m.TenSize.Contains(searchdocs));
            }

            quanlyphanphoikhoYodyContext = quanlyphanphoikhoYodyContext.OrderBy(m => m.MaSize);

            int pageSize = Convert.ToInt32(_configuration["PageList:PageSize"]);
            int currentPage = pageNumber ?? 1;

            ViewData["CurrentSearchDocs"] = searchdocs;

            return View(await quanlyphanphoikhoYodyContext.ToPagedListAsync(currentPage, pageSize));
        }

        // GET: Sizes/Details/5
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

            var size = await _context.Sizes
                .FirstOrDefaultAsync(m => m.MaSize == id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        // GET: Sizes/Create
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

        // POST: Sizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSize,TenSize")] Size size)
        {
            if (ModelState.IsValid)
            {
                _context.Add(size);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(size);
        }

        // GET: Sizes/Edit/5
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

            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }
            return View(size);
        }

        // POST: Sizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaSize,TenSize")] Size size)
        {
            if (id != size.MaSize)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(size);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizeExists(size.MaSize))
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
            return View(size);
        }

        // GET: Sizes/Delete/5
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

            var size = await _context.Sizes
                .FirstOrDefaultAsync(m => m.MaSize == id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        // POST: Sizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size != null)
            {
                _context.Sizes.Remove(size);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SizeExists(int id)
        {
            return _context.Sizes.Any(e => e.MaSize == id);
        }
    }
}
