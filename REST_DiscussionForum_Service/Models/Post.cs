using System;
using System.ComponentModel.DataAnnotations;

namespace RESTful_Forum_Service.Models
{
    // a user post in a forum
    public class UserPost
    {
        [Required(ErrorMessage = "Invalid Subject")]
        [StringLength(25)]
        public String Subject { get; set; }

        [Required(ErrorMessage = "Invalid Mesage")]
        [StringLength(100)]
        public String Message { get; set; }
    }


    // a post made in a forum with assigned ID
    public class ForumPost
    {
        public int Id { get; set; }

        public UserPost UserPost { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}