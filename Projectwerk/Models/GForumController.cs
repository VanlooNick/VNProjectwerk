using Projectwerk.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Projectwerk.Controllers
{
    [RequireHttps]
    public class GForumController : Controller
    {
        public ActionResult Index()
        {
            string uri = "https://" + Request.Url.Host + ':' + Request.Url.Port + "/api/Forum";
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

        public ActionResult TopicIndex(int forumId)
        {
            string uri = "https://" + Request.Url.Host + ':' + Request.Url.Port + "/api/Topics?forumId=" + forumId;
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

        public ActionResult PostIndex(int topicId)
        {
            string uri = "https://" + Request.Url.Host + ':' + Request.Url.Port + "/api/Posts?topicId=" + topicId;
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

        //public ActionResult PostTopic(int forumId)
        //{
        //    TopicDTO tempTopicDto = new TopicDTO {ForumId = forumId };
        //    return View(tempTopicDto);
        //}

        public ActionResult PostTopic(int forumId)
        {
            PostTopicDTO2 tempPostTopicDto = new PostTopicDTO2();
            tempPostTopicDto.PostDto = new PostDTO();
            tempPostTopicDto.TopicDto = new TopicDTO();
            tempPostTopicDto.TopicDto.ForumId = forumId;
            return View(tempPostTopicDto);
        }

        public ActionResult PostAPost(int topicId)
        {
            PostDTO tempPostDto = new PostDTO { TopicId = topicId };
            return View(tempPostDto);
        }
    }
}
