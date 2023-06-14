using AuthorizationService.Dtos;

namespace AuthorizationService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void RemoveUserPosts(UserRemovedDto userRemovedDto);
    }
}
