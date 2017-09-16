using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }


    public class UserViewModel
    {
        [Required]
        [Remote("IsUserNameAvailable", ErrorMessage = "UserName Already Exist.")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }

    }



}