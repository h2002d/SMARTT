using MvcApplication2.Controllers;
using smart_Taxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace smart_Taxi.Controllers
{
    public class DriverController : Controller
    {
        //
        // GET: /Driver/
        public static Drivers CurrentDriver = null;

        public ActionResult Index(int id)
        {
            return View();
        }
        [HttpPost]
        public void SendDriverLocation(string lat, string lng)
        {
            int driverId=0;
            if (Request.Cookies["sm_ds_a_cookie2"] != null)
            {
                driverId = Convert.ToInt32(Request.Cookies["sm_ds_a_cookie2"]["id"]);
            }
            Repository.SetDriverLocation(Convert.ToInt32(driverId), lat, lng);
        }
        [HttpPost]
        public JsonResult GetOrderIfavailable()
        {
            int driverId = 0;
            if (Request.Cookies["sm_ds_a_cookie2"] != null)
            {
                driverId = Convert.ToInt32(Request.Cookies["sm_ds_a_cookie2"]["id"]);
            }
            Orders Order = Repository.GetOrdersForDriver(driverId);
           return Json(Order, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SetOrderStatus(string orderId,string status)
        {
            int driverId = 0;
            int OrderId =Convert.ToInt32(orderId);
            int StatusId = Convert.ToInt32(status);
            if (Request.Cookies["sm_ds_a_cookie2"] != null)
            {
                driverId = Convert.ToInt32(Request.Cookies["sm_ds_a_cookie2"]["id"]);
            }
            Repository.SetOrderStatus(driverId, OrderId, StatusId);
            return null;
        }


    }
}
