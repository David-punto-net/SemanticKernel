using Microsoft.Extensions.VectorData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smkChatVolatileMemoryStore
{
    internal class TextSnippet<TKey>
    {
        [VectorStoreRecordKey]
        public required TKey Key { get; set; }

        [VectorStoreRecordData]
        public string? Text { get; set; }

        [VectorStoreRecordData]
        public string? ReferenceDescription { get; set; }

        [VectorStoreRecordData]
        public string? ReferenceLink { get; set; }

        [VectorStoreRecordVector(1536)]
        public ReadOnlyMemory<float> TextEmbedding { get; set; }
    }
}
