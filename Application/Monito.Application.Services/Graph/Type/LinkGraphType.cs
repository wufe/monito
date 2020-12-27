using GraphQL.Types;
using Monito.Application.Model;

namespace Monito.Application.Services.Graph.Type
{
    public class LinkGraphType : ObjectGraphType<LinkApplicationModel> {
        public LinkGraphType()
        {
            Field("id", t => t.ID);
            Field("url", t => t.URL);
            Field(t => t.Status);
            Field(t => t.StatusCode);
        }
    }
}