using System;
using Monito.Persistence.Model.Interface;

namespace Monito.Persistence.Model {
	public class FilePersistenceModel : IPrimaryKeyEntity, ITimestampTrackedEntity {
		public int ID { get; set; }
		public string Name { get; set; }
		public FileType Type { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int RequestID { get; set; }
		public RequestPersistenceModel Request { get; set; }
	}

	public enum FileType : byte {
		TXT = 1,
		CSV = 2
	}
}