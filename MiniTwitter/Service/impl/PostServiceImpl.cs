using Microsoft.EntityFrameworkCore;
using MiniTwitter.Model;
using MiniTwitter.Repository;

namespace MiniTwitter.Service.impl
{
    public class PostServiceImpl : PostService
    {
        private readonly MiniTwitterDb db;

        public PostServiceImpl(MiniTwitterDb db)
        {
            this.db = db;
        }

        public List<Post> getPosts()
        {
            return db.Posts.Include(s=>s.user).ToList();
        }
    }
}
