using Api.Extensions;
using Api.Filters;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Twilio;

[ProjectMember]
[Route("/projects/{projectId:guid}/videochat")]
public class TwilioController : ControllerBase
{
    private readonly TwilioService _twilioService;

    public TwilioController(TwilioService twilioService)
    {
        _twilioService = twilioService;
    }

    [HttpGet("token")]
    public IActionResult GetToken() =>
         new JsonResult(_twilioService.GetTwilioJwt(User.GetUserId().ToString()));

    [HttpGet("rooms")]
    public async Task<IActionResult> GetRooms() =>
        new JsonResult(await _twilioService.GetAllRoomsAsync());
}
