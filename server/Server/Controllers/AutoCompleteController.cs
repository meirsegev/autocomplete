using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Server.Interfaces;

namespace Server.Controllers
{
    [Route("api/auto-complete")]
    [ApiController]
    public class AutoCompleteController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AutoCompleteController> _logger;
        private IAutoCompleteService _autoCompleteService;

        public AutoCompleteController(
            IConfiguration configuration,
            ILogger<AutoCompleteController> logger,
            IAutoCompleteService autoCompleteService
            )
        {
            _configuration = configuration;
            _logger = logger;
            _autoCompleteService = autoCompleteService;
        }

        // GET api/autocomplete
        [HttpGet("get-suggestions")]
        public async Task<ActionResult<IEnumerable<string>>> AutoCompleteAsync([FromQuery] string prefix, CancellationToken ct)
        {
            try
            {
                var res = await _autoCompleteService.GetSuggestionsAsync(prefix, ct);
                return res;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
