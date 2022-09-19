using System.ComponentModel.DataAnnotations;

namespace ASK.Shared.Models.Job;

public class JobListRequest : RequestBase
{
	// for now get all, later can be filtered by type
}

public class JobSelectRequest : RequestBase
{
	[Required]
	public int JobId { get; set; } = 0;

	//[Required]
	//[FromClaim( "UserId" )]
	//public int UserId { get; set; } = 0; see Base
}

public class JobBinCheckRequest : RequestBase
{
	[Required]
	public string Code { get; set; } = string.Empty;
}

public class JobAttachRequest : RequestBase
{
	[Required]
	public int JobId { get; set; } = 0;

	public List<string> BinCodes { get; set; } = new List<string>();
}

public class JobPickListRequest : RequestBase
{
	[Required]
	public int JobId { get; set; } = 0;
}

public class JobDoneRequest : RequestBase
{
	[Required]
	public int JobId { get; set; } = 0;
}