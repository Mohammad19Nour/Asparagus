using AsparagusN.DTOs;

namespace AsparagusN.Interfaces;

public interface IStatisticService
{
    Task<StatisticDto> GetStatistics();
}