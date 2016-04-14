using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using smart_Taxi.Models;
using MvcApplication2.Controllers;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
namespace smart_Taxi
{
    public class Repository
    {
        public static SqlConnection conn = new SqlConnection(GetConnectionString());
        static Regex MobileCheck = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        static Regex MobileVersionCheck = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        public static bool fBrowserIsMobile()
        {
            Debug.Assert(HttpContext.Current != null);

            if (HttpContext.Current.Request != null && HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                var u = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString();

                if (u.Length < 4)
                    return false;

                if (MobileCheck.IsMatch(u) || MobileVersionCheck.IsMatch(u.Substring(0, 4)))
                    return true;
            }

            return false;
        }
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["smart_t_connectionString"].ConnectionString;
        }

        public static List<Users> GetAllUsers()
        {
            CloseConnection();
            SqlCommand command = new SqlCommand("sp_sm_GetAllUsers", conn);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                SqlDataReader rdr = command.ExecuteReader();
                List<Users> UserList = new List<Users>();
                while (rdr.Read())
                {
                    Users user = new Users();
                    user.Id = Convert.ToInt32(rdr[0]);
                    user.Name = rdr[1].ToString();
                    user.Email = rdr[3].ToString();
                    user.AddDate = Convert.ToDateTime(rdr[5].ToString());
                    user.Phone = rdr[2].ToString();
                    user.Card = rdr[4].ToString();
                    user.Type = rdr[6].ToString();
                    UserList.Add(user);
                }
                conn.Close();
                return UserList;

            }
            catch
            {
                conn.Close();
                return null;
            }
        }

        public static int Order_cab(int userid, string lat, string lng)
        {
            CloseConnection();
            int orderId = 0;
            SqlCommand command = new SqlCommand("sp_sm_Taxi_Order_Taxi", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@UserId", SqlDbType.Int);
            command.Parameters.Add("@Latitude", SqlDbType.NVarChar);
            command.Parameters.Add("@Longitude", SqlDbType.NVarChar);
            command.Parameters["@UserId"].Value = userid;
            command.Parameters["@Latitude"].Value = lat;
            command.Parameters["@Longitude"].Value = lng;
            try
            {
                conn.Open();
                SqlDataReader rdr = command.ExecuteReader();
                if (rdr.Read())
                {
                    orderId= Convert.ToInt32(rdr[0]);
                }
                conn.Close();
                return orderId;
            }
            catch
            {
                conn.Close();
                return 1;
            }

        }

        public static List<Drivers> GetDriver()
        {
            CloseConnection();
            SqlCommand command = new SqlCommand("sp_sm_Get_Taxi_Driver", conn);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                SqlDataReader rdr = command.ExecuteReader();
                List<Drivers> DriverList = new List<Drivers>();
                while (rdr.Read())
                {
                    Drivers driver = new Drivers();
                    driver.Id = Convert.ToInt32(rdr[0]);
                    driver.Name = rdr[1].ToString();
                    driver.Car_Color = rdr[2].ToString();
                    driver.Car_Name = rdr[3].ToString();
                    driver.Phone = rdr[5].ToString();
                    driver.Plate = rdr[4].ToString();
                    driver.Distrinct = rdr[6].ToString();

                    DriverList.Add(driver);
                }
                conn.Close();
                return DriverList;

            }
            catch
            {
                conn.Close();
                return null;
            }


        }
        public static List<Drivers> GetDriverPosition()
        {
            CloseConnection();
            SqlCommand command = new SqlCommand("sp_sm_GetDriversPosAdmin", conn);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                SqlDataReader rdr3 = command.ExecuteReader();
                List<Drivers> DriverList = new List<Drivers>();
                while (rdr3.Read())
                {
                    Drivers driver = new Drivers();
                    driver.Id = Convert.ToInt32(rdr3[0]);
                    driver.Name = rdr3[1].ToString();
                    driver.Car_Name = rdr3[2].ToString();
                    driver.Plate = rdr3[3].ToString();
                    driver.Phone = rdr3[4].ToString();
                    driver.Latitude = rdr3[5].ToString();
                    driver.Longitude = rdr3[6].ToString();

                    DriverList.Add(driver);
                }
                conn.Close();
                return DriverList;

            }
            catch
            {
                conn.Close();
                return null;
            }


        }
        public static List<Drivers> GetDriverPosition1()
        {
            CloseConnection();
            SqlCommand command = new SqlCommand("sp_sm_GetDriversPosAdmin", conn);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                SqlDataReader rdr4 = command.ExecuteReader();
                List<Drivers> DriverList = new List<Drivers>();
                while (rdr4.Read())
                {
                    Drivers driver = new Drivers();
                    driver.Id = Convert.ToInt32(rdr4[0]);
                    driver.Name = rdr4[1].ToString();
                    driver.Car_Name = rdr4[2].ToString();
                    driver.Plate = rdr4[3].ToString();
                    driver.Phone = rdr4[4].ToString();
                    driver.Latitude = rdr4[5].ToString();
                    driver.Longitude = rdr4[6].ToString();

                    DriverList.Add(driver);
                }
                conn.Close();
                return DriverList;

            }
            catch
            {
                conn.Close();
                return null;
            }


        }
        public static Users GetUserLogin(string Phone, string Password)
        {
            CloseConnection();
            SqlCommand command = new SqlCommand("sp_sm_GetUserForLogin", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Phone", SqlDbType.Int);
            command.Parameters.Add("@Password", SqlDbType.NVarChar);
            command.Parameters["@Phone"].Value = Phone;
            command.Parameters["@Password"].Value = Password;
            Users CurrUser = new Users();
            try
            {
                conn.Open();

                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {

                    CurrUser.Id = Convert.ToInt32(rdr[0].ToString());
                    CurrUser.Name = rdr[1].ToString();
                    CurrUser.Phone = rdr[2].ToString();
                    CurrUser.Password = rdr[3].ToString();
                    CurrUser.Email = rdr[4].ToString();
                    CurrUser.Card = rdr[5].ToString();
                    CurrUser.AddDate = Convert.ToDateTime(rdr[6].ToString());
                    CurrUser.Type = rdr[7].ToString();
                }
                conn.Close();
                return CurrUser;

            }
            catch
            {
                conn.Close();
                return null;
            }

        }


        public static string RegisterUser(string phone)
        {
            CloseConnection();
            string password = CreatePassword(4);
            SqlCommand command = new SqlCommand("sp_sm_Taxi_reg_user", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Phone", SqlDbType.Int);
            command.Parameters.Add("@Password", SqlDbType.NVarChar);
            command.Parameters["@Phone"].Value = phone;
            command.Parameters["@Password"].Value = password;
            Users CurrUser = new Users();
            try
            {
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                password = null;
                conn.Close();
            }
            return password;
        }

        public static string RegisterSuperUser(LoginModel model)
        {
            CloseConnection();
            string password = CreatePassword(4);
            SqlCommand command = new SqlCommand("sp_sm_Taxi_reg_user_super", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Name", SqlDbType.NVarChar);
            command.Parameters.Add("@Phone", SqlDbType.NVarChar);
            command.Parameters.Add("@Password", SqlDbType.NVarChar);
            command.Parameters.Add("@Email", SqlDbType.NVarChar);
            command.Parameters.Add("@Type", SqlDbType.NVarChar);
            command.Parameters["@Name"].Value = model.Name;
            command.Parameters["@Phone"].Value = model.Phone;
            command.Parameters["@Password"].Value = model.Password;
            command.Parameters["@Email"].Value = model.Mail;
            command.Parameters["@Type"].Value = "1";
            Users CurrUser = new Users();
            try
            {
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                password = null;
                conn.Close();
            }
            return password;
        }

        public static string CreatePassword(int length)
        {
            CloseConnection();
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static Drivers GetDriverLogin(string Phone, string Password)
        {
            CloseConnection();
            SqlCommand command = new SqlCommand("sp_sm_GetDriverLogin", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Phone", SqlDbType.Int);
            command.Parameters.Add("@Password", SqlDbType.NVarChar);
            command.Parameters["@Phone"].Value = Phone;
            command.Parameters["@Password"].Value = Password;
            Drivers CurrDriver = new Drivers();
            try
            {
                conn.Open();

                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {

                    CurrDriver.Id = Convert.ToInt32(rdr[0].ToString());
                    CurrDriver.Name = rdr[1].ToString();
                    CurrDriver.Phone = rdr[2].ToString();
                    CurrDriver.Password = rdr[3].ToString();
                    CurrDriver.Car_Name = rdr[4].ToString();
                    CurrDriver.Distrinct = rdr[5].ToString();
                    CurrDriver.Plate = rdr[6].ToString();
                    CurrDriver.Car_Color = rdr[7].ToString();
                }
                conn.Close();
                return CurrDriver;

            }
            catch
            {
                conn.Close();
                return null;
            }

        }

        public static int SetDriverLocation(int id, string latitude, string longitude)
        {
            CloseConnection();
            SqlCommand command = new SqlCommand("SetDriverPosition", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@DriverId", SqlDbType.Int);
            command.Parameters.Add("@Latitude", SqlDbType.NVarChar);
            command.Parameters.Add("@Longitude", SqlDbType.NVarChar);
            command.Parameters["@DriverId"].Value = id;
            command.Parameters["@Latitude"].Value = latitude;
            command.Parameters["@Longitude"].Value = longitude;
            Users CurrUser = new Users();
            try
            {
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                return 0;
            }
            catch
            {
                conn.Close();
                return 1;

            }


        }

        public static void SaveDriver(Drivers driver)
        {
            CloseConnection();
            SqlCommand command = new SqlCommand("sp_sm_CreateDriver", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Name", SqlDbType.NVarChar);
            command.Parameters.Add("@CarName", SqlDbType.NVarChar);
            command.Parameters.Add("@Plate", SqlDbType.NVarChar);
            command.Parameters.Add("@Phone", SqlDbType.NVarChar);
            command.Parameters.Add("@CarColor", SqlDbType.NVarChar);
            command.Parameters.Add("@Distrinct", SqlDbType.NVarChar);
            command.Parameters.Add("@Password", SqlDbType.NVarChar);
            command.Parameters["@Name"].Value = driver.Name;
            command.Parameters["@CarName"].Value = driver.Car_Name;
            command.Parameters["@Plate"].Value = driver.Plate;
            command.Parameters["@Phone"].Value = driver.Phone;
            command.Parameters["@CarColor"].Value = driver.Car_Color;
            command.Parameters["@Distrinct"].Value = driver.Distrinct;
            command.Parameters["@Password"].Value = driver.Password;
            Users CurrUser = new Users();
            try
            {
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                conn.Close();
            }
        }


        public static List<Orders> GetOrders()
        {
            CloseConnection();
            SqlCommand command = new SqlCommand("sp_sm_GetOrders", conn);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();

                SqlDataReader rdr = command.ExecuteReader();
                List<Orders> orderList = new List<Orders>();
                while (rdr.Read())
                {
                    Orders order = new Orders();
                    order.Id = Convert.ToInt32(rdr[0].ToString());
                    order.DriverName = rdr[1].ToString();
                    order.CarName = rdr[2].ToString();
                    order.Userphone = rdr[3].ToString();
                    order.OrderDate = rdr[4].ToString();
                    orderList.Add(order);
                }
                conn.Close();
                return orderList;

            }
            catch
            {
                conn.Close();
                return null;
            }
        }

        public static Orders GetOrdersForDriver(int driverId)
        {
            CloseConnection();
            SqlCommand command = new SqlCommand("sp_sm_GetOrdersForDriver", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@DriverId", SqlDbType.Int);
            command.Parameters["@DriverId"].Value = driverId;
            try
            {
                conn.Open();

                SqlDataReader rdr1 = command.ExecuteReader();
                Orders order = new Orders();
                while (rdr1.Read())
                {
                    order.Id = Convert.ToInt32(rdr1[0].ToString());
                    order.Latitude = rdr1[1].ToString();
                    order.Longitude = rdr1[2].ToString();
                    order.Userphone = rdr1[3].ToString();
                }
                conn.Close();
                return order;

            }
            catch
            {
                conn.Close();
                return null;
            }
        }

        public static Orders GetOrdersForAdminSimple(int orderId)
        {
            CloseConnection();
            SqlCommand command = new SqlCommand("sp_sm_GetSimpleOrder", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@OrderId", SqlDbType.Int);
            command.Parameters["@OrderId"].Value = orderId;
            try
            {
                conn.Open();
                SqlDataReader rdrord = command.ExecuteReader();
                Orders order = new Orders();
                while (rdrord.Read())
                {
                    order.Id = Convert.ToInt32(rdrord[0].ToString());
                    order.Latitude = rdrord[1].ToString();
                    order.Longitude = rdrord[2].ToString();
                    order.Userphone = rdrord[3].ToString();
                }
                conn.Close();
                return order;

            }
            catch
            {
                conn.Close();
                return null;
            }
        }

        public static void SaveBenefit(string htmlbenefit)
        {
            CloseConnection();
            SqlCommand command = new SqlCommand("sp_sm_Save_Benefits", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@BenefitText", SqlDbType.NVarChar);
            command.Parameters["@BenefitText"].Value = htmlbenefit;
            try
            {
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                conn.Close();
            }
        }

        public static string GetBenefit(int htmlbenefit)
        {
            CloseConnection();
            string html = "";
            SqlCommand command = new SqlCommand("sp_sm_GetBenefit", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@BenefitId", SqlDbType.Int);
            command.Parameters["@BenefitId"].Value = htmlbenefit;
            try
            {
                conn.Open();
                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    html = rdr[0].ToString();
                }
                conn.Close();
                return html;
            }
            catch
            {
                conn.Close();
                return "";
            }
        }

        public static string CheckOrderStatusForUser(int orderId)
        {
            CloseConnection();
            string status = "";
            SqlCommand command = new SqlCommand("sp_sm_CheckOrderStatus", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@OrderID", SqlDbType.Int);
            command.Parameters["@OrderID"].Value = orderId;
            try
            {
                conn.Open();
                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    status = rdr[0].ToString() + "," + rdr[1].ToString()+" "+ 
                        rdr[2].ToString()+" "+ rdr[3].ToString()+" "+ rdr[4].ToString()+" ";
                }
                conn.Close();
                return status;
            }
            catch
            {
                conn.Close();
                return "";
            }
        }

        public static void SetOrderStatus(int driverId,int orderId,int status)
        {
            CloseConnection();
            SqlCommand command = new SqlCommand("sp_sm_SetOrderStatus", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@DriverID", SqlDbType.Int);
            command.Parameters["@DriverID"].Value = driverId;
            command.Parameters.Add("@OrderID", SqlDbType.Int);
            command.Parameters["@OrderID"].Value = orderId;
            command.Parameters.Add("@Status", SqlDbType.Int);
            command.Parameters["@Status"].Value = status;
            try
            {
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                conn.Close();
            }
        }

        public static void SetDriverForOrderAdmin(int driverId, int orderId)
        {
            CloseConnection();
            SqlCommand command = new SqlCommand("sp_sm_SetDriverForOrder", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@DriverID", SqlDbType.Int);
            command.Parameters["@DriverID"].Value = driverId;
            command.Parameters.Add("@OrderID", SqlDbType.Int);
            command.Parameters["@OrderID"].Value = orderId;          
            try
            {
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                conn.Close();
            }
        }

        public static void CloseConnection()
        {
            if (conn.State.Equals("Open"))
            {
                conn.Close();
            }
            
        }

    }
}