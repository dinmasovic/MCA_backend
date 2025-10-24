using MediatR;
using MiniTwitter.Dto;

namespace MiniTwitter.CQRS.User.GetUserPosts
{
    public record GetUserPostsQuery(string username) : IRequest<List<DisplayPostDto>>
    {
    }
}
