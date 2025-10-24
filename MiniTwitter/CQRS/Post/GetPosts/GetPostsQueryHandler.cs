using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniTwitter.Dto;
using MiniTwitter.Repository;

namespace MiniTwitter.CQRS.Post.GetPosts
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, List<DisplayPostDto>>
    {   
        private readonly MiniTwitterDb db;
        public GetPostsQueryHandler(MiniTwitterDb db) {
            this.db = db;
        }
        public async Task<List<DisplayPostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await db.Posts.Include(s=>s.user).ToListAsync(cancellationToken);
            return DisplayPostDto.toDto(posts);
        }
    }
}
