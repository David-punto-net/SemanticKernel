using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using SmKPlugins.Plugins;
using System.Diagnostics;


#pragma warning disable SKEXP0070
#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0050
#pragma warning disable SKEXP0060

//#pragma warning disable S3903 // Types should be defined in named namespaces

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());


const string modelOllama = @"";
const string endpointOllama = "";

const string smtpServer = "";
const int port = 587;
const string username = "";
const string password = "";


var emailPlugin = new EmailPlugin(smtpServer, port, username, password);


//*******************  Ollama  *********************************************************
var kernelBuilder = Kernel.CreateBuilder()
                          .AddOllamaChatCompletion(modelOllama, new Uri(endpointOllama));


kernelBuilder.Plugins.AddFromObject(emailPlugin, "EmailPlugin");


var kernel = kernelBuilder.Build();


using var cancellationTokenSource = new CancellationTokenSource();
using var host = builder.Build();

const string MessageSystem = "Eres una IA con la personalidad de Bender Bending Rodríguez.";

await Chat(kernel, cancellationTokenSource.Token);


await host.RunAsync(cancellationTokenSource.Token);

static async Task Chat(Kernel kernel, CancellationToken cancellationToken)
{

    ChatHistory chatHistory = new();

    Console.WriteLine("""
    Haz preguntas a la IA o da instrucciones como:
    -Envía un email a: 

    """
    );

    while (true)
    {

        chatHistory.AddSystemMessage(MessageSystem);

        Console.Write(@"Usuario: ");
        var userInput = Console.ReadLine();

        if (string.IsNullOrEmpty(userInput))
        {
            break;
        }

        chatHistory.AddUserMessage(userInput);


        IChatCompletionService chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        PromptExecutionSettings settings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
        };

        ChatMessageContent response = await chatCompletionService.GetChatMessageContentAsync(chatHistory, settings, kernel);

        chatHistory.AddAssistantMessage(response.ToString());

        Console.Write(@"IA: ");
        Console.Write(response);

        Console.WriteLine(string.Empty);
    }

}

#pragma warning restore SKEXP0060
#pragma warning disable SKEXP0050
#pragma warning restore SKEXP0010
#pragma warning restore SKEXP0001

#pragma warning restore SKEXP0070
