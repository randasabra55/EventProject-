using EventProject.AppContext;
using EventProject.Models;
using EventProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace EventProject.Controllers
{
    public class RegistrationController : SurfaceController
    {
        Context context;
        public RegistrationController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, Context context) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            this.context = context;
        }
        [HttpPost]
        public IActionResult Submit(EventRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool alreadyRegistered = context.eventRegistrations.Any(e =>
                e.Email == model.Email && e.EventId == model.EventId);

                if (alreadyRegistered)
                {
                    TempData["Failed"] = "You have already registered for this event.";
                    TempData["FailedEventId"] = model.EventId.ToString();
                    //return RedirectToCurrentUmbracoPage();
                    //return RedirectToCurrentUmbracoPage();
                    return RedirectToHomeOrEvent(model?.EventId ?? 0);
                    //return View("EventsListPage");
                }
                var eventt = new EventRegistrations
                {
                    Email = model.Email,
                    UserName = model.Name,
                    RegistrationAt = DateTime.Now,
                    EventId = model.EventId
                };
                context.eventRegistrations.Add(eventt);
                context.SaveChanges();
                TempData["Success"] = "تم التسجيل بنجاح في " + model.EventTitle;
                TempData["SuccessEventId"] = model.EventId.ToString();
                //return RedirectToCurrentUmbracoPage();
                return RedirectToHomeOrEvent(model.EventId);
                //return View("EventsListPage");
                // return Redirect("/events");
            }
            TempData["Failed"] = "you must fill all fields";
            TempData["FailedEventId"] = model.EventId.ToString();

            //return RedirectToCurrentUmbracoPage();
            return RedirectToHomeOrEvent(model?.EventId ?? 0);
            //return View("EventsListPage");
        }

        private IActionResult RedirectToHomeOrEvent(int eventId)
        {
            try
            {
                // حاول نرجع للصفحة الحالية
                //_logger.LogInformation("Attempting to redirect to current Umbraco page for EventId={EventId}", eventId);
                return RedirectToCurrentUmbracoPage();
            }
            catch (InvalidOperationException ex)
            {
                // _logger.LogError(ex, "Failed to redirect to current Umbraco page for EventId: {EventId}", eventId);
                // لو فشلنا، نروح لصفحة الإيفنت أو الصفحة الرئيسية
                var eventPage = UmbracoContext.Content?.GetById(eventId);
                if (eventPage != null)
                {
                    //_logger.LogInformation("Redirecting to event page: {Url}", eventPage.Url());
                    return Redirect(eventPage.Url());
                }
                //     _logger.LogWarning("Falling back to homepage redirect for EventId={EventId}", eventId);
                return Redirect("/events"); // رجّع لصفحة الإيفنتات
            }
        }

        /*public IActionResult countUsersInEvent(int eventId)
        {
            var users = context.eventRegistrations.Where(e => e.EventId == eventId).ToList();
            return View(users);
        }*/
        /*public IActionResult getUsersInEvent(int eventId)
        {
            var users=IServiceProvider.
        }*/


    }
}


/*using EventProject.AppContext;
using EventProject.Models;
using EventProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace EventProject.Controllers
{
    public class RegistrationController : SurfaceController
    {
        private readonly Context context;
        private readonly IProfilingLogger _logger;

        public RegistrationController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            Context context)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            this.context = context;
            _logger = profilingLogger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(EventRegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // _logger.LogInformation("ModelState is invalid: {Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    TempData["Failed"] = "لازم تملّي كل الحقول بشكل صحيح";
                    TempData["FailedEventId"] = model?.EventId.ToString();
                    return SafeRedirect(model?.EventId ?? 0);
                }

                // التحقق لو المستخدم مسجل بالفعل
                bool alreadyRegistered = context.eventRegistrations.Any(e =>
                    e.Email == model.Email && e.EventId == model.EventId);

                if (alreadyRegistered)
                {
                    TempData["Failed"] = "أنت مسجل بالفعل في الإيفنت ده.";
                    TempData["FailedEventId"] = model.EventId.ToString();
                    return SafeRedirect(model.EventId);
                }

                // إضافة التسجيل الجديد
                var eventt = new EventRegistrations
                {
                    Email = model.Email,
                    UserName = model.Name,
                    RegistrationAt = DateTime.Now,
                    EventId = model.EventId
                };
                context.eventRegistrations.Add(eventt);
                context.SaveChanges();

                TempData["Success"] = "تم التسجيل بنجاح في " + model.EventTitle;
                TempData["SuccessEventId"] = model.EventId.ToString();
                return SafeRedirect(model.EventId);
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error in Submit action for EventId: {EventId}", model?.EventId);
                TempData["Failed"] = "حصل خطأ أثناء التسجيل، جرب تاني.";
                TempData["FailedEventId"] = model?.EventId.ToString();
                return SafeRedirect(model?.EventId ?? 0);
            }
        }

        private IActionResult SafeRedirect(int eventId)
        {
            try
            {
                return RedirectToCurrentUmbracoPage();
            }
            catch (InvalidOperationException ex)
            {
                // _logger.LogError(ex, "Failed to redirect to current Umbraco page for EventId: {EventId}", eventId);
                var eventPage = UmbracoContext.Content?.GetById(eventId);
                if (eventPage != null)
                {
                    return Redirect(eventPage.Url());
                }
                return Redirect("/");
            }
        }
    }
}*/