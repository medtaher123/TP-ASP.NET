namespace ChronoLink.Services
{
    public interface IGeminiService
    {
        Task<string> AskGeminiAsync(string prompt, string question);
    }
}
