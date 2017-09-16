using Blog.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class SearchController : Controller
    {
        // this is the serach function for title
        [Route("Search", Name = "Search")]
        public ActionResult Search()
        {
            string search = this.Request.QueryString["search"];
            string pageNumber = this.Request.QueryString["pageNumber"];

            if (pageNumber == null)
                pageNumber = "1";

            int pageNum = int.Parse(pageNumber);
            
            string connectionString = ConfigurationManager.ConnectionStrings["BlogDB"].ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();


            var sql =
            String.Format(@"SELECT * from dbo.Post p LEFT JOIN dbo.[User] u ON u.Id = p.UserId WHERE p.Title LIKE '%{0}%' ORDER BY p.TimeStamp DESC", search);

            var posts = connection.Query<Post, User, Post>(sql, (post, user) => { post.User = user; return post; }).ToList();

            connection.Close();


            Pagination<Post> pn = new Pagination<Post>(posts, 3, pageNum);

            Tuple<List<string>, List<string>> categories = Blog.Helpers.Helpers.GetCategory();
            ViewData["categoryLeft"] = categories.Item1;
            ViewData["categoryRight"] = categories.Item2;

            ViewData["search"] = search;

            return View("Search", pn);
        }



        // this is the serach function for author
        [Route("Search/Author", Name = "SearchAuthor")]
        public ActionResult SearchAuthor()
        {
            string author = this.Request.QueryString["author"];
            string pageNumber = this.Request.QueryString["pageNumber"];

            if (pageNumber == null)
                pageNumber = "1";

            int pageNum = int.Parse(pageNumber);

            string connectionString = ConfigurationManager.ConnectionStrings["BlogDB"].ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();


            var sql =
            String.Format(@"SELECT * from dbo.Post p LEFT JOIN dbo.[User] u ON u.Id = p.UserId WHERE u.UserName = '{0}' ORDER BY p.TimeStamp DESC", author);

            var posts = connection.Query<Post, User, Post>(sql, (post, user) => { post.User = user; return post; }).ToList();

            connection.Close();


            Pagination<Post> pn = new Pagination<Post>(posts, 3, pageNum);

            Tuple<List<string>, List<string>> categories = Blog.Helpers.Helpers.GetCategory();
            ViewData["categoryLeft"] = categories.Item1;
            ViewData["categoryRight"] = categories.Item2;

            ViewData["author"] = author;

            return View("SearchAuthor", pn);
        }



        // this is the serach function for category
        [Route("Search/Category", Name = "SearchCategory")]
        public ActionResult SearchCategory()
        {
            string category = this.Request.QueryString["category"];
            string pageNumber = this.Request.QueryString["pageNumber"];

            if (pageNumber == null)
                pageNumber = "1";

            int pageNum = int.Parse(pageNumber);

            string connectionString = ConfigurationManager.ConnectionStrings["BlogDB"].ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();


            var sql =
            String.Format(@"SELECT * from dbo.Post p LEFT JOIN dbo.[User] u ON u.Id = p.UserId WHERE p.Category = '{0}' ORDER BY p.TimeStamp DESC", category);

            var posts = connection.Query<Post, User, Post>(sql, (post, user) => { post.User = user; return post; }).ToList();

            connection.Close();


            Pagination<Post> pn = new Pagination<Post>(posts, 3, pageNum);

            Tuple<List<string>, List<string>> categories = Blog.Helpers.Helpers.GetCategory();
            ViewData["categoryLeft"] = categories.Item1;
            ViewData["categoryRight"] = categories.Item2;

            ViewData["category"] = category;

            return View("SearchCategory", pn);
        }


    }
}