// Forum controller for anonymous discussion forum
// uses attribute based routing
// /forum/all, /forum/id/2, /forum/last/3 etc.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using RESTful_Forum_Service.Models;

namespace RESTful_Forum_Service.Controllers
{
    [RoutePrefix("forum")]
    public class ForumController : ApiController
    {
        // posts for this forum - in-memory
        private static List<ForumPost> posts = new List<ForumPost>();

        [Route("all")]
        [HttpGet]
        public IHttpActionResult GetAllPosts()
        {
            lock (posts)                 // make thread safe
            {
                return Ok(posts);
            }
        }


        // GET forum/id/1
        [Route("id/{id:int}")]
        [HttpGet]
        public IHttpActionResult GetPostById(int id)
        {
            lock (posts)
            {
                // LINQ query, find post for matching id
                ForumPost post = posts.FirstOrDefault(p => p.Id == id);
                if (post == null)
                {
                    return NotFound();
                }
                return Ok(post);
            }
        }


        // POST /forum
        [Route("")]
        [HttpPost]
        public IHttpActionResult PostAddPost(UserPost userPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ForumPost forumPost;
            lock (posts)
            {
                // determine next ID
                int id;
                if (posts.Count == 0)
                {
                    id = 0;
                }
                else
                {
                    id = posts[posts.Count - 1].Id + 1;
                }
                forumPost = new ForumPost()
                {
                    Id = id,
                    UserPost = userPost,
                    TimeStamp = DateTime.Now
                };
                posts.Add(forumPost);
            }
            // create http response with Created status code and forum post serialised as content and Location header set to URI for new resource
            string uri = Request.RequestUri.ToString() + "/id/" + forumPost.Id;
            return Created(uri, forumPost);
        }


        // GET forum/last/5
        // get last 5 posts (or less if there aren't 5) in ID order newest to oldest
        [Route("last/{count:min(1)}")]
        [HttpGet]
        public IHttpActionResult GetLastPosts(int count)
        {
            var recentPosts = posts.OrderByDescending(p => p.Id).Take(count);
            return Ok(recentPosts.ToList());
        }
    }
}
