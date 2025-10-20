using MiniTwitter.Model;

namespace MiniTwitter.Service
{
    public interface UserService
    {
        public User getUserByName(string username);

        public User getUserById(int userId);
        public List<Post> getUserPosts(string username);
        public Post createPost(string content, int userId);

        public Post updatePost(string content, int postId);
        public void deletePost(int postId);

    }
}
