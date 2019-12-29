using System.Linq;
using Monito.Database.Entities;
using Monito.Web.Models.Input;
using Monito.Web.Services.Interface;

namespace Monito.Web.Services {
	public class JobService : IJobService
	{
		public Request BuildRequest(SaveJobInputModel inputModel, User user)
		{
			var links = inputModel.Links.Split('\n')
				.Select(link => new Link() {
					OriginalURL = link.Trim()
				})
				.ToList();
			return new Request() {
				Links = links,
				Type = RequestType.Simple,
				UserID = user.ID
			};
		}
	}
}