using Microsoft.Extensions.AI;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using UglyToad.PdfPig;


var qClient = new QdrantClient("localhost");

IEmbeddingGenerator<string, Embedding<float>> generator =
    new OllamaEmbeddingGenerator(new Uri("http://localhost:11434/"), "nomic-embed-text");



Console.WriteLine($"cargando PDFs...");

string pdfDirectoryPath = ""; // Inserta la ruta de los PDF que se cargaran Qdrant

var pdfFiles = Directory.GetFiles(pdfDirectoryPath, "*.pdf");


var qdrantRecords = new List<PointStruct>();

foreach (var pdfFile in pdfFiles)
{

    var text = ExtractTextFromPdf(pdfFile);

    var embedding = (await generator.GenerateAsync(new List<string> { text }))[0].Vector.ToArray();


    var record = new PointStruct
    {
        Id = new PointId((uint)new Random().Next(0, 10000000)),
        Vectors = embedding,
        Payload =
            {
                ["file_name"] = Path.GetFileName(pdfFile),
                ["text"] = text
            }
    };

    qdrantRecords.Add(record);
}

await qClient.CreateCollectionAsync("mi-database", new VectorParams { Size = 768, Distance = Distance.Cosine });
await qClient.UpsertAsync("mi-database", qdrantRecords);
Console.WriteLine("Carga de PDF finalizada!");


string ExtractTextFromPdf(string pdfFilePath)
{
    using (var pdfDocument = PdfDocument.Open(pdfFilePath))
    {
        var text = string.Empty;
        foreach (var page in pdfDocument.GetPages())
        {
            text += page.Text;
        }
        return text;
    }
}