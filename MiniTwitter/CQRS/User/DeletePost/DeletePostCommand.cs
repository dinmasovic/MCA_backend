using MediatR;

namespace MiniTwitter.CQRS.User.DeletePost
{
    public record DeletePostCommand(int PostId) : IRequest
    {
    }
}
