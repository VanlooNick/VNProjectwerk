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
    public class PersonalMessagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PersonalMessages
        public IQueryable<PersonalMessage> GetPersonalMessages()
        {
            return db.PersonalMessages;
        }
        
        // GET: api/PersonalMessages/5
        [ResponseType(typeof(PersonalMessage))]
        public async Task<IHttpActionResult> GetUnreadPMsForUser(string id)
        {
            PersonalMessage personalMessage = await db.PersonalMessages.FindAsync(id);
            if (personalMessage == null)
            {
                return NotFound();
            }

            Mapper.Initialize(cfg => cfg.CreateMap<PersonalMessage, PMDTO>());
            List<PMDTO> pms = await db.PersonalMessages.Where(p => p.RecieverId == id).ProjectTo<PMDTO>().ToListAsync();


            return Ok(pms);
        }

        [ResponseType(typeof(PersonalMessage))]
        public async Task<IHttpActionResult> GetPersonalMessagesByUser(int Userid)
        {
            PersonalMessage personalMessage = await db.PersonalMessages.FindAsync(Userid);
            if (personalMessage == null)
            {
                return NotFound();
            }

            return Ok(personalMessage);
        }

        // PUT: api/PersonalMessages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPersonalMessage(int id, PersonalMessage personalMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != personalMessage.Id)
            {
                return BadRequest();
            }

            db.Entry(personalMessage).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonalMessageExists(id))
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

        // POST: api/PersonalMessages
        [ResponseType(typeof(PersonalMessage))]
        public async Task<IHttpActionResult> PostPersonalMessage(PersonalMessage personalMessage)
        {
            if (personalMessage.ThePM == null)
            {
                string sen = db.Users.Where(u => u.Id == personalMessage.SenderId).SingleOrDefault().UserName;
                string rec = db.Users.Where(u => u.Id == personalMessage.RecieverId).SingleOrDefault().UserName;
                return Redirect(Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/GForum/CreatePM?sender="+sen+"&reciever="+rec);
            }

            if (personalMessage.Subject == null) personalMessage.Subject = "(No Subject)";

            db.PersonalMessages.Add(personalMessage);
            await db.SaveChangesAsync();
            
            return Redirect(Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/GForum/PMConfirmation");

        }

        // DELETE: api/PersonalMessages/5
        [ResponseType(typeof(PersonalMessage))]
        public async Task<IHttpActionResult> DeletePersonalMessage(int id)
        {
            PersonalMessage personalMessage = await db.PersonalMessages.FindAsync(id);
            if (personalMessage == null)
            {
                return NotFound();
            }

            db.PersonalMessages.Remove(personalMessage);
            await db.SaveChangesAsync();

            return Ok(personalMessage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonalMessageExists(int id)
        {
            return db.PersonalMessages.Count(e => e.Id == id) > 0;
        }
    }
}