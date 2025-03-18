using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using SmKPlugins.Options;
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


const string modelOllama = @"llama3.1:8b";
const string endpointOllama = "http://localhost:11434";

const string smtpServer = "smtp.gmail.com";
const int port = 587;
const string username = "app.netcore2025@gmail.com";
const string password = "ndxrdwhgwrtqmxyu";


var emailPlugin = new EmailPlugin(smtpServer, port, username, password);

var connectionString = new CustomerPlugin("Data Source=.;Initial Catalog=BD_VentaWeb;User ID=sa;Password=Pedro.1990;TrustServerCertificate=true;");

/*
const string modelo = @"gpt-4o-mini";
const string endpoint = "https://david-m5e7j0ru-swedencentral.openai.azure.com/";
const string keys = "G5zWmPx8q7mSueyQjCuox74nvrZvNc3ngLcKP692iyejAPCh43RGJQQJ99BAACfhMk5XJ3w3AAAAACOGewmJ";
*/



//*******************  Ollama  *********************************************************
var kernelBuilder = Kernel.CreateBuilder()
                          .AddOllamaChatCompletion(modelOllama, new Uri(endpointOllama));



//kernelBuilder.Plugins.AddFromType<LightsPlugin>("Lights");
//kernelBuilder.Plugins.AddFromType<TimeInformationPlugin>("Time");
kernelBuilder.Plugins.AddFromObject(connectionString, "CustomerPlugin");
kernelBuilder.Plugins.AddFromObject(emailPlugin, "EmailPlugin");


var kernel = kernelBuilder.Build();

//****************************************************************************************


//*******************  Azure OpenAI  ******************************************************

//builder.Services.AddOptions<AzureOpenAIOptions>()
//                .Bind(builder.Configuration.GetSection(nameof(AzureOpenAIOptions)))
//                .ValidateDataAnnotations()
//                .ValidateOnStart();

//builder.Services.AddTransient(serviceProvider =>
//{
//    var oaiOptions = serviceProvider.GetRequiredService<IOptions<AzureOpenAIOptions>>().Value;

//    var oaiClient = new AzureOpenAIClient(oaiOptions.Endpoint, new AzureKeyCredential(oaiOptions.Key), new AzureOpenAIClientOptions(oaiOptions.ServiceVersion));

//    var kernelBuilder = Kernel.CreateBuilder();

//    kernelBuilder.AddAzureOpenAIChatCompletion(oaiOptions.ChatModelName, oaiClient);


//    kernelBuilder.Plugins.AddFromType<LightsPlugin>("Lights");
//    kernelBuilder.Plugins.AddFromType<TimeInformationPlugin>("Time");

//    var kernel = kernelBuilder.Build();

//    return kernel;
//});
//****************************************************************************************

using var cancellationTokenSource = new CancellationTokenSource();
using var host = builder.Build();

//var kernel = host.Services.GetRequiredService<Kernel>();


const string MessageSystem = "Eres una IA con la personalidad de Bender Bending Rodríguez.";
//const string MessageSystem = "Eres una asistente IA experto en genera email. El cuerpo del email debe contener formato HTML usando negritas , etc. El mensaje debe tener un tono profesional.";

await Chat(kernel, cancellationTokenSource.Token);

//await UseKernel(kernel, cancellationTokenSource.Token);
//await UseKerneAzureOpenAI(kernel, cancellationTokenSource.Token);

await host.RunAsync(cancellationTokenSource.Token);

static async Task Chat(Kernel kernel, CancellationToken cancellationToken)
{

    ChatHistory chatHistory = new();

    Console.WriteLine("""
    Haz preguntas a la IA o da instrucciones como:
    - ¿Está encendida la luz?
    - Apaga la luz, por favor.
    - Dime la hora actual
    -Evia un email a: 

    """
    );

    while (true)
    {

        chatHistory.AddSystemMessage(MessageSystem);

        Console.Write(@"User: ");
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

        Console.Write(@"Assistem IA: ");
        Console.Write(response);

        Console.WriteLine(string.Empty);
    }

}

static async Task UseKernel(Kernel kernel, CancellationToken cancellationToken)
{
    const string UserMessage = "da la hora actual, y enciende las luces";

    var stopwatch = Stopwatch.StartNew();

    ChatHistory chatHistory = new();
    chatHistory.AddUserMessage(UserMessage);

    IChatCompletionService chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

    //OllamaPromptExecutionSettings settings = new()
    PromptExecutionSettings settings = new()
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
    };

    ChatMessageContent response = await chatCompletionService.GetChatMessageContentAsync(chatHistory, settings, kernel);

    stopwatch.Stop();

    ShowKernelResults(response, chatHistory, stopwatch.Elapsed.Seconds);
}
static async Task UseKerneAzureOpenAI_Ollama(Kernel kernel, CancellationToken cancellationToken)
{
    const string UserMessage = "da la hora actual, y enciende las luces";

    var stopwatch = Stopwatch.StartNew();

    ChatHistory chatHistory = new();
    chatHistory.AddUserMessage(UserMessage);

    IChatCompletionService chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

    // OllamaPromptExecutionSettings settings = new()
    // AzureOpenAIPromptExecutionSettings settings = new()
    PromptExecutionSettings settings = new()
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
    };


    var result = await chatCompletionService.GetChatMessageContentAsync(chatHistory, settings, kernel, cancellationToken);

    stopwatch.Stop();

    ShowKernelResults(result, chatHistory, stopwatch.Elapsed.Seconds);

}

static void ShowKernelResults(ChatMessageContent result, ChatHistory plan, int elapsedSeconds)
{
    ShowPlanFromChatHistory(plan);

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("\nKernel execution result:");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(result);

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\nKernel execution total time: {elapsedSeconds} seconds");

    Console.ResetColor();
}

static void ShowPlanFromChatHistory(ChatHistory chatHistory)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("The plan is:\n");
    foreach (var item in chatHistory)
    {
        ConsoleColor roleColor;

        if (item.Role == AuthorRole.System)
        {
            roleColor = ConsoleColor.DarkBlue;
        }
        else if (item.Role == AuthorRole.Tool)
        {
            roleColor = ConsoleColor.DarkMagenta;
        }
        else if (item.Role == AuthorRole.User)
        {
            roleColor = ConsoleColor.DarkGreen;
        }
        else if (item.Role == AuthorRole.Assistant)
        {
            roleColor = ConsoleColor.DarkYellow;
        }
        else
        {
            roleColor = ConsoleColor.DarkGray;
        }

        Console.BackgroundColor = roleColor;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{item.Role.Label}");

        Console.ResetColor();
        Console.ForegroundColor = roleColor;
        Console.WriteLine($"\n{item.Content!}\n");

        Console.ResetColor();
    }
}


//#pragma warning restore S3903 // Types should be defined in named namespaces

#pragma warning restore SKEXP0060
#pragma warning disable SKEXP0050
#pragma warning restore SKEXP0010
#pragma warning restore SKEXP0001

#pragma warning restore SKEXP0070