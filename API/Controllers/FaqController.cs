using System.ComponentModel.DataAnnotations;
using AsparagusN.Data.Entities;
using AsparagusN.DTOs;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers;

public class FaqController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FaqController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<FaqDto>>> GetFaqs()
    {
        var spec = new FAQSpecification();
        var faqs = await _unitOfWork.Repository<FAQ>().ListWithSpecAsync(spec);
        var result = new List<FaqDto>();

        foreach (var faq in faqs)
        {
            var qstn = new List<QuestionDto>();

            foreach (var fuck in faq.FAQChildern)
            {
                var tmpFuck = new QuestionDto
                {
                    QuestionId = fuck.Id,
                    Question = fuck.Title,
                    AnswerId = fuck.FAQChildern[0].Id,
                    Answer = fuck.FAQChildern[0].Title
                };
                qstn.Add(tmpFuck);
            }

            var q = new FaqDto
            {
                Id = faq.Id,
                Title = faq.Title,
                Questions = qstn
            };
            result.Add(q);
        }

        return Ok(new ApiOkResponse<List<FaqDto>>(result));
    }

    [Authorize(Roles = nameof(DashboardRoles.UserQuestion) + "," + nameof(Roles.Admin))]
    [HttpPost]
    public async Task<ActionResult> AddTitle(string title)
    {
        var question = new FAQ
        {
            Title = title
        };
        _unitOfWork.Repository<FAQ>().Add(question);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<FAQ>(question));

        return Ok(new ApiResponse(400, "Failed to add"));
    }

    [Authorize(Roles = nameof(DashboardRoles.UserQuestion) + "," + nameof(Roles.Admin))]
    [HttpPost("question/{id:int}")]
    public async Task<ActionResult> AddTitle(int id, string question, string answer)
    {
        var title = await _unitOfWork.Repository<FAQ>().GetByIdAsync(id);

        if (title == null) return Ok(new ApiResponse(404, "Title not found"));

        var child = new FAQ
        {
            Title = question,
            FAQChildern = new List<FAQ> { new FAQ { Title = answer } }
        };
        title.FAQChildern.Add(child);
        _unitOfWork.Repository<FAQ>().Update(title);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<FAQ>(title));

        return Ok(new ApiResponse(400, "Failed to add"));
    }

    [Authorize(Roles = nameof(DashboardRoles.UserQuestion) + "," + nameof(Roles.Admin))]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> AddTitle(int id, string text)
    {
        var title = await _unitOfWork.Repository<FAQ>().GetByIdAsync(id);

        if (title == null) return Ok(new ApiResponse(404, "Title not found"));

        title.Title = text;
        _unitOfWork.Repository<FAQ>().Update(title);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<FAQ>(title));

        return Ok(new ApiResponse(400, "Failed to add"));
    }

    [Authorize(Roles = nameof(DashboardRoles.UserQuestion) + "," + nameof(Roles.Admin))]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteTitle(int id)
    {
        var spec = new FAQSpecification(id);
        var faq = await _unitOfWork.Repository<FAQ>().GetEntityWithSpec(spec);

        if (faq == null) return Ok(new ApiResponse(404, "Title not found"));


        recDelete(faq);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200, "Deleted"));

        return Ok(new ApiResponse(400, "Failed to delete"));
    }

    private void recDelete(FAQ? faq)
    {
        if (faq == null) return;

        foreach (var ch in faq.FAQChildern)
            recDelete(ch);
        _unitOfWork.Repository<FAQ>().Delete(faq);
    }
}