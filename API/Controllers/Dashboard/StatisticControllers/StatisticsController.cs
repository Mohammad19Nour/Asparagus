using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard.StatisticControllers;

public partial class StatisticsController : BaseApiController
{
 
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStatisticService _statisticService;

    public StatisticsController(IUnitOfWork unitOfWork,IMapper mapper,IStatisticService statisticService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _statisticService = statisticService;
    }

    [HttpGet]
    public async Task<ActionResult<StatisticDto>> GetAllStatistic()
    {
        var result = await _statisticService.GetStatistics();

        return Ok(new ApiOkResponse<StatisticDto>(result));
    }

}