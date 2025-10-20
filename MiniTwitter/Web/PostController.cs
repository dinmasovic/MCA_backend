using Microsoft.AspNetCore.Mvc;
using MiniTwitter.Dto;
using MiniTwitter.Service;

namespace MiniTwitter.Web
{

    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService postService;

        public PostController(PostService postService)
        {
            this.postService = postService;
        }
        [HttpGet]
        public IActionResult getPosts()
        {
            return Ok(DisplayPostDto.toDto(postService.getPosts()));
        }
    }
}
