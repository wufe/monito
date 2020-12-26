using System.Linq;
using Monito.Domain.Service.Interface;

namespace Monito.Domain.Service
{
    public class UserService : IUserService
	{
		// private readonly IRepository<User> _userRepository;

		// public UserService(IRepository<User> userRepository)
		// {
		// 	_userRepository = userRepository;
		// }

		// public User FindByIP(string IP)
		// {
		// 	return _userRepository
		// 		.FindAll()
		// 		.FirstOrDefault(x => x.IP == IP);
		// }

		// public void Add(User user)
		// {
		// 	_userRepository.Insert(user);
		// 	_userRepository.SaveChanges();
		// }

		// public User FindOrCreateUserByIP(string IP) {
		// 	var user = FindByIP(IP);
		// 	if (user == null) {
		// 		user = new User() {
		// 			IP = IP
		// 		};
		// 		Add(user);
		// 	}
		// 	return user;
		// }
	}
}