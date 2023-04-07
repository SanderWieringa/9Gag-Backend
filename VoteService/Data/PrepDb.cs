using CommandService.Data;
using VoteService.Models;

namespace VoteService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), serviceScope.ServiceProvider.GetService<IVoteRepo>(), isProduction);
            }
        }

        private static void SeedData(AppDbContext context, IVoteRepo voteRepo, bool isProduction)
        {
            Console.WriteLine("Seeding new posts...");
            Console.WriteLine("production: ", isProduction);

        }
    }
}
