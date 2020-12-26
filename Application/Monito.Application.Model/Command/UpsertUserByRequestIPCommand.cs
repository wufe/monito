using MediatR;

namespace Monito.Application.Model.Command
{
    public class UpsertUserByRequestIPCommand : IRequest<MinimalUserApplicationModel> {
        public string RequestIP { get; set; }
        public static UpsertUserByRequestIPCommand Build(string requestIP) {
            return new UpsertUserByRequestIPCommand() {
                RequestIP = requestIP
            };
        }
    }
}