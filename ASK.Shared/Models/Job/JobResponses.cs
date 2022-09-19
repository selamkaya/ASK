namespace ASK.Shared.Models.Job;

public class JobListResponse
{
	public List<JobModel> Jobs { get; set; } = new List<JobModel>();
}

public class JobSelectResponse
{
	public bool Done { get; set; } = false;
}

public class JobBinCheckResponse
{
	public bool Exists { get; set; } = false;
}

public class JobAttachResponse
{
	public bool Done { get; set; } = false;
}

public class JobPickListResponse
{
	public List<PickListModel> PickList { get; set; } = new List<PickListModel>();
}

public class JobDoneResponse
{
	public bool Done { get; set; } = false;
}
