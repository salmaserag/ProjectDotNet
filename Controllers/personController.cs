using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using day4.Models;

namespace day4.Controllers
{
    public class personController : Controller
    {
        private readonly libContext _context;

        public personController(libContext context)
        {
            _context = context;
        }

        // GET: person
        public async Task<IActionResult> Index()
        {
            var libContext = _context.users.Include(u => u.admin);
            return View(await libContext.ToListAsync());
        }

        // GET: person/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.users == null)
            {
                return NotFound();
            }

            var user = await _context.users
                .Include(u => u.admin)
                .FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: person/Create
        public IActionResult Create()
        {
            ViewData["admin_id"] = new SelectList(_context.users, "id", "email");
            return View();
        }


        [HttpPost]
        public IActionResult create(user p, IFormFile pho)
        {
            //upload file in my folder
            string path = $"wwwroot/pecture/{pho.FileName}";
            FileStream fs = new FileStream(path, FileMode.Create);
            pho.CopyTo(fs);

            p.picture = $"/pecture/{pho.FileName}";
            
            //p.author_id = HttpContext.Session.GetInt32("userid");
            ViewData["admin_id"] = new SelectList(_context.users, "id", "email", p.admin_id);

            //save path in db
            _context.users.Add(p);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }




        // POST: person/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("id,name,username,email,password,picture,age,admin_id")] user user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(user);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["admin_id"] = new SelectList(_context.users, "id", "email", user.admin_id);
        //    return View(user);
        //}




        // GET: person/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.users == null)
            {
                return NotFound();
            }

            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["admin_id"] = new SelectList(_context.users, "id", "email", user.admin_id);
            return View(user);
        }

        // POST: person/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,username,email,password,picture,age,admin_id")] user user , IFormFile pho)
        {
            if (id != user.id)
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

                    user.picture = $"/pecture/{pho.FileName}";
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!userExists(user.id))
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
            ViewData["admin_id"] = new SelectList(_context.users, "id", "email", user.admin_id);
            return View(user);
        }

        // GET: person/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.users == null)
            {
                return NotFound();
            }

            var user = await _context.users
                .Include(u => u.admin)
                .FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.users == null)
            {
                return Problem("Entity set 'libContext.users'  is null.");
            }
            var user = await _context.users.FindAsync(id);
            if (user != null)
            {
                _context.users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool userExists(int id)
        {
          return _context.users.Any(e => e.id == id);
        }
    }
}
