using Microsoft.EntityFrameworkCore;
using MiniTwitter.Model;
using MiniTwitter.Repository;

namespace MiniTwitter.Service.impl
{
    public class UserServiceImpl : UserService
    {
        private readonly MiniTwitterDb db;
        public UserServiceImpl(MiniTwitterDb db) { 
            this.db = db;
        }
        public Post createPost(string content, int userId)
        {
            if (content.Length < 12)
            {
                throw new ArgumentException("Content must be between 12 and 140 characters.");

            }
            Post tmp = new Post(content, userId);
            db.Posts.Add(tmp);
            db.SaveChanges();

            tmp.user = db.Users.Find(tmp.UserId);

            return tmp;
        }

        public void deletePost(int postId)
        {
            IQueryable<Post> tmp = db.Posts.Where(s => s.Id == postId);
            foreach (Post post in tmp)
            {
                db.Posts.Remove(post);

            }
            db.SaveChanges();
        }

        public User getUserByName(string username)
        {
            return db.Users.FirstOrDefault(user => user.Username.Equals(username));
        }

        public User getUserById(int userId)
        {
            return db.Users.FirstOrDefault(user => user.Id.Equals(userId));
        }
        public List<Post> getUserPosts(string username)
        {
            User tmp = db.Users
                .Include(u => u.Posts)
                .FirstOrDefault(user => user.Username.Equals(username));

            if (tmp == null)
            {
                return null;
            }
            return tmp.Posts;
        }

        public Post updatePost(string content, int postId)
        {
            Post tmp = db.Posts.Find(postId);
            if (tmp == null)
            {
                throw new Exception("The post does not exist");
            }

            tmp.content = content;
            db.SaveChanges();
            return tmp;
        }
    }
}
