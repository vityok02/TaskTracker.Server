using Api.Users.Dtos;
using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Users;
[Route("users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly LinkGenerator _linkGenerator;

    public UserController(IUserRepository userRepository, LinkGenerator linkGenerator)
    {
        _userRepository = userRepository;
        _linkGenerator = linkGenerator;
    }

    [HttpGet]
    [ActionName("GetUser")]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return Ok(user);
    }

    [HttpPost]
    [ActionName("CreateUser")]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserDto userDto)
    {
        // password logic
        var user = Domain.User.Create(Guid.NewGuid(), userDto.UserName, userDto.Email, userDto.Password);

        var id = await _userRepository.CreateAsync(user);

        var uri = _linkGenerator.GetUriByAction(HttpContext, "GetUser", values: new { id });

        return Created(uri, new UserDetailsDto(user.Id, user.UserName, user.Email));
    }
}
