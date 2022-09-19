namespace ASK.Shared.Models.Job;

public class JobModel
{
	public int Id { get; set; }

	public string Name { get; set; }

	public int Priority { get; set; }

	public int Shipments { get; set; }

	public int Products { get; set; }

	public int HighPrios { get; set; }

	public int MarketPlaceOrders { get; set; }

	public int B2bOrders { get; set; }
}

