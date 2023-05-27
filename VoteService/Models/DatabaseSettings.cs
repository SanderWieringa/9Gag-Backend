namespace VoteService.Models
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string PostCollectionName { get; set; } = String.Empty;
        public string VoteCollectionName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
        public string PostDatabaseName { get; set; } = String.Empty;
        public string VoteDatabaseName { get; set; } = String.Empty;
    }
}
