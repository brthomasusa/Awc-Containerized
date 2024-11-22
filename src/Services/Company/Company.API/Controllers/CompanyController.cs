using System.Diagnostics;
using Awc.Services.Company.API.Application.Features.GetCompanyById;
using Awc.Services.Company.API.ViewModels;
using MediatR;

namespace Awc.Services.Company.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CompanyController(ISender sender, ILogger<CompanyController> logger) : ControllerBase
    {
        private readonly ISender _sender = sender;
        private readonly ILogger<CompanyController> _logger = logger;

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CompanyViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<CompanyViewModel>> GetCompanyById(int id)
        {
            try
            {

                Result<CompanyViewModel> result = await _sender.Send(new GetCompanyByIdQuery(CompanyId: id));

                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }

                return NotFound(result.Error.Message);
            }
            catch(Exception ex)
            {
                string errMsg = Helpers.GetExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);

                return StatusCode(500, errMsg);
            }
        }                
    }
}