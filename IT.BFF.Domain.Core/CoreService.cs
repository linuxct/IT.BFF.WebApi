using System;
using System.Threading.Tasks;
using IT.BFF.Domain.Contracts;
using IT.BFF.Infra.TelegraphConnector;
using Microsoft.Extensions.Logging;

namespace IT.BFF.Domain.Core
{
    public class CoreService : ICoreService
    {
        private readonly ILogger<CoreService> _logger;
        private readonly ITelegraphConnector _telegraphConnector;

        public CoreService(ILogger<CoreService> logger, ITelegraphConnector telegraphConnector)
        {
            _logger = logger;
            _telegraphConnector = telegraphConnector;
        }

        public async Task<TelegraphSimplePage> RetrieveTodaysPage()
        {
            DateTime now = DateTime.Now.Date;
            return await _telegraphConnector.RetrieveTodaysPost(now);
        }
        
        
        
    }
}