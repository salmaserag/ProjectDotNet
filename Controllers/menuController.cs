using Microsoft.AspNetCore.Mvc;
using day4.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace day4.Controllers
{
    public class menuController : Controller
    {
        libContext li;
        public menuController(libContext li)
        {
            this.li = li;
        }

        public IActionResult allproduct()
        {
            List<photo> products = li.photos.Include(n => n.cat).Include(n=>n.author).OrderByDescending(n => n.date).ToList();

            return View(products);
        }

        public IActionResult nacklace()
        {
            List<photo> products = li.photos.Include(n => n.cat).Where(n=>n.cat.name == "nacklace").Include(n => n.author).OrderByDescending(n => n.date).ToList();
            return View(products);
        }

        public IActionResult ring()
        {
            List<photo> products = li.photos.Include(n => n.cat).Where(n => n.cat.name == "ring").Include(n => n.author).OrderByDescending(n => n.date).ToList();
            return View(products);
        }

        public IActionResult earring()
        {
            List<photo> products = li.photos.Include(n => n.cat).Where(n => n.cat.name == "earring").Include(n => n.author).OrderByDescending(n => n.date).ToList();
            return View(products);
        }

        public IActionResult anklet()
        {
            List<photo> products = li.photos.Include(n => n.cat).Where(n => n.cat.name == "anklet").Include(n => n.author).OrderByDescending(n => n.date).ToList();
            return View(products);
        }

        public IActionResult watch()
        {
            List<photo> products = li.photos.Include(n => n.cat).Where(n => n.cat.name == "watch").Include(n => n.author).OrderByDescending(n => n.date).ToList();
            return View(products);
        }

        public IActionResult barrette()
        {
            List<photo> products = li.photos.Include(n => n.cat).Where(n => n.cat.name == "barrete").Include(n => n.author).OrderByDescending(n => n.date).ToList();
            return View(products);
        }


        public IActionResult bracelate()
        {
            List<photo> products = li.photos.Include(n => n.cat).Where(n => n.cat.name == "bracelet").Include(n => n.author).OrderByDescending(n => n.date).ToList();
            return View(products);
        }

    }
}
