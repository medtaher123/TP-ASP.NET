using Mscc.GenerativeAI;

namespace ChronoLink.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly string _apiKey;

        public GeminiService(IConfiguration configuration)
        {
            _apiKey = configuration["Gemini:ApiKey"];
        }

        public async Task<string> AskGeminiAsync(string prompt, string question)
        {
            var googleApi = new GoogleAI(_apiKey);
            var model = googleApi.GenerativeModel(Model.Gemini15Flash8B);
            var request = new GenerateContentRequest(prompt + ' ' + question);
            var response = await model.GenerateContent(request);
            return response?.Text ?? string.Empty;
        }
    }
}
