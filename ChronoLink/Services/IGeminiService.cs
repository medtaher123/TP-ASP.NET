namespace ChronoLink.Services
{
    public interface IGeminiService
    {
        string AskGemini(string prompt, string question);
    }
}
