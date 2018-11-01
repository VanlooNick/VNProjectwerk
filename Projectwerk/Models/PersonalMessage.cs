using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projectwerk.Models
{
    public class PersonalMessage
    {
        public int Id { get; set; }
        public string RecieverId { get; set; }
        public string SenderId { get; set; }
        public string ThePM { get; set; }
        public string Subject { get; set; }
        public bool Read { get; set; }
    }
}