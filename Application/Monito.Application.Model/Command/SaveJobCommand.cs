using System;
using MediatR;

namespace Monito.Application.Model.Command
{
    public class SaveJobCommand : IRequest<Guid> {
        public string Links { get; set; }
        public SaveJobCommandHttpMethod Method { get; set; }
        public int Redirects { get; set; }
        public int Threads { get; set; }
        public int Timeout { get; set; }
        public string UserAgent { get; set; }
        public string RequestingIP { get; set; }
        public int UserID { get; set; }

        public static SaveJobCommand Build(
            string requestingIP,
            int userID
        ) {
            return new SaveJobCommand() {
                RequestingIP = requestingIP,
                UserID = userID
            };
        }
    }

    public enum SaveJobCommandHttpMethod {
        GET  = 1,
        HEAD = 2,
    }
}