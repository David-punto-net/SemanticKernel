using Azure.AI.OpenAI;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;



namespace SmKPlugins.Options
{
    public class AzureOpenAIOptions
    {
        public AzureOpenAIClientOptions.ServiceVersion ServiceVersion { get; set; } = AzureOpenAIClientOptions.ServiceVersion.V2024_10_01_Preview;

        public Uri Endpoint { get; init; }
        public string Key { get; init; }

        public string ChatModelDeploymentName { get; init; }

        public string ChatModelName { get; init; }

        public string ImageGenerationModelDeploymentName { get; init; }

        public string ImageGenerationModelName { get; init; }
    }
}

