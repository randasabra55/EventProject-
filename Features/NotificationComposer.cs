using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Notifications;

namespace EventProject.Features
{
    public class NotificationComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<ContentSavingNotification, PublishEventContent>();
        }
    }
}
