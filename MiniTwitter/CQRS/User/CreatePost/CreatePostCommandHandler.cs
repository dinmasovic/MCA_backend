using MediatR;
using MiniTwitter.Dto;
using MiniTwitter.Model;
using MiniTwitter.Repository;

namespace MiniTwitter.CQRS.User.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, DisplayPostDto>
    {
        private readonly MiniTwitterDb db;
        public CreatePostCommandHandler(MiniTwitterDb db)
        {
            this.db = db;
        }

        public async Task<DisplayPostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            if (request.content.Length < 12 || request.content.Length > 140)
            {
                throw new ArgumentException("Content must be between 12 and 140 characters.");

            }
            MiniTwitter.Model.Post tmp = new MiniTwitter.Model.Post(request.content, request.userId);
            await db.Posts.AddAsync(tmp);
            await db.SaveChangesAsync(cancellationToken);

            MiniTwitter.Model.User user = await db.Users.FindAsync(tmp.UserId, cancellationToken);

            if (user == null)
            {
                throw new InvalidOperationException($"Cannot create post: User with ID {tmp.UserId} not found.");
            }

            tmp.user = user;

            return DisplayPostDto.toDto(tmp);
        }
    }
}
