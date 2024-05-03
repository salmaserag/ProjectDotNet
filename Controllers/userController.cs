using Microsoft.AspNetCore.Mvc;
using day4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
namespace day4.Controllers
{
    public class userController : Controller
    {
        libContext li;
        public userController(libContext li)
        {
            this.li = li;
        }
        public ActionResult register()
        {

            return View();
        }
        [HttpPost]
        public ActionResult register(user u)
        {
            li.users.Add(u);
            li.SaveChanges();
            return RedirectToAction("login");
            
        }

        public ActionResult login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult login(string username , string password)
        {
            user u =li.users.Where(n=>n.username == username && n.password==password).FirstOrDefault();
            if(u.id == u.admin_id)
            {
                HttpContext.Session.SetInt32("userid", u.id);
                return RedirectToAction("admin");
            }

            else if (u != null)
            {
                //login
                HttpContext.Session.SetInt32("userid", u.id);
                return RedirectToAction("profile");
            }
            else
            {
                //not login
                ViewBag.status = "InCorrect UserName Or Password";
                return View();

            }
            return View();
        }

        public ActionResult profile()
        {
            int? id = HttpContext.Session.GetInt32("userid");
            if (id == null)
            {
                return RedirectToAction("login");
            }
            user u = li.users.Where(n => n.id==id).FirstOrDefault();
            return View(u);
        }




        public IActionResult updateProfile()
        {
            int? id = HttpContext.Session.GetInt32("userid");
            user s = li.users.Where(n => n.id == id).FirstOrDefault();
            return View(s);
        }
        [HttpPost]
        public IActionResult updateProfile(user s)
        {
            int? id = HttpContext.Session.GetInt32("userid");
            s.id = (int)id;
            s.password = li.users.Where(n => n.id == id).Select(n => n.password).FirstOrDefault();
            s.confirm_password = li.users.Where(n => n.id == id).Select(n => n.password).FirstOrDefault();
            li.Entry(s).State = EntityState.Modified;
            li.SaveChanges();
            return RedirectToAction("profile");
        }

        public IActionResult changePas()
        {
            int? id = HttpContext.Session.GetInt32("userid");
            user s = li.users.Where(n => n.id == id).FirstOrDefault();
            return View(s);
        }
        [HttpPost]
        public IActionResult changePas(string password, string confirm_password)
        {
            int? id = HttpContext.Session.GetInt32("userid");
            user s = li.users.Where(n => n.id == id).FirstOrDefault();
            s.password = password;
            s.confirm_password = confirm_password;
            li.SaveChanges();
            return RedirectToAction("profile");
        }
        public IActionResult changePhoto()
        {
            int? id = HttpContext.Session.GetInt32("userid");
            user s = li.users.Where(n => n.id == id).FirstOrDefault();
            return View(s);
        }

        [HttpPost]
        public IActionResult changePhoto(IFormFile img)
        {
            int? id = HttpContext.Session.GetInt32("userid");
            user s = li.users.Where(n => n.id == id).FirstOrDefault();
            string path = $"wwwroot/pecture/{img.FileName}";
            FileStream fs = new FileStream(path, FileMode.Create);
            img.CopyTo(fs);
            s.picture = $"/pecture/{img.FileName}";
            li.SaveChanges();
            return RedirectToAction("profile");
        }

        public ActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("login");
        }

        public IActionResult admin()
        {
            int? id = HttpContext.Session.GetInt32("userid");
            if (id == null)
            {
                return RedirectToAction("login");
            }
            user u = li.users.Where(n => n.id == id).FirstOrDefault();
            
            return View(u);
        }

        public IActionResult home()
        {

               return View();
        }

        

    }
}
