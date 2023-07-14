using BusinessLogic.Dto;
using BusinessLogic.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Exceptions;
using Presentation.Models;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMessageService _messageService;
    private readonly ISourceService _sourceService;

    public ClientController(IAuthService authService, IMessageService messageService, ISourceService sourceService)
    {
        _authService = authService;
        _messageService = messageService;
        _sourceService = sourceService;
    }
    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost("/Source/Create")]
    public async Task<ActionResult<SourceDto>> CreateSourceAsync([FromBody] CreateSourceModel model)
    {
        SourceDto sourceDto =
            await _sourceService.CreateSourceAsync(model.OwnerId, model.Type, model.Info, CancellationToken);
        return Ok(sourceDto);
    }
    [HttpPost("/Report/Create")]
    public async Task<ActionResult<ReportDto>> CreateReportAsync(DateTime start)
    {
        EmployeeDto currentClientDto = await _authService.GetCurEmployee(CancellationToken);
        if (currentClientDto.AccessLevel < AccessLevel.Manager)
            throw new AccessDeniedException("Access denied");
        ReportDto reportDto = await _messageService.CreateReportAsync(start, CancellationToken);
        return Ok(reportDto);
    }
    [HttpPost("/Message/Create")]
    public async Task<ActionResult<MessageDto>> CreateMessageAsync([FromBody] MessageModel model)
    {
        MessageDto messageDto = await _messageService.CreateMessageAsync(model.SenderSourceId, model.RecipientSourceId,
            model.Content, CancellationToken);
        return Ok(messageDto);
    }

    [HttpPost("/Message/Read")]
    public async Task<ActionResult<MessageDto>> ReadMessageAsync([FromBody] ReadMessageModel model)
    {
        MessageDto messageDto = await _messageService.ReadMessageAsync(model.Id, CancellationToken);
        return Ok(messageDto);
    }

    [HttpPost("/Message/Reply")]
    public async Task<ActionResult<MessageDto>> ReplyToMessageAsync([FromBody] ReplyToMessageModel model)
    {
        MessageDto messageDto = await _messageService.ReplyToMessageAsync(model.MessageId, model.Content, CancellationToken);
        return Ok(messageDto);
    }

    [HttpGet("/Messages/Unprocessed")]
    public async Task<ActionResult<List<MessageDto>>> UnprocessedMessagesAsync()
    {
        EmployeeDto currentClientDto = await _authService.GetCurEmployee(CancellationToken);
        List<MessageDto> messageDtos =
            await _messageService.UnprocessedMessagesAsync(currentClientDto.Id, CancellationToken);
        return Ok(messageDtos);
    }
    [HttpGet("/Messages/All")]
    public async Task<ActionResult<List<MessageDto>>> AllMessagesAsync()
    {
        EmployeeDto currentClientDto = await _authService.GetCurEmployee(CancellationToken);
        List<MessageDto> messageDtos =
            await _messageService.AllMessagesAsync(currentClientDto.Id, CancellationToken);
        return Ok(messageDtos);
    }
    [HttpGet("/Messages/Unread")]
    public async Task<ActionResult<List<MessageDto>>> UnreadMessagesAsync()
    {
        EmployeeDto currentClientDto = await _authService.GetCurEmployee(CancellationToken);
        List<MessageDto> messageDtos =
            await _messageService.UnreadMessagesAsync(currentClientDto.Id, CancellationToken);
        return Ok(messageDtos);
    }
    [HttpPost("/Auth/LogIn")]
    public async Task<ActionResult<EmployeeDto>> LogInAsync([FromBody] LogInModel model)
    {
        EmployeeDto employeeDto = await _authService.LogInAsync(model.Login, model.Password, CancellationToken);
        return Ok(employeeDto);
    }

    [HttpPost("/Auth/LogOut")]
    public void LogOutAsync()
    {
        _authService.LogOutAsync(CancellationToken);
    }
}