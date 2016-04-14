using smart_Taxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smart_Taxi.Controllers
{
    public class AdminSmController : Controller
    {
        //
        // GET: /AdminSm/

        public ActionResult Index()
        {
            return View("AdminPanel");
        }
        
        
        
        [HttpPost]
        public JsonResult GetDriversPositions()
        {
            DriverList listmodel = new DriverList();
            listmodel.driversList = Repository.GetDriverPosition1();
            return Json(listmodel.driversList.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveBenefits(string htmbenefit)
        {
            Repository.SaveBenefit(htmbenefit);
            return null;
        }

        [HttpPost]
        public JsonResult GetOrderIfavailable(string orderid)
        {            
            Orders Order = Repository.GetOrdersForAdminSimple(Convert.ToInt32(orderid));
            return Json(Order, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult DriversWorkspace()
        {
            DriverList listmodel = new DriverList();
            listmodel.driversList = Repository.GetDriver();
            return PartialView("_DriversWorkspace", listmodel);
        }
        
        [HttpGet]
        public PartialViewResult OrderPopup()
        {
            DriverList listmodel = new DriverList();
            listmodel.driversList = Repository.GetDriver();
            return PartialView("_OrderPopup", listmodel);
        }

        [HttpPost]
        public ActionResult DriversWorkspace(string name, string phone, string carname, string plate, string color, string distrinct, string password)
        {
            Drivers driverforReg = new Drivers();
            driverforReg.Name=name;
            driverforReg.Phone=phone;
            driverforReg.Car_Name=carname;
            driverforReg.Plate=plate;
            driverforReg.Car_Color=color;
            driverforReg.Distrinct=distrinct;
            driverforReg.Password=password;
            Repository.SaveDriver(driverforReg);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public PartialViewResult OrdersWorkspace()
        {
            OrderList listmodel = new OrderList();
            listmodel.OrdersList = Repository.GetOrders();
            return PartialView("_OrdersWorkspace", listmodel);
        }

        [HttpGet]
        public PartialViewResult UsersWorkspace()
        {
            UsersList listmodel = new UsersList();
            listmodel.usersList = Repository.GetAllUsers();
            return PartialView("_UsersWorkspace", listmodel);
        }
        [HttpGet]
        public PartialViewResult BenefitssWorkspace()
        {

            return PartialView("_BenefitsWorkspace");
        }
        
        [HttpPost]
        public void SetDriverForOrder(string orderid,string driverId)
        {
            int OrderId = Convert.ToInt32(orderid);
            int DriverId = Convert.ToInt32(driverId);
            Repository.SetDriverForOrderAdmin(DriverId, OrderId);            
        }

    }
}
