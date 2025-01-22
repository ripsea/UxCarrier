
using Asp.Versioning;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using UxCarrier.Helper;
using UxCarrier.Models;
using UxCarrier.Models.Dto;
using UxCarrier.Repository.IRepository;
using UxCarrier.Services;
using static UxCarrier.Models.Dto.UxBindStep1Dto;

namespace UxCarrier.Controllers
{
    [Route("api/v1/uxbind")]
    public class UxBindController : Controller
    {
        private readonly ILogger<UxBindController> _logger;
        private readonly ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly UxBindService _uxBindService;
        private readonly IRepositoryWrapper _repo;
        private static IHttpContextAccessor m_httpContextAccessor;
        public static HttpContext Current => m_httpContextAccessor.HttpContext;
        public UxBindController(
            IMapper mapper, ILogger<UxBindController> logger,
            UxBindService uxBindService,
            IRepositoryWrapper repo, IHttpContextAccessor contextAccessor)
        {
            _mapper = mapper;
            this._response = new();
            _logger = logger;
            _uxBindService = uxBindService;
            _repo = repo;
            m_httpContextAccessor = contextAccessor;
        }

        [HttpGet("trystart")]
        public IActionResult Step0()
        {
            ViewData["url"] = $"{Current.Request.PathBase}/api/v1/uxbind/send";
            return View();
        }

        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("send")]
        public ActionResult<ApiResponse> Step1([FromForm] UxBindStep1Dto dto)
        {

            var validationResult = new PostStep1DtoValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                //_response.ErrorMessage(BaseService.GetFluentValidationResult(validationResult));
                ViewData["message"] = BaseService.GetFluentValidationResult(validationResult);
                return View("~/Views/Home/ModalMessage.cshtml");
                //return Redirect("/Home/ModalMessage");
                //return Ok(_response);
            }

            var cardNo = new JwtSecurityTokenHandler().ReadJwtToken(dto.Token).GetCardNo();
            var email = new JwtSecurityTokenHandler().ReadJwtToken(dto.Token).GetEmail();
            var card = _repo.Card.FindByCondition(x => x.CardNo == cardNo);

            if (Utilities.IsNull(card.FirstOrDefault()))
            {
                //_response.ErrorMessage("非會員");
                //return Ok(_response);
                ViewData["message"] = "非會員，請重新登入";
                return View("~/Views/Home/ModalMessage.cshtml");
            }

            var uxBindCard = _uxBindService.UxBindCardGenerator(email);
            _repo.BindCard.Create(uxBindCard);
            _repo.Save();

            UxBindStep1PostDto data = _mapper.Map<UxBindStep1PostDto>(uxBindCard);
            data.url_step1 = _uxBindService.GetUxBindUrl();
            _logger.LogInformation(data.ToString());
            _response.Result = data;

            return View("~/Views/Uxbind/Send.cshtml", data);
            //return Ok(_response);

        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost("validate")]
        public ActionResult<ApiResponse> step2([FromForm] UxBindStep2Dto dto)
        {
            _logger.LogInformation(dto.ToString());
            UxBindDto uxBindDto = _mapper.Map<UxBindDto>(dto);

            var bindCard = _repo.BindCard
                .FindByCondition(x => x.token == uxBindDto.token
                    && x.card_no1 == uxBindDto.card_no1
                    && x.card_no2 == uxBindDto.card_no2
                    && x.card_ban == uxBindDto.card_ban)
                .FirstOrDefault();

            if (Utilities.IsNotNull(bindCard))
            {
                bindCard.Step2Result = "Y";
                bindCard.UpdatedDate = DateTime.Now;
                _repo.BindCard.Update(bindCard);
                _repo.Save();
                return Ok("Y");
            }

            return Ok("N");
        }

        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("confirm")]
        public ActionResult<ApiResponse> Step4([FromForm] UxBindStep4Dto dto)
        {
            _logger.LogInformation(dto.ToString());

            UxBindDto uxBindDto = _mapper.Map<UxBindDto>(dto);

            //wait to do...card should include cardemail
            //wait to do...cors https://ithelp.ithome.com.tw/articles/10245157
            //wait to do...email otp

            var card = _repo.Card
                .FindByCondition(x => x.CardNo == uxBindDto.card_no1)
                .FirstOrDefault();

            if (Utilities.IsNotNull(card))
            {
                card.UxBind = (dto.rtn_flag == "Y");
                card!.UpdatedDate = DateTime.Now;
                _repo.Card.Update(card);
                _repo.Save();
            }

            _response.Result = dto;

            return Ok(_response);
        }
    }
}
