using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UxCarrier.Helper;
using UxCarrier.Models;

namespace UxCarrier.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static IHttpContextAccessor m_httpContextAccessor;
        public static HttpContext Current => m_httpContextAccessor.HttpContext;
        public static string AppBaseUrl => $"{Current.Request.Scheme}://{Current.Request.Host}{Current.Request.PathBase}";

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            m_httpContextAccessor = contextAccessor;
        }

        public IActionResult Index()
        {
            
            return Redirect(AppBaseUrl);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ModalMessage(string message) 
        {
            ViewData["message"] = string.IsNullOrEmpty(message) ? "請重新登入" : message;
            return View();
        }


        [HttpPost]
        public IActionResult GetStep2Response()
        {
            //throw new ArgumentException("argumentException");
            //return StatusCode(500);
            return Ok(new { token_flag = "Y", nonce = "1234567890ABCDEF", err_msg = "something wrong." });
        }
    }
}
