using MediatR;
using MiniTwitter.Model;
using MiniTwitter.Repository;

namespace MiniTwitter.CQRS.User.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
    {
        private readonly MiniTwitterDb db;
        public DeletePostCommandHandler(MiniTwitterDb db)
        {
            this.db = db;
        }

        public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            IQueryable<MiniTwitter.Model.Post> tmp = db.Posts.Where(s => s.Id == request.PostId);
            foreach (MiniTwitter.Model.Post post in tmp)
            {
                db.Posts.Remove(post);

            }
            await db.SaveChangesAsync();
        }
    }
}
