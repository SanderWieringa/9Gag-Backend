using MongoDB.Bson;
using PostService.Data;
using PostService.Dtos;
using PostService.Models;
using System.Text.Json;

namespace PostService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.UserRemoved:
                    RemovePosts(message);
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
                case "User_Removed":
                    Console.WriteLine("--> User Removed Event Detected");
                    return EventType.UserRemoved;
                default:
                    Console.WriteLine("--> Could not determine event type");
                    return EventType.Undetermined;
            }
        }

        private void RemovePosts(string userRemovedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IPostCollectionRepo>();

                UserRemovedDto userRemovedDto = JsonSerializer.Deserialize<UserRemovedDto>(userRemovedMessage);

                try
                {
                    /*IEnumerable<Post> postsToDelete = repo.GetAllPosts().Where(x => x.UserId == ObjectId.Parse(userRemovedDto.UserId));
                    if (postsToDelete.Any()) {
                        repo.DeletePosts(postsToDelete);
                    }*/

                    if (userRemovedDto.UserId != null)
                    {
                        repo.DeletePosts(ObjectId.Parse(userRemovedDto.UserId));
                    }
                    
                    /*PostModel postmodel = new PostModel(postPublishedDto);
                    var post = _mapper.Map<Post>(postmodel);
                    PostDbDto postDto = new PostDbDto(post);
                    if (!repo.ExternalPostExists(postDto.ExternalId))
                    {
                        repo.CreatePost(postDto);
                        Console.WriteLine("--> Post added...");
                    }*/
                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> Could not add Post to DB: {e.Message}");
                }
            }
        }

        enum EventType
        {
            UserRemoved,
            Undetermined
        }
    }
}
