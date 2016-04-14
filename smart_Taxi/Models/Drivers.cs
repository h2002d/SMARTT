using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smart_Taxi.Models
{
    public class Drivers
    {
        private int id;
        private string name;
        private string phone;
        private string plate;
        private string password;
        private string distrinct;
        private string car_name;
        private string car_color;
        private string latitude;
        private string longitude;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value;}
        }
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        public string Plate
        {
            get { return plate; }
            set { plate = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Distrinct
        {
            get { return distrinct; }
            set { distrinct = value; }
        }
        public string Car_Name
        {
            get { return car_name; }
            set { car_name = value; }
        }
        public string Car_Color
        {
            get { return car_color; }
            set { car_color = value; }
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

    }

    public class DriverList
    {
        public List<Drivers> driversList ;
    }
}