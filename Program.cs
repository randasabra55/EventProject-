using EventProject.AppContext;
using EventProject.Features;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.CreateUmbracoBuilder()
       .AddBackOffice()
       .AddWebsite()
       .AddComposers()
       //.AddNotificationHandler<ContentPublishingNotification, PublishEventContent>()
       .Build();

builder.Services.AddDbContext<Context>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("umbracoDbDSN")));

builder.Services.AddHostedService<UpcomingEventsJob>();

WebApplication app = builder.Build();

await app.BootUmbracoAsync();


app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
