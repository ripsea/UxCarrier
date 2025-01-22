using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Text;
using UxCarrier.Models;
using UxCarrier.Models.Dto;
using UxCarrier.Repository.IRepository;
using UxCarrier.Services;

namespace UxCarrier.Controllers
{
    public class PageController: Controller
    {
        private readonly ILogger<PageController> _logger;
        private readonly UxBindService _uxBindService;
        private readonly IRepositoryWrapper _repo;
        private readonly ApiResponse _response;

        public PageController(ILogger<PageController> logger,
                        UxBindService uxBindService,
            IRepositoryWrapper repo)
        {
            _logger = logger;
            _uxBindService = uxBindService;
            _repo = repo;
            this._response = new();
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task step1ByFormPost([FromBody] NameValueCollection data, string url)
        public IActionResult UxBindStep1View(UxBindStep1PostDto data)
        {
            //NameValueCollection nameValueCollection = data.ToNameValueCollection();
            //StringBuilder formPostString = _uxBindService.UxBindSendFormPostString(nameValueCollection);
            return View("~/Views/Home/UxBindStep1View.cshtml", data);
        }

        public IActionResult UxbindSend()
        {
            return Ok("Ok");
        }

    }
}