using System;
using MediatR;

namespace Monito.Application.Model.Query
{
    public class GetRequestStatusByUUIDQuery : IRequest<MinimalRequestStatusApplicationModel> {
        public Guid UUID { get; set; }
        public static GetRequestStatusByUUIDQuery Build(Guid uuid)
        {
            return new GetRequestStatusByUUIDQuery()
            {
                UUID = uuid
            };
        }
    }
}