using ASK.Shared.Models;
using ASK.Shared.Models.Bin;
using ASK.Shared.Models.Job;
using ASK.Shared.Models.Location;
using ASK.Shared.Models.Product;
using ASK.Shared.Models.Stock;
namespace ASK.Shared.Interfaces;

public interface IAppService
{
	Task<AppResult<ProductInfoResponse>> GetProductInfo(ProductInfoRequest request);

	Task<AppResult<LocationInfoResponse>> GetLocationInfo(LocationInfoRequest request);

	Task<AppResult<BinInfoResponse>> GetBinInfo(BinInfoRequest request);

	Task<AppResult<StockTransferResponse>> StockTransfer(StockTransferRequest request);

	Task<AppResult<JobListResponse>> JobList(JobListRequest jobRequest);

	Task<AppResult<JobSelectResponse>> JobSelect(JobSelectRequest request);

	Task<AppResult<JobBinCheckResponse>> JobBinCheck(JobBinCheckRequest jobRequest);

	Task<AppResult<JobAttachResponse>> JobAttach(JobAttachRequest jobRequest);

	Task<AppResult<JobPickListResponse>> JobPickList(JobPickListRequest jobRequest);

	Task<AppResult<JobDoneResponse>> JobDone(JobDoneRequest jobRequest);
}
