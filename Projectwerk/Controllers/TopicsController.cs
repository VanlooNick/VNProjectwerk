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
using System.Web.Mvc;

using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Text.RegularExpressions;

namespace Projectwerk.Controllers
{
    public class TopicsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Topics/5
        [ResponseType(typeof(Topic))]
        public async Task<IHttpActionResult> GetTopic(int id)
        {
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            return Ok(topic);
        }

        [ResponseType(typeof(Topic))]
        public async Task<IHttpActionResult> GetTopicsByForumId(int forumId, int page)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Topic, TopicDTO>());
            List<TopicDTO> topics = await db.Topics.Where(f => f.ForumId == forumId).ProjectTo<TopicDTO>().ToListAsync();
            topics.Reverse();

            List<TopicDTO> toSendBack = new List<TopicDTO>();                      

            int topicsPerPage = 5;
            int count = topicsPerPage * (page - 1);
            decimal pages = Math.Ceiling(Convert.ToDecimal(topics.Count) / Convert.ToDecimal(topicsPerPage));

            for (int i = 0; i < topicsPerPage; i++)
            {
                if(i + count < topics.Count) toSendBack.Add(topics[i + count]);
            }

            foreach (TopicDTO t in toSendBack)
            {
                t.ForumName = db.Fora.Where(f => f.Id == forumId).SingleOrDefault().ForumName;
                t.PostAmount = db.Posts.Where(p => p.TopicId == t.Id).Count();
                t.isLocked = db.Topics.Where(r => r.Id == t.Id).SingleOrDefault().Locked;

                t.TopicPages = Convert.ToInt32(pages);
                t.CurrentPage = page;
            }

            if (topics == null)
            {
                return NotFound();
            }

            return Ok(toSendBack);
        }

        [ResponseType(typeof(Topic))]
        public async Task<IHttpActionResult> GetTopicsBySearch(string searchBar)
        {
            List<string> SearchBar = new List<string>();

            Mapper.Initialize(cfg => cfg.CreateMap<Topic, TopicDTO>());
            List<Topic> topics = await db.Topics.ToListAsync();
            List<Topic> onlyXAmountPls = new List<Topic>();
            List<TopicDTO> results = new List<TopicDTO>();           
            
            string pattern = @"\s";

            Regex searchBarExpr = new Regex(pattern);
            MatchCollection searchMatches = searchBarExpr.Matches(searchBar);

            int prevIndex2 = 0;
            foreach (Match sMatch in searchMatches)
            {
                SearchBar.Add(searchBar.Substring(prevIndex2, sMatch.Index - prevIndex2).ToLower());
                prevIndex2 = sMatch.Index + 1;
                
            }

            SearchBar.Add(searchBar.Substring(prevIndex2, searchBar.Count() - prevIndex2).ToLower());
            
            bool flag = false;
            foreach (Topic t in topics)
            {
                flag = false;
                Regex expr = new Regex(pattern);
                MatchCollection matches = expr.Matches(t.TopicName);

                foreach (string str in SearchBar)
                {
                    if (!flag)
                    {
                        int prevIndex = 0;

                        foreach (Match match in matches)
                        {
                            if (t.TopicName.Substring(prevIndex, (match.Index - prevIndex)).ToLower() == str)
                            {
                                TopicDTO temp = await db.Topics.Where(b => b.Id == t.Id).ProjectTo<TopicDTO>().SingleOrDefaultAsync();
                                temp.PostAmount = db.Posts.Where(p => p.TopicId == t.Id).Count();
                                temp.isLocked = db.Topics.Where(r => r.Id == t.Id).SingleOrDefault().Locked;
                                temp.CurrentPage = 1;
                                results.Add(temp);
                                flag = true;
                            }
                            prevIndex = match.Index + 1;
                        }

                        if (t.TopicName.Substring(prevIndex, t.TopicName.Count() - prevIndex).ToLower() == str)
                        {
                            TopicDTO temp = await db.Topics.Where(b => b.Id == t.Id).ProjectTo<TopicDTO>().SingleOrDefaultAsync();
                            temp.PostAmount = db.Posts.Where(p => p.TopicId == t.Id).Count();
                            temp.isLocked = db.Topics.Where(r => r.Id == t.Id).SingleOrDefault().Locked;
                            temp.CurrentPage = 1;
                            results.Add(temp);
                            flag = true;
                        }
                    }

                }
            }

            return Ok(results);
        }

        // PUT: api/Topics/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTopic(int id, Topic topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != topic.Id)
            {
                return BadRequest();
            }

            db.Entry(topic).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(id))
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

        // POST: api/Topics
        [ResponseType(typeof(PostTopicDTO2))]
        public async Task<IHttpActionResult> PostTopic(PostTopicDTO2 postTopicDTO2)
        {
            if (postTopicDTO2.TopicDto.TopicName == null || postTopicDTO2.PostDto.ThePost == null)
            {
                return Redirect(Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/GForum/PostTopic?forumId="+postTopicDTO2.TopicDto.ForumId);
            }

            Topic topic = new Topic();
            topic.ForumId = postTopicDTO2.TopicDto.ForumId;
            topic.TopicName = postTopicDTO2.TopicDto.TopicName;
            topic.CreatedBy = postTopicDTO2.TopicDto.CreatedBy;

            db.Topics.Add(topic);
            await db.SaveChangesAsync();

            Post post = new Post();
            post.TopicId = topic.Id;
            post.ThePost = postTopicDTO2.PostDto.ThePost;
            post.TimePosted = DateTime.Now;
            post.PostedBy = postTopicDTO2.PostDto.PostedBy;

            db.Posts.Add(post);
            await db.SaveChangesAsync();

            return Redirect(Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/GForum/PostIndex?topicId=" + topic.Id + "&page=1");
        }

        // DELETE: api/Topics/5
        [ResponseType(typeof(Topic))]
        public async Task<IHttpActionResult> DeleteTopic(int id)
        {
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            db.Topics.Remove(topic);
            await db.SaveChangesAsync();

            return Ok(topic);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TopicExists(int id)
        {
            return db.Topics.Count(e => e.Id == id) > 0;
        }
         
    }
}