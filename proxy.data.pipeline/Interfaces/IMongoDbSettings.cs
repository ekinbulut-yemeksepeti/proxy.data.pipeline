namespace proxy.data.pipeline.Interfaces
{
    public interface IMongoDbSettings
    {
        string DatabaseName { get; set; }
        string Host { get; set; }
        int Port { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}