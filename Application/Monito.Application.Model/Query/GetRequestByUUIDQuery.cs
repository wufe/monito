using System;
using MediatR;

namespace Monito.Application.Model.Query
{
    public class GetRequestByUUIDQuery : IRequest<MinimalRequestWithDoneLinksCountApplicationModel> {
        public Guid UUID { get; set; }
        public static GetRequestByUUIDQuery Build(Guid uuid) {
            return new GetRequestByUUIDQuery() {
                UUID = uuid
            };
        }
    }
}