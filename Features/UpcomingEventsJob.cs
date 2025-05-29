
using Umbraco.Cms.Core.Services;

namespace EventProject.Features
{
    public class UpcomingEventsJob : BackgroundService
    {
        IContentService contentService;
        public UpcomingEventsJob(IContentService contentService)
        {
            this.contentService = contentService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var allEvents = contentService.GetRootContent()
                .SelectMany(x => contentService.GetPagedDescendants(x.Id, 0, int.MaxValue, out var _))
                .Where(x => x.ContentType.Alias == "events")
                .ToList();
            var upcoming = allEvents.Where(x =>
            {
                var startDate = x.GetValue<DateTime>("startDate");
                return startDate >= DateTime.Today && startDate <= DateTime.Today.AddDays(7);
            });

            // For testing: Just log the titles
            foreach (var item in upcoming)
            {
                Console.WriteLine($"📅 Upcoming Event: {item.Name}");
            }
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
