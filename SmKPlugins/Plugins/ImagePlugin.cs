using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextToImage;

#pragma warning disable SKEXP0001

namespace SmKPlugins.Plugins
{
    internal class ImagePlugin
    {
        private readonly IChatCompletionService chatCompletionService;
        private readonly ITextToImageService textToImageService;

        public ImagePlugin(Kernel kernel)
        {
            chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
            textToImageService = kernel.GetRequiredService<ITextToImageService>();
        }

        //[KernelFunction]
        //[Description("Creates an image from a text description")]
        //public async Task<string> CreateImageFromTextAsync(string description, CancellationToken cancellationToken)
        //{
        //    var imageTask = textToImageService.GenerateImageAsync(description, 1024, 1024, cancellationToken: cancellationToken);

        //    var systemPrompt = $@"Create a human response to indicate users that the image they requested has been created. The prompt for the image the user has requested is: {description}";

        //    var messageTask = chatCompletionService.GetChatMessageContentAsync(systemPrompt, new OpenAIPromptExecutionSettings()
        //    {
        //        MaxTokens = 50,
        //        Temperature = 1.0,
        //        TopP = 1.0,
        //    }, cancellationToken: cancellationToken);

        //    await Task.WhenAll(imageTask, messageTask);

        //    return $"{messageTask.Result.Content!} \n\n URL: {imageTask.Result}";
        //}

    }
}


#pragma warning restore SKEXP0001
