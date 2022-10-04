using GV.DomainModel.SharedKernel.Filters;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Mvc;

namespace ESA.API.Controllers
{
    [ApiController]
    [ResultToActionResult]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationMessage), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationMessage), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ValidationMessage), StatusCodes.Status500InternalServerError)]
    public class ApiControllerBase : ControllerBase
    {
    }
}
