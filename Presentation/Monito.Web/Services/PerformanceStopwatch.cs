using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Monito.Web.Services {
    public class PerformanceStopwatch : IDisposable
    {
        private readonly string _label;
        private readonly Stopwatch _stopwatch;
        private readonly ILogger _logger;

        public PerformanceStopwatch(string label, ILogger logger) {
            _label = label;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            _logger = logger;
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            var elapsed = _stopwatch.Elapsed;
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
				elapsed.Hours, elapsed.Minutes, elapsed.Seconds,
				elapsed.Milliseconds / 10);
            _logger.LogWarning($"{_label}: {elapsedTime}");
        }
    }
}