using Api.Base;
using Application.Projects.CreateProject;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Projects;

[Route("users/{userId:guid}/projects")]
public class ProjectController : ApiController
{
    public ProjectController(ISender sender, LinkGenerator linkGenerator)
        : base(sender, linkGenerator)
    {
    }


    [HttpPost]
    [ActionName("CreateProject")]
    public async Task<IActionResult> CreateProject(
        [FromBody] ProjectRequest projectRequest,
        [FromRoute] Guid userId)
    {
        var result = await Sender.Send(new CreateProjectCommand(userId, projectRequest));

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        var uri = LinkGenerator.GetUriByAction(HttpContext, "GetProject", values: new { result.Value.Id });

        return Created(uri, result.Value);
    }
}
