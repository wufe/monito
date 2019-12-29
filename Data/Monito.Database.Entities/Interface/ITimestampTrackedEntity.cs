using System;

namespace Monito.Database.Entities.Interface {

	public interface ITimestampTrackedEntity {
		DateTime CreatedAt { get; set; }
		DateTime UpdatedAt { get; set; }
	}

}