using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using GraphQL.Utilities;
using Monito.Application.Model;
using Monito.Persistence.Model;
using Monito.Persistence.Repository.Interface;

namespace Monito.Application.Services.Graph.Type
{
    public class RequestGraphType : ObjectGraphType<MinimalRequestApplicationModel> {
        public RequestGraphType()
        {
            Field("id", t => t.ID);
            Field("uuid", t => t.UUID).Description("Unique ID of the request. Commonly used by the frontend instead of the ID.");
            Field(t => t.Status);
            Field(t => t.Type);
            Field(t => t.Options, type: typeof(RequestOptionsType));
            Field(t => t.CreatedAt);
            Field(t => t.UpdatedAt);

            Field<ListGraphType<LinkGraphType>>(
                "links",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType>() {
                        Name = "after"
                    },
                    new QueryArgument<IntGraphType>() {
                        Name = "take"
                    },
                    new QueryArgument<EnumerationGraphType<LinkStatus>>() {
                        Name = "status"
                    }
                ),
                resolve: context => {

                    var after = context.GetArgument<int?>("after");
                    var take = context.GetArgument<int?>("take");
                    var status = context.GetArgument<LinkStatus?>("status");
                    
                    var dataLoaderAccessor = context.RequestServices.GetRequiredService<IDataLoaderContextAccessor>();

                    var dataLoader = dataLoaderAccessor.Context.GetOrAddCollectionBatchLoader<int, LinkApplicationModel>(
                        "GetLinksByRequestID",
                        GetLinksForRequests(context.RequestServices, after, take, status)
                    );
                    return dataLoader.LoadAsync(context.Source.ID);
                }
            );
        }

        public Func<IEnumerable<int>, Task<ILookup<int, LinkApplicationModel>>> GetLinksForRequests(
            IServiceProvider serviceProvider,
            int? after,
            int? take,
            LinkStatus? status
        ) {
            return (requestIDs) => {

                var linksRepository = serviceProvider.GetRequiredService<IReadRepository<LinkPersistenceModel>>();
                var mapper = serviceProvider.GetRequiredService<IMapper>();

                var linksQuery = linksRepository
                    .FindAll(x => requestIDs.Contains(x.RequestID));

                if (after.HasValue) {
                    linksQuery = linksQuery
                        .Where(x => x.ID > after.Value);
                }

                if (status.HasValue) {
                    linksQuery = linksQuery
                        .Where(x => x.Status == status.Value);
                }

                if (take.HasValue) {
                    linksQuery = linksQuery
                        .Take(take.Value);
                }

                var lookup = linksQuery
                    .ProjectTo<LinkApplicationModel>(mapper.ConfigurationProvider)
                    .ToLookup(x => x.RequestID);
                return Task.FromResult(lookup);
            };
        }
    }
}