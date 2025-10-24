using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniTwitter.Dto;
using MiniTwitter.Repository;
using MiniTwitter.Model;

namespace MiniTwitter.CQRS.User.GetUserPosts
{
    public class GetUserPostsQueryHandler : IRequestHandler<GetUserPostsQuery, List<DisplayPostDto>>
    {
        private readonly MiniTwitterDb db;
        public GetUserPostsQueryHandler(MiniTwitterDb db)
        {
            this.db = db;
        }
        public async Task<List<DisplayPostDto>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
           MiniTwitter.Model.User tmp =await db.Users
                  .Include(u => u.Posts)
                  .FirstOrDefaultAsync(user => user.Username.Equals(request.username));

            if (tmp == null)
            {
                return null;
            }
            return DisplayPostDto.toDto(tmp.Posts);
        }
    }
}
