using AuthorizationService.AsyncDataServices;
using AuthorizationService.Dtos;
using AuthorizationService.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace AuthorizationService.Data
{
    public class CaAuthorizationService : IAuthService
    {
        private readonly IUserRepo _repo;
        private readonly IMessageBusClient _messageBusClient;

        public CaAuthorizationService(IUserRepo repo, IMessageBusClient messageBusClient)
        {
            _repo = repo;
            _messageBusClient = messageBusClient;
        }

        public async Task<User> Authenticate(Payload payload)
        {
            return FindUserOrAdd(payload);
        }

        public User GetUserByEmail(Payload payload)
        {
            return _repo.GetAllUsers().Where(x => x.email == payload.Email).FirstOrDefault();
        }

        public void RemoveUser(string jwt)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = tokenHandler.ReadJwtToken(jwt);

            var id = token.Claims.First(user => user.Type == "userId").Value;
            ObjectId userId = _repo.GetAllUsers().Where(x => x.id == ObjectId.Parse(id)).FirstOrDefault().id;

            // Send Async Message
            try
            {
                UserRemovedDto userRemovedDto = new UserRemovedDto(id);
                /*var platformPublishedDto = _mapper.Map<PostPublishedDto>(postReadDto);*/
                userRemovedDto.Event = "User_Removed";
                _messageBusClient.RemoveUserPosts(userRemovedDto);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not send asynchronously: {e.Message}");
            }

            if (userId != null)
            {
                _repo.DeleteUser(userId);
            }
        }

        private User FindUserOrAdd(Payload payload)
        {
            var u = _repo.GetAllUsers().Where(x => x.email == payload.Email).FirstOrDefault();
            if (u == null)
            {
                u = new User()
                {
                    name = payload.Name,
                    email = payload.Email,
                    oauthSubject = payload.Subject,
                    oauthIssuer = payload.Issuer
                };
                _repo.CreateUser(u);
            }
            return u;
        }





        /*private readonly IAuthorizationContext authorizationContext;

        public async Task<string> AuthorizeAsync(AuthenticateResult authResult)
        {
            //  AuthResponseDto response = await _authHttpRequest.SendAuthRequest(code);
            //List<Claim> claims = authResult.Principal.Claims.ToList();

            var claims = authResult.Principal.Identities.FirstOrDefault()
                .Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value,
                    claim.Subject
                });
            var email = claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            var name = claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            // var identifier = claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/identifier").Value;
            //Check if Account already exists, and register one if this is not the case. Afterwards generate a JWT
            if (!CheckAccountExistsAsync(email).Result)
            {
                await CreateAccountAsync(email, name);
            }

            return null;//TokenGenerator.GenerateToken(JWTSettings.SecretKey, email);
        }

        public async Task<bool> CheckAccountExistsAsync(string email)
        {
            return authorizationContext.Users.FirstOrDefault(x => x.Email == email) != null;
        }

        private async Task<bool> CreateAccountAsync(string email, string name)
        {
            User newUser = new User()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email
            };
            authorizationContext.Users.Add(newUser);
            bool success = await authorizationContext.SaveChangesAsync() > 0;

            UserDto userDTO = newUser;

            //await _publisher.PublishMessageAsync<UserDTO>("UserCreatedEvent", userDTO);
            if (!success)
            {
                throw new InvalidOperationException("Something went wrong trying to create the account");
            }
            return true;
        }*/
    }
}
