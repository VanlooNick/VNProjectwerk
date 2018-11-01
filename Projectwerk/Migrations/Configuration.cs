namespace Projectwerk.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Projectwerk.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using System.Data.Entity.Infrastructure;

    internal sealed class Configuration : DbMigrationsConfiguration<Projectwerk.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Projectwerk.Models.ApplicationDbContext";
        }

        protected override void Seed(Projectwerk.Models.ApplicationDbContext context)
        {
            string[] roles = new string[3] {"User","Administrator","Owner"};

            foreach(string s in roles)
            {
                if (!context.Roles.Any(r => r.Name == s))
                {
                    var store = new RoleStore<IdentityRole>(context);
                    var manager = new RoleManager<IdentityRole>(store);
                    var role = new IdentityRole { Name = s };

                    manager.Create(role);
                }
            }

            var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("P4ssword!");
            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser()
                {
                    Id = "3fdb19e9-069d-4183-aad6-b0e86c6e7822",
                    UserName = "Owner",
                    PasswordHash = password,
                    Email = "test@hotmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true
                });

            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);

            passwordHash = new PasswordHasher();
            password = passwordHash.HashPassword("P4ssword!");
            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser()
                {
                    Id = "3fdb19e9-069d-4183-aad6-b0e86c6e7823",
                    UserName = "Admin",
                    PasswordHash = password,
                    Email = "popostrous@hotmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    LockoutEnabled = true
                });
            
            do
            {
                saveFailed = false;

                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);   


            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            UserManager.AddToRole("3fdb19e9-069d-4183-aad6-b0e86c6e7822", "Owner");
            UserManager.AddToRole("3fdb19e9-069d-4183-aad6-b0e86c6e7823", "Administrator");

            context.Fora.AddOrUpdate(x => x.Id,
                new Forum() { Id = 1, ForumName = "Announcements" },
                new Forum() { Id = 2, ForumName = "General Discussions" },
                new Forum() { Id = 3, ForumName = "Technical Support" }
                );

            context.Topics.AddOrUpdate(x => x.Id,
                new Topic() { Id = 1, TopicName = "The Forum is working", ForumId = 1, CreatedBy = "Owner" },
                new Topic() { Id = 2, TopicName = "Something", ForumId = 2, CreatedBy = "Admin" },
                new Topic() { Id = 3, TopicName = "Huray", ForumId = 3, CreatedBy = "Owner" },
                new Topic() { Id = 4, TopicName = "Hello from belgium", ForumId = 1, CreatedBy = "Owner" },
                new Topic() { Id = 5, TopicName = "Just introducing myself", ForumId = 1, CreatedBy = "Owner" },
                new Topic() { Id = 6, TopicName = "Yet another topic", ForumId = 1, CreatedBy = "Owner" },
                new Topic() { Id = 7, TopicName = "Generic Topic", ForumId = 1, CreatedBy = "Owner" },
                new Topic() { Id = 8, TopicName = "Another one in the list", ForumId = 1, CreatedBy = "Owner" }
                );

            context.Posts.AddOrUpdate(x => x.Id,
                new Post() { Id = 1, ThePost = "Just writing something, doesn't really matter what it is.", TimePosted = DateTime.Now, TopicId = 1, PostedBy = "Owner" },
                new Post() { Id = 2, ThePost = "Just a silly post", TimePosted = DateTime.Now, TopicId = 2, PostedBy = "Admin" },
                new Post() { Id = 3, ThePost = "I don't really know", TimePosted = DateTime.Now, TopicId = 3, PostedBy = "Owner" },

                new Post() { Id = 4, ThePost = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", TimePosted = DateTime.Now, TopicId = 4, PostedBy = "Owner" },
                new Post() { Id = 5, ThePost = "Nullam nec mi vitae leo hendrerit fermentum ut non orci.", TimePosted = DateTime.Now, TopicId = 5, PostedBy = "Owner" },
                new Post() { Id = 6, ThePost = "Fusce laoreet ex in ipsum feugiat, eu condimentum quam feugiat.", TimePosted = DateTime.Now, TopicId = 6, PostedBy = "Owner" },
                new Post() { Id = 7, ThePost = "usce ornare, dui malesuada ultricies luctus, dolor libero rhoncus tellus, a molestie nibh leo non lectus.", TimePosted = DateTime.Now, TopicId = 7, PostedBy = "Owner" },
                new Post() { Id = 8, ThePost = "Morbi vulputate magna nec turpis facilisis malesuada at et ligula.", TimePosted = DateTime.Now, TopicId = 8, PostedBy = "Owner" },

                new Post() { Id = 9, ThePost = "Test post.", TimePosted = DateTime.Now, TopicId = 8, PostedBy = "Owner" },
                new Post() { Id = 10, ThePost = "Writing numero 3", TimePosted = DateTime.Now, TopicId = 8, PostedBy = "Owner" },
                new Post() { Id = 11, ThePost = "Hello", TimePosted = DateTime.Now, TopicId = 8, PostedBy = "Owner" },
                new Post() { Id = 12, ThePost = "Hello to you too", TimePosted = DateTime.Now, TopicId = 8, PostedBy = "Admin", ReplyToPostId = 11},
                new Post() { Id = 13, ThePost = "Hello once again", TimePosted = DateTime.Now, TopicId = 8, PostedBy = "Owner", ReplyToPostId = 12}
                );
        }
    }
}
