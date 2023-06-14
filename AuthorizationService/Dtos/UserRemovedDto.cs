using MongoDB.Bson;

namespace AuthorizationService.Dtos
{
    public class UserRemovedDto
    {
        public string UserId { get; set; }
        public string Event { get; set; }

        public UserRemovedDto(string userId)
        {
            UserId = userId;
        }
    }
}
