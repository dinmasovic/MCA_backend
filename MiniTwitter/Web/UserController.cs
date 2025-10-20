using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniTwitter.Dto;
using MiniTwitter.Model;
using MiniTwitter.Service;

namespace MiniTwitter.Web
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("userPosts")]
        public IActionResult GetUserPosts()
        {
            List<DisplayPostDto> userPosts = DisplayPostDto.toDto(userService.getUserPosts("user"));
            if (userPosts == null)
            {
                return BadRequest("There is no user with the given username");
            }
            return Ok(userPosts);
        }
        [HttpPost("createPost")]
        public IActionResult createPost(CreatePostDto dto)
        {
            try
            {
                Post tmp = userService.createPost(dto.content, dto.userId);
                return Ok(DisplayPostDto.toDto(tmp));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("deletePost/{postId}")]
        public IActionResult deletePost(int postId)
        {
            try
            {
                userService.deletePost(postId);
                return Ok("Post has been deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
    }
}
