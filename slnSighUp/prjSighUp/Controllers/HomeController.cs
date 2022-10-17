using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using prjSighUp.Models;
using System.Web.Security;
using PagedList;

namespace prjSighUp.Controllers
{
    public class HomeController : Controller
    {
        SighUpEntities db = new SighUpEntities();
        // GET: Home
        public ActionResult Index()
        {
            var activities = db.TableMembers1092034
                .OrderBy(m => m.mId).ToList();

            return View(activities);
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string acc, string pw)
        {
            var temp = db.TableMembers1092034
                .Where(m => m.account == acc && m.password == pw)
                .FirstOrDefault();
            if(temp == null)
            {
                ViewBag.Message = "帳號或密碼錯誤，登入失敗";
                return View();
            }
            FormsAuthentication.RedirectFromLoginPage(acc, true);
            return RedirectToAction("Index","Member");
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(TableMembers1092034 memberIn)
        {
            //-----------------------------
            if (ModelState.IsValid == false)
            {
                return View();
            }
            //-----------------------------
            //判斷帳號密碼是否有註冊過
            var member = db.TableMembers1092034
                .Where(m => m.account == memberIn.account && m.password == memberIn.password)
                .FirstOrDefault();
            //若此帳密沒人註冊
            if(member==null)
            {
                db.TableMembers1092034.Add(memberIn);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            ViewBag.Err = "此信箱密碼已有人註冊，請重新註冊";
            return View();
        }
    }
}