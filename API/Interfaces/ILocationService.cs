using AsparagusN.DTOs.AddressDtos;
using Stripe;
using Address = AsparagusN.Data.Entities.Address;

namespace AsparagusN.Interfaces;

public interface ILocationService
{
    Task<decimal> GetDrivingDistanceAsync(decimal startLatitude, decimal startLongitude, decimal endLatitude, decimal endLongitude);
    Task<bool> CanDeliver(AddressDto shippingToAddress);
    Task<int> GetClosestBranch(Address address);
}