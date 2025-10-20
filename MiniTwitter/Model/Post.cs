using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniTwitter.Model;

public class Post
{
    public int Id { get; private set; }
    [MaxLength(140)]
    public string content { get;  set; }
    public DateOnly created { get; private set; }

    [ForeignKey("User")]   
    public int UserId { get; set; }
    public User user { get; set; }

    public Post() { }
    public Post(string content, int userId)
    {
        this.content = content;
        created = DateOnly.FromDateTime(DateTime.Now);
        this.UserId = userId;
    }
    public Post(int id,string content, int userId)
    {   
        this.Id= id;
        this.content = content;
        created = DateOnly.FromDateTime(DateTime.Now);
        this.UserId = userId;
    }

}
