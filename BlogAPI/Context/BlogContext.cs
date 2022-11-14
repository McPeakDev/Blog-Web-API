namespace BlogAPI.Context {
    using BlogAPI.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System;

    /// <summary>
    /// Connects the DB with Entity Framework Core
    /// </summary>
    public class BlogContext
        : IdentityDbContext<IdentityUser> {

        /// <summary>
        /// Holds the application's configuration from appsettings.json
        /// </summary>
        protected readonly IConfiguration Configuration;


        /// <summary>
        /// Initializes the BlogContext with the appilcations configuration.
        /// </summary>
        public BlogContext(IConfiguration configuration){
            Configuration = configuration;
        }

        /// <summary>
        /// During Configuration, we need to make sure we connect to the DB.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder options){
            options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));
        }


        /// <summary>
        /// When performing migrations, make sure we scaffold the tables and users correctly. 
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            //Run the inherited model creation. 
            base.OnModelCreating(modelBuilder);

            //Verify we want to make a default login. 
            if (Configuration.GetValue<bool>("CreateDefaultLogin")) {
                var hasher = new PasswordHasher<IdentityUser>();

                //Get the config specified username and password.
                var username = Configuration["DefaultUsername"] ?? "admin";
                var password = Configuration["DefaultPassword"] ?? "admin";

                //Create the new user.
                var user = new IdentityUser {
                    Email = $"{username}@admin.com",
                    NormalizedEmail = $"{username.ToUpper()}@ADMIN.COM",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = username,
                    NormalizedUserName = username.ToUpper(),
                    EmailConfirmed = true
                };

                //Create the user's password.
                user.PasswordHash = hasher.HashPassword(user, password);

                //Save the user.
                modelBuilder.Entity<IdentityUser>().ToTable("Users", "dbo").HasData(user);
            }
        }


        /// <summary>
        /// Define the post table, so we can find posts.
        /// </summary>
        public DbSet<Post> Posts { get; set; }
    }
}
