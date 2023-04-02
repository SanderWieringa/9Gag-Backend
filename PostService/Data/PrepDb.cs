using PostService.Models;
using System;

namespace PostService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<PostDbContext>(), isProduction);
            }
        }

        private static void SeedData(PostDbContext context, bool isProduction)
        {
            if (isProduction)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    //context.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> Could not run migrations: {e.Message}");
                }

            }

            if (!context.Posts.Any())
            {
                Console.WriteLine("--> Seeding data 1...");

                context.Posts.AddRange(
                    new Post { PostId = 1, Title = "Test", Image = "asdf", Photo = null }
                ); ;

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data!");
            }
        }
    }
}
