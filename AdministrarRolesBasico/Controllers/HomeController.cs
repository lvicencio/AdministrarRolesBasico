using AdministrarRolesBasico.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AdministrarRolesBasico.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles="Admin")]
        public ActionResult Administrador()
        {
            return View();
        }
        [Authorize(Roles="Editor")]
        public ActionResult Editor()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuarios model, string returnUrl)
        {

            netRolEntities db = new netRolEntities();

            var dataItem = db.Usuarios.Where(x => x.Username == model.Username && x.Password == model.Password).FirstOrDefault();

            if (dataItem != null)
            {
                FormsAuthentication.SetAuthCookie(dataItem.Username, false);
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//")
                    && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    switch (dataItem.Role)
                    {
                       case "Admin":
                            return RedirectToAction("Administrador");
                            break;
                        case "Editor":
                            return RedirectToAction("Editor");
                            break;
                       default:
                            return RedirectToAction("Index");
                            break;
                    }


                   return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "Usuario/Contraseña no es Valido");
                return View();
            }
             
        }

        [Authorize]
        public  ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}