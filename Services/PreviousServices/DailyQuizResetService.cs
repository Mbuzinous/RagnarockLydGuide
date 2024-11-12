using RagnarockTourGuide.Interfaces;

namespace RagnarockTourGuide.Services.PreviousServices
{
    public class DailyQuizResetService : BackgroundService
    {
        private readonly QuizCRUDRepository _quizRepository;
        private readonly ILogger<DailyQuizResetService> _logger;

        public DailyQuizResetService(QuizCRUDRepository quizRepository, ILogger<DailyQuizResetService> logger)
        {
            _quizRepository = quizRepository;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRunTime = DateTime.Today.AddHours(18); // Næste kl. 18:00
                if (now > nextRunTime)
                    nextRunTime = nextRunTime.AddDays(1);

                var delay = nextRunTime - now;
                _logger.LogInformation($"Nulstilling af quizzer planlagt til: {nextRunTime}");

                await Task.Delay(delay, stoppingToken);

                await _quizRepository.ResetDailyQuizzesAsync();
                _logger.LogInformation("Quizzes nulstillet kl. 18:00");
            }
        }
    }
}
