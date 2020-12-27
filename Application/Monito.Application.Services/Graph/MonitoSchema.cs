using GraphQL.Types;

namespace Monito.Application.Services.Graph
{
    public class MonitoSchema : Schema {
        public MonitoSchema(
            MonitoQuery query
        )
        {
            Query = query;
        }
    }
}