using MvcApplication2.Controllers;
using smart_Taxi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace smart_Taxi.Controllers
{
    public class HomeController : Controller
    {
        // public static SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionDB"].ConnectionString);
        //
        // GET: /Home/
        public static Users CurrentUser = null;

        public ActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                if (Request.Cookies["sm_ds_a_cookie2"] != null)
                {
                    HttpCookie cookie = Request.Cookies["sm_ds_a_cookie2"];
                    int id = Convert.ToInt32(cookie["Id"]);
                    if (Repository.fBrowserIsMobile())
                    {
                        return RedirectToAction("index", "driver", new { id = id });
                    }

                }
                else
                {
                    return RedirectToAction("OnlineMap");
                }
            }

            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            //Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            //Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (!u.Contains("Windows"))
            {
                // ViewBag.d = u;
                return View();
            }
            else
            {
                // ViewBag.d = u;
                //return View();
                ViewBag.BenefitText = Repository.GetBenefit(1);
                return View("Index_web");
            }


        }


        public ActionResult Login()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (Request.Cookies["sm_ds_a_cookie1"] != null)
                    {
                        return RedirectToAction("OnlineMap");
                    }
                    if (Request.Cookies["sm_ds_a_cookie2"] != null)
                    {
                        HttpCookie cookie = Request.Cookies["sm_ds_a_cookie2"];
                        int id = Convert.ToInt32(cookie["Id"]);
                        return RedirectToAction("index", "driver", new { id = id });

                    }
                }
            }
            catch
            {
            }
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            if (!u.Contains("Windows"))
            {
                // ViewBag.d = u;
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }

        }


        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                CurrentUser = Repository.GetUserLogin(model.Phone, model.Password);
                FormsAuthentication.SetAuthCookie(model.Phone, false);
                HttpCookie myCookie = null;

                if (CurrentUser.Type == "1")
                {

                    myCookie = new HttpCookie("sm_ds_a_cookie1");
                    myCookie["Id"] = CurrentUser.Id.ToString();
                    myCookie["Phone"] = CurrentUser.Phone;
                    myCookie["Password"] = CurrentUser.Password;
                    myCookie.Expires = DateTime.Now.AddDays(365);
                    HttpContext.Response.Cookies.Add(myCookie);
                    if (CurrentUser.Id != 0)
                    {
                        return RedirectToAction("index", "home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Password is incorrect");
                    }

                }
                else
                {
                    myCookie = new HttpCookie("sm_ds_a_cookie2");
                    myCookie["Id"] = CurrentUser.Id.ToString();
                    myCookie["Phone"] = CurrentUser.Phone;
                    myCookie["Password"] = CurrentUser.Password;
                    myCookie.Expires = DateTime.Now.AddDays(365);
                    HttpContext.Response.Cookies.Add(myCookie);

                    if (CurrentUser.Id != 0)
                    {
                        return RedirectToAction("index", "driver", new { id = CurrentUser.Id });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Password is incorrect");
                    }
                }



            }

            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            //Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            //Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (!u.Contains("Windows"))
            {
                // ViewBag.d = u;
                return View();
            }
            else
            {
                // ViewBag.d = u;
                //return View();
                return View("Index_web");
            }

        }

        public ActionResult Registration()
        {
            ViewBag.Info = "Գախտնաբառը կստանաք sms-ով";
            return View();
        }

        [HttpPost]
        public JsonResult Registration(string phone)
        {
            try
            {
                Repository.RegisterUser(phone);
            }
            catch
            {

            }
            return null;
        }
        public ActionResult SpecialRegistration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SpecialRegistration(LoginModel model)
        {
            try
            {
                Repository.RegisterSuperUser(model);
                return View("Login");
            }
            catch
            {
                return View("SpecialRegistration");
            }

        }
        [Authorize]
        public void OrderTaxi(string lat, string lng)
        {
            

            HttpCookie currentUserCookie = Request.Cookies["sm_ds_a_cookie1"];
           int orderId= Repository.Order_cab(Convert.ToInt32(currentUserCookie["Id"]), lat.ToString(), lng.ToString());
            HttpCookie myCookie = new HttpCookie("sm_ds_a_cookieorder");
            myCookie["Id"] = orderId.ToString();           
            myCookie.Expires = DateTime.Now.AddDays(365);
            HttpContext.Response.Cookies.Add(myCookie);
        }

        [Authorize]
        public ActionResult OnlineMap()
        {


            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            //Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            //Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (!u.Contains("Windows"))
            {
                // ViewBag.d = u;
                return View();
            }
            else
            {
                return View("OnlineMap_web");
            }
        }


        [Authorize]
        public ActionResult LogOut(int id)
        {
            FormsAuthentication.SignOut();
            HttpCookie currentUserCookie = Request.Cookies["sm_ds_a_cookie" + id];
            HttpContext.Response.Cookies.Remove("sm_ds_a_cookie" + id);
            currentUserCookie.Expires = DateTime.Now.AddDays(-10);
            currentUserCookie.Value = null;
            HttpContext.Response.SetCookie(currentUserCookie);
            return RedirectToAction("Index");

        }
        [Authorize]
        [HttpPost]
        public JsonResult checkOrderStatus()
        {
            string status = "";
            HttpCookie currentUserCookie = Request.Cookies["sm_ds_a_cookieorder"];
            status = Repository.CheckOrderStatusForUser(Convert.ToInt32(currentUserCookie["Id"]));
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetDriversPositions()
        {
            DriverList listmodel = new DriverList();
            listmodel.driversList = Repository.GetDriverPosition();
            return Json(listmodel.driversList.ToList(), JsonRequestBehavior.AllowGet);
        }


    }
}
