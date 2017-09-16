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
    public class PostController : Controller
    {

        // this is the view for showing a specific post
        [Route("Post/{postId}", Name = "Post")]
        public ActionResult Search(int postId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BlogDB"].ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();


            var sql =
            String.Format(@"SELECT * from dbo.Post p LEFT JOIN dbo.[User] u ON u.Id = p.UserId WHERE p.Id = {0} ORDER BY p.Id", postId);

            var posts = connection.Query<Post, User, Post>(sql, (post, user) => { post.User = user; return post; }).FirstOrDefault();

            if (posts != null)
                posts.Body = CommonMark.CommonMarkConverter.Convert(posts.Body);

            sql =
            String.Format(@"SELECT c.Id, c.Body, c.PostId, u.UserName, c.TimeStamp from dbo.Comment c LEFT JOIN dbo.[User] u ON u.Id = c.UserId WHERE c.postId = {0} ORDER BY c.TimeStamp DESC", postId);

            var comments = connection.Query<Comment>(sql, null).ToList();

            connection.Close();

            Tuple<List<string>, List<string>> categories = Blog.Helpers.Helpers.GetCategory();
            ViewData["categoryLeft"] = categories.Item1;
            ViewData["categoryRight"] = categories.Item2;
            ViewData["posts"] = posts;

            return View("Post", comments);
        }






        // this is the view to handle the comments submitted
        [Authorize]
        [HttpPost]
        [Route("CreateComment", Name = "CreateComment")]
        public ActionResult CreateComment(CommentForm commentInput)
        {
            string comment = commentInput.comment;
            string postIdString = commentInput.postId;
            int postId = int.Parse(postIdString);

            string connectionString = ConfigurationManager.ConnectionStrings["BlogDB"].ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();


            string query = string.Format("SELECT Id FROM dbo.[User] WHERE UserName = '{0}'", User.Identity.Name);
            int userId = connection.Query<int>(query, null).FirstOrDefault();

            DateTime TimeStamp = DateTime.Now;
            query = "INSERT INTO dbo.Comment (Body, PostId, UserId, TimeStamp) VALUES (@comment, @postId, @userId, @TimeStamp)";
            connection.Execute(query, new { comment, postId, userId, TimeStamp });


            connection.Close();

            return RedirectToRoute("Post", new { postId = postId });
        }

    }
}