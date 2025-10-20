using System;

namespace MiniTwitter.Model;

public class User
{
    public int Id { get; private set; }
    public string Username { get; set; }

    public List<Post> Posts { get; set; } = new List<Post>();

    public User() { }
    public User(string Username)
    {
        this.Username = Username;
    }

    public User(string Username, int id)
    {
        this.Username = Username;
        this.Id = id;
    }
}
