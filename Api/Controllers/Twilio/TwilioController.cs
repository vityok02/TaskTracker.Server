using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Twilio;

[Route("api/twilio")]
public class TwilioController : ControllerBase
{
    private readonly TwilioService _twilioService;

    public TwilioController(TwilioService twilioService)
    {
        _twilioService = twilioService;
    }

    [HttpGet("token")]
    public IActionResult GetToken() =>
         new JsonResult(_twilioService.GetTwilioJwt(User.Identity.Name));

    [HttpGet("rooms")]
    public async Task<IActionResult> GetRooms() =>
        new JsonResult(await _twilioService.GetAllRoomsAsync());
}
