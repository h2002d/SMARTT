using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smart_Taxi.Models
{
    public class Users
    {
        private int id;
        private string name;
        private string phone;
        private string password;
        private string email;
        private string card;
        private DateTime addDate;
        private string type;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Card
        {
            get { return card; }
            set { card = value; }
        }
        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }


        

    }
    public class UsersList
    {
        public List<Users> usersList;
    }
}