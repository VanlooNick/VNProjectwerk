using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projectwerk.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string ThePost { get; set; }
        public DateTime TimePosted { get; set; }
        public int TopicId { get; set; }
        public string PostedBy { get; set; }
        public bool isDeleted { get; set; }
        public int ReplyToPostId { get; set; }
    }
}