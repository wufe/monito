using GraphQL.Types;
using Monito.Application.Model;

namespace Monito.Application.Services.Graph.Type
{
    public class RequestOptionsType : ObjectGraphType<RequestOptionsApplicationModel> {
        public RequestOptionsType()
        {
            Field(t => t.Method);
            Field(t => t.Redirects);
            Field(t => t.Threads);
            Field(t => t.Timeout);
            Field(t => t.UserAgent);
        }
    }
}