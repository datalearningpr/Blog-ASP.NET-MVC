using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Blog.Helpers
{
    public static class Helpers
    {
        public static Tuple<List<string>, List<string>> GetCategory()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BlogDB"].ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();


            var query =
            @"SELECT DISTINCT(Category) from dbo.Post";

            List<string> CategoryList = connection.Query<String>(query, null).ToList();

            return Tuple.Create(CategoryList.Take(CategoryList.Count() / 2).ToList(), CategoryList.Skip(CategoryList.Count() / 2).ToList());

        }
    }
}