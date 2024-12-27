using OpenAI.Threads;
using OpenAI;

namespace AIQueryGeneratorDemo.Services
{
    public class AssistantService(IConfiguration configuration) : IAssistantService
    {
        private readonly string apiKey = 
            configuration["OpenAI:ApiKey"] ?? throw new ArgumentException("Api key is not provided");

        private readonly string assistantId = 
            configuration["OpenAI:AssistantId"] ?? throw new ArgumentException("AssistantId is not provided");

        public async Task<string> ExecuteQuery(string query, string schema)
        {
            var _openAIClient = new OpenAIClient(apiKey);
            var thread = await _openAIClient.ThreadsEndpoint.CreateThreadAsync();

            var prompt =
                "You are an AI assistant that generates SQL (Transact-SQL) queries based on a given database schema and user input. " +
                $"Database schema: {schema}. " +
                $"User query: {query}. " +
                "Generate a SQL query that includes all fields from the requested entities. " +
                "Return the SQL query as a JSON object in this format: { \"query\": \"SELECT * FROM TableName\" }. " +
                "sql query must be in T-SQL (Transact-SQL). " +
                "The response must be plain text without additional formatting or explanations.";

            await _openAIClient.ThreadsEndpoint.CreateMessageAsync(thread.Id, new CreateMessageRequest(prompt));
            var run = await _openAIClient.ThreadsEndpoint.CreateRunAsync(thread.Id, new CreateRunRequest(assistantId));

            while (run.Status != RunStatus.Completed && run.Status != RunStatus.Failed)
            {
                await Task.Delay(1000);
                run = await _openAIClient.ThreadsEndpoint.RetrieveRunAsync(thread.Id, run.Id);
            }

            var messages = (await _openAIClient.ThreadsEndpoint.ListMessagesAsync(thread.Id)).Items;
            return messages.First().PrintContent();
        }
    }
}
