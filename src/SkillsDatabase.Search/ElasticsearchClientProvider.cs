using Elasticsearch.Net;
using Nest;
using System;
using System.Diagnostics;
using System.Text;

namespace SkillsDatabase.Search.Api
{
    public class ElasticsearchClientProvider : IElasticsearchClientProvider
    {
        private ElasticClient _esClient;
        private static Uri esNode1 = new Uri("http://172.28.128.10:9200");
        private static Uri esNode2 = new Uri("http://172.28.128.11:9200");
        private static Uri esNode3 = new Uri("http://172.28.128.12:9200");

        public ElasticClient Instance
        {
            get
            {
                if (_esClient == null)
                    _esClient = BuildElasticClientInstance();

                return _esClient;
            }
        }

        private ElasticClient BuildElasticClientInstance()
        {
            //var connectionPool = new SniffingConnectionPool(new[] { esNode1, esNode2, esNode3 });
            var connectionPool = new SniffingConnectionPool(new[] { esNode1 });
            var settings = new ConnectionSettings(connectionPool)
                .DefaultIndex("skills-spike")
                //.EnableTrace(true).ExposeRawResponse(true);
                .DisableDirectStreaming()
               
                .OnRequestCompleted(details =>
                {
                    Debug.WriteLine("### ES REQEUST ###");
                    if (details.RequestBodyInBytes != null) Debug.WriteLine(Encoding.UTF8.GetString(details.RequestBodyInBytes));
                    Debug.WriteLine("### ES RESPONSE ###");
                    if (details.ResponseBodyInBytes != null) Debug.WriteLine(Encoding.UTF8.GetString(details.ResponseBodyInBytes));
                })
                .PrettyJson();

            return new ElasticClient(settings);
        }
    }
}
