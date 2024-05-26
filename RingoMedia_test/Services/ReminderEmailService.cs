using Microsoft.EntityFrameworkCore;
using RingoMedia_test.Data;

namespace RingoMedia_test.Services
{
    public class ReminderEmailService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReminderEmailService> _logger;
        private Timer _timer;

        public ReminderEmailService(IServiceProvider serviceProvider, ILogger<ReminderEmailService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reminder Email Service is starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbcontext>();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

            var indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var list = await context.Reminders.ToListAsync();
            foreach (var item in list)
            {
                // Ensure DateTime is in UTC
                var reminderDateTime = DateTime.SpecifyKind(item.DateTime, DateTimeKind.Utc);

                // Convert reminderDateTime from UTC to IST
                var reminderDateTimeIST = TimeZoneInfo.ConvertTimeFromUtc(reminderDateTime, indiaTimeZone);

                // Get the current time in IST
                var currentTimeIST = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, indiaTimeZone);

                if (reminderDateTimeIST < currentTimeIST && !item.EmailSent)
                {
                    // Send email
                    await emailSender.SendEmailAsync(item.ToEmail, "Reminder", item.Title);

                    // Mark reminder as sent
                    item.EmailSent = true;
                    context.Update(item);
                }
            }

            await context.SaveChangesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reminder Email Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
