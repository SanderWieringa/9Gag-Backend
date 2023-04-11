using PostService.Dtos;

namespace PostService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPost(PostPublishedDto postPublishedDto);
    }
}
