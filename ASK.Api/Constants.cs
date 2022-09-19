namespace ASK.Api;

public static class Constants
{
	public static class ApiRoutes
	{
		public const string AccountLogin = "/account/login";

		public const string LocationInfoRoute = "location/info/";
		public const string ProductInfoRoute = "product/info/";
		public const string BinInfoRoute = "bin/info/";

		public const string StockTransferRoute = "stock/transfer/";

		//public const string TransferLocationRoute = "transfer/location/";
		//public const string TransferProductRoute = "transfer/product/";
		//public const string LocationUpdateRoute = "location/update/";


		public const string JobListRoute = "job/list/"; // gets a list of available jobs
		public const string JobSelectRoute = "job/select/"; // user select 1 job to process, user is mapped to job
		public const string JobBinCheckRoute = "job/bincheck/"; // user select 1 job to process, user is mapped to job
		public const string JobAttachRoute = "job/attach/";  // user scans bins and these bins will be attached to shipments in job
		public const string JobPickListRoute = "job/picklist/"; // gets the picklist for job
		public const string JobDoneRoute = "job/done/"; // finishing a job
	}
}