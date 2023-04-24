namespace AuthorizationService.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LoginProviderId { get; set; }
        public string Email { get; set; }
    }
}
