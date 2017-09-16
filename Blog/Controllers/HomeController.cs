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

namespace Blog.Controllers
{
    public class HomeController : Controller
    {

        // index, home page
        [Route("")]
        [Route("Home")]
        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BlogDB"].ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var sql =
            @"SELECT * from dbo.Post p LEFT JOIN dbo.[User] u ON u.Id = p.UserId ORDER BY p.TimeStamp DESC";

            var posts = connection.Query<Post, User, Post>(sql, (post, user) => { post.User = user; return post; }).ToList();

            connection.Close();

            Pagination<Post> pn = new Pagination<Post>(posts, 3, 1);

            Tuple<List<string>, List<string>> categories = Blog.Helpers.Helpers.GetCategory();
            ViewData["categoryLeft"] = categories.Item1;
            ViewData["categoryRight"] = categories.Item2;

            return View(pn);
        }



        // this is the view for ajax result generating
        // using ajax to do the partial refresh of (previous/next list of posts)
        [Route("RenderPost")]
        [HttpPost]
        public ActionResult RenderPost(AjaxForm data)
        {
            int newPageNo;
            if (data.action == "previous")
                newPageNo = int.Parse(data.pageNo) + 1;
            else if (data.action == "next")
                newPageNo = int.Parse(data.pageNo) - 1;
            else
                newPageNo = 1;

            if (newPageNo < 1)
                newPageNo = 1;
            

            string connectionString = ConfigurationManager.ConnectionStrings["BlogDB"].ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();


            var sql =
            @"SELECT * from dbo.Post p LEFT JOIN dbo.[User] u ON u.Id = p.UserId ORDER BY p.TimeStamp DESC";

            var posts = connection.Query<Post, User, Post>(sql, (post, user) => { post.User = user; return post; }).ToList();
            Pagination<Post> pn = new Pagination<Post>(posts, 3, newPageNo);

            if ((pn.Items().Count() == 0) && (newPageNo != 1))
            {
                newPageNo = newPageNo - 1;
                pn = new Pagination<Post>(posts, 3, newPageNo);
            }
            
            connection.Close();
            return View(pn);
        }






        // this is tha about page view
        [Authorize]
        [Route("About")]
        public ActionResult About()
        {
            ViewBag.Message = "ASP MVC Blog App";
            return View();
        }



        [Authorize]
        [Route("SubmitPost")]
        public ActionResult SubmitPost()
        {
            return View();
        }

        // this is the view for creating a new post, using the markdown format
        [Authorize]
        [Route("SubmitPost")]
        [HttpPost]
        public ActionResult SubmitPost(PostViewModel Post)
        {
            string Title = Post.Title;
            string Body = Post.Body;
            string Category = Post.Category;
            string connectionString = ConfigurationManager.ConnectionStrings["BlogDB"].ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string query = string.Format("SELECT * FROM dbo.[User] WHERE UserName = '{0}'", User.Identity.Name);
            User selectedUser = connection.Query<User>(query, null).First();

            int userId = selectedUser.Id;
            DateTime timeStamp = DateTime.Now;

            query = @"INSERT INTO dbo.[Post] ([Title],[Body],[Category],[UserId],[TimeStamp]) 
                             VALUES (@Title, @Body, @Category, @userId, @timeStamp)";
            connection.Execute(query, new { Title, Body, Category, userId, timeStamp});

            connection.Close();

            return RedirectPermanent("SubmitPost");

        }


    }
}