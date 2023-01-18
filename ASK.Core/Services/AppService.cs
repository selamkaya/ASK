using ASK.Core.Data;
using ASK.Shared.Interfaces;
using ASK.Shared.Models;
using ASK.Shared.Models.Bin;
using ASK.Shared.Models.Job;
using ASK.Shared.Models.Location;
using ASK.Shared.Models.Product;
using ASK.Shared.Models.Stock;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Wms.Domain.Entities;
using Wms.Domain.Enums;
using Wms.Domain.Sprocs;

namespace ASK.Core.Services;

public class AppService : IAppService
{
	private readonly IUnitOfWork _uow;
	private readonly ISprocRepository _sproc;

	public AppService(IUnitOfWork uow, ISprocRepository sproc)
	{
		_uow = uow;
		_sproc = sproc;
	}

	#region GetProductInfo
	public async Task<AppResult<ProductInfoResponse>> GetProductInfo(ProductInfoRequest request)
	{
		try
		{
			var entities = _uow.Repository<Stock>().Entities
					.Include(i => i.Product)
					.Include(i => i.Location)
					.Where(p => p.Product.BarCode == request.BarCode)
					.Select(s => new StockModel
					{
						Quantity = s.Quantity,
						ReceiveDate = s.ReceiveDate,
						ExpDate = s.ExpDate,
						LotNumber = s.LotNumber,
						Quarantaine = s.Quarantaine,

						Product = new ProductModel
						{
							Id = s.ProductId,
							Sku = s.Product.Sku,
							BarCode = s.Product.BarCode ?? "-",
							Name = s.Product.Name,
						},

						Location = new LocationModel
						{
							Id = s.LocationId,
							Code = s.Location.Code,
							Type = s.Location.LocationType.Name
						}
					})
					.ToList();

			if (entities.Any())
			{
				var product = entities.First().Product;

				var response = new ProductInfoResponse
				{
					Sku = product.Sku,
					BarCode = product.BarCode,
					Name = product.Name,
					Stocks = entities
				};

				return await AppResult<ProductInfoResponse>.SuccessAsync(response);
			}
			else
				return await AppResult<ProductInfoResponse>.FailAsync("Product has no Stock!");

		}
		catch (Exception ex)
		{
			return await AppResult<ProductInfoResponse>.FailAsync("Error in GetProductInfo");
		}
	}
	#endregion

	#region GetLocationInfo
	public async Task<AppResult<LocationInfoResponse>> GetLocationInfo(LocationInfoRequest request)
	{
		try
		{
			//get data
			var response = await _uow.Repository<Location>().Entities
					.Include(i => i.LocationType)
					.Include(i => i.Stocks)
					.ThenInclude(i => i.Product)
					.Where(p => p.Code == request.BarCode)
					.Select(l => new LocationInfoResponse
					{
						Id = l.Id,
						Code = l.Code,
						Type = l.LocationType.Name,
						IsActive = l.IsActive,
						Stocks = l.Stocks.Select(s => new StockModel
						{
							Id = s.Id,
							Quantity = s.Quantity,
							ReceiveDate = s.ReceiveDate,
							ExpDate = s.ExpDate,
							LotNumber = s.LotNumber,
							Quarantaine = s.Quarantaine,
							Product = new ProductModel
							{
								Id = s.ProductId,
								Sku = s.Product.Sku,
								BarCode = s.Product.BarCode ?? "-",
								Name = s.Product.Name,
							},
							Location = new LocationModel { Id = s.LocationId },
						}).ToList()
					}).FirstOrDefaultAsync();

			// validation
			if (response == null)
				return await AppResult<LocationInfoResponse>.FailAsync("Location not found!");

			//result
			//LocationInfoResponse response = new LocationInfoResponse
			//{
			//	Id = location.Id,
			//	Code = location.Code,
			//	Type = location.LocationType.Name,
			//	IsActive = location.IsActive,
			//	Stocks = location.Stocks.Select( s => new StockModel
			//	{
			//		Id = s.Id,
			//		Quantity = s.Quantity,
			//		ReceiveDate = s.ReceiveDate,
			//		ExpDate = s.ExpDate,
			//		LotNumber = s.LotNumber,
			//		Quarantaine = s.Quarantaine,
			//		Product = new ProductModel
			//		{
			//			Id = s.ProductId,
			//			Sku = s.Product.Sku,
			//			Ean = s.Product.BarCode ?? "-",
			//			Name = s.Product.Name,
			//		},
			//		Location = new LocationModel { Id = location.Id },
			//	} ).ToList()
			//};

			return await AppResult<LocationInfoResponse>.SuccessAsync(response);

		}
		catch (Exception)
		{
			return await AppResult<LocationInfoResponse>.FailAsync("Error in GetLocationInfo!");
		}
	}
	#endregion

