using MediatR;
using MiniTwitter.Dto;

namespace MiniTwitter.CQRS.User.CreatePost
{
    public record CreatePostCommand(string content, int userId) : IRequest<DisplayPostDto>
    {
    }
}
