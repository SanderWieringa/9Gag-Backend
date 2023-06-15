using PostService.Dtos;

namespace PostService.AsyncDataServices
{
    public interface IRabbitMQProducer
    {
        void PublishNewPost(PostPublishedDto postPublishedDto);
    }
}
