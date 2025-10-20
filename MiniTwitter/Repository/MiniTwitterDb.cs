using Microsoft.EntityFrameworkCore;
using MiniTwitter.Model;
using System;
namespace MiniTwitter.Repository;

public class MiniTwitterDb : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }

    public MiniTwitterDb(DbContextOptions<MiniTwitterDb> options) : base(options)
    {
    }

}
