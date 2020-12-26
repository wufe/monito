using System;

namespace Monito.Persistence.Model.Interface {
	public interface IUUIDTrackedEntity {
		Guid UUID { get; set; }
	}
}