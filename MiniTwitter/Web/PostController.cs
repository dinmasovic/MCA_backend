using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniTwitter.CQRS.Post.GetPosts;
using MiniTwitter.Dto;

namespace MiniTwitter.Web
{

    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<List<DisplayPostDto>>> getPosts()
        {
            var result = await _mediator.Send(new GetPostsQuery());
            return Ok(result);
        }
    }
}
