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

namespace Projectwerk.Controllers
{
    public class ForumController : ApiController
    {
        private ForumServiceContext db = new ForumServiceContext();

        // GET: api/Forum
        public IQueryable<ForumDTO> GetForums()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Forum, ForumDTO>());
            var forums = db.Forums.ProjectTo<ForumDTO>();
            return forums;
        }

        ////GET: api/Forum
        //public IQueryable<Forum> GetForums()
        //{
        //    return db.Forums;
        //}

        // GET: api/Forum/5
        [ResponseType(typeof(Forum))]
        public async Task<IHttpActionResult> GetForum(int id)
        {
            Forum forum = await db.Forums.FindAsync(id);
            if (forum == null)
            {
                return NotFound();
            }

            return Ok(forum);
        }

        // PUT: api/Forum/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutForum(int id, Forum forum)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != forum.Id)
            {
                return BadRequest();
            }

            db.Entry(forum).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumExists(id))
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

        // POST: api/Forum
        [ResponseType(typeof(Forum))]
        public async Task<IHttpActionResult> PostForum(Forum forum)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Forums.Add(forum);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = forum.Id }, forum);
        }

        // DELETE: api/Forum/5
        [ResponseType(typeof(Forum))]
        public async Task<IHttpActionResult> DeleteForum(int id)
        {
            Forum forum = await db.Forums.FindAsync(id);
            if (forum == null)
            {
                return NotFound();
            }

            db.Forums.Remove(forum);
            await db.SaveChangesAsync();

            return Ok(forum);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ForumExists(int id)
        {
            return db.Forums.Count(e => e.Id == id) > 0;
        }
    }
}