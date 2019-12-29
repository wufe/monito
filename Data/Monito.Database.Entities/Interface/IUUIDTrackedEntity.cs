using System;

namespace Monito.Database.Entities {
	public interface IUUIDTrackedEntity {
		Guid UUID { get; set; }
	}
}