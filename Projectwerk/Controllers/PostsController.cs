using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Projectwerk.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace Projectwerk.Controllers
{
    public class PostsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Posts
        public IQueryable<Post> GetPosts()
        {
            return db.Posts;
        }

        // GET: api/Posts/5
        [ResponseType(typeof(Post))]
        public async Task<IHttpActionResult> GetPost(int id)
        {
            Post post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [ResponseType(typeof(Post))]
        public async Task<IHttpActionResult> GetPostByTopic(int topicId, int page)
        {
            var UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            Mapper.Initialize(cfg => cfg.CreateMap<Post, PostDTO>());
            var post = await db.Posts.Where(p => p.TopicId == topicId).ProjectTo<PostDTO>().ToListAsync();
            
            if (post == null)
            {
                return NotFound();
            }

            List<PostDTO> toSendBack = new List<PostDTO>();

            int postsPerPage = 5;
            int count = postsPerPage * (page - 1);
            decimal pages = Math.Ceiling(Convert.ToDecimal(post.Count) / Convert.ToDecimal(postsPerPage));
            
            for(int i = 0; i < postsPerPage; i++)
            {
                if (i + count < post.Count) toSendBack.Add(post[i + count]);
            }

            ApplicationUser user;
            MD5 hasher = MD5.Create();
            StringBuilder sb = new StringBuilder();

            foreach (PostDTO p in toSendBack)
            {
                Topic temp = await db.Topics.Where(t => t.Id == p.TopicId).SingleOrDefaultAsync();
                p.ForumId = temp.ForumId;
                p.ForumName = temp.Forum.ForumName;
                p.TopicName = temp.TopicName;
                p.IsTopicLocked = temp.Locked;

                var GetUser = await UserManager.FindByNameAsync(p.PostedBy);
                var roleList = await UserManager.GetRolesAsync(GetUser.Id);
                p.Role = roleList[0];

                p.PostPages = Convert.ToInt32(pages);
                p.CurrentPage = page;

                user = await UserManager.FindByNameAsync(p.PostedBy);

                byte[] data = hasher.ComputeHash(Encoding.Default.GetBytes(user.Email));

                foreach (byte d in data)
                    sb.Append(d.ToString("x2"));

                p.emailHash = "https://www.gravatar.com/avatar/" + sb.ToString();

                sb.Clear();
            }

            return Ok(toSendBack);
        }

        // PUT: api/Posts/5
        [ResponseType(typeof(Post))]
        public async Task<IHttpActionResult> PutPost(int id,Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != post.Id)
            {
                return BadRequest();
            }

            db.Entry(post).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Posts
        [ResponseType(typeof(Post))]
        public async Task<IHttpActionResult> PostPost(Post post)
        {
            if (!ModelState.IsValid)
            {
                string topicname = "";
                topicname = db.Topics.Where(t => t.Id == post.TopicId).SingleOrDefault().TopicName;
                return Redirect(Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/GForum/PostAPost?topicId="+post.TopicId+"&TopicName="+topicname);
            }

            var _post = db.Posts.Where(p => p.Id == post.Id).SingleOrDefault();

            if (_post == null)
            {
                post.TimePosted = DateTime.Now;
                db.Posts.Add(post);
                await db.SaveChangesAsync();
            }            
            else
            {
                _post.ThePost = post.ThePost;
                await db.SaveChangesAsync();
            }

            return Redirect(Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/GForum/PostIndex?topicId=" + post.TopicId + "&page=1");
        }

        // DELETE: api/Posts/5
        [ResponseType(typeof(Post))]
        public async Task<IHttpActionResult> DeletePost(int id)
        {
            Post post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            db.Posts.Remove(post);
            await db.SaveChangesAsync();

            return Ok(post);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PostExists(int id)
        {
            return db.Posts.Count(e => e.Id == id) > 0;
        }
    }
}