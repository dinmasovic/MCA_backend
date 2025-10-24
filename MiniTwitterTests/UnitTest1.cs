using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniTwitter.Model;
using MiniTwitter.Repository;
using MiniTwitter.CQRS.Post.GetPosts;       
using MiniTwitter.CQRS.User.CreatePost;     
using MiniTwitter.CQRS.User.GetUserPosts;   
using MiniTwitter.CQRS.User.DeletePost;    
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection; 

namespace MiniTwitterTests
{
    public class MediatorTests
    {

        private MiniTwitterDb GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<MiniTwitterDb>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new MiniTwitterDb(options);
        }


        private IMediator GetMediator(MiniTwitterDb context)
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddSingleton(context);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePostCommandHandler).Assembly));
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IMediator>();
        }

  

        [Fact]
        public void TestGetPosts()
        {
            using var context = GetInMemoryDb();
            var mediator = GetMediator(context); 

            var user = new User("user");
            context.Users.Add(user);
            context.SaveChanges();

            mediator.Send(new CreatePostCommand("Hello this is my first post", user.Id)).Wait();
            mediator.Send(new CreatePostCommand("Hello this is my second post", user.Id)).Wait();
            mediator.Send(new CreatePostCommand("Hello this is my third post", user.Id)).Wait();

            var allPosts = mediator.Send(new GetPostsQuery()).Result;

            Assert.Equal(3, allPosts.Count);
        }

        [Fact]
        public void TestGetUserPosts()
        {
            using var context = GetInMemoryDb();
            var mediator = GetMediator(context);

            var user1 = new User("user");
            var user2 = new User("user2");
            context.Users.AddRange(user1, user2);
            context.SaveChanges();

            mediator.Send(new CreatePostCommand("User1 first post", user1.Id)).Wait();
            mediator.Send(new CreatePostCommand("User2 first post", user2.Id)).Wait();
            mediator.Send(new CreatePostCommand("User2 second post", user2.Id)).Wait();

            var user1Posts = mediator.Send(new GetUserPostsQuery("user")).Result;

            Assert.Single(user1Posts);
            Assert.Equal("User1 first post", user1Posts.First().content);
        }

        [Fact]
        public void TestCreatePost_Validation()
        {
            using var context = GetInMemoryDb();
            var mediator = GetMediator(context);
            var user = new User("user");
            context.Users.Add(user);
            context.SaveChanges();

            var ex = Assert.Throws<AggregateException>(() =>
                mediator.Send(new CreatePostCommand("Too short", user.Id)).Wait());

            Assert.Equal("Content must be between 12 and 140 characters.", ex.InnerException.Message);

            var validCommand = new CreatePostCommand("This is a valid post content", user.Id);
            var post = mediator.Send(validCommand).Result;

            Assert.Equal("This is a valid post content", post.content);
        }

        [Fact]
        public void TestDeletePost()
        {
            using var context = GetInMemoryDb();
            var mediator = GetMediator(context);
            var user = new User("user");
            context.Users.Add(user);
            context.SaveChanges();

            var post1 = mediator.Send(new CreatePostCommand("First post, lllllllllll", user.Id)).Result;
            var post2 = mediator.Send(new CreatePostCommand("Second post, lllllllllllll", user.Id)).Result;

            mediator.Send(new DeletePostCommand(post1.id)).Wait();

            var remainingPosts = context.Posts.ToList();
            Assert.Single(remainingPosts);
            Assert.Equal(post2.id, remainingPosts.First().Id);
        }
    }
}