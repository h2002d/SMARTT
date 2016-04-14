using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smart_Taxi.Models
{
    public class Orders
    {
        private int id;
        private string driverid;
        private string driverName;
        private string carName;
        private string userphone;
        private string orderDate;
        private string latitude;
        private string longitude;
        private string status;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string DriverId
        {
            get { return driverid; }
            set { driverid = value; }
        }
        public string DriverName
        {
            get { return driverName; }
            set { driverName = value; }
        }
        public string CarName
        {
            get { return carName; }
            set { carName = value; }
        }
        public string Userphone
        {
            get { return userphone; }
            set { userphone = value; }
        }
        public string OrderDate
        {
            get { return orderDate; }
            set { orderDate = value; }
        }
        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        
    }
    public class OrderList
    {
        public List<Orders> OrdersList;
    }
}