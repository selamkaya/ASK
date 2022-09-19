using System.ComponentModel.DataAnnotations;

namespace ASK.Shared.Models;

public abstract class ModelBase
{
	[Key]
	public int Id { get; set; }

	//public int? CreatedBy { get; set; }

	//public int? ModifiedBy { get; set; }

	public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

	public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
}
