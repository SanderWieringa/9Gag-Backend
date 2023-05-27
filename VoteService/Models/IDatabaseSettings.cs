namespace VoteService.Models
{
    public interface IDatabaseSettings
    {
        string PostCollectionName { get; set; }
        string VoteCollectionName { get; set; }
        string ConnectionString { get; set; }
        string PostDatabaseName { get; set; }
        string VoteDatabaseName { get; set; }
    }
}
