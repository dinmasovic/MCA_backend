using MediatR;
using MiniTwitter.Dto;

namespace MiniTwitter.CQRS.Post.GetPosts
{
    public record GetPostsQuery() : IRequest<List<DisplayPostDto>>
    {
    }
}
