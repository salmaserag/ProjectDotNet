using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using day4.Models;



//Create , delete , edit , details --> catalog



namespace day4.Controllers
{
    public class catalogsController : Controller
    {
        private readonly libContext _context;

        public catalogsController(libContext context)
        {
            _context = context;
        }

        // GET: catalogs
        public async Task<IActionResult> Index()
        {
            var libContext = _context.catalogs.Include(c => c.admin);
            return View(await libContext.ToListAsync());
        }

        // GET: catalogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.catalogs == null)
            {
                return NotFound();
            }

            var catalog = await _context.catalogs
                .Include(c => c.admin)
                .FirstOrDefaultAsync(m => m.id == id);
            if (catalog == null)
            {
                return NotFound();
            }

            return View(catalog);
        }

        // GET: catalogs/Create
        public IActionResult Create()
        {
            ViewData["admin_id"] = new SelectList(_context.users, "id", "email");
            return View();
        }

        // POST: catalogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,des,admin_id")] catalog catalog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catalog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["admin_id"] = new SelectList(_context.users, "id", "email", catalog.admin_id);
            return View(catalog);
        }






        // GET: catalogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.catalogs == null)
            {
                return NotFound();
            }

            var catalog = await _context.catalogs.FindAsync(id);
            if (catalog == null)
            {
                return NotFound();
            }
            ViewData["admin_id"] = new SelectList(_context.users, "id", "email", catalog.admin_id);
            return View(catalog);
        }

        // POST: catalogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,des,admin_id")] catalog catalog)
        {
            if (id != catalog.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catalog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!catalogExists(catalog.id))
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
            ViewData["admin_id"] = new SelectList(_context.users, "id", "email", catalog.admin_id);
            return View(catalog);
        }

        // GET: catalogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.catalogs == null)
            {
                return NotFound();
            }

            var catalog = await _context.catalogs
                .Include(c => c.admin)
                .FirstOrDefaultAsync(m => m.id == id);
            if (catalog == null)
            {
                return NotFound();
            }

            return View(catalog);
        }

        // POST: catalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.catalogs == null)
            {
                return Problem("Entity set 'libContext.catalogs'  is null.");
            }
            var catalog = await _context.catalogs.FindAsync(id);
            if (catalog != null)
            {
                _context.catalogs.Remove(catalog);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool catalogExists(int id)
        {
          return (_context.catalogs?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
