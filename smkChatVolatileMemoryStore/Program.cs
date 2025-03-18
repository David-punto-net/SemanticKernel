using Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Plugins.Memory;
using smkChatVolatileMemoryStore;


#pragma warning disable SKEXP0070
#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0050


#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0060

const string modelOllama = @"llama3.1:8b";
const string endpointOllama = "http://localhost:11434";




//var kernelBuilder = Kernel.CreateBuilder()
//                           .AddAzureOpenAIChatCompletion("", "", "")
//                           .AddAzureOpenAITextEmbeddingGeneration("", "", "");

//kernelBuilder.AddAzureAISearchVectorStoreRecordCollection<TextSnippet<string>>("", new Uri(""), new AzureKeyCredential(""));

//var kernel = kernelBuilder.Build();



var kernelBuilder = Kernel.CreateBuilder()
                          .AddOllamaChatCompletion(modelOllama, new Uri(endpointOllama))
                          .AddOllamaTextEmbeddingGeneration(modelOllama, new Uri(endpointOllama));

var kernel = kernelBuilder.Build();


var embeddingGenerator = kernel.Services.GetRequiredService<ITextEmbeddingGenerationService>();
var memory = new SemanticTextMemory(new VolatileMemoryStore(), embeddingGenerator);

// Add some facts to the collection...
const string MemoryCollectionName = @"MyPersonalFacts";

await memory.SaveInformationAsync(MemoryCollectionName, id: Guid.NewGuid().ToString(), text: @"Information de ejemplo");
await memory.SaveInformationAsync(MemoryCollectionName, id: Guid.NewGuid().ToString(), text: @"Information de ejemplo");
await memory.SaveInformationAsync(MemoryCollectionName, id: Guid.NewGuid().ToString(), text: @"Information de ejemplo");
await memory.SaveInformationAsync(MemoryCollectionName, id: Guid.NewGuid().ToString(), text: @"Information de ejemplo");
await memory.SaveInformationAsync(MemoryCollectionName, id: Guid.NewGuid().ToString(), text: @"Information de ejemplo");
await memory.SaveInformationAsync(MemoryCollectionName, id: Guid.NewGuid().ToString(), text: @"Information de ejemplo");

TextMemoryPlugin memoryPlugin = new(memory);
kernel.ImportPluginFromObject(memoryPlugin);



OllamaPromptExecutionSettings settings = new()
{
    Temperature = (float?)0.0
};

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();


const string SystemPrompt = @"
You are a personal Artificial Intelligence assistant.
Respond with short and succinct answers.
The user’s question is: {{$input}}
Only if you don’t know the answer to a question, just reply 'I’m sorry, I don’t know that!'.
Only use the following memory content if makes sense to answer the question: {{Recall}}

";


while (true)
{
    Console.Write(@"Question: ");
    var userQuestion = Console.ReadLine();

    if (string.IsNullOrEmpty(userQuestion))
    {
        break;
    }

    var arguments = new KernelArguments(settings)
    {
        { "input", userQuestion.ToString() },
        { "collection", MemoryCollectionName },
    };

    var response = kernel.InvokePromptStreamingAsync(SystemPrompt, arguments);

    Console.Write(@"Answer: ");
    await foreach (var item in response)
    {
        Console.Write(item);
    }

    Console.WriteLine(string.Empty);
}


#pragma warning restore SKEXP0070
#pragma warning restore SKEXP0001
#pragma warning restore SKEXP0050

