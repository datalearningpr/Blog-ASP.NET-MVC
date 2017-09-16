using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using Blog.Models;
using Dapper;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Configuration;
using System.Web.Caching;

namespace Blog.Controllers
{
    public class AdminController : Controller
    {
        // this is the view for logout
        [Authorize]
        [Route("LogoutUser")]
        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie 
            SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            HttpCookie cookie2 = new HttpCookie(sessionStateSection.CookieName, "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);
            return Redirect("/Login");
        }

        
        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }


        // this is the view for login
        [Route("Login")]
        [HttpPost]
        public ActionResult Login(User loginUser)
        {
            string userName = loginUser.UserName;
            string passWord = loginUser.Password;

            string connectionString = ConfigurationManager.ConnectionStrings["BlogDB"].ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            try
            {
                string query = string.Format("SELECT * FROM dbo.[User] WHERE UserName = '{0}' AND PassWord = '{1}'", userName, passWord);
                var temp = connection.Query<User>(query, null).ToList();

                connection.Close();

                if (temp.Count == 0)
                    return RedirectPermanent("Login");
                else
                {
                    FormsAuthentication.SetAuthCookie(userName, false);
                    return RedirectPermanent("Home");
                }
            }
            catch (Exception)
            {
                connection.Close();
                return RedirectPermanent("Login");
            }

        }



        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }


        // this is the view for register
        [Route("Register")]
        [HttpPost]
        public ActionResult Register(UserViewModel newUser)
        {
            string userName = newUser.UserName;
            string passWord = newUser.Password;

            string connectionString = ConfigurationManager.ConnectionStrings["BlogDB"].ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string query = "INSERT INTO dbo.[User] (UserName, PassWord) VALUES (@userName, @passWord)";
            connection.Execute(query, new { userName, passWord });

            return RedirectPermanent("Home");
        }


        // this is the view for helping validation
        [Route("IsUserNameAvailable", Name = "IsUserNameAvailable")]
        public ActionResult IsUserNameAvailable(string userName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BlogDB"].ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            try
            {
                string query = string.Format("SELECT * FROM dbo.[User] WHERE UserName = '{0}'", userName);
                var temp = connection.Query<User>(query, null).ToList();

                connection.Close();

                if (temp.Count == 0)
                    return Json(true, JsonRequestBehavior.AllowGet);
                else
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                connection.Close();
                return Json(true, JsonRequestBehavior.AllowGet);
            }

        }

    }
}