using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projectwerk.Models
{
    public class Topic
    {
        public int Id { get; set; }
        [Required]
        public string TopicName { get; set; }
        public virtual ICollection Posts { get; set; }
        public string CreatedBy { get; set; }

        public int ForumId { get; set; }
        public virtual Forum Forum { get; set; }
        public bool Locked {get;set;}
        public string WhoLocked { get; set; }
    }
}