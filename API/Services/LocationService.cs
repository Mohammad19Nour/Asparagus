using AsparagusN.Data.Entities;
using AsparagusN.DTOs.AddressDtos;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using GeoCoordinatePortable;
using Newtonsoft.Json.Linq;

namespace AsparagusN.Services;

public class LocationService : ILocationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public LocationService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<decimal> GetDrivingDistanceAsync(decimal startLatitude, decimal startLongitude,
        decimal endLatitude, decimal endLongitude)
    {
        var apiKey = _configuration["MapApiKey"];
        var url = _configuration["MapUrl"] +
                  $"directions/json?origin={startLatitude},{startLongitude}&destination={endLatitude},{endLongitude}&key={apiKey}";
        using (var httpClient = new HttpClient())
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Read the response content
                string responseContent = await response.Content.ReadAsStringAsync();

                var distance = ParseDistanceFromGoogleMapsResponse(responseContent);

                return distance;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }

        return 0m;
    }

    public async Task<bool> CanDeliver(AddressDto shippingToAddress)
    {
        //   var spec = new BranchWithAddressSpecification();
        //    var branches = await _unitOfWork.Repository<Branch>().ListWithSpecAsync(spec);
        return true;
    }

    public async Task<int> GetClosestBranch(decimal userLat, decimal userLong)
    {
        var closestBranch = 1;

        var spec = new BranchWithAddressSpecification();
        var branches = await _unitOfWork.Repository<Branch>().ListWithSpecAsync(spec);

        var userGeo = new GeoCoordinate((double)userLat, (double)userLong);
        double shortestDistance = Double.MaxValue;

        foreach (var branch in branches)
        {
            var branchGeo = new GeoCoordinate((double)branch.Address.Latitude, (double)branch.Address.Longitude);
            var distance = userGeo.GetDistanceTo(branchGeo);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestBranch = branch.Id;
            }
        }

        return closestBranch;
    }

    private decimal ParseDistanceFromGoogleMapsResponse(string responseContent)
    {
        // Parse the JSON response to extract the driving distance
        dynamic jsonResponse = JObject.Parse(responseContent);

        if (jsonResponse.routes == null || jsonResponse.routes.Count == 0)
        {
            return 1100000m;
            throw new Exception("No routes found in the response.");
        }

        var distanceText = jsonResponse.routes[0].legs[0].distance.text.ToString(); // Ensure it's a string

        decimal distanceValue;

        // Extract the numeric value from the distance text (assuming the format is like "1234.56 km")
        distanceText = distanceText.Substring(0, distanceText.IndexOf(" ")); // Remove units
        distanceText = distanceText.Replace(",", ""); // Remove commas
        if (decimal.TryParse(distanceText, out distanceValue))
        {
            return distanceValue;
        }
        else
        {
            throw new Exception($"Failed to parse distance from response: {distanceText}");
        }
    }
}