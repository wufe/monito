using System;
using System.Collections;
using System.Collections.Generic;
using MediatR;

namespace Monito.Application.Model.Query
{
    public class GetRequestLinksByUUIDQuery : IRequest<IEnumerable<MinimalLinkApplicationModel>> {
        public Guid UUID { get; set; }
        public static GetRequestLinksByUUIDQuery Build(Guid uuid) {
            return new GetRequestLinksByUUIDQuery() {
                UUID = uuid
            };
        }
    }
}