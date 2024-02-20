using AsparagusN.DTOs.AddressDtos;

namespace AsparagusN.Interfaces;

public interface ILocationService
{
    Task<decimal> GetDrivingDistanceAsync(decimal startLatitude, decimal startLongitude, decimal endLatitude, decimal endLongitude);
    Task<bool> CanDeliver(AddressDto shippingToAddress);
}