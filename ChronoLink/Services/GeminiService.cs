using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.AIPlatform.V1;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using AIPlatformValue = Google.Protobuf.WellKnownTypes.Value;

namespace ChronoLink.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly string _apiKey;

        public GeminiService(IConfiguration configuration)
        {
            _apiKey = configuration["Gemini:ApiKey"];
        }

        public string AskGemini(string prompt, string question)
        {
            var client = new PredictionServiceClientBuilder
            {
                Endpoint = "us-central1-aiplatform.googleapis.com",
                GoogleCredential = GoogleCredential.FromAccessToken(_apiKey),
            }.Build();

            var request = new PredictRequest
            {
                Endpoint =
                    $"projects/your-project-id/locations/us-central1/publishers/google/models/gemini",
                Instances =
                {
                    new AIPlatformValue
                    {
                        StructValue = new Struct
                        {
                            Fields =
                            {
                                {
                                    "prompt",
                                    new AIPlatformValue { StringValue = prompt }
                                },
                            },
                        },
                    },
                },
            };

            var response = client.Predict(request);
            return response.Predictions[0].StructValue.Fields["response"].StringValue;
        }
    }
}
