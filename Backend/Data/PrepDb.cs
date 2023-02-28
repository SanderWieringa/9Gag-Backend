using Backend.Models;
using System;

namespace Backend.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<PostDbContext>());
            }
        }

        private static void SeedData(PostDbContext context)
        {
            if (!context.Posts.Any())
            {
                Console.WriteLine("--> Seeding data...");

                context.Posts.AddRange(
                    new Post { Id = 1, Title = "Test" }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data!");
            }
        }
    }
}
