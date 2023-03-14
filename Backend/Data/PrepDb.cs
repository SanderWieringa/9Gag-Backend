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

                var photo = new Photo { PhotoId = 1, Bytes = {} };


                context.Posts.AddRange(
                    
                    new Post { PostId = 1, Title = "Test", Photo = photo }
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
