using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using day4.Models;



//Create , delete , edit , details --> products



namespace day4.Controllers
{
    public class productController : Controller
    {
        private readonly libContext _context;

        public productController(libContext context)
        {
            _context = context;
        }

        // GET: product
        public async Task<IActionResult> Index()
        {
            var libContext = _context.photos.Include(p => p.author).Include(p => p.cat);
            return View(await libContext.ToListAsync());
        }

        // GET: product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.photos == null)
            {
                return NotFound();
            }

            var photo = await _context.photos
                .Include(p => p.author)
                .Include(p => p.cat)
                .FirstOrDefaultAsync(m => m.id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // GET: product/Create

        public IActionResult create()
        {
            ViewData["author_id"] = new SelectList(_context.users, "id", "email");
             ViewData["cat_id"] = new SelectList(_context.catalogs, "id", "name");
           
                return View();
            
        }

        [HttpPost]
        public IActionResult create(photo p, IFormFile pho)
        {
            //upload file in my folder
            string path = $"wwwroot/pecture/{pho.FileName}";
            FileStream fs = new FileStream(path, FileMode.Create);
            pho.CopyTo(fs);

            p.img = $"/pecture/{pho.FileName}";
            p.date = DateTime.Now;
            //p.author_id = HttpContext.Session.GetInt32("userid");
            ViewData["author_id"] = new SelectList(_context.users, "id", "email", p.author_id);
             ViewData["cat_id"] = new SelectList(_context.catalogs, "id", "name", p.cat_id);

            //save path in db
            _context.photos.Add(p);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }




        ////public IActionResult Create()
        ////{
        ////    ViewData["author_id"] = new SelectList(_context.users, "id", "email");
        ////    ViewData["cat_id"] = new SelectList(_context.catalogs, "id", "id");
        ////    return View();
        ////}

        // POST: product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public async Task<IActionResult> Create([Bind("id,title,bref,price,img,date,author_id,cat_id")] photo photo , IFormFile pho)
        ////{
        ////    if (ModelState.IsValid)
        ////    {
        ////        _context.Add(photo);
        ////        await _context.SaveChangesAsync();
        ////        return RedirectToAction(nameof(Index));

        ////    }




        ////    ViewData["author_id"] = new SelectList(_context.users, "id", "email", photo.author_id);
        ////    ViewData["cat_id"] = new SelectList(_context.catalogs, "id", "id", photo.cat_id);
        ////    return View(photo);
        ////}




        // GET: product/Edit/5




        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.photos == null)
            {
                return NotFound();
            }

            var photo = await _context.photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            ViewData["author_id"] = new SelectList(_context.users, "id", "email", photo.author_id);
            ViewData["cat_id"] = new SelectList(_context.catalogs, "id", "name", photo.cat_id);
            return View(photo);
        }

        // POST: product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,title,bref,price,img,date,author_id,cat_id")] photo photo , IFormFile pho)
        {
            if (id != photo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string path = $"wwwroot/pecture/{pho.FileName}";
                    FileStream fs = new FileStream(path, FileMode.Create);
                    pho.CopyTo(fs);

                    photo.img = $"/pecture/{pho.FileName}";
                    
                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!photoExists(photo.id))
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
            ViewData["author_id"] = new SelectList(_context.users, "id", "email", photo.author_id);
            ViewData["cat_id"] = new SelectList(_context.catalogs, "id", "name", photo.cat_id);
            return View(photo);
        }





        // GET: product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.photos == null)
            {
                return NotFound();
            }

            var photo = await _context.photos
                .Include(p => p.author)
                .Include(p => p.cat)
                .FirstOrDefaultAsync(m => m.id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.photos == null)
            {
                return Problem("Entity set 'libContext.photos'  is null.");
            }
            var photo = await _context.photos.FindAsync(id);
            if (photo != null)
            {
                _context.photos.Remove(photo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool photoExists(int id)
        {
          return (_context.photos?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
