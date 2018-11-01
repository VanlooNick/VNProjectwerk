using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projectwerk.Models
{
    public class ForumDTO
    {
        public int Id { get; set; }
        public String ForumName { get; set; }
        public int TopicAmount { get; set; }
    }

    public class TopicDTO
    {
        public int Id { get; set; }
        public string TopicName { get; set; }
        public int ForumId { get; set; }
        public string ForumName { get; set; }
        public int PostAmount { get; set; }
        public string CreatedBy { get; set; }
        public bool isLocked { get; set;}
        public int TopicPages { get; set; }
        public int CurrentPage { get; set; }
    }

    public class PostDTO
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string ThePost { get; set; }
        public DateTime TimePosted { get; set; }
        public int ForumId { get; set; }
        public string ForumName { get; set; }
        public string PostedBy { get; set; }
        public string Role { get; set; }
        public bool IsTopicLocked { get; set; }
        public bool isDeleted { get; set; }
        public int PostPages { get; set; }
        public int CurrentPage { get; set; }
        public int ReplyToPostId { get; set; }
        public string emailHash { get; set; }
    }

    public class EditPostDTO
    {
        public int postId { get; set; }
        public int topicId { get; set; }
        public string thePost { get; set; }                
    }

    public class PostTopicDTO
    {
        public int Id { get; set; }
        public string TopicName { get; set; }
        public int ForumId { get; set; }
        public string ForumName { get; set; }

        public int PostId { get; set; }
        public string ThePost { get; set; }
        public DateTime TimePosted { get; set; }
    }

    public class PostTopicDTO2
    {
        public TopicDTO TopicDto { get; set; }
        public PostDTO PostDto { get; set; }
    }

    public class UserDto
    {
        public ApplicationUser user { get; set; }
        public String role { get; set; }
    }

    public class PMDTO
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string RecieverId { get; set; }
        public string RecieverName { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string ThePM { get; set; }
        public bool Read { get; set; }
    }
}