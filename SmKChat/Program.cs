using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;


#pragma warning disable SKEXP0070


const string modelOllama = @"llama3.1:8b";
const string endpointOllama = "http://localhost:11434";


var kernelBuilder = Kernel.CreateBuilder()
                          .AddOllamaChatCompletion(modelOllama, new Uri(endpointOllama));

var kernel = kernelBuilder.Build();


OllamaPromptExecutionSettings settings = new()
{
    Temperature = (float?)0.5
};

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

ChatHistory chatHistory = new();



/**** App ****/
const string SystemPrompt = "Eres un agente de IA muy creativo, con una personalidad igual a la Bender Bending Rodríguez de Futurama.";

chatHistory.AddSystemMessage(SystemPrompt);


while (true)
{
    Console.Write(@"Question: ");
    var userQuestion = Console.ReadLine();

    if (string.IsNullOrEmpty(userQuestion))
    {
        break;
    }


    chatHistory.AddUserMessage(userQuestion);

    var response = chatCompletionService.GetStreamingChatMessageContentsAsync(chatHistory, settings, kernel);

    chatHistory.AddAssistantMessage(response.ToString());

    Console.Write(@"Answer: ");
    await foreach (var item in response)
    {
        Console.Write(item);
    }

    Console.WriteLine(string.Empty);
}


#pragma warning restore SKEXP0070
