using Microsoft.EntityFrameworkCore;
using MiniTwitter.Model;
using MiniTwitter.Repository;
using MiniTwitter.Service.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MiniTwitterTests
{
    public class UserServiceTests
    {
        private MiniTwitterDb GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<MiniTwitterDb>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            return new MiniTwitterDb(options);
        }

        [Fact]
        public void TestGetPosts()
        {
            using var context = GetInMemoryDb();

            var user = new User("user");
            context.Users.Add(user);
            context.SaveChanges();

            var postService = new PostServiceImpl(context);
            var userService = new UserServiceImpl(context);

            userService.createPost("Hello this is my first post", user.Id);
            userService.createPost("Hello this is my second post", user.Id);
            userService.createPost("Hello this is my third post", user.Id);

            var allPosts = postService.getPosts();

            Assert.Equal(3, allPosts.Count);
        }

        [Fact]
        public void TestGetUserPosts()
        {
            using var context = GetInMemoryDb();

            var user1 = new User("user");
            var user2 = new User("user2");
            context.Users.AddRange(user1, user2);
            context.SaveChanges();

            var userService = new UserServiceImpl(context);

            userService.createPost("User1 first post", user1.Id);
            userService.createPost("User2 first post", user2.Id);
            userService.createPost("User2 second post", user2.Id);

            var user1Posts = userService.getUserPosts("user");

            Assert.Single(user1Posts);
            Assert.Equal("User1 first post", user1Posts.First().content);
        }

        [Fact]
        public void TestCreatePost_Validation()
        {
            using var context = GetInMemoryDb();
            var user = new User("user");
            context.Users.Add(user);
            context.SaveChanges();

            var userService = new UserServiceImpl(context);
            var ex = Assert.Throws<ArgumentException>(() =>
                userService.createPost("Too short", user.Id));

            Assert.Equal("Content must be between 12 and 140 characters.", ex.Message);

            var post = userService.createPost("This is a valid post content", user.Id);
            Assert.Equal("This is a valid post content", post.content);
        }

        [Fact]
        public void TestDeletePost()
        {
            using var context = GetInMemoryDb();
            var user = new User("user");
            context.Users.Add(user);
            context.SaveChanges();

            var userService = new UserServiceImpl(context);

            var post1 = userService.createPost("First post, lllllllllll", user.Id);
            var post2 = userService.createPost("Second post, lllllllllllll", user.Id);

            userService.deletePost(post1.Id);

            var remainingPosts = context.Posts.ToList();
            Assert.Single(remainingPosts);
            Assert.Equal(post2.Id, remainingPosts.First().Id);
        }
    }
}
