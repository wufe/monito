using System;
using MediatR;

namespace Monito.Application.Model.Command
{
    public class AbortRequestCommand : IRequest {
        public Guid UUID { get; set; }

        public static AbortRequestCommand Build(Guid uuid) {
            return new AbortRequestCommand() {
                UUID = uuid
            };
        }
    }
}