	public async Task<AppResult<BinInfoResponse>> GetBinInfo(BinInfoRequest request)
	{
		try
		{
			//get data
			var response = await (from bin in _uow.Repository<Bin>().Entities
								  from shipmentBin in _uow.Repository<ShipmentBin>().Entities
								  from shipment in _uow.Repository<Shipment>().Entities
								  from shipmentItem in _uow.Repository<ShipmentItem>().Entities
								  from product in _uow.Repository<Product>().Entities
								  from order in _uow.Repository<Order>().Entities
								  from company in _uow.Repository<Company>().Entities
								  where bin.Code.ToLower() == request.BarCode.ToLower()
								  && bin.Id == shipmentBin.BinId && shipmentBin.ShipmentId == shipment.Id
								  && shipment.OrderId == order.Id && order.CompanyId == company.Id
								  && shipment.Id == shipmentItem.ShipmentId && shipmentItem.ProductId == product.Id
								  orderby shipmentBin.Id descending
								  select new BinInfoResponse
								  {
									  BinCode = bin.Code,
									  CompanyName = company.Name,
									  OrderNumber = order.OrderNumber,
									  ShippingMethod = "TODO",
									  Products = new List<ProductModel>
															{
																new ProductModel
																{
																	Name = product.Name,
																	Sku = product.Sku,
																	BarCode = product.BarCode?? "no barcode",
																}
															}
								  })
														 .FirstOrDefaultAsync();

			//var response = await _uow.Repository<Bin>().Entities
			//		.Include( i => i.ShipmentBins )
			//		.ThenInclude( i => i.Shipment )
			//		.ThenInclude( i => i.Order )
			//		.Where( p => p.Code == request.BarCode )
			//		.Select( s => new BinInfoResponse
			//		{
			//			Code = s.Code,
			//			Shipments = s.ShipmentBins.Select( s => s.Shipment )
			//		.Select( s => new ShipmentModel
			//		{
			//			CompanyName = s.Order.Company.Name,
			//			OrderNumber = s.Order.OrderNumber,
			//			OrderDate = s.Order.OrderDate,


			//		} ).ToList()
			//		} ).FirstOrDefaultAsync();

			// validation
			if (response == null)
				return await AppResult<BinInfoResponse>.FailAsync("Bin not found!");

			//result
			return await AppResult<BinInfoResponse>.SuccessAsync(response);

		}
		catch (Exception)
		{
			return await AppResult<BinInfoResponse>.FailAsync("Error in GetBinInfo!");
		}
	}

	#region StockTransfer
	public async Task<AppResult<StockTransferResponse>> StockTransfer(StockTransferRequest request)
	{
		StockTransferResponse response = new();

		try
		{
			// RULES
			if (!request.Transfers.Any())
			{
				return await AppResult<StockTransferResponse>.FailAsync("Transfer/Stocklist is empty!");
			}

			foreach (var trans in request.Transfers)
			{
				bool ok = MoveStock(trans);
			}

			await _uow.Commit(new CancellationToken());

			//result
			return await AppResult<StockTransferResponse>.SuccessAsync(response);

		}
		catch (Exception e)
		{
			return await AppResult<StockTransferResponse>.FailAsync("Error in StockTransfer! " + e.Message);
		}
	}
	#endregion



	public async Task<AppResult<JobListResponse>> JobList(JobListRequest request)
	{
		try
		{
			//get data
			List<JobModel> jobs = _sproc.GetStoredProcedure("SpJobs")
				.ExecuteStoredProcedureAsync<JobModel>()
				.Result.ToList();

			// validation
			//if ( jobs == null )
			//	return await AppResult<LocationInfoResponse>.FailAsync( "Location not found!" );

			//result
			JobListResponse response = new();
			response.Jobs = jobs;

			return await AppResult<JobListResponse>.SuccessAsync(response);

		}
		catch (Exception)
		{
			return await AppResult<JobListResponse>.FailAsync("Error in GetLocationInfo!");
		}
	}

	public async Task<AppResult<JobSelectResponse>> JobSelect(JobSelectRequest request)
	{
		try
		{
			var job = _uow.Repository<Job>().Entities.FirstOrDefault(p => p.Id == request.JobId);
			job.Status = JobStatusEnum.Selected;
			job.UserId = request.UserId;
			job.StartTime = DateTime.UtcNow;

			await _uow.Commit(new CancellationToken());

			JobSelectResponse response = new();
			response.Done = true;

			return await AppResult<JobSelectResponse>.SuccessAsync(response);

		}
		catch (Exception)
		{
			return await AppResult<JobSelectResponse>.FailAsync("Error in GetLocationInfo!");
		}
	}

	public async Task<AppResult<JobBinCheckResponse>> JobBinCheck(JobBinCheckRequest request)
	{
		try
		{
			var bin = _uow.Repository<Bin>().Entities.FirstOrDefault(p => p.Code == request.Code);
			if (bin == null)
				return await AppResult<JobBinCheckResponse>.FailAsync("Bin does not exist!");

			JobBinCheckResponse response = new();
			response.Exists = true;

			return await AppResult<JobBinCheckResponse>.SuccessAsync(response);

		}
		catch (Exception)
		{
			return await AppResult<JobBinCheckResponse>.FailAsync("Error in JobBinCheck!");
		}
	}

	public async Task<AppResult<JobAttachResponse>> JobAttach(JobAttachRequest request)
	{
		try
		{
			// get the picklist to determine the shipment ids
			var picklist = _sproc.GetStoredProcedure("SpPickList")
				.WithSqlParams
				(
					("JobId", request.JobId)
				)
				.ExecuteStoredProcedureAsync<SpPickList>().Result
				.OrderBy(p => p.TrackingNumber)
				.ToList();

			var shipmentIds = picklist.Select(s => s.ShipmentId)
				.Distinct()
				.ToList();

			// remove older shipmentbins
			var sb = _uow.Repository<ShipmentBin>().Entities.Where(p => shipmentIds.Contains(p.ShipmentId));

			await _uow.Repository<ShipmentBin>().DeleteAsync(sb.ToArray());


			//foreach ( var code in request.BinCodes )
			for (int i = 0; i < request.BinCodes.Count; i++)
			{
				var binCode = request.BinCodes[i];
				var shipmentId = shipmentIds[i];

				var bin = await _uow.Repository<Bin>().Entities
					//.Include( i => i.ShipmentBins )
					.FirstOrDefaultAsync(p => p.Code == binCode);
				if (bin == null)
					return await AppResult<JobAttachResponse>.FailAsync($"bin {binCode} doen not exist!");


				// all ok
				//attach new shipments
				bin.Status = BinStatusEnum.Attached;
				bin.ShipmentBins.Add(new ShipmentBin
				{
					BinId = bin.Id,
					ShipmentId = shipmentId
				});
			}

			await _uow.Commit(new CancellationToken());

			JobAttachResponse response = new();
			response.Done = true;

			return await AppResult<JobAttachResponse>.SuccessAsync(response);

		}
		catch (Exception)
		{
			return await AppResult<JobAttachResponse>.FailAsync("Error in GetLocationInfo!");
		}
	}

	public async Task<AppResult<JobPickListResponse>> JobPickList(JobPickListRequest request)
	{
		try
		{
			var job = _sproc.GetStoredProcedure("SpPickList")
				.WithSqlParams
				(
					("JobId", request.JobId)
				)
				.ExecuteStoredProcedureAsync<PickListModel>().Result.ToList();

			JobPickListResponse response = new();
			response.PickList = job;

			return await AppResult<JobPickListResponse>.SuccessAsync(response);

		}
		catch (Exception)
		{
			return await AppResult<JobPickListResponse>.FailAsync("Error in JobPickList!");
		}
	}

