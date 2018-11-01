using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projectwerk.Models
{
    public class Forum
    {
        public int Id { get; set; }
        [Required]
        public string ForumName { get; set; }
        public virtual ICollection Topics { get; set; }
    }
}