using Monito.Database.Entities;

namespace Monito.Domain.Service.Interface {
	public interface IUserService {
		User FindByIP(string IP);
		void Add(User user);
		User FindOrCreateUserByIP(string IP);
	}
}