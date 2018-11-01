using Projectwerk.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Security.Cryptography;
using System.Text;

namespace Projectwerk.Controllers
{
    public class GForumController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            string uri = "http://" + Request.Url.Host + ':' + Request.Url.Port + "/api/Forum";
            
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);

                return
                    View(
                            Task.Factory.StartNew
                            (
                                () => JsonConvert
                                        .DeserializeObject<List<ForumDTO>>(response.Result)
                            )
                            .Result
                         );
            }
        }

        public ActionResult TopicIndex(int forumId, int page)
        {
            string uri = "http://" + Request.Url.Host + ':' + Request.Url.Port + "/api/Topics?forumId=" + forumId + "&page=" + page;
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);

                return
                    View(
                            Task.Factory.StartNew
                            (
                                () => JsonConvert
                                        .DeserializeObject<List<TopicDTO>>(response.Result)
                            )
                            .Result
                         );
            }
        }

        public ActionResult TopicIndexBySearch(string searchBar)
        {
            string uri = "http://" + Request.Url.Host + ':' + Request.Url.Port + "/api/Topics?searchBar=" + searchBar;
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);

                return
                    View(
                            Task.Factory.StartNew
                            (
                                () => JsonConvert
                                        .DeserializeObject<List<TopicDTO>>(response.Result)
                            )
                            .Result
                         );
            }
        }

        public ActionResult PostIndex(int topicId, int page)
        {
            string uri = "http://" + Request.Url.Host + ':' + Request.Url.Port + "/api/Posts?topicId=" + topicId + "&page=" + page;
            using (HttpClient httpClient = new HttpClient())
            {

                Task<String> response = httpClient.GetStringAsync(uri);

                return
                    View(
                            Task.Factory.StartNew
                            (
                                () => JsonConvert
                                        .DeserializeObject<List<PostDTO>>(response.Result)
                            )
                            .Result
                         );
            }
        }


        [Authorize]
        public async Task<ActionResult> PostTopic(int forumId)
        {
            var findUserName = HttpContext.User.Identity;
            var user = await UserManager.FindByNameAsync(findUserName.Name);            

            PostTopicDTO2 tempPostTopicDto = new PostTopicDTO2();
            tempPostTopicDto.PostDto = new PostDTO();
            tempPostTopicDto.PostDto.PostedBy = user.UserName;

            tempPostTopicDto.TopicDto = new TopicDTO();
            tempPostTopicDto.TopicDto.ForumId = forumId;
            tempPostTopicDto.TopicDto.CreatedBy = user.UserName;

            return View(tempPostTopicDto);
        }

        [Authorize]
        public async Task<ActionResult> PostAPost(int topicId, string topicName)
        {
            var findUserName = HttpContext.User.Identity;
            var user = await UserManager.FindByNameAsync(findUserName.Name);
            bool isLocked = db.Topics.Where(t => t.Id == topicId).SingleOrDefault().Locked;


            PostDTO tempPostDto = new PostDTO { TopicId = topicId, TopicName = topicName };
            tempPostDto.PostedBy = user.UserName;
            if (!isLocked) { return View("partialPost", tempPostDto); }
            else return View("Locked");
        }

        [Authorize]
        public async Task<ActionResult> LockTopic(int topicId)
        {
            if(!db.Topics.Where(t => t.Id == topicId).FirstOrDefault().Locked) db.Topics.Where(t => t.Id == topicId).FirstOrDefault().Locked = true;
            
            else db.Topics.Where(t => t.Id == topicId).FirstOrDefault().Locked = false;

            await db.SaveChangesAsync();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [Authorize]
        public async Task<ActionResult> DeletePost(int postId)
        {
            db.Posts.Where(p => p.Id == postId).FirstOrDefault().isDeleted = true;
            await db.SaveChangesAsync();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [Authorize]
        public ActionResult EditPostV(int postId)
        {
            string uri = "http://" + Request.Url.Host + ':' + Request.Url.Port + "/api/Posts/" + postId;
            using (HttpClient httpClient = new HttpClient())
            {

                Task<String> response = httpClient.GetStringAsync(uri);
                
                return View("EditPost", Task.Factory.StartNew(() => JsonConvert.DeserializeObject<PostDTO>(response.Result)).Result);
            }
        }

        [Authorize]
        public async Task<ActionResult> Reply(int topicId, string topicName, int postId)
        {
            List<PostDTO> postList = new List<PostDTO>();
            PostDTO test = new PostDTO();

            var findUserName = HttpContext.User.Identity;
            var user = await UserManager.FindByNameAsync(findUserName.Name);
            bool isLocked = db.Topics.Where(t => t.Id == topicId).SingleOrDefault().Locked;

            PostDTO tempPostDto = new PostDTO { TopicId = topicId, TopicName = topicName };
            tempPostDto.PostedBy = user.UserName;
            tempPostDto.ReplyToPostId = postId;
            postList.Add(tempPostDto);

            if (!isLocked) { return View("PostAPost", tempPostDto); }
            else return View("Locked");
        }

        [ChildActionOnly]
        public PartialViewResult GetReply(int replyto)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Post, PostDTO>());
            PostDTO temp = db.Posts.Where(p => p.Id == replyto).ProjectTo<PostDTO>().SingleOrDefault();
            return PartialView("partialViewPost", temp);
        }

        [Authorize]
        public async Task<ActionResult> CreatePM(string sender, string reciever)
        {
            PMDTO tempPM = new PMDTO();

            var name = await UserManager.FindByNameAsync(sender);
            tempPM.SenderId = name.Id;
            tempPM.SenderName = name.UserName;

            name = await UserManager.FindByNameAsync(reciever);
            tempPM.RecieverId = name.Id;
            tempPM.RecieverName = name.UserName;            

            return View(tempPM);
        }

        [Authorize]
        public async Task<ActionResult> GetPMsForUser(string reciever)
        {
            var user = await UserManager.FindByNameAsync(reciever);
            string recieverId = user.Id;

            Mapper.Initialize(cfg => cfg.CreateMap<PersonalMessage, PMDTO>());
            var temp = db.PersonalMessages.Where(p => p.RecieverId == recieverId).ProjectTo<PMDTO>().ToList();
            
            foreach(PMDTO p in temp)
            {
                p.RecieverName = reciever;
                p.SenderName = UserManager.FindById(p.SenderId).UserName;
            }

            return View("PersonalMessages", temp);
        }

        public ActionResult PMConfirmation()
        {
            return View();
        }

        public PartialViewResult GetUnreadPMsForUserPV(string id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<PersonalMessage, PMDTO>());
            List<PMDTO> pms = db.PersonalMessages.Where(p => p.RecieverId == id && p.Read == false).ProjectTo<PMDTO>().ToList();

            return PartialView(pms.Count);
        }

        public void SetPmRead(int pmId)
        {
            db.PersonalMessages.Where(p => p.Id == pmId).SingleOrDefault().Read = true;
            db.SaveChangesAsync();            
        }
    }
}
