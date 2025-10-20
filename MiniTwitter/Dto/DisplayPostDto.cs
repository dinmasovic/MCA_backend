using MiniTwitter.Model;

namespace MiniTwitter.Dto
{
    public record DisplayPostDto(int id,string content, DateOnly created, string username)
    {

        static public List<DisplayPostDto> toDto(List<Post> posts)
        {
            return posts.Select(s => new DisplayPostDto(s.Id,s.content, s.created, s.user.Username)).ToList();
        }

        static public DisplayPostDto toDto(Post post)
        {
            return new DisplayPostDto(post.Id,post.content, post.created, post.user.Username);
        }
    }
}
