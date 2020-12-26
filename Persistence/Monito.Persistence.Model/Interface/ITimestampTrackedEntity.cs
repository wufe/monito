using System;

namespace Monito.Persistence.Model.Interface {

	public interface ITimestampTrackedEntity {
		DateTime CreatedAt { get; set; }
		DateTime UpdatedAt { get; set; }
	}

}