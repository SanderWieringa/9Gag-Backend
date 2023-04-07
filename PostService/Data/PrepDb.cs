﻿using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.Data
{
    public static class PrepDb
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
                    context.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> Could not run migrations: {e.Message}");
                }

            }

            else 
            {
                if (!context.Posts.Any())
                {
                    Console.WriteLine("--> Seeding data...");

                    /* byte[] data = new byte[3];
                     data[0] = byte.MinValue;
                     data[1] = 0;
                     data[2] = byte.MaxValue;*/

                    //var photo = new Photo { PhotoId = 1, Photo = data };


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
}