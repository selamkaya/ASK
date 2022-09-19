using ASK.Shared.Models;
using ASK.Shared.Models.Job;

namespace ASK.WebApp.Helpers
{
	public class WmsAppState
	{
		//identity
		public int UserId { get; set; }

		public string UserName { get; set; }

		//transfers
		public string LocationCodeFrom { get; set; } = string.Empty;

		public string LocationCodeTo { get; set; } = string.Empty;

		public List<StockModel> StocksToMove { get; set; } = new();

		//jobs
		public JobModel Job { get; set; } = new(); // selected job by user

		public List<string> BinList { get; set; } = new(); //scanned list for attaching

		public List<PickListModel> PickList { get; set; } = new(); //picklist for selected job
	}
}
