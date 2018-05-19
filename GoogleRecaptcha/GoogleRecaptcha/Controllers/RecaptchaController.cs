using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoogleRecaptcha.Models;
using Newtonsoft.Json.Linq;

namespace GoogleRecaptcha.Controllers
{
    public class RecaptchaController : Controller
    {
        // GET: Recaptcha
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FormPost(FormCollection Form)
        {
            string token = Form["g-recaptcha-response"];
            if (!ValidRecaptcha(token))
            {
                return View();//fail
            }
            return RedirectToAction("You RedirectAction");
        }

        [HttpPost]
        public ActionResult AjaxPost(FormCollection form)
        {
            AjaxResult result = new AjaxResult();
            string token = form["g-recaptcha-response"];
            if (!ValidRecaptcha(token))
            {
                result.Result = false;
                result.Message = "google recaptcha驗證失敗";
            }
            result.Result = true;
            result.Message = "success";

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public bool ValidRecaptcha(string token)
        {
            var client = new System.Net.WebClient();
            string Secret_key = "6Le_KloUAAAAAIuWk37KDt4h7HjiC-YNQtc6GxSr";
            var googleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", Secret_key, token));

            dynamic Json = JObject.Parse(googleReply);
            var success = Json.success;
            return success == "true" ? true : false;
        }
    }
}