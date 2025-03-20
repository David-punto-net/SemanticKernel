using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.Embeddings;
using Qdrant.Client;
using System.Text;



#pragma warning disable SKEXP0070
#pragma warning disable SKEXP0001


const string modelOllama = @"llama3.1:8b";
const string endpointOllama = "http://localhost:11434";
const string modelEmbedding = @"nomic-embed-text";

var qClient = new QdrantClient("localhost");

var kernelBuilder = Kernel.CreateBuilder()
                          .AddOllamaChatCompletion(modelOllama, new Uri(endpointOllama))
                          .AddOllamaTextEmbeddingGeneration(modelEmbedding, new Uri(endpointOllama));

var kernel = kernelBuilder.Build();


OllamaPromptExecutionSettings settings = new()
{
    Temperature = (float?)0.5
};

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
var embeddingService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

ChatHistory chatHistory = new();

Console.WriteLine("IA RAG - (Hazle preguntas a la IA sobre los documentos PDF cargados a BD Qdrant).");
while (true)
{
    Console.Write(@"Pregunta: ");
    var userQuestion = Console.ReadLine();

    var questionEmbedding = await embeddingService!.GenerateEmbeddingAsync(userQuestion);
    var vector = questionEmbedding.ToArray();

    // Buscar en Qdrant
    var searchResults = await qClient.SearchAsync(
        collectionName: "mi-database",
        vector: vector,
        limit: 25,
        payloadSelector: true,
        searchParams: null,
        timeout: null
    );


    var contextBuilder = new StringBuilder();
    foreach (var result in searchResults)
    {
        if (result.Payload.TryGetValue("text", out var textPayload))
        {
            contextBuilder.AppendLine(textPayload.StringValue);
        }
    }

    var context = contextBuilder.ToString();

    chatHistory.AddSystemMessage(@$"Eres un asistente inteligente y alegre que prioriza las respuestas a las preguntas de los usuarios utilizando los datos de esta conversación. 
                Si no sabes la respuesta, di 'No tengo información.'.'. 
                Responde la siguinete pregunta: 
                
                [Question]
                {userQuestion}

                Prioriza los siguientes datos para responder la pregunta:
                [Data]
                {contextBuilder}
    ");

    chatHistory.AddUserMessage(userQuestion!);


    var response = await chatCompletionService.GetChatMessageContentAsync(chatHistory);
    Console.WriteLine($"\nAsistente IA: {response.Content}");

    Console.WriteLine(string.Empty);
}


#pragma warning restore SKEXP0070
#pragma warning restore SKEXP0001
