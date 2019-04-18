using System;
using Nest;
using Fame.Search.Models;
using Fame.Common;
using Microsoft.Extensions.Options;

namespace Fame.Search
{
    public class ElasticSearch : IElasticSearch
    {
        private ElasticClient _elasticClient;
        
        public ElasticSearch(IOptions<FameConfig> fameConfig)
        {
            ConnectionUri = new Uri(fameConfig.Value.Elastic.ConnectionString);
            IndexName = fameConfig.Value.Elastic.SearchIndexName;
            ConnectionSettings = new ConnectionSettings(ConnectionUri)
                .DisableDirectStreaming()
                .DefaultMappingFor<ProductDocument>(m => m.IndexName(IndexName))
                .DefaultIndex(IndexName);
        }

        public string IndexName { get; }

        public Uri ConnectionUri { get; }

        public ConnectionSettings ConnectionSettings { get; }

        public ElasticClient Client => _elasticClient ?? (_elasticClient = new ElasticClient(ConnectionSettings));
    }

    public interface IElasticSearch
    {
        string IndexName { get; }
        ElasticClient Client { get; }
        ConnectionSettings ConnectionSettings { get; }
        Uri ConnectionUri { get; }
    }
}
