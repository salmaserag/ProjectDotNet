using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using day4.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace day4.Controllers
{
    public class photoController : Controller
    {
        libContext li;

        public photoController(libContext li)
        {
            this.li = li;
        }

        public IActionResult create()
        {
            if (HttpContext.Session.GetInt32("userid") == null)
                return RedirectToAction("login", "user");
            else
            {
                List<catalog> ct = li.catalogs.ToList();
                ViewBag.cat = new SelectList(ct, "id", "name");

                return View();
            }
        }

        [HttpPost]
        public IActionResult create(photo p ,IFormFile pho )
        {
            //upload file in my folder
            string path = $"wwwroot/pecture/{pho.FileName}";
            FileStream fs = new FileStream(path, FileMode.Create);
            pho.CopyTo(fs);

            p.img = $"/pecture/{pho.FileName}";
            p.date = DateTime.Now;
            p.author_id = HttpContext.Session.GetInt32("userid");

            //save path in db
            li.photos.Add(p);
            li.SaveChanges();

            return RedirectToAction("displaypost");
        }

        public IActionResult displaypost()
        {
            int? id = HttpContext.Session.GetInt32("userid");
            List<photo> photos = li.photos.Include(n=>n.cat).Where(n=>n.author_id==id).OrderByDescending(n=>n.date).ToList();
            return View(photos);
        }
    }
}
