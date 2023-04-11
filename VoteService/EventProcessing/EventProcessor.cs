using AutoMapper;
using System.Text.Json;
using VoteService.Data;
using VoteService.Dtos;
using VoteService.Models;

namespace VoteService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PostPublished:
                    AddPost(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case "Post_Published":
                    Console.WriteLine("--> Post Published Event Detected");
                    return EventType.PostPublished;
                default:
                    Console.WriteLine("--> Could not determine event type");
                    return EventType.Undetermined;
            }
        }

        private void AddPost(string postPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IVoteRepo>();

                var platformPublishedDto = JsonSerializer.Deserialize<PostPublishedDto>(postPublishedMessage);

                try
                {
                    var post = _mapper.Map<Post>(platformPublishedDto);
                    if (!repo.ExternalPostExists(post.ExternalId))
                    {
                        repo.CreatePost(post);
                        repo.SaveChanges();
                        Console.WriteLine("--> Post added...");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> Could not add Post to DB {e.Message}");
                }
            }
        }
    }

    enum EventType
    {
        PostPublished,
        Undetermined
    }
}
