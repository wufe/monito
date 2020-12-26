using System.Collections.Generic;
using MediatR;

namespace Monito.Application.Model.Query
{
    public class GetDoneRequestLinksByRequestIDAfterIDQuery : IRequest<IEnumerable<MinimalLinkApplicationModel>> {
        public int RequestID { get; set; }
        public int LinkID { get; set; }
        public static GetDoneRequestLinksByRequestIDAfterIDQuery Build(int requestID, int linkID) {
            return new GetDoneRequestLinksByRequestIDAfterIDQuery() {
                LinkID = linkID,
                RequestID = requestID
            };
        }
    }
}