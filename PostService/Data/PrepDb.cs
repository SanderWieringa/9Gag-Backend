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
            /*
            if (!context.Posts.Any())
            {
                Console.WriteLine("--> Seeding data...");

                byte[] data = new byte[3];
                data[0] = byte.MinValue;
                data[1] = 0;
                data[2] = byte.MaxValue;

                //var photo = new Photo { PhotoId = 1, Photo = data };


                context.Posts.AddRange(
                    
                    new Post { PostId = 1, Title = "Test", Photo = null }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data!");
            }*/
        }
    }
}
