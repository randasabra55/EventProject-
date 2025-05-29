using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace EventProject.Features
{
    public class PublishEventContent : INotificationHandler<ContentSavingNotification>
    {
        public void Handle(ContentSavingNotification notification)
        {
            foreach (var item in notification.SavedEntities)
            {
                if (item.ContentType.Alias != "events")
                    continue;
                var startDate = item.GetValue<DateTime>("startDate");
                var EndDate = item.GetValue<DateTime>("endDate");
                var duration = EndDate - startDate;
                if (duration.TotalHours < 2)
                {
                    notification.CancelOperation(new EventMessage(
                    category: "Error",
                    message: "Event duration must be at least 2 hours.",
                    EventMessageType.Error
                    //Type: EventMessageType.Error
                ));
                }
            }
        }
    }


}
