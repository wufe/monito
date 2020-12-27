using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GraphQL;
using GraphQL.Types;
using Monito.Application.Model;
using Monito.Persistence.Model;
using Monito.Persistence.Repository.Interface;
using Monito.Application.Services.Graph.Type;

namespace Monito.Application.Services.Graph
{
    public class MonitoQuery : ObjectGraphType {
        public MonitoQuery(
            IMapper mapper,
            IReadRepository<RequestPersistenceModel> requestRepository
        )
        {
            Field<ListGraphType<RequestGraphType>>(
                "requests",
                resolve: context =>
                    requestRepository
                        .FindAll()
                        .ProjectTo<MinimalRequestApplicationModel>(mapper.ConfigurationProvider)
            );

            Field<RequestGraphType>(
                "request",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>() {
                        Name = "id"
                    }
                ),
                resolve: context => {
                    var id = context.GetArgument<int>("id");
                    return requestRepository
                        .FindAll(x => x.ID == id)
                        .ProjectTo<MinimalRequestApplicationModel>(mapper.ConfigurationProvider)
                        .FirstOrDefault();
                }
            );
        }
    }
}