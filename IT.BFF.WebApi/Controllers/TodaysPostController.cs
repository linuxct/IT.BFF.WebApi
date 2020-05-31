using System.Collections;
using System.Threading.Tasks;
using IT.BFF.Domain.Contracts;
using IT.BFF.Domain.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IT.BFF.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodaysPostController : ControllerBase
    {

        private readonly ILogger<TodaysPostController> _logger;
        private readonly ICoreService _coreService;

        public TodaysPostController(ILogger<TodaysPostController> logger, ICoreService coreService)
        {
            _logger = logger;
            _coreService = coreService;
        }
        
        [HttpGet]
        public async Task<TelegraphSimplePage> Get()
        {
            return await _coreService.RetrieveTodaysPage();
        }
    }
}