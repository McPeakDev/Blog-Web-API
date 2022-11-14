using BlogAPI.Models;
using BlogAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BlogAPI.Controllers {


    /// <summary>
    /// Controller for CRUD functions on Posts for the Blog Web API.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase {


        /// <summary>
        /// DB context for access to posts.
        /// </summary>
        private readonly BlogContext _db;


        /// <summary>
        /// Error logger for reporting issues with posts.
        /// </summary>
        private readonly ILogger<PostsController> _logger;


        /// <summary>
        /// Initializes the PostsController with a logger and DB context.
        /// </summary>
        public PostsController(ILogger<PostsController> logger, BlogContext db) {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Returns all Blog Posts.
        /// </summary>
        [HttpGet]
        public IActionResult GetAll() {

            //Create a new Response
            Response response = new Response();

            //Get all Posts
            DbSet<Post> posts = _db.Posts;

            //Verify that Posts actually exist
            if (posts.Count() > 0){
                response.Data = posts;
            } else {
                response.StatusCode = 404;
                response.Message = "No Posts are available";
            }
    
            //Respond back to the client.
            return response;
        }

        /// <summary>
        /// Creates a Blog Post.
        /// </summary>
        [HttpPost]
        [Route("create")]
        public IActionResult Create(Post post) {

            //Create a new Response
            Response response = new Response();

            //Verify the model is valid.
            if (ModelState.IsValid) {

                //Add the post to the posts.
                _db.Posts.Add(post);

                //Make sure the DB saves the post.
                var success = _db.SaveChanges();

                //Verify we saved it succesfully.
                if (success == 1){
                    response.Message = "Post has successfully been created!";
                } else{
                    response.StatusCode = 500;
                    response.Error = "Post failed to be created.";
                    _logger.LogError(response.Error, post);
                }
            }

            //Respond back to the client.
            return response;
        }

        /// <summary>
        /// Updates an existing Blog Post.
        /// </summary>
        [HttpPut]
        [Route("update")]
        public IActionResult Update(Post post) {

            //Create a new Response
            Response response = new Response();

            //Verify the model is valid.
            if (ModelState.IsValid) {

                //Update the post with the new one.
                _db.Posts.Update(post);

                //Make sure the DB saves the post.
                var success = _db.SaveChanges();

                //Verify we updated it succesfully.
                if (success == 1) {
                    response.Message = "Post has been updated successfully!";
                } else {
                    response.StatusCode = 500;
                    response.Error = "Post failed to be updated.";
                    _logger.LogError(response.Error, post);
                }
            }

            //Respond back to the client.
            return response;
        }


        /// <summary>
        /// Deletes an existing Blog Post.
        /// </summary>
        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(Post post) {

            //Create a new Response
            Response response = new Response();

            //Verify the model is valid.
            if (ModelState.IsValid) {

                //Delete the post.
                _db.Posts.Remove(post);

                //Make sure the DB saves the post.
                var success = _db.SaveChanges();

                //Verify we deleted it succesfully.
                if (success == 1) {
                    response.Message = "Post has been deleted successfully!";
                } else {
                    response.StatusCode = 500;
                    response.Error = "Post failed to be deleted.";
                    _logger.LogError(response.Error, post);
                }
            }

            //Respond back to the client.
            return response;
        }
    }
}
