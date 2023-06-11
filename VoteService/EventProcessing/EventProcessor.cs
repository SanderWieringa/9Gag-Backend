using AutoMapper;
using MongoDB.Bson.IO;
using System.Text;
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

        public class StringFile
        {
            public string FileName { get; set; }
            public string Content { get; set; }
        }

        private void AddPost(string postPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IVoteRepo>();
                /*StringFile stringFile = JsonSerializer.Deserialize<StringFile>(postPublishedMessage);
                Console.WriteLine("stringFile: ", stringFile);
                byte[] fileBytes = Encoding.UTF8.GetBytes(stringFile.Content);
                Console.WriteLine("fileBytes: ", fileBytes);
                IFormFile imageFile;

                byte[] bytes = Encoding.UTF8.GetBytes(postPublishedMessage);

                using (var memoryStream = new MemoryStream())
                {
                    imageFile = new FormFile(memoryStream, 0, bytes.Length, null, ".png");
                }*/

                PostPublishedDto postPublishedDto = JsonSerializer.Deserialize<PostPublishedDto>(postPublishedMessage);

                try
                {
                    PostModel postmodel = new PostModel(postPublishedDto);
                    var post = _mapper.Map<Post>(postmodel);
                    PostDbDto postDto = new PostDbDto(post);
                    if (!repo.ExternalPostExists(postDto.ExternalId))
                    {
                        repo.CreatePost(postDto);
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
