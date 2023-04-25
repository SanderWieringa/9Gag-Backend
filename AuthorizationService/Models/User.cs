namespace AuthorizationService.Models
{
    public class User
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string oauthSubject { get; set; }
        public string oauthIssuer { get; set; }


        /*public Guid Id { get; set; }
        public string Name { get; set; }
        public string LoginProviderId { get; set; }
        public string Email { get; set; }*/
    }
}
