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
    [Authorize]
    public class MemberController : Controller
    {
        SighUpEntities db = new SighUpEntities();
        // GET: Member
        public ActionResult Index()
        {
            var activities = db.TableMembers1092034
                .OrderBy(m => m.mId).ToList();

            return View("../Home/Index", "_LayoutMember", activities);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        int pageSize = 7;
        public ActionResult List(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            var products = db.TableActivitys1092034
                .OrderBy(m => m.aNo).ToList();
            var result = products.ToPagedList(currentPage, pageSize);
            return View("../Member/List", "_LayoutMember", result);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TableActivitys1092034 act)
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }
            var creact = db.TableActivitys1092034
                .Where(m => m.aNo == act.aNo || m.aName == act.aName)
                .FirstOrDefault();
            //此活動沒人註冊
            if(creact==null)
            {
                db.TableActivitys1092034.Add(act);
                db.SaveChanges();
                return RedirectToAction("List","Member");
            }
            ViewBag.Error = "此活動已有人註冊，請重新註冊";
            return View(act);
        }

        public ActionResult Delete(string aNo)
        {
            var temp = db.TableActivitys1092034
                .Where(m => m.aNo == aNo)
                .FirstOrDefault();
            db.TableActivitys1092034.Remove(temp);
            db.SaveChanges();
            return RedirectToAction("List");
        }
    }
}