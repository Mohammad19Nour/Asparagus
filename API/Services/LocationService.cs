using AsparagusN.Data.Entities;
using AsparagusN.DTOs.AddressDtos;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;

namespace AsparagusN.Services;

public class LocationService : ILocationService
{
    private readonly IUnitOfWork _unitOfWork;

    public LocationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<decimal> GetDrivingDistanceAsync(decimal startLatitude, decimal startLongitude, decimal endLatitude, decimal endLongitude)
    {
        return 0m;
    }

    public async Task<bool> CanDeliver(AddressDto shippingToAddress)
    {
        var spec = new BranchWithAddressSpecification();
        var branches = await _unitOfWork.Repository<Branch>().ListWithSpecAsync(spec);
        return true;
    }
}