	public async Task<AppResult<JobDoneResponse>> JobDone(JobDoneRequest request)
	{
		try
		{
			//1. update job status to Done
			//2. update shipment status to Picked
			//3. update bin status to Picked

			var job = _uow.Repository<Job>().Entities.FirstOrDefault(p => p.Id == request.JobId);
			job.Status = JobStatusEnum.Done;
			job.UserId = request.UserId;
			job.EndTime = DateTime.UtcNow;

			var shipments = _uow.Repository<Shipment>().Entities.Where(p => p.JobId == request.JobId);
			await shipments.ForEachAsync(p => p.Status = ShipmentStatusEnum.Picked);

			var shipmentIds = shipments.Select(p => p.Id).ToList();
			var shipmentBins = _uow.Repository<ShipmentBin>().Entities.Where(p => shipmentIds.Contains(p.ShipmentId))
				.Include(i => i.Bin);
			await shipmentBins.ForEachAsync(p => p.Bin.Status = BinStatusEnum.Picked);


			await _uow.Commit(new CancellationToken());

			JobDoneResponse response = new();
			response.Done = true;

			return await AppResult<JobDoneResponse>.SuccessAsync(response);

		}
		catch (Exception)
		{
			return await AppResult<JobDoneResponse>.FailAsync("Error in GetLocationInfo!");
		}
	}


