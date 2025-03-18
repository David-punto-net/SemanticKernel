﻿using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Connectors.AzureAISearch;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins.Memory;
using Azure.Search.Documents.Indexes.Models;
using Azure.AI.OpenAI;
using Azure;
using Microsoft.SemanticKernel.ChatCompletion;



#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0020
#pragma warning disable SKEXP0050



const string modelo = @"gpt-4o-mini";
const string endpoint = "https://david-m5e7j0ru-swedencentral.openai.azure.com/";
const string keys = "\"G5zWmPx8q7mSueyQjCuox74nvrZvNc3ngLcKP692iyejAPCh43RGJQQJ99BAACfhMk5XJ3w3AAAAACOGewmJ\"";

const string searchendpoint = "https://search-openaidemo.search.windows.net";
const string searckeys = "0nBDRolttltInbSF2qUh0efqg64AFRKVCvjNcefr2mAzSeBST7Y5";
const string searchindex = "text-index";



var kernelBuilder = Kernel.CreateBuilder()
                          .AddAzureOpenAIChatCompletion(modelo, endpoint, keys);


var kernel = kernelBuilder.Build();


var memoryBuilder = new MemoryBuilder();
memoryBuilder.WithOpenAITextEmbeddingGeneration(modelo, keys);

var aoaiSearchStore = new AzureAISearchMemoryStore(searchendpoint, searckeys);
memoryBuilder.WithMemoryStore(aoaiSearchStore);


var memory = memoryBuilder.Build();

var textSchemaBD = File.ReadAllText(@"Data/schemaDb.txt");


await memory.SaveInformationAsync(searchindex, id: "doc-1", text: textSchemaBD);


kernel.ImportPluginFromObject(new TextMemoryPlugin(memory));

var prompt = @"";


var setting = new OpenAIPromptExecutionSettings { MaxTokens = 500, Temperature = 0.8};
var chatFunction = kernel.CreateFunctionFromPrompt(prompt, setting);

var history = "";

var arguments = new KernelArguments
{
    {"fact1","esquema de la base de datos" },
    {TextMemoryPlugin.CollectionParam,searchindex},
    {TextMemoryPlugin.LimitParam,"1"},
    {TextMemoryPlugin.RelevanceParam,"0.7"},
    {history,history }
};









#pragma warning restore SKEXP0050
#pragma warning restore SKEXP0020
#pragma warning restore SKEXP0010
#pragma warning restore SKEXP0001
