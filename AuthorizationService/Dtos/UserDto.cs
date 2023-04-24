using AuthorizationService.Models;

namespace AuthorizationService.Dtos
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public static implicit operator UserDto(User u)
        {
            UserDto user = new UserDto()
            {
                Name = u.Name,
                Email = u.Email
            };
            return user;
        }
    }
}