	private bool MoveStock(StockTransferModel model)
	{
		try
		{
			//RULES
			var stockRepo = _uow.Repository<Stock>();
			var locationRepo = _uow.Repository<Location>();
			var stockMoveRepo = _uow.Repository<StockMove>();
			var stockMutationRepo = _uow.Repository<StockMutation>();


			var stockFrom = stockRepo.Entities.Where(p => p.ProductId == model.ProductId && p.LocationId == model.LocationFromId)
				//&& p.Location.WarehouseId == _user.WarehouseId 			);
				.Include(i => i.Product)
				.Include(i => i.Location)
				.FirstOrDefault();

			if (stockFrom == null)
				throw new ValidationException("voorraad-van niet gevonden");

			if (stockFrom.Quantity < model.Quantity)
				throw new ValidationException("voorraad-van niet voldoende");

			if (stockFrom.ProductId != model.ProductId)
				throw new ValidationException("Product niet gevonden op locatie");

			if (stockFrom.LocationId != model.LocationFromId)
				throw new ValidationException("locatie-van niet gevonden");


			var locationTo = locationRepo.Entities.FirstOrDefault(p => p.Id == model.LocationToId);
			//&& p.WarehouseId == _user.WarehouseId );

			if (locationTo == null || !locationTo.IsActive)
				throw new ValidationException("locatie-naar niet gevonden of niet actief");

			// THT CHECK
			var stockTo = stockRepo.Entities.FirstOrDefault(p => p.ProductId == model.ProductId && p.LocationId == model.LocationToId);
			//&& p.Location.WarehouseId == _user.WarehouseId );

			if (stockTo != null && stockFrom.ExpDate.HasValue && stockTo.ExpDate.HasValue && stockFrom.ExpDate.Value != stockTo.ExpDate.Value)
				throw new ValidationException("locatie-naar bevat al product met andere THT");

			// LOT CHECK
			if (stockTo != null && !string.Equals(stockFrom.LotNumber ?? string.Empty, stockTo.LotNumber ?? string.Empty, StringComparison.InvariantCultureIgnoreCase))
				throw new ValidationException("locatie-naar bevat al product met andere LOT");

			// FIFO/LIFO CHECK
			//Product product = stockFrom.Product;

			if ((stockFrom.Product.ShippingOrder == ShippingOrderEnum.FIFO || stockFrom.Product.ShippingOrder == ShippingOrderEnum.LIFO)
				&& stockTo != null
				&& stockFrom.ReceiveDate.HasValue
				&& stockTo.ReceiveDate.HasValue
				&& stockFrom.ReceiveDate.Value != stockTo.ReceiveDate.Value)
				throw new ValidationException("locatie-naar bevat al product met andere ontvangstdatum");

			// EAN CHECK
			//if ( locTo.IsPickLocation )
			//{
			//	var a = ( product.HasNoBarCode == true );
			//	var b = locTo.Stocks.Any( p => p.Product != null && p.Product.HasNoBarCode == true && p.Product.Id != product.Id && p.Location.WarehouseId == _user.WarehouseId );

			//	if ( a && b )
			//	{
			//		//var c = (locTo.Stocks.Count == 1);
			//		//var d = locTo.Stocks.Any(p => p.ProductId == product.Id);
			//		//if (!locTo.Stocks.Any(p => p.ProductId == product.Id))
			//		throw new ValidationException( "locatie-naar bevat al product zonder barcode" );
			//	}
			//}

			// Quarantaine
			if (stockTo != null)
			{
				if (!stockFrom.Quarantaine.HasValue)
					stockFrom.Quarantaine = false;

				if (!stockTo.Quarantaine.HasValue)
					stockTo.Quarantaine = false;

				if (stockFrom.Quarantaine != stockTo.Quarantaine)
					throw new ValidationException("Quarantaine van locaties komen niet overeen.");
			}
			// END RULES

			// MOVE STOCK
			var a = new StockMove
			{
				Product = stockFrom.Product,
				LocationFrom = stockFrom.Location,
				LocationTo = locationTo,
				Quantity = model.Quantity,
				Added = true
			};
			stockMoveRepo.AddAsync(a);

			//MUTATIE
			var b = new StockMutation
			{
				Product = stockFrom.Product,
				ExpDate = stockFrom.ExpDate,
				LotNumber = stockFrom.LotNumber,
				ReceiveDate = stockFrom.ReceiveDate,
				Location = stockFrom.Location,
				Quantity = -model.Quantity,
				MutationType = MutationTypeEnum.Decrease,
				MutationConcerns = MutationConcernsEnum.Movement,
				CorrectionReason = string.Format("Stock moved from {0} to {1}", stockFrom.Location.Code, locationTo.Code),
			};
			stockMutationRepo.AddAsync(b);

			var c = new StockMutation
			{
				Product = stockFrom.Product,
				ExpDate = stockFrom.ExpDate,
				LotNumber = stockFrom.LotNumber,
				ReceiveDate = stockFrom.ReceiveDate,
				Location = locationTo,
				Quantity = model.Quantity,
				MutationType = MutationTypeEnum.Increase,
				MutationConcerns = MutationConcernsEnum.Movement,
				CorrectionReason = string.Format("Stock moved from {0} to {1}", stockFrom.Location.Code, locationTo.Code),
			};
			stockMutationRepo.AddAsync(c);

			//STOCK
			//stockFrom = stockRepo.Entities.FirstOrDefault( p => p.ProductId == model.ProductId && p.LocationId == model.LocationFromId);
			//&& p.Location.WarehouseId == _user.WarehouseId ); //reload ivm concurrency
			stockFrom.Quantity -= model.Quantity;
			if (stockFrom.Quantity > 0)
				stockRepo.UpdateAsync(stockFrom);
			else
				stockRepo.DeleteAsync(stockFrom);

			//stockTo = _stockRepo.GetFirstOrDefaultTracked( p => p.ProductId == model.ProductId && p.LocationId == model.LocationToId);
			//&& p.Location.WarehouseId == _user.WarehouseId ); //reload ivm concurrency
			if (stockTo == null)
				stockTo = new Stock(); // { ProductId = stockFrom.ProductId, LocationId = locationTo.Id };//, Quantity = 0, Quarantaine = false };

			stockTo.Product = stockFrom.Product;
			stockTo.Location = locationTo;
			stockTo.Quantity += model.Quantity;
			stockTo.ExpDate = stockFrom.ExpDate;
			stockTo.LotNumber = stockFrom.LotNumber;
			if (!stockTo.ReceiveDate.HasValue || (stockFrom.ReceiveDate.HasValue && stockFrom.ReceiveDate.Value < stockTo.ReceiveDate))
				stockTo.ReceiveDate = stockFrom.ReceiveDate;
			stockTo.Quarantaine = stockFrom.Quarantaine;

			if (stockTo.Id > 0)
				stockRepo.UpdateAsync(stockTo);
			else
				stockRepo.AddAsync(stockTo);

			//_uow.Commit( new CancellationToken() );

			return true;
		}
		catch (ValidationException e)
		{
			throw new ValidationException(e.Message);
		}
		catch (Exception e)
		{
			throw new Exception(e.Message);
		}
	}


}
