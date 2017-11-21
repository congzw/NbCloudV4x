using System.Web.Mvc;
using NbCloud.Common.Demos;

namespace NbCloud.Web.Controllers
{
    public class DemoController : Controller
    {
        private readonly IDemoService _demoService;
        private readonly IDemoService _demoService2;

        public DemoController(IDemoService demoService, IDemoService demoService2)
        {
            _demoService = demoService;
            _demoService2 = demoService2;
        }

        public ActionResult Index()
        {
            var hello = _demoService.Hello();
            var s = _demoService2.Hello();
            ViewBag.Message = hello + " " + s;
            return View();
        }
        
        public ActionResult _Run(string view = null)
        {
            if (!string.IsNullOrWhiteSpace(view))
            {
                return View(view);
            }
            return View();
        }
    }
}