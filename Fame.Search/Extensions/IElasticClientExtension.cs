using System;
using System.Collections.Generic;
using System.Linq;
using Nest;

namespace Fame.Search.Extensions
{
    public static class IElasticClientExtension
    {
        public static IEnumerable<T> GetAll<T>(this IElasticClient client, QueryContainer query) where T : class
        {
            String scrollId = null;
            var hasMore = true;

            while (hasMore)
            {
                ISearchResponse<T> response;
                if (scrollId != null)
                {
                    response = client.Scroll<T>("5m", scrollId);
                }
                else
                {
                    response = client.Search<T>(new SearchRequest<T>
                    {
                        Query = query,
                        Scroll = "1m",
                        Size = 100
                    });
                }

                foreach (var document in response.Documents)
                {
                    yield return document;
                }


                hasMore = response.Documents.Any();
                scrollId = response.ScrollId;
            }
        }
    }
}
