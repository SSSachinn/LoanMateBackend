namespace LoanManagementSystem.Service
{
    public class OverdueRepaymentChecker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OverdueRepaymentChecker> _logger;

        public OverdueRepaymentChecker(IServiceScopeFactory scopeFactory, ILogger<OverdueRepaymentChecker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var repaymentService = scope.ServiceProvider.GetRequiredService<IRepaymentService>();

                    try
                    {
                        await repaymentService.MarkOverdueRepaymentsAsync();
                        _logger.LogInformation("Overdue repayments checked at {time}", DateTimeOffset.Now);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error while marking overdue repayments");
                    }
                }

                // Run once a day
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }

}
