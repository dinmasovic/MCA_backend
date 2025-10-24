using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniTwitter.CQRS.User.GetUserPosts;
using MiniTwitter.Dto;
using MiniTwitter.Model;
using System.Threading.Tasks;

namespace MiniTwitter.Web
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("userPosts")]
        public async Task<ActionResult<List<DisplayPostDto>>> GetUserPosts()
        {
            List<DisplayPostDto> result =await _mediator.Send(new GetUserPostsQuery("user"));
            if (result == null)
            {
                return BadRequest("There is no user with the given username");
            }
            return Ok(result);
        }
        [HttpPost("createPost")]
        public async Task<ActionResult<DisplayPostDto>> createPost(CreatePostDto dto)
        {
            try
            {
                DisplayPostDto result = await _mediator.Send(new MiniTwitter.CQRS.User.CreatePost.CreatePostCommand(dto.content, dto.userId));
                return Ok(result);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("deletePost/{postId}")]
        public async Task<IActionResult> deletePost(int postId)
        {
            try
            {
                await _mediator.Send(new MiniTwitter.CQRS.User.DeletePost.DeletePostCommand(postId));
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
