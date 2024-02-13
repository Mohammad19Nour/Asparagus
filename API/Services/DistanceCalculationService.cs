using AsparagusN.Interfaces;

namespace AsparagusN.Services;

public class DistanceCalculationService : IDistanceCalculationService
{
    public async Task<decimal> GetDrivingDistanceAsync(decimal startLatitude, decimal startLongitude, decimal endLatitude, decimal endLongitude)
    {
        return 0m;
    }
}