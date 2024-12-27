namespace AIQueryGeneratorDemo.Services
{
    public interface IAssistantService
    {
        Task<string> ExecuteQuery(string query, string schema);
    }
}
