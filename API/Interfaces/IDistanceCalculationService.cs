namespace AsparagusN.Interfaces;

public interface IDistanceCalculationService
{
    Task<decimal> GetDrivingDistanceAsync(decimal startLatitude, decimal startLongitude, decimal endLatitude, decimal endLongitude);

}