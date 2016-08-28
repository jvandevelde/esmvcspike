using Nest;

namespace SkillsDatabase.Search.Api
{
    public interface IElasticsearchClientProvider
    {
        ElasticClient Instance { get; }
    }
}