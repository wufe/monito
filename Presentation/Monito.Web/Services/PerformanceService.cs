using Microsoft.Extensions.Logging;
using Monito.Web.Services.Interface;

namespace Monito.Web.Services {
    public class PerformanceService : IPerformanceService
    {
        private readonly ILogger<IPerformanceService> _logger;

        public PerformanceService(ILogger<IPerformanceService> logger) {
            _logger = logger;
        }

        public PerformanceStopwatch Start(string label)
        {
            return new PerformanceStopwatch(label, _logger);
        }
    }
}