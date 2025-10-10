namespace LoanManagementSystem.Service
{
    public class RepaymentReminder : BackgroundService
    {
        private readonly ILogger<RepaymentReminder> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        //private readonly IRepaymentService _service; 

        public RepaymentReminder(ILogger<RepaymentReminder> logger,
                                       IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            //_service = service;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Running repayment reminder job...");
                    // Create a new DI scope for each run
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetRequiredService<IRepaymentService>();
                        await service.SendRepaymentReminder();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending repayment reminders");
                }

                // wait before running again – for example once a day at 1 AM
